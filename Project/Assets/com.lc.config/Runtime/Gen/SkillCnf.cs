using MemoryPack;
using System;

namespace Cnf
{
    
    /// <summary>
    /// 技能信息
    /// </summary>
    [MemoryPackable]
    public partial class SkillCnf
    {
        
        /// <summary>
        /// 技能Id
        /// </summary>
        public int id;

        /// <summary>
        /// 技能名
        /// </summary>
        public string name;

        /// <summary>
        /// 技能CD
        /// </summary>
        public string cd;

        /// <summary>
        /// 技能timeline名
        /// </summary>
        public string timeline;

    }

}

