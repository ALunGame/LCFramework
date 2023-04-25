using System;
using System.Collections.Generic;
using Demo;
using LCToolkit;

namespace LCMap
{
    public class ActorRequestServer
    {
        private Dictionary<int, ActorRequest> requestMap = new Dictionary<int, ActorRequest>();

        public ActorRequestServer()
        {
            foreach (Type type in ReflectionHelper.GetChildTypes<ActorRequest>())
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;
                ActorRequest request = ReflectionHelper.CreateInstance(type) as ActorRequest;
                RegRequest(request.RequestId, request);
            }
        }

        /// <summary>
        /// 注册请求
        /// </summary>
        /// <param name="pReqId"></param>
        /// <param name="pRequest"></param>
        private void RegRequest(int pReqId, ActorRequest pRequest)
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
                if (requestMap.TryGetValue(pReqId,out ActorRequest request))
                {
                    pActor.CurrRequest = request.CreateSpec(pActor);
                    pActor.CurrRequest.SetFinishCallBack(pReqFinsihCallBack);
                    pActor.CurrRequest.OnEnter(pParams);
                    GameLocate.Log.LogWarning("切换请求》》",pReqId);
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
                    pActor.CurrRequest.OnExit();
                    
                    //更新数据
                    pActor.CurrRequest = request.CreateSpec(pActor);
                    pActor.CurrRequest.SetFinishCallBack(pReqFinsihCallBack);
                    pActor.CurrRequest.OnEnter(pParams);
                    GameLocate.Log.LogWarning("切换请求》》",pReqId);
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
            if (pActor.CurrRequest != null)
            {
                pActor.CurrRequest.OnExit();
                pActor.CurrRequest.ExecuteFinishCallBack();
            }
        }

        /// <summary>
        /// 检测是否可以执行该请求
        /// </summary>
        /// <param name="pActor"></param>
        /// <param name="pReqId"></param>
        /// <returns></returns>
        public bool CheckCanRequest(Actor pActor, int pReqId)
        {
            if (pActor.CurrRequestId == pReqId)
            {
                return true;
            }
            int currReqId = pActor.CurrRequestId;
            if (!requestMap.ContainsKey(currReqId))
            {
                return true;
            }
            
            ActorRequest currRequest = requestMap[currReqId];
            if (requestMap.TryGetValue(pReqId,out ActorRequest request))
            {
                if (request.Weight >= currRequest.Weight)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}