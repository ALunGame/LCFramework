using Demo.Com;
using LCECS;
using LCECS.Core.ECS;
using LCECS.Layer.Info;
using LCHelp;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Info
{
    public enum EntityReqInfoType
    {
        GetEntitysByArea,           //指定区域的实体
    }

    [WorldSensor(SensorType.Entity)]
    public class EntitySensor : ISensor
    {
        private List<Entity> cllectEntitys = new List<Entity>();
        private Rect skillCheckRect = new Rect();

        /// <summary>
        /// 查询技能作用区域内的实体列表
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="area"></param>
        /// <param name="hasComType"></param>
        public List<Entity> CollectEntitysBySkillArea(Vector3 pos, Vector3 area)
        {
            cllectEntitys.Clear();

            Dictionary<int, Entity> allEntitys = ECSLocate.ECS.GetAllEntitys();
            skillCheckRect.x = pos.x;
            skillCheckRect.y = pos.y;
            skillCheckRect.width  = area.x;
            skillCheckRect.height = area.y;

            Rect entityRect = new Rect();
            foreach (Entity item in allEntitys.Values)
            {
                SkillCom skillCom = item.GetCom<SkillCom>();
                if (skillCom==null)
                {
                    continue;
                }
                entityRect.x = skillCom.SkillCheckPoint.position.x;
                entityRect.y = skillCom.SkillCheckPoint.position.y;
                entityRect.width = skillCom.SkillCheckSize.x;
                entityRect.height = skillCom.SkillCheckSize.y;

                if (skillCheckRect.Overlaps(entityRect))
                {
                    cllectEntitys.Add(item);
                }
            }

            return cllectEntitys;
        }
    }
}
