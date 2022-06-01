using LCECS;
using LCMap;
using LCSkill;
using System.Collections.Generic;
using LCECS;
using LCToolkit;
using LCECS.Core;
using Demo.Com;

namespace Demo
{
    [WorldSensor(SensorType.Skill_Aoe)]
    public class AoeSensor : BaseAoeSensor
    {
        public override bool CheckActorInRange(AoeObj aoeObj, ActorObj actor)
        {
            Shape checkShape = aoeObj.CalcArea();
            Entity entity = ECSLocate.ECS.GetEntity(actor.Uid);
            Shape body = EntityGetter.GetEntityColliderShape(entity);
            return checkShape.Intersects(body);
        }

        public override bool CheckBulletInRange(AoeObj aoeObj, BulletObj bullet)
        {
            return false;
        }

        public override List<ActorObj> GetActorsInRange(AoeObj aoeObj)
        {
            List<ActorObj> result = new List<ActorObj>();

            int uid             = aoeObj.ower.EntityId;
            ActorObj actor      = MapLocate.Map.GetActor(uid);
            Entity selfEntity   = ECSLocate.ECS.GetEntity(uid);
            CampCom selfCampCom = selfEntity.GetCom<CampCom>();

            Shape checkShape    = aoeObj.CalcArea();

            //先检测玩家
            Entity playerEntity = LCECS.ECSLocate.Player.GetPlayerEntity();
            CampCom playerCampCom = playerEntity.GetCom<CampCom>();
            if (playerEntity != null && playerEntity.Uid != uid && playerCampCom.Camp != selfCampCom.Camp)
            {
                Shape playerBody = EntityGetter.GetEntityColliderShape(playerEntity);
                if (checkShape.Intersects(playerBody))
                {
                    result.Add(MapLocate.Map.PlayerActor);
                }
            }


            //敌对演员
            foreach (var item in actor.Area.Actors)
            {
                if (item.Key == aoeObj.ower.EntityId)
                    continue;
                Entity entity = ECSLocate.ECS.GetEntity(item.Key);
                if (entity != null)
                {
                    CampCom campCom = entity.GetCom<CampCom>();
                    if (campCom != null && campCom.Camp != selfCampCom.Camp)
                    {
                        Shape body = EntityGetter.GetEntityColliderShape(entity);
                        if (checkShape.Intersects(body))
                        {
                            result.Add(item.Value);
                        }
                    }
                }
            }

            return result;
        }

        public override List<BulletObj> GetBulletsInRange(AoeObj aoeObj)
        {
            return new List<BulletObj>();
        }
    }
}
