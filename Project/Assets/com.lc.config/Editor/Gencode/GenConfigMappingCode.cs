using UnityEditor;
using UnityEngine;
using LCToolkit;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace LCConfig
{
    public static class GenConfigMappingCode
    {
        //代码模板文件
        private const string CodeModelPath = "Assets/com.lc.config/Editor/Gencode/ConfigMappingTemplate.txt";
        //代码生成路径
        private const string CodeGenPath = "Assets/com.lc.config/Runtime/Config.cs";

        private const string RelaodCode = @"
            if(#NAME#!= null)
				#NAME#.Clear();";
        public static void GenCode()
        {
            string codeModeStr = AssetDatabase.LoadAssetAtPath<TextAsset>(CodeModelPath).text;
            string resStr = codeModeStr;

            //命名空间
            string usingNameStr = "";
            string cnfStr = "";
            string reloadValue = "";
            List<string> usingNames = new List<string>();
            foreach (var item in ReflectionHelper.GetChildTypes<IConfig>())
            {
                cnfStr += GenCnfCode(item);

                if (!usingNames.Contains(item.Namespace))
                {
                    usingNames.Add(item.Namespace);
                    string str = string.Format("using {0};\n", item.Namespace);
                    usingNameStr = usingNameStr + str;
                }

                //Reload
                string fileName = GenTBConfigCode.GetCodeClassFileName(item);
                string reloadStr = RelaodCode;
                reloadStr = Regex.Replace(reloadStr, "#NAME#", "_" + item.Name) + "\n";
                reloadValue += reloadStr;
            }
            resStr = Regex.Replace(resStr, "#USINGNAME#", usingNameStr);
            resStr = Regex.Replace(resStr, "#CNFSTR#", cnfStr);
            resStr = Regex.Replace(resStr, "#RELOADVALUE#", reloadValue);

            //生成
            IOHelper.WriteText(resStr, CodeGenPath);
        }

        private const string CnfCode = @"
        private static #CLASS# #NAME01# = null;
        /// <summary>
        /// #DISPLAYNAME#
        /// </summary>
        public static #CLASS# #NAME02#
        {
            get
            {
                if (#NAME01# == null)
                {
                    string jsonStr = LoadHelper.LoadString(""#NAME03#"");
                    List<#TYPE#> configs = LCJson.JsonMapper.ToObject<List<#TYPE#>>(jsonStr);
                    #NAME01# = new #CLASS#();
                    #NAME01#.AddConfig(configs);
                }
                return #NAME01#;
            }
        }";

        private static string GenCnfCode(Type cnfType)
        {
            string className = GenTBConfigCode.GetCodeClassName(cnfType);
            string classFileName = GenTBConfigCode.GetCodeClassFileName(cnfType);
            string displayName = cnfType.Name;
            if (AttributeHelper.TryGetTypeAttribute(cnfType, out ConfigAttribute attr))
                displayName = attr.DisplayName;

            string resStr = CnfCode;
            resStr = Regex.Replace(resStr, "#DISPLAYNAME#", displayName);
            resStr = Regex.Replace(resStr, "#CLASS#", className);
            resStr = Regex.Replace(resStr, "#NAME#", classFileName);
            resStr = Regex.Replace(resStr, "#TYPE#", cnfType.Name);
            resStr = Regex.Replace(resStr, "#NAME01#", "_"+cnfType.Name);
            resStr = Regex.Replace(resStr, "#NAME02#", cnfType.Name);
            resStr = Regex.Replace(resStr, "#NAME03#", ConfigDef.GetCnfNoExName(cnfType.Name));
            resStr += "\n";
            return resStr;
        }
    }
}