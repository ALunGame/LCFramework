using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 管理Buff的更新，移除
    /// </summary>
    [System(InFixedUpdate = true)]
    public class BuffSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(SkillCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            SkillCom skillCom = GetCom<SkillCom>(comList[0]);
            UpdateBuff(skillCom);
        }

        private void UpdateBuff(SkillCom skillCom)
        {
            float deltaTime = SkillLocate.Skill.GetSkillDeltaTime();
            //技能冷却
            for (int i = 0; i < skillCom.Skills.Count; i++)
            {
                SkillObj skillObj = skillCom.Skills[i];
                skillObj.coldDown -= deltaTime;
            }
            //更新Buff
            for (int i = 0; i < skillCom.Buffs.Count; i++)
            {
                BuffObj buffObj = skillCom.Buffs[i];
                //持续时间
                if (buffObj.isPermanent == false)
                    buffObj.duration -= deltaTime;
                //存在时间
                buffObj.timeElapsed += deltaTime;
                //检测OnTick
                if (buffObj.model.tickTime > 0)
                {
                    //为了精度*1000
                    if (Mathf.RoundToInt(buffObj.timeElapsed * 1000) % Mathf.RoundToInt(buffObj.model.tickTime * 1000) == 0)
                    {
                        BuffTickFunc.ExecuteFunc(buffObj.model.onTickFunc, buffObj);
                        buffObj.tickCnt += 1;
                    }
                }
                //检测移除
                if (buffObj.duration <= 0 || buffObj.stack <= 0)
                {
                    BuffRemovedFunc.ExecuteFunc(buffObj.model.onRemovedFunc, buffObj);
                    skillCom.RemoveBuff(buffObj);
                }
            }
        }
    }
}
