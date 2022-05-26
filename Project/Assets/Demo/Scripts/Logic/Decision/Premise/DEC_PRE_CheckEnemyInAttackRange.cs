using Demo.Com;
using LCECS.Core;
using LCECS.Core.Tree.Base;
using LCECS.Data;
using LCMap;
using LCToolkit;
using System;

namespace Demo.Decision
{
    /// <summary>
    /// 检测敌人在攻击范围内
    /// </summary>
    public class DEC_PRE_CheckEnemyInAttackRange : NodePremise
    {
        [NonSerialized]
        public const string EnemyInAttackRangeKey = "DEC_PRE_EnemyInAttackRangeKey";

        //检测范围
        public Shape checkRange;

        public override bool OnMakeTrue(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            TransformCom selfTransCom = workData.MEntity.GetCom<TransformCom>();
            ActorObj actor = MapLocate.Map.GetActor(workData.MEntity.Uid);
            CampCom selfCampCom = workData.MEntity.GetCom<CampCom>();
            if (actor == null || actor.Area == null)
                return false;

            if (selfCampCom == null || selfCampCom.Camp == CampType.Neutral)
                return false;

            Shape checkShape = checkRange;
            checkShape.Translate(selfTransCom.GetPos());

            GameLocate.ShapeRender.AddShape(ShapeRenderType.攻击范围, workData.MEntity.Uid, checkShape);

            //检测保存的
            if (wData.Blackboard.ContainsKey(EnemyInAttackRangeKey))
            {
                int saveEntityId = (int)wData.Blackboard[EnemyInAttackRangeKey];
                Entity saveEntity = LCECS.ECSLocate.ECS.GetEntity(saveEntityId);
                if (saveEntity == null)
                {
                    wData.Blackboard.Remove(EnemyInAttackRangeKey);
                }
                Shape saveShape = GetEntityColliderShape(saveEntity);
                if (!saveShape.Intersects(checkShape))
                {
                    wData.Blackboard.Remove(EnemyInAttackRangeKey);
                }
                else
                {
                    return true;
                }
            }

            //先检测玩家
            Entity playerEntity = LCECS.ECSLocate.Player.GetPlayerEntity();
            if (playerEntity != null)
            {
                TransformCom transCom = playerEntity.GetCom<TransformCom>();
                if (checkShape.ContainPoint(transCom.GetPos()))
                {
                    wData.Blackboard.Add(EnemyInAttackRangeKey, playerEntity.Uid);
                    return true;
                }
            }

            //敌对演员
            foreach (var item in actor.Area.Actors.Keys)
            {
                if (item == workData.MEntity.Uid)
                {
                    continue;
                }
                Entity entity = LCECS.ECSLocate.ECS.GetEntity(item);
                if (entity != null)
                {
                    CampCom campCom = entity.GetCom<CampCom>();
                    if (campCom != null && campCom.Camp != selfCampCom.Camp)
                    {
                        TransformCom transCom = entity.GetCom<TransformCom>();
                        if (checkShape.ContainPoint(transCom.GetPos()))
                        {
                            wData.Blackboard.Add(EnemyInAttackRangeKey, entity.Uid);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private Shape GetEntityColliderShape(Entity entity)
        {
            Collider2DCom collider2DCom = entity.GetCom<Collider2DCom>();
            TransformCom transCom = entity.GetCom<TransformCom>();
            Shape shape = collider2DCom.colliderShape;
            shape.Translate(transCom.GetPos());
            return shape;
        }
    }
}
