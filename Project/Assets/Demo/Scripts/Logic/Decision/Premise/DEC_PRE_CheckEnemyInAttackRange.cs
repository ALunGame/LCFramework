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
            TransCom selfTransCom = workData.MEntity.GetCom<TransCom>();
            Actor actor = ActorMediator.GetActor(workData.MEntity.Uid);

            MapArea mapArea = actor.CurrArea;
            CampCom selfCampCom = workData.MEntity.GetCom<CampCom>();
            if (actor == null || mapArea == null)
                return false;

            if (selfCampCom == null || selfCampCom.Camp == CampType.Neutral)
                return false;

            Shape checkShape = checkRange;
            checkShape.Translate(selfTransCom.Pos);

            GameLocate.ShapeRender.AddShape(ShapeRenderType.攻击范围, workData.MEntity.Uid, checkShape);

            //检测保存的
            if (wData.Blackboard.ContainsKey(EnemyInAttackRangeKey))
            {
                string saveEntityUid = (string)wData.Blackboard[EnemyInAttackRangeKey];
                Entity saveEntity = LCECS.ECSLocate.ECS.GetEntity(saveEntityUid);
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
                TransCom transCom = playerEntity.GetCom<TransCom>();
                if (checkShape.ContainPoint(transCom.Pos))
                {
                    wData.Blackboard.Add(EnemyInAttackRangeKey, playerEntity.Uid);
                    return true;
                }
            }

            //敌对演员
            foreach (var item in mapArea.InAreaAtors.Keys)
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
                        TransCom transCom = entity.GetCom<TransCom>();
                        if (checkShape.ContainPoint(transCom.Pos))
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
            TransCom transCom = entity.GetCom<TransCom>();
            Shape shape = collider2DCom.colliderShape;
            shape.Translate(transCom.Pos);
            return shape;
        }
    }
}
