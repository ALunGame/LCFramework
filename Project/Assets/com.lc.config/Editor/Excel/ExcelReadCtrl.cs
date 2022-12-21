
using System.Collections.Generic;
using System.IO;
using com.lc.config.Editor.Excel.Core;
using LCConfig.Excel.Export;
using LCConfig.Excel.GenCode;
using LCConfig.Excel.GenCode.CommonExcel;
using LCConfig.Excel.GenCode.Property;
using LCToolkit;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

namespace LCConfig.Excel
{
    public static class ExcelReadCtrl
    {
        private static List<GenConfigInfo> ReadBaseExcel(bool pNeedGenCode = false)
        {
            //导出自定义枚举
            EnumExcelGenCode enumExcelGenCode = new EnumExcelGenCode("基础配置/GenEnumConfig.xlsx");
            List<EnumInfo> enumInfos = pNeedGenCode ? enumExcelGenCode.GenAllClass() : enumExcelGenCode.GetAllEnum();
            ExcelPropertyMap.SetEnumInfos(enumInfos);

            //导出自定义类
            CustomClassExcelGenCode customExcelGenCode = new CustomClassExcelGenCode("基础配置/GenClassConfig.xlsx");
            List<ClassInfo> classInfos =  pNeedGenCode ? customExcelGenCode.GenAllClass() : customExcelGenCode.GetAllClass();
            ExcelPropertyMap.SetClassInfos(classInfos);

            //导出所有表格Json
            GenConfigExcelRead genConfigExcelRead = new GenConfigExcelRead("基础配置/GenConfig.xlsx");
            List<GenConfigInfo> configs = genConfigExcelRead.ReadAllConfigs();
            return configs;
        }


        [MenuItem("表格/生成代码")]
        public static void GenCode()
        {
            List<GenConfigInfo> configs = ReadBaseExcel(true);
            ExcelGenCode genCode = new ExcelGenCode();
            genCode.Clear();
            foreach (GenConfigInfo info in configs)
            {
                genCode.GenCommonExcelCode(info);
            }
            genCode.Save();
            
            //生成映射代码
            ExcelGenConfigMappingCode mappingCode = new ExcelGenConfigMappingCode();
            mappingCode.GenMappingCode(configs);
            
            AssetDatabase.Refresh();
        }
        
        [MenuItem("表格/导出所有")]
        public static void ExportAll()
        {
            List<GenConfigInfo> configs = ReadBaseExcel();
            ExcelExportSystem exportSystem = new ExcelExportSystem();
            exportSystem.ExportAll(configs);
            
            AssetDatabase.Refresh();
        }
        
        [MenuItem("表格/设置")]
        public static void CreateSetting()
        {
            ExcelReadSetting.CreateSetting();
        }

        [MenuItem("表格/打开表格目录 &e")]
        public static void OpenExcelRootPath()
        {
            MiscHelper.OpenDirectory(ExcelReadSetting.Setting.ConfigRootPath);
        }
        
        public static List<T> GetConfig<T>() where T:new()
        {
            List<GenConfigInfo> configs = ReadBaseExcel();
            
            string className = typeof(T).Name;
            GenConfigInfo selInfo = null;
            foreach (GenConfigInfo configInfo in configs)
            {
                if (configInfo.className == className)
                {
                    selInfo = configInfo;
                    break;
                }
            }

            if (selInfo == null)
            {
                Debug.LogError($"获得配置失败，没有对应配置>>{className}");
                return null;
            }
            
            ExcelExportSystem exportSystem = new ExcelExportSystem();
            selInfo.isRead = true;
            return exportSystem.Export<T>(selInfo);
        }
    }
}