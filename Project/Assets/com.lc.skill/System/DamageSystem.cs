using LCECS.Core;
using System;
using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// 处理所有的伤害请求
    /// </summary>
    [System(InFixedUpdate = true)]
    public class DamageSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
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
                if (damageInfo.attacker != null)
                {
                    for (int j = 0; j < damageInfo.attacker.Buffs.Count; j++)
                    {
                        BuffObj hitBuff = damageInfo.attacker.Buffs[j];
                        BuffHitFunc.ExecuteFunc(hitBuff.model.onHitFunc, hitBuff, ref damageInfo, damageInfo.target);
                    }
                }
                //执行目标者的Buff,BeHurt函数
                if (damageInfo.target != null)
                {
                    for (int j = 0; j < damageInfo.target.Buffs.Count; j++)
                    {
                        BuffObj beHurtBuff = damageInfo.target.Buffs[j];
                        BuffBeHurt.ExecuteFunc(beHurtBuff.model.onBeHurtFunc, beHurtBuff, ref damageInfo, damageInfo.attacker);
                    }
                }
                //执行伤害
                bool isDead = DamageCalcFunc.ExecuteDamage(damageInfo.attacker, damageInfo, damageInfo.target);
                //执行死亡的Buff函数
                if (isDead)
                {
                    //攻击者Kill
                    for (int j = 0; j < damageInfo.attacker.Buffs.Count; j++)
                    {
                        BuffObj killBuff = damageInfo.attacker.Buffs[j];
                        BuffKillFunc.ExecuteFunc(killBuff.model.onKillFunc, killBuff, damageInfo, damageInfo.target);
                    }
                    //目标OnBeKilled
                    for (int j = 0; j < damageInfo.target.Buffs.Count; j++)
                    {
                        BuffObj beKillBuff = damageInfo.target.Buffs[j];
                        BuffBeKilledFunc.ExecuteFunc(beKillBuff.model.onBeKilledFunc, beKillBuff, damageInfo, damageInfo.attacker);
                    }
                }
                //伤害结束，添加Buff
                for (int j = 0; j < damageInfo.addBuffs.Count; j++)
                {
                    AddBuffInfo addBuffInfo = damageInfo.addBuffs[j];
                    SkillCom buffTarget = addBuffInfo.target;
                    if (buffTarget != null)
                    {
                        buffTarget.AddBuff(addBuffInfo);
                    }
                }
            }
        }
    }
}
