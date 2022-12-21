using System.Collections.Generic;
using System.Text.RegularExpressions;
using LCConfig.Excel.GenCode.CommonExcel;

namespace LCConfig.Excel.GenCode.Property
{
    internal class IntProperty : BaseProperty
    {
        public override string TypeName { get => "int"; }
        public override string NameSpace { get => "using System;"; }
        public override bool CanBeKey { get => true; }

        public override bool CanCatch(string pValue)
        {
            if (string.IsNullOrEmpty(pValue))
            {
                return true;
            }
            return int.TryParse(pValue, out var t);
        }

        public override object Parse(string pValue)
        {
            if (string.IsNullOrEmpty(pValue))
            {
                return 0;
            }
            return int.Parse(pValue);
        }

        public override string CreateExportStr(string pExportName, string pRowValueName)
        {
            string codeStr = "\t\t\t\t#NAME#.#PRONAME# = (#ValueTypeName#)GetProp(pProps,\"#PRONAME#\").Parse(propDict[\"#PRONAME#\"][0]);";
            codeStr = Regex.Replace(codeStr, "#NAME#", pExportName);
            codeStr = Regex.Replace(codeStr, "#PRONAME#", name);
            codeStr = Regex.Replace(codeStr, "#ValueTypeName#", TypeName);
            return codeStr;
        }
    }

    internal class FloatProperty : BaseProperty
    {
        public override string TypeName { get => "float"; }
        public override string NameSpace { get => "using System;"; }
        public override bool CanBeKey { get => true; }
        public override bool CanCatch(string pValue)
        {
            if (string.IsNullOrEmpty(pValue))
            {
                return true;
            }
            return float.TryParse(pValue, out var t);
        }

        public override object Parse(string pValue)
        {
            if (string.IsNullOrEmpty(pValue))
            {
                return 0;
            }
            return float.Parse(pValue);
        }

        public override string CreateExportStr(string pExportName, string pRowValueName)
        {
            string codeStr = "\t\t\t\t#NAME#.#PRONAME# = (#ValueTypeName#)GetProp(pProps,\"#PRONAME#\").Parse(propDict[\"#PRONAME#\"][0]);";
            codeStr = Regex.Replace(codeStr, "#NAME#", pExportName);
            codeStr = Regex.Replace(codeStr, "#PRONAME#", name);
            codeStr = Regex.Replace(codeStr, "#ValueTypeName#", TypeName);
            return codeStr;
        }
    }

    internal class BoolProperty : BaseProperty
    {
        public override string TypeName { get => "bool"; }
        public override string NameSpace { get => "using System;"; }
        public override bool CanBeKey { get => true; }
        public override bool CanCatch(string pValue)
        {
            if (string.IsNullOrEmpty(pValue))
            {
                return true;
            }
            return bool.TryParse(pValue, out var t);
        }

        public override object Parse(string pValue)
        {
            if (string.IsNullOrEmpty(pValue))
            {
                return false;
            }
            return bool.Parse(pValue);
        }

        public override string CreateExportStr(string pExportName, string pRowValueName)
        {
            string codeStr = "\t\t\t\t#NAME#.#PRONAME# = (#ValueTypeName#)GetProp(pProps,\"#PRONAME#\").Parse(propDict[\"#PRONAME#\"][0]);";
            codeStr = Regex.Replace(codeStr, "#NAME#", pExportName);
            codeStr = Regex.Replace(codeStr, "#PRONAME#", name);
            codeStr = Regex.Replace(codeStr, "#ValueTypeName#", TypeName);
            return codeStr;
        }
    }

    internal class StringProperty : BaseProperty
    {
        public override string TypeName { get => "string"; }
        public override string NameSpace { get => "using System;"; }
        public override bool CanBeKey { get => true; }
        public override bool CanCatch(string pValue)
        {
            return true;
        }

        public override object Parse(string pValue)
        {
            return pValue;
        }

        public override string CreateExportStr(string pExportName, string pRowValueName)
        {
            string codeStr = "\t\t\t\t#NAME#.#PRONAME# = (#ValueTypeName#)GetProp(pProps,\"#PRONAME#\").Parse(propDict[\"#PRONAME#\"][0]);";
            codeStr = Regex.Replace(codeStr, "#NAME#", pExportName);
            codeStr = Regex.Replace(codeStr, "#PRONAME#", name);
            codeStr = Regex.Replace(codeStr, "#ValueTypeName#", TypeName);
            return codeStr;
        }
    }
    
    internal class EnumProperty : BaseProperty
    {
        public EnumInfo enumInfo;
        
        public override string TypeName { get => enumInfo.enumName; }
        public override string NameSpace { get => $"using {enumInfo.nameSpace};" ; }
        public override bool CanBeKey { get => true; }
        
        public override bool CanCatch(string pValue)
        {
            for (int i = 0; i < enumInfo.fields.Count; i++)
            {
                EnumFieldInfo fieldInfo = enumInfo.fields[i];
                if (fieldInfo.name == pValue || fieldInfo.exName == pValue)
                {
                    return true;
                }
            }
            return false;
        }

        public override object Parse(string pValue)
        {
            for (int i = 0; i < enumInfo.fields.Count; i++)
            {
                EnumFieldInfo fieldInfo = enumInfo.fields[i];
                if (fieldInfo.name == pValue || fieldInfo.exName == pValue)
                {
                    return fieldInfo.value;
                }
            }
            return 0;
        }

        public override string CreateExportStr(string pExportName, string pRowValueName)
        {
            string codeStr = "\t\t\t\t#NAME#.#PRONAME# = (#ValueTypeName#)GetProp(pProps,\"#PRONAME#\").Parse(propDict[\"#PRONAME#\"][0]);";
            codeStr = Regex.Replace(codeStr, "#NAME#", pExportName);
            codeStr = Regex.Replace(codeStr, "#PRONAME#", name);
            codeStr = Regex.Replace(codeStr, "#ValueTypeName#", TypeName);
            return codeStr;
        }
    }
}