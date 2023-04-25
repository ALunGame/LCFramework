using System;
using System.Collections.Generic;
using Demo;

namespace LCMap
{
    public static partial class ActorMediator
    {
        public static Actor GetMainActor()
        {
            return ActorLocate.Actor.GetMainActor();
        }
        
        public static Actor GetActor(string pActorUid)
        {
            return ActorLocate.Actor.GetActor(pActorUid);
        }

        public static List<Actor> GetActors(int pActorId)
        {
            return ActorLocate.Actor.GetActors(pActorId);
        }
        
        public static Actor GetActor(int pActorId)
        {
            List<Actor> actors = GetActors(pActorId);
            if (actors.IsLegal())
            {
                return actors[0];
            }
            return null;
        }
        
        /// <summary>
        /// 获得带有指定组件的演员
        /// </summary>
        /// <param name="pComTypeFullName"></param>
        /// <returns></returns>
        public static IEnumerable<Actor> GetActors(string pComTypeFullName)
        {
            return ActorLocate.Actor.GetActors(pComTypeFullName);
        }
        
        
        #region 请求

        public static bool Requeset(Actor pActor, ActorRequestType pRequestType, Action pReqFinsihCallBack, params object[] pParams)
        {
            return ActorLocate.ActorRequest.Request(pActor, (int)pRequestType,pReqFinsihCallBack,pParams);
        }

        public static void FinishRequest(Actor pActor, ActorRequestType pRequestType)
        {
            ActorLocate.ActorRequest.FinishRequest(pActor,(int)pRequestType);
        }
        
        public static bool CheckCanRequest(Actor pActor, ActorRequestType pRequestType)
        {
            return ActorLocate.ActorRequest.CheckCanRequest(pActor, (int)pRequestType);
        }

        #endregion
    }
}