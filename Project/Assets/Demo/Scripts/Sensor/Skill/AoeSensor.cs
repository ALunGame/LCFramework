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
        public override bool CheckActorInRange(AoeObj aoeObj, Actor actor)
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

        public override List<Actor> GetActorsInRange(AoeObj aoeObj)
        {
            List<Actor> result = new List<Actor>();

            string uid          = aoeObj.ower.EntityUid;
            Actor actor      = MapLocate.Map.GetActor(uid);
            MapArea mapArea = MapLocate.Map.GetAreaByActor(actor);
            CampCom selfCampCom = actor.GetCom<CampCom>();

            Shape checkShape    = aoeObj.CalcArea();

            //�ȼ�����
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


            //�ж���Ա
            foreach (var item in mapArea.Actors)
            {
                if (item.Key == aoeObj.ower.EntityUid)
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
