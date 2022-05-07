namespace LCSkill
{
    public interface IDamageServer
    {
        /// <summary>
        /// 计算伤害
        /// </summary>
        /// <param name="damageInfo"></param>
        /// <returns>本次伤害是否死亡</returns>
        public bool CalcDamage(AddDamageInfo damageInfo);
    }
}
