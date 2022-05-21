using LCECS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Com
{
    [Com(GroupName = "Bullet", ViewName = "子弹组件")]
    public class BulletCom : BaseCom
    {
        //发射的实体Id
        [ComValue]
        public int ShotEntityId;
    }
}
