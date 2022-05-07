using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 管理Buff的更新，移除
    /// </summary>
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
            float deltaTime = SkillLocate.DeltaTime;
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
                        ExecuteBuffTick(buffObj);
                        buffObj.tickCnt += 1;
                    }
                }
                //检测移除
                if (buffObj.duration <= 0 || buffObj.stack <= 0)
                {
                    ExecuteBuffRemove(buffObj);
                    skillCom.RemoveBuff(buffObj);
                }
            }
        }

        private void ExecuteBuffTick(BuffObj buffObj)
        {
            if (buffObj.model.onTickFunc == null)
            {
                return;
            }
            for (int i = 0; i < buffObj.model.onTickFunc.Count; i++)
            {
                BuffLifeCycleFunc func = buffObj.model.onTickFunc[i];
                func.Execute(buffObj);
            }
        }

        private void ExecuteBuffRemove(BuffObj buffObj)
        {
            if (buffObj.model.onRemovedFunc == null)
            {
                return;
            }
            for (int i = 0; i < buffObj.model.onRemovedFunc.Count; i++)
            {
                BuffLifeCycleFunc func = buffObj.model.onRemovedFunc[i];
                func.Execute(buffObj);
            }
        }
    }
}
