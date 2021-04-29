using System;
using System.Collections.Generic;
using LCECS.Data;
using LCECS.Layer.Request;
using LCHelp;
using UnityEngine;
using XPToolchains.Json;
#if UNITY_EDITOR
using LCECS.Scene;
#endif

namespace LCECS.Server.Layer
{
    public class RequestServer : IRequestServer
    {
        private Dictionary<int, IRequest> RequestDict = new Dictionary<int, IRequest>();
        private ReqWeightJson reqWeightConf;
        
        private void RegAllRequest()
        {
            List<Type> requestTypes = LCReflect.GetInterfaceByType<IRequest>();
            if (requestTypes == null)
                return;
            
            foreach (Type type in requestTypes)
            {
                RequestAttribute attr = LCReflect.GetTypeAttr<RequestAttribute>(type);
                if (attr == null)
                {
                    ECSLocate.ECSLog.Log("有请求没有加特性 走权重 >>>>>>", type.Name);
                    return;
                }

                IRequest request = LCReflect.CreateInstanceByType<IRequest>(type.FullName);
                RequestDict.Add((int)attr.ReqId, request);
            }
        }
        
        private IRequest GetEntityRequest(int key)
        {
            if (!RequestDict.ContainsKey(key))
                return null;
            return RequestDict[key];
        }
        
        /// 请求置换规则
        private void CheckCanSwitch(int pushId, ref int nextId, ref int curId, ref int clearId, bool isEntity)
        {
            //小于零错误
            if (pushId <= 0)
            {
                return;
            }

            int pushWeight = GetRequestWeight(pushId);
            int nextWeight = GetRequestWeight(nextId);
            int curWeight = GetRequestWeight(curId);

            //没有执行的请求---直接换
            if (curId == 0)
            {
                //最新的比原本下一个权重高（覆盖）
                if (pushWeight > nextWeight)
                {
                    curId = pushId;
                    nextId = 0;
                }
                else
                {
                    curId = nextId;
                    nextId = pushId;
                }
            }
            else
            {
                //强制置换（覆盖）
                if (pushWeight == ECSDefinition.REForceSwithWeight)
                {
                    curId = pushId;
                    nextId = 0;
                    return;
                }
                else
                {
                    //判断当前的
                    if (pushWeight > curWeight)
                    {
                        curId = pushId;
                    }

                    //判断本来的下一个
                    if (pushWeight > nextWeight && curId != pushId)
                    {
                        nextId = pushId;
                    }
                }
            }
        }
        
        public void Init()
        {
            TextAsset jsonData = ECSLocate.Factory.GetProduct<TextAsset>(FactoryType.Asset, null, ECSDefinitionPath.LogicReqWeightPath);
            ReqWeightJson json = JsonMapper.ToObject<ReqWeightJson>(jsonData.text);
            reqWeightConf = json;
            RegAllRequest();
        }
        
        public int GetRequestWeight(int reqId)
        {
            if (reqId <= 0)
                return 0;
            for (int i = 0; i < reqWeightConf.ReqWeights.Count; i++)
            {
                WeightJson json = reqWeightConf.ReqWeights[i];
                if (json.Key == reqId)
                {
                    return json.Weight;
                }
            }
            ECSLocate.ECSLog.LogR("有实体请求没有设置权重>>>>", reqId.ToString());
            return 0;
        }
        
        public void PushRequest(int entityId, int reqId)
        {
            //数据
            EntityWorkData workData = ECSLayerLocate.Info.GetEntityWorkData(entityId);
            if (workData == null)
            {
                return;
            }

            //请求
            IRequest pushRequest = GetEntityRequest(reqId);
            int oldReqId = workData.CurrReqId;

            //请求不需要自身处理
            if (pushRequest == null)
            {
                //权重置换规则
                CheckCanSwitch(reqId, ref workData.NextReqId, ref workData.CurrReqId, ref workData.ClearReqId, true);
                if (workData.CurrReqId == oldReqId)
                {
                    return;
                }
                //执行请求
                ECSLayerLocate.Behavior.PushBev(workData);
                return;
            }

            int selfSwId = reqId;
            //请求内部置换
            int rule = pushRequest.SwitchRequest(reqId, ref selfSwId);
            //只需要自身判断
            if (rule == ECSDefinition.RESwithRuleSelf)
            {
                //没有变化
                if (workData.CurrReqId == selfSwId)
                {
                    return;
                }
                workData.CurrReqId = selfSwId;

                //执行请求
                ECSLayerLocate.Behavior.PushBev(workData);
                return;
            }

            //自身判断也需要权重置换规则
            CheckCanSwitch(selfSwId, ref workData.NextReqId, ref workData.CurrReqId, ref workData.ClearReqId, true);
            if (workData.CurrReqId == oldReqId)
            {
                return;
            }
            //执行请求
            ECSLayerLocate.Behavior.PushBev(workData);
        }
    }
}
