using System.Collections.Generic;
using System.Linq;
using LCConfig.Excel;
using LCConfig.Excel.GenCode;
using LCJson;
using LCToolkit;
using Newtonsoft.Json;
using OfficeOpenXml;
using UnityEngine;

namespace com.lc.config.Editor.Excel.Core
{
    public class GenConfigInfo
    {
        public string nameSpace;
        public string className;
        public List<string> filePaths = new List<string>();
        public string sheetName;
        public string comment;

        public bool isRead = false;

        public object resValue;
        public void SaveJson(object value)
        {
            resValue = value;
            if (isRead)
            {
                return;
            }
            string savePath = $"{ExcelReadSetting.Setting.GenJsonRootPath}/{GetFileName()}";
            
            string jsonStr = JsonMapper.ToJson(value);
            IOHelper.WriteText(jsonStr,savePath);
            Debug.Log($"配置导出成功>>>{className}-->{savePath}");
        }

        public string GetFileName()
        {
            return $"{ExcelGenCode.Tb}{className}{ExcelReadSetting.Setting.GenJsonExName}";
        }
        
        public string GetFileNameNoExName()
        {
            return $"{ExcelGenCode.Tb}{className}";
        }
    }
    
    public class GenConfigExcelRead
    {
        private string excelPath;
        
        public GenConfigExcelRead(string pExcelPath)
        {
            excelPath = pExcelPath;
        }

        public List<GenConfigInfo> ReadAllConfigs()
        {
            ExcelPackage tPackage = null;
            ExcelWorksheet sheet = ExcelReader.ReadAllSheets(excelPath,out tPackage)[0];

            List<GenConfigInfo> infos = new List<GenConfigInfo>();
            
            //最大行
            int _MaxRowNum = sheet.Dimension.End.Row;
            //最小行
            int _MinRowNum = sheet.Dimension.Start.Row;
            
            //最大列
            int _MaxColumnNum = sheet.Dimension.End.Column;
            //最小列
            int _MinColumnNum = sheet.Dimension.Start.Column;

            for (int row = 2; row <= _MaxRowNum; row++)
            {
                string firsetValue = ExcelReader.GetCellValue(sheet, row, 1);
                if (firsetValue.Contains("##"))
                    continue;
               
                string nameSpaceName = ExcelReader.GetCellValue(sheet, row, 2);
                if (string.IsNullOrEmpty(nameSpaceName))
                    continue;
                
                GenConfigInfo info = new GenConfigInfo();
                info.nameSpace = nameSpaceName;
                info.className = ExcelReader.GetCellValue(sheet, row, 3);
                info.filePaths = ExcelReader.GetCellValue(sheet, row, 4).Split(",").ToList();
                info.sheetName = ExcelReader.GetCellValue(sheet, row, 5);
                info.comment = ExcelReader.GetCellValue(sheet, row, 6);
                
                infos.Add(info);
            }

            tPackage.Dispose();
            return infos;
        }
    }
}