namespace LCSkill
{
    public interface IDamageServer
    {
        /// <summary>
        /// 添加伤害
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <param name="target">目标</param>
        /// <param name="damage">伤害</param>
        /// <param name="angle">伤害角度</param>
        void AddDamage(SkillCom attacker, SkillCom target, DamageModel damage, float angle = 0);

        /// <summary>
        /// 计算伤害
        /// </summary>
        /// <param name="damageInfo"></param>
        /// <returns>本次伤害是否死亡</returns>
        bool CalcDamage(AddDamageInfo damageInfo);
    }
}
