using LCECS.Data;
using LCECS.Layer.Request;
using LCToolkit;
using LCJson;
using System;
using System.Collections.Generic;
using UnityEngine;
using LCECS.Config;
#if UNITY_EDITOR
#endif

namespace LCECS.Server.Layer
{
    public class RequestServer : IRequestServer
    {
        private Dictionary<RequestId, IRequest> RequestDict = new Dictionary<RequestId, IRequest>();
        
        private void RegAllRequest()
        {
            foreach (Type type in ReflectionHelper.GetChildTypes<IRequest>())
            {
                if (AttributeHelper.TryGetTypeAttribute(type, out RequestAttribute attr))
                {
                    IRequest request = ReflectionHelper.CreateInstance<IRequest>(type.FullName);
                    RequestDict.Add(attr.ReqId, request);
                }
                else
                {
                    ECSLocate.Log.Log("有请求没有加特性 走权重 >>>>>>", type.Name);
                    return;
                }
            }
        }
        
        private IRequest GetEntityRequest(RequestId key)
        {
            if (!RequestDict.ContainsKey(key))
                return null;
            return RequestDict[key];
        }
        
        /// 请求置换规则
        private void CheckCanSwitch(RequestId pushId, ref RequestId nextId, ref RequestId curId, ref RequestId clearId, bool isEntity)
        {
            //小于零错误
            if (pushId <= 0)
            {
                return;
            }

            int pushSort = GetRequestSort(pushId);
            int nextSort = GetRequestSort(nextId);
            int curSort = GetRequestSort(curId);

            //没有执行的请求---直接换
            if (curId == 0)
            {
                //最新的比原本下一个排序高（覆盖）
                if (pushSort < nextSort)
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
                if (pushSort == ECSDefinition.REForceSwithWeight)
                {
                    curId = pushId;
                    nextId = 0;
                    return;
                }
                else
                {
                    //判断当前的
                    if (pushSort < curSort)
                    {
                        curId = pushId;
                    }

                    //判断本来的下一个
                    if (pushSort < nextSort && curId != pushId)
                    {
                        nextId = pushId;
                    }
                }
            }
        }
        
        public void Init()
        {
            RegAllRequest();
        }
        
        public int GetRequestSort(RequestId reqId)
        {
            RequestSort sort = ECSLocate.Center.GetRequestSort(reqId);
            if (sort == null)
                return 0;
            return sort.sort;
        }
        
        public void PushRequest(int entityId, RequestId reqId)
        {
            //数据
            EntityWorkData workData = ECSLayerLocate.Info.GetEntityWorkData(entityId);
            if (workData == null)
            {
                return;
            }

            //请求
            IRequest pushRequest = GetEntityRequest(reqId);
            RequestId oldReqId = workData.CurrReqId;

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
                ECSLayerLocate.Behavior.ReqBev(workData);
                return;
            }

            RequestId selfSwId = reqId;
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
                ECSLayerLocate.Behavior.ReqBev(workData);
                return;
            }

            //自身判断也需要权重置换规则
            CheckCanSwitch(selfSwId, ref workData.NextReqId, ref workData.CurrReqId, ref workData.ClearReqId, true);
            if (workData.CurrReqId == oldReqId)
            {
                return;
            }
            //执行请求
            ECSLayerLocate.Behavior.ReqBev(workData);
        }
    }
}
