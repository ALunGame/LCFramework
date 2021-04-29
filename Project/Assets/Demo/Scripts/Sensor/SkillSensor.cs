using Demo.Com;
using Demo.System;
using LCECS;
using LCECS.Core.ECS;
using LCECS.Layer.Info;
using LCHelp;
using LCSkill;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Info
{
    [WorldSensor(SensorType.Skill)]
    public class SkillSensor : ISensor
    {
        //收集被技能作用的实体
        public List<Entity> CollectSkillUseEntitys(SkillCom skillCom, SkillImpact impact, GameObjectCom goCom,SkillSystem skillSystem)
        {
            List<Entity> entitys = new List<Entity>();

            //他人
            if (impact.OtherInfo != null)
            {
                Vector3 pos = (Vector3)LCConvert.StrChangeToObject(impact.OtherInfo.Pos, typeof(Vector3).FullName);
                Vector3 area = (Vector3)LCConvert.StrChangeToObject(impact.OtherInfo.Area, typeof(Vector3).FullName);
                pos = new Vector3(goCom.Tran.position.x + pos.x, goCom.Tran.position.y + pos.y, goCom.Tran.position.z + pos.z);

                EntitySensor entitySensor = ECSLayerLocate.Info.GetSensor<EntitySensor>(SensorType.Entity);
                entitys = entitySensor.CollectEntitysBySkillArea(pos, area);

#if UNITY_EDITOR
                Rect drawRect = new Rect(pos, area);
                skillSystem.SetEntityDrawData(skillCom.EntityId, drawRect);
#endif

                if (impact.OtherInfo.Filter.TargetType == SkillTargetType.Friend)
                {
                    //TODO  友方没有实现
                }
                else if (impact.OtherInfo.Filter.TargetType == SkillTargetType.FriendAndSelf)
                {
                    //TODO  友方没有实现
                }
                else if (impact.OtherInfo.Filter.TargetType == SkillTargetType.Enemy)
                {
                }
                FilterSkillUseEntitys(ref entitys, impact.OtherInfo.Filter, skillCom);
            }

            return entitys;
        }

        //剔除实体
        private void FilterSkillUseEntitys(ref List<Entity> entitys, SkillFilter filter, SkillCom skillCom)
        {
            //先按规则排序
            if (filter.Filter != SkillFilterRule.None)
            {
                entitys.Sort((left, right) =>
                {
                    AttributeCom leftAttr = left.GetCom<AttributeCom>();
                    AttributeCom rightAttr = right.GetCom<AttributeCom>();

                    float leftValue = leftAttr.AttrDict[filter.DataName];
                    float rightValue = rightAttr.AttrDict[filter.DataName];

                    int bigIndex = filter.Filter == SkillFilterRule.Max ? 1 : -1;
                    int samllIndex = filter.Filter == SkillFilterRule.Min ? 1 : -1;
                    if (leftValue > rightValue)
                    {
                        return bigIndex;
                    }
                    else if (leftValue < rightValue)
                    {
                        return samllIndex;
                    }
                    else
                    {
                        return 0;
                    }
                });
            }

            for (int i = 0; i < entitys.Count; i++)
            {
                if (entitys[i].GetHashCode() == skillCom.EntityId)
                {
                    entitys.RemoveAt(i);
                }
            }

            //按数量剔除
            if (filter.TargetCnt != -1 && entitys.Count > filter.TargetCnt)
            {
                for (int i = filter.TargetCnt; i < entitys.Count; i++)
                {
                    entitys.RemoveAt(i);
                }
            }
        }

        //处理技能作用属性值
        public void HandleSkillUseAttrData(List<Entity> entitys, List<SkillDataOperate> datas, bool reSet = false)
        {
            for (int i = 0; i < entitys.Count; i++)
            {
                for (int j = 0; j < datas.Count; j++)
                {
                    SkillDataOperate dataOperate = datas[j];

                    AttributeCom attributeCom = entitys[i].GetCom<AttributeCom>();
                    float oldValue = attributeCom.AttrDict[dataOperate.Name];
                    float newValue;
                    if (reSet)
                        newValue = RetMathSkillUseAttrData(dataOperate, oldValue);
                    else
                        newValue = SetMathSkillUseAttrData(dataOperate, oldValue);
                    attributeCom.AttrDict[dataOperate.Name] = newValue;

                    if (reSet == false)
                    {
                        if (dataOperate.Name == "Hp")
                        {
                            if (dataOperate.MathType == SkillDataMathType.Remove)
                            {
                                FightUpWorldPanel panel = LCUI.LCUILocate.GetUIPanel<FightUpWorldPanel>(LCUI.UIPanelId.FightUpWorld);
                                panel.PlayUpWorldItem(entitys[i], "-" + dataOperate.Data);
                            }
                        }
                    }
                }

            }
        }

        //设置技能数值
        private float SetMathSkillUseAttrData(SkillDataOperate dataJson, float oldData)
        {
            switch (dataJson.MathType)
            {
                case SkillDataMathType.Add:
                    return oldData + (float)dataJson.Data;
                case SkillDataMathType.Remove:
                    return oldData - (float)dataJson.Data;
                case SkillDataMathType.Ride:
                    return oldData * (float)dataJson.Data;
                case SkillDataMathType.Cover:
                    return (float)dataJson.Data;
                default:
                    return oldData;
            }
        }

        //重置技能数值
        private float RetMathSkillUseAttrData(SkillDataOperate dataJson, float oldData)
        {
            switch (dataJson.MathType)
            {
                case SkillDataMathType.Add:
                    return oldData - (float)dataJson.Data;
                case SkillDataMathType.Remove:
                    return oldData + (float)dataJson.Data;
                case SkillDataMathType.Ride:
                    return oldData / (float)dataJson.Data;
                default:
                    return oldData;
            }
        }
    }
}
