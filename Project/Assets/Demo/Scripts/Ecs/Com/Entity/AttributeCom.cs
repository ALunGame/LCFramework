using LCECS.Core.ECS;
using System.Collections.Generic;
using UnityEngine;
using LCConfig;

namespace Demo.Com
{
    [Com(ViewName = "属性组件",GroupName = "Entity")]
    public class AttributeCom : BaseCom
    {
        public Dictionary<string, float> AttrDict = new Dictionary<string, float>();

        protected override void OnInit(GameObject go)
        {
            Dictionary<string, string> attrDict = LCConfigLocate.GetConfigItemDataDict("BaseAttr", EntityCnfId.ToString());
            foreach (var item in attrDict)
            {
                AttrDict.Add(item.Key, float.Parse(item.Value));
            }
        }
    }
}
