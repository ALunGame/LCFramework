using Demo.Com;
using LCMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    public static partial class ActorExtend
    {
        public static int GetDirValue(this Actor pActor)
        {
            return pActor.Trans.GetDir() == DirType.Right ? 1 : -1;
        }

        public static DirType GetDir(this Actor pActor)
        {
            return pActor.Trans.GetDir();
        }
    }
}
