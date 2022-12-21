using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LCTimeline;
using LCJson;

namespace LCSkill.Serialize
{
    public class SkillSerializeHelp
    {
        //收集参数
        public static Dictionary<string, object> CollectParams(ClipModel clipData,params string[] ignoreFileNames)
        {
            Dictionary<string, object> paramDict = new Dictionary<string, object>();
            foreach (FieldInfo fInfo in clipData.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance))
            {
                JsonIgnore ignoreAttr = (JsonIgnore)fInfo.GetCustomAttribute(typeof(JsonIgnore));
                if (ignoreAttr != null)
                    continue;

                if (ignoreFileNames != null && ignoreFileNames.Contains(fInfo.Name))
                    continue;

                object obj = fInfo.GetValue(clipData);
                if (obj == null)
                    continue;

                paramDict.Add(fInfo.Name, obj);
            }
            return paramDict;
        }
    }
}