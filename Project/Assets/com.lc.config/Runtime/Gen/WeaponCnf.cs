using MemoryPack;
using System;

namespace Cnf
{
    
    /// <summary>
    /// 武器基础属性
    /// </summary>
    [MemoryPackable]
    public partial class WeaponCnf
    {
        
        /// <summary>
        /// 武器Id
        /// </summary>
        public int id;

        /// <summary>
        /// 武器名字
        /// </summary>
        public string name;

        /// <summary>
        /// 预制体
        /// </summary>
        public string prefab;

        /// <summary>
        /// 使用动画
        /// </summary>
        public string useAnim;

        /// <summary>
        /// 使用武器时释放的技能Id
        /// </summary>
        public int useSkillId;

    }

}

