using LCECS.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Com
{
    [Com(ViewName = "属性组件",GroupName = "Entity")]
    public class AttributeCom : BaseCom
    {
        public Dictionary<string, float> AttrDict = new Dictionary<string, float>();

        protected override void OnInit(GameObject go)
        {
        }
    }
}
