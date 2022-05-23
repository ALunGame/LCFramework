using LCECS.Core;
using System;

namespace Demo.Com
{
    public class WanderCom : BaseCom
    {
        [NonSerialized]
        public DirType WanderDir = DirType.None;

        [NonSerialized]
        public float WanderRange = -1;
    }
}
