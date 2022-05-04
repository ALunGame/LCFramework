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
        
        public void PushRequest(int entityId, RequestId reqId, ParamData paramData)
        {
            //数据
            EntityWorkData workData = ECSLayerLocate.Info.GetEntityWorkData(entityId);
            if (workData == null)
            {
                return;
            }

            if (workData.CurrReqId == reqId)
            {
                workData.AddParam(paramData);
                return;
            }

            //请求
            IRequest pushRequest = GetEntityRequest(reqId);
            RequestId oldReqId   = workData.CurrReqId;


            //请求不需要自身处理
            if (pushRequest == null)
            {
                ChangeRequest(workData, reqId, paramData);
            }
            else
            {
                RequestId selfSwId = reqId;
                //请求内部置换
                int rule = pushRequest.SwitchRequest(reqId, ref selfSwId);
                //只需要自身判断
                if (rule == ECSDefinition.RESwithRuleSelf)
                {
                    if (workData.CurrReqId != selfSwId)
                    {
                        workData.ChangeRequestId(reqId);
                        workData.AddParam(paramData);
                    }
                }
                else
                {
                    //自身判断也需要权重置换规则
                    ChangeRequest(workData, reqId, paramData);
                }
            }

            if (workData.CurrReqId == oldReqId)
                return;

            //执行请求
            ECSLayerLocate.Behavior.ReqBev(workData, oldReqId);
        }

        private void ChangeRequest(EntityWorkData workData, RequestId reqId, ParamData paramData)
        {
            if (workData.CurrReqId == RequestId.None)
            {
                workData.ChangeRequestId(reqId);
                workData.AddParam(paramData);
            }
            else
            {
                int pushSort = GetRequestSort(reqId);
                int curSort = GetRequestSort(workData.CurrReqId);

                //强制置换（覆盖）
                if (pushSort == ECSDefinition.REForceSwithWeight)
                {
                    workData.ChangeRequestId(reqId);
                    workData.AddParam(paramData);
                }
                else
                {
                    //判断当前的
                    if (pushSort < curSort)
                    {
                        workData.ChangeRequestId(reqId);
                        workData.AddParam(paramData);
                    }
                }
            }
        }
    }
}
