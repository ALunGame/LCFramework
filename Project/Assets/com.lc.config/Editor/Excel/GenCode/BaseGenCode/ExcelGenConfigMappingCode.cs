using System.Collections.Generic;
using System.Text.RegularExpressions;
using com.lc.config.Editor.Excel.Core;
using LCToolkit;

namespace LCConfig.Excel.GenCode.CommonExcel
{
    public class ExcelGenConfigMappingCode
    {
        private const string FileStr = @"
using System;
using System.Collections.Generic;
using IAFramework;
using MemoryPack;
#USINGNAME#

namespace LCConfig
{
    public static class Config
    {
        #CNFSTR#

        public static void Reload()
        {
#RELOADVALUE#
        }
    }
}";
        
        private const string RelaodCode = @"
            if(#NAME#!= null)
				#NAME#.Clear();";
        
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
                    Byte[] byteArray = GameContext.Asset.LoadBytes(""#NAME03#"");
                    List<#TYPE#> configs = MemoryPackSerializer.Deserialize<List<#TYPE#>>(byteArray);
                    #NAME01# = new #CLASS#();
                    #NAME01#.AddConfig(configs);
                }
                return #NAME01#;
            }
        }";


        public void GenMappingCode(List<GenConfigInfo> pInfos)
        {
            string resStr = FileStr;
            
            //命名空间
            string usingNameStr = "";
            string cnfStr = "";
            string reloadValue = "";
            List<string> usingNames = new List<string>();
            foreach (var item in pInfos)
            {
                cnfStr += GenCnfCode(item);

                if (!usingNames.Contains(item.nameSpace))
                {
                    usingNames.Add(item.nameSpace);
                    string str = string.Format("using {0};\n", item.nameSpace);
                    usingNameStr = usingNameStr + str;
                }

                //Reload
                string reloadStr = RelaodCode;
                reloadStr = Regex.Replace(reloadStr, "#NAME#", "_" + item.className) + "\n";
                reloadValue += reloadStr;
            }
            resStr = Regex.Replace(resStr, "#USINGNAME#", usingNameStr);
            resStr = Regex.Replace(resStr, "#CNFSTR#", cnfStr);
            resStr = Regex.Replace(resStr, "#RELOADVALUE#", reloadValue);

            //生成
            IOHelper.WriteText(resStr, ExcelReadSetting.RunningRootPath+"/Config.cs");
        }
        
        private string GenCnfCode(GenConfigInfo pInfo)
        {
            string className = pInfo.className;
            string classFileName = pInfo.GetFileName();

            string resStr = CnfCode;
            resStr = Regex.Replace(resStr, "#DISPLAYNAME#", pInfo.comment);
            resStr = Regex.Replace(resStr, "#CLASS#",  ExcelGenCode.Tb+className);
            resStr = Regex.Replace(resStr, "#TYPE#", pInfo.className);
            resStr = Regex.Replace(resStr, "#NAME01#", "_"+pInfo.className);
            resStr = Regex.Replace(resStr, "#NAME02#", pInfo.className);
            resStr = Regex.Replace(resStr, "#NAME03#", pInfo.GetFileNameNoExName());
            resStr += "\n";
            return resStr;
        }
    }
}