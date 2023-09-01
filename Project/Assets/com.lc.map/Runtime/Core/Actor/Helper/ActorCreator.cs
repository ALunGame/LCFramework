using LCConfig;
using LCECS.Core;
using System.Collections.Generic;
using UnityEngine;

namespace LCMap
{
    public static class ActorCreator
    {
        #region Create

        private static Actor CreateEntity(ActorInfo actorInfo)
        {
            ActorCnf actorCnf = LCConfig.Config.ActorCnf[actorInfo.id];
            List<BaseCom> resComs = LCECS.ECSLocate.ECS.GetEntityComsModel(actorCnf.entityId);

            //构造
            Actor actor = new Actor(actorInfo.uid);
            actor.EntityId = actorCnf.entityId;
            actor.Id = actorInfo.id;
            actor.isActive = actorInfo.isActive;
            
            //组件
            for (int i = 0; i < resComs.Count; i++)
                actor.AddCom(resComs[i]);

            //保存 并 调用 组件 Awake
            LCECS.ECSLocate.ECS.AddEntity(actor);

            //基础数据赋值
            actor.SetState(actorInfo.stateName);
            actor.SetPos(actorInfo.pos);
            actor.SetRoate(actorInfo.roate);
            actor.SetScale(actorInfo.scale);
            
            return actor;
        }

        private static void CreateGo(Actor pActor)
        {
            ActorCnf actorCnf = LCConfig.Config.ActorCnf[pActor.Id];
            //预制体
            GameObject assetGo = IAFramework.GameContext.Asset.LoadPrefab(actorCnf.prefab);
            GameObject actorGo = GameObject.Instantiate(assetGo);
            pActor.SetBindGo(actorGo);

            Debug.LogWarning("创建演员预制体>>>>>>>>>");
        }

        public static Actor CreateActor(ActorInfo actorInfo)
        {
            Actor tActor = CreateEntity(actorInfo);
            CreateGo(tActor);
            tActor.Enable();
            return tActor;
        }
        
        public static Actor CreateActor(ActorInfo pActorInfo,GameObject pActorGo)
        {
            Actor tActor = CreateEntity(pActorInfo);
            tActor.SetBindGo(pActorGo);
            tActor.Enable();
            return tActor;
        }

        #endregion

        #region Destroy

        /// <summary>
        /// 销毁演员
        /// </summary>
        /// <param name="pActor"></param>
        public static void DestroyActor(Actor pActor)
        {
            //有就先删除数据
            if (ActorLocate.Actor.CheckHasActor(pActor))
                ActorLocate.Actor.RemoveActor(pActor);
            pActor.CurrArea?.ExitArea(pActor);
            
            LCECS.ECSLocate.ECS.RemoveEntity(pActor);
        }

        #endregion

    }
}
