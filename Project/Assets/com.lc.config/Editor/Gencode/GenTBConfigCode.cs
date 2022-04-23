using LCToolkit;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace LCConfig
{
    public static class GenTBConfigCode
    {
        //代码模板文件
        private const string CodeModelPath = "Assets/com.lc.config/Editor/Gencode/TBConfigCodeTemplate.txt";
        //代码生成路径
        private const string CodeGenPath = "Assets/com.lc.config/Runtime/Gen/";

        //引用命名空间
        private const string USINGName = "#USINGNAME#";
        //提示名
        private const string DisplayName = "#DISPLAYNAME#";
        //配置名
        private const string ConfigName = "#CONFIGNAME#";
        //目标类
        private const string ClassName = "#CLASSNAME#";
        //键名
        private const string KeyName = "#KEY#";
        //值名
        private const string ValueName = "#VALUE#";

        //获得生成代码类名
        public static string GetCodeClassName(Type cnfType)
        {
            return "TB" + cnfType.Name;
        }

        //获得生成代码字段名
        public static string GetCodeClassFileName(Type cnfType)
        {
            return "tB" + cnfType.Name;
        }

        [MenuItem("配置/测试")]
        public static void TestGenCode()
        {
            GenCode(typeof(TestConfig));
            GenConfigMappingCode.GenCode();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }

        public static void GenCode(Type cnfType)
        {
            string codeModeStr = AssetDatabase.LoadAssetAtPath<TextAsset>(CodeModelPath).text;

            string resStr     = codeModeStr;

            //注释以及类名
            string displayName = cnfType.Name;
            string configName  = GetCodeClassName(cnfType);
            if (AttributeHelper.TryGetTypeAttribute(cnfType, out ConfigAttribute attr))
                displayName = attr.DisplayName;
            resStr = Regex.Replace(resStr, DisplayName, displayName);
            resStr = Regex.Replace(resStr, ConfigName, configName);
            resStr = Regex.Replace(resStr, ClassName, cnfType.Name);

            //命名空间
            string usingNameStr = "";
            List<string> usingNames = new List<string>();
            //收集配置键
            List<FieldInfo> keyFields = new List<FieldInfo>();
            foreach (var item in ReflectionHelper.GetFieldInfos(cnfType))
            {
                if (AttributeHelper.TryGetFieldAttribute(item, out ConfigKeyAttribute keyAttr))
                {
                    keyFields.Add(item);
                }
                if (!usingNames.Contains(item.FieldType.Namespace))
                {
                    usingNames.Add(item.FieldType.Namespace);
                    string str = string.Format("using {0};\n", item.FieldType.Namespace);
                    usingNameStr = usingNameStr + str;
                }
            }
            resStr = Regex.Replace(resStr, USINGName, usingNameStr);
            keyFields.Sort((x, y) => {
                AttributeHelper.TryGetFieldAttribute(x, out ConfigKeyAttribute xKeyAttr);
                AttributeHelper.TryGetFieldAttribute(y, out ConfigKeyAttribute yKeyAttr);

                if (xKeyAttr.keyIndex == yKeyAttr.keyIndex)
                    return 1;
                else if (xKeyAttr.keyIndex > yKeyAttr.keyIndex)
                    return 1;
                else if (xKeyAttr.keyIndex < yKeyAttr.keyIndex)
                    return -1;
                else
                    return 1;
            });

            //类键与值
            string keyName = GenKeyStr(keyFields[0]);
            string valueName = GenValueStr(keyFields,cnfType);
            resStr = Regex.Replace(resStr, KeyName, keyName);
            resStr = Regex.Replace(resStr, ValueName, valueName);

            //函数
            resStr = GenFunc1(resStr, keyFields, cnfType);
            resStr = GenFunc2(resStr, keyFields, cnfType);

            //生成
            string filePath = CodeGenPath + configName + ".cs";
            IOHelper.WriteText(resStr, filePath);
        }

        #region FUNC1

        private static string GenFunc1(string resStr, List<FieldInfo> keyFields, Type cnfType)
        {
            //函数参数
            string func1Param = "";
            for (int i = 0; i < keyFields.Count; i++)
            {
                FieldInfo fieldInfo = keyFields[i];
                func1Param = func1Param + (GenKeyStr(fieldInfo) + " key" + (i + 1) + ",") + " ";
            }
            func1Param = func1Param + cnfType.Name + " config";
            resStr = Regex.Replace(resStr, "#FUNC1PARAM#", func1Param);

            //函数提示
            string func1TipStr = GenFuc1TipStr(keyFields, cnfType);
            resStr = Regex.Replace(resStr, "#FUNC1TIP#", func1TipStr);

            //函数内容
            string fun1ValueStr = GenFunc1Value(keyFields, cnfType);
            resStr = Regex.Replace(resStr, "#FUNC1VALUE#", fun1ValueStr);
            return resStr;
        }

        public static string GenFuc1TipStr(List<FieldInfo> keyFields, Type cnfType)
        {
            string resValue = "";
            for (int i = 0; i < keyFields.Count; i++)
            {
                string formatStr = "\t\t/// <param name=\"#KEY#\">#NAME#</param>";
                FieldInfo fieldInfo = keyFields[i];
                if (AttributeHelper.TryGetFieldAttribute(fieldInfo, out ConfigKeyAttribute keyAttr))
                {
                    formatStr = Regex.Replace(formatStr, "#KEY#", "key" + (i + 1));
                    formatStr = Regex.Replace(formatStr, "#NAME#", keyAttr.DisplayName);
                    resValue += formatStr;
                }
                if (i != keyFields.Count - 1)
                {
                    resValue += "\n";
                }
            }
            return resValue;
        }

        public static string GenFunc1Value(List<FieldInfo> keyFields, Type cnfType)
        {
            string genValueStr(int index, List<FieldInfo> keyFields, Type cnfType)
            {
                if (index == keyFields.Count - 1)
                {
                    return "config";
                }

                string resValue = "new ";
                for (int i = index + 1; i < keyFields.Count; i++)
                {
                    string formatStr = "Dictionary<{0}, ";
                    FieldInfo tmpField = keyFields[i];
                    resValue += string.Format(formatStr, GenKeyStr(tmpField));
                }
                resValue += cnfType.Name;
                for (int i = index; i < keyFields.Count - 1; i++)
                {
                    resValue += ">";

                }
                resValue += "()";
                return resValue;
            }

            string currStr = "";
            for (int i = 0; i < keyFields.Count; i++)
            {
                string valueStr = @"
            if (!this#VALUE1#ContainsKey(#VALUE2#))
            {
                this#VALUE1#Add(#VALUE2#, #VALUE3#);
            }";

                string value1 = "";
                for (int j = 1; j <= i; j++)
                {
                    value1 += string.Format("[{0}]", "key" + j);
                }
                value1 += ".";

                string value2 = string.Format("{0}", "key" + (i + 1));

                valueStr = Regex.Replace(valueStr, "#VALUE1#", value1);
                valueStr = Regex.Replace(valueStr, "#VALUE2#", value2);
                valueStr = Regex.Replace(valueStr, "#VALUE3#", genValueStr(i, keyFields, cnfType));
                currStr += valueStr;
            }

            return currStr;
        }

        #endregion

        #region FUNC2

        private static string GenFunc2(string resStr, List<FieldInfo> keyFields, Type cnfType)
        {
            //函数参数
            string func2Param = "";
            for (int i = 0; i < keyFields.Count; i++)
            {
                FieldInfo fieldInfo = keyFields[i];
                func2Param = func2Param + string.Format("config.{0}", fieldInfo.Name) + ", ";
            }
            func2Param = func2Param + "config";
            resStr = Regex.Replace(resStr, "#FUNC2PARAM#", func2Param);
            return resStr;
        }


        #endregion

        private static string GenKeyStr(FieldInfo fieldInfo)
        {
            return fieldInfo.FieldType.Name;
        }

        private static string GenValueStr(List<FieldInfo> keyFields, Type cnfType)
        {
            if (keyFields.Count <= 1)
                return cnfType.Name;
            string resValue = "";
            for (int i = 1; i < keyFields.Count; i++)
            {
                FieldInfo tmpField = keyFields[i];
                string formatStr = "Dictionary<{0}, ";
                resValue += string.Format(formatStr, GenKeyStr(tmpField));
            }
            resValue += cnfType.Name;
            for (int i = 1; i < keyFields.Count; i++)
            {
                resValue += ">";
            }
            return resValue;
        }
    }
}
