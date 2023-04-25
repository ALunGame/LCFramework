using System.Collections.Generic;
using LCToolkit;

namespace LCGAS
{
    public class AttributeName
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string Name = "";

#if UNITY_EDITOR
        private static List<string> attrNameList = new List<string>();
        public List<string> GetAllAttrNames()
        {
            if (attrNameList.IsLegal())
            {
                return attrNameList;
            }
            foreach (var item in LCToolkit.ReflectionHelper.GetChildTypes<AttributeValue>())
            {
                if (item.IsAbstract)
                    continue;
                AttributeValue value = LCToolkit.ReflectionHelper.CreateInstance(item) as AttributeValue;
                if (attrNameList.Contains(value.Name))
                {
                    LCGAS.GASLocate.Log.LogError("属性名重复",value.Name);
                    continue;
                }
                attrNameList.Add(value.Name);
            }
            return attrNameList;
        }
#endif
    }
}