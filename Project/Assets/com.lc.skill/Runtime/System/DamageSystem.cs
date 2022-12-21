using LCECS.Core;
using System;
using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// 处理所有的伤害请求
    /// </summary>
    public class DamageSystem : BaseSystem
    {
        protected override List<Type> RegContainListenComs()
        {
            return new List<Type>() { typeof(DamageCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            DamageCom damageCom = GetCom<DamageCom>(comList[0]);
            DealDamage(damageCom);
        }

        private void DealDamage(DamageCom damageCom)
        {
            if (damageCom.Damages == null || damageCom.Damages.Count <= 0)
                return;
            for (int i = 0; i < damageCom.Damages.Count; i++)
            {
                AddDamageInfo damageInfo = damageCom.Damages[i];
                //执行攻击者的Buff,OnHit函数
                ExecuteHurtFunc(damageInfo);
                //执行目标者的Buff,BeHurt函数
                ExecuteBeHurtFunc(damageInfo);
                //执行伤害
                bool isDead = SkillLocate.Damage.CalcDamage(damageInfo);
                //执行死亡的Buff函数
                if (isDead)
                {
                    //攻击者Kill
                    ExecuteKilledFunc(damageInfo);
                    //目标OnBeKilled
                    ExecuteBeKilledFunc(damageInfo);
                }
                //伤害结束，添加Buff
                for (int j = 0; j < damageInfo.model.addBuffs.Count; j++)
                {
                    AddBuffModel addBuff = damageInfo.model.addBuffs[j];
                    SkillLocate.Skill.CreateBuff(damageInfo.attacker, damageInfo.target, addBuff);
                }
                //移除
                damageCom.RemoveDamageInfo(damageInfo);
            }
        }

        #region 攻击者

        private void ExecuteHurtFunc(AddDamageInfo damageInfo)
        {
            if (damageInfo.attacker == null)
                return;
            //执行攻击者的Buff,OnHit函数
            for (int i = 0; i < damageInfo.attacker.Buffs.Count; i++)
            {
                BuffObj hitBuff = damageInfo.attacker.Buffs[i];
                if (hitBuff.model.onHurtFunc != null)
                {
                    for (int j = 0; j < hitBuff.model.onHurtFunc.Count; j++)
                    {
                        BuffHurtFunc func = hitBuff.model.onHurtFunc[j];
                        func.Execute(hitBuff, ref damageInfo, damageInfo.target);
                    }
                }
            }
        }

        private void ExecuteKilledFunc(AddDamageInfo damageInfo)
        {
            if (damageInfo.attacker == null)
                return;
            //执行攻击者的Buff,OnHit函数
            for (int i = 0; i < damageInfo.attacker.Buffs.Count; i++)
            {
                BuffObj killBuff = damageInfo.attacker.Buffs[i];
                if (killBuff.model.onKilledFunc != null)
                {
                    for (int j = 0; j < killBuff.model.onKilledFunc.Count; j++)
                    {
                        BuffKilledFunc func = killBuff.model.onKilledFunc[j];
                        func.Execute(killBuff, damageInfo, damageInfo.target);
                    }
                }
            }
        }

        #endregion

        #region 被攻击

        private void ExecuteBeHurtFunc(AddDamageInfo damageInfo)
        {
            if (damageInfo.target == null)
                return;
            //执行攻击者的Buff,OnHit函数
            for (int i = 0; i < damageInfo.target.Buffs.Count; i++)
            {
                BuffObj beHurtBuff = damageInfo.target.Buffs[i];
                if (beHurtBuff.model.onBeHurtFunc != null)
                {
                    for (int j = 0; j < beHurtBuff.model.onBeHurtFunc.Count; j++)
                    {
                        BuffBeHurtFunc func = beHurtBuff.model.onBeHurtFunc[j];
                        func.Execute(beHurtBuff, ref damageInfo, damageInfo.attacker);
                    }
                }
            }
        }

        private void ExecuteBeKilledFunc(AddDamageInfo damageInfo)
        {
            if (damageInfo.target == null)
                return;
            //执行攻击者的Buff,OnHit函数
            for (int i = 0; i < damageInfo.target.Buffs.Count; i++)
            {
                BuffObj beKillBuff = damageInfo.target.Buffs[i];
                if (beKillBuff.model.onBeKilledFunc != null)
                {
                    for (int j = 0; j < beKillBuff.model.onBeKilledFunc.Count; j++)
                    {
                        BuffBeKilledFunc func = beKillBuff.model.onBeKilledFunc[j];
                        func.Execute(beKillBuff, damageInfo, damageInfo.attacker);
                    }
                }
            }
        } 

        #endregion
    }
}
