using System;
using System.Collections.Generic;

namespace LCMap
{
    public class ActorRequestServer
    {
        private Dictionary<int, ActorRequest> requestMap = new Dictionary<int, ActorRequest>();

        /// <summary>
        /// 注册请求
        /// </summary>
        /// <param name="pReqId"></param>
        /// <param name="pRequest"></param>
        public void RegRequest(int pReqId, ActorRequest pRequest)
        {
            if (requestMap.ContainsKey(pReqId))
            {
                LCMap.MapLocate.Log.Log("注册请求失败，请求Id重复>>",pReqId);
                return;
            }
            requestMap.Add(pReqId,pRequest);
        }
        
        private bool ChangeRequest(Actor pActor, int pReqId, Action pReqFinsihCallBack, params object[] pParams)
        {
            int currReqId = pActor.CurrRequestId;
            if (!requestMap.ContainsKey(currReqId))
            {
                pActor.RequestFinishCallBack = pReqFinsihCallBack;
                pActor.CurrRequestId = pReqId;

                if (requestMap.TryGetValue(pReqId,out ActorRequest request))
                {
                    request.OnEnter(pActor, pParams);
                }

                return true;
            }
            else
            {
                ActorRequest currRequest = requestMap[currReqId];
                if (requestMap.TryGetValue(pReqId,out ActorRequest request))
                {
                    if (request.Weight < currRequest.Weight)
                    {
                        LCMap.MapLocate.Log.Log("演员无法请求，当前有请求尚未结束>>",pActor.Id,currReqId);
                        return false;
                    }
                    
                    //离开
                    currRequest.OnExit(pActor);
                    
                    //更新数据
                    pActor.RequestFinishCallBack = pReqFinsihCallBack;
                    pActor.CurrRequestId = pReqId;
                    request.OnEnter(pActor, pParams);
                    return true;
                }
                return false;
            }
        }
        
        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="pActor">演员</param>
        /// <param name="pReqId">请求Id</param>
        /// <param name="pReqFinsihCallBack">请求完成回调</param>
        /// <param name="pParams">请求参数</param>
        /// <returns></returns>
        public bool Request(Actor pActor, int pReqId, Action pReqFinsihCallBack, params object[] pParams)
        {
            return ChangeRequest(pActor, pReqId, pReqFinsihCallBack, pParams);
        }

        /// <summary>
        /// 完成请求
        /// </summary>
        /// <param name="pActor">演员</param>
        /// <param name="pReqId">请求Id</param>
        public void FinishRequest(Actor pActor, int pReqId)
        {
            int currReqId = pActor.CurrRequestId;
            if (pReqId != currReqId)
            {
                LCMap.MapLocate.Log.Log("演员无法完成请求,当前请求不一致>>",pActor.Id,currReqId,pReqId);
                return;
            }

            //离开
            if (requestMap.TryGetValue(pReqId,out ActorRequest request))
            {
                Action finishCallBack = pActor.RequestFinishCallBack;
                pActor.RequestFinishCallBack = null;
                pActor.CurrRequestId = NullActorRequest.NullRequestId;
                
                //离开
                request.OnExit(pActor);
                
                //完成回调
                finishCallBack?.Invoke();
            }
        }
    }
}