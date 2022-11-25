using LCConfig;
using LCECS.Core;
using LCLoad;
using System.Collections.Generic;
using UnityEngine;

namespace LCMap
{
    public static class ActorCreator
    {
        public static Actor CreateEntity(ActorInfo actorInfo)
        {
            ActorCnf actorCnf = Config.ActorCnf[actorInfo.id];
            List<BaseCom> resComs = LCECS.ECSLocate.ECS.GetEntityComsModel(actorCnf.entityId);

            //配置覆盖
            if (actorCnf.comCnfs != null)
            {
                for (int i = 0; i < actorCnf.comCnfs.Count; i++)
                {
                    BaseCom checkCom = actorCnf.comCnfs[i];
                    bool hasCom = false;
                    for (int j = 0; j < resComs.Count; j++)
                    {
                        if (resComs[j].GetComName() == checkCom.GetComName())
                        {
                            resComs[j] = checkCom;
                            hasCom = true;
                            break;
                        }
                    }
                    if (!hasCom)
                    {
                        resComs.Add(checkCom);
                    }
                }
            }

            //基础数据赋值
            Actor actor = new Actor(actorInfo.uid);
            actor.EntityId = actorCnf.entityId;
            actor.Id = actorInfo.id;
            actor.isActive = actorInfo.isActive;
            actor.SetState(actorInfo.stateName);
            actor.SetPos(actorInfo.pos);
            actor.SetRoate(actorInfo.roate);
            actor.SetScale(actorInfo.scale);

            //组件
            for (int i = 0; i < resComs.Count; i++)
            {
                actor.AddCom(resComs[i]);
            }

            //保存
            LCECS.ECSLocate.ECS.AddEntity(actor);
            return actor;
        }

        public static void CreateGo(Actor pActor)
        {
            ActorCnf actorCnf = Config.ActorCnf[pActor.Id];
            //预制体
            GameObject assetGo = LoadHelper.LoadPrefab(actorCnf.prefab.ObjName);
            GameObject actorGo = GameObject.Instantiate(assetGo);
            pActor.SetBindGo(actorGo);
        }
    }
}
