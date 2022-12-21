using System.Collections.Generic;
using com.lc.config.Editor.Excel.Core;
using LCConfig.Excel.GenCode.CommonExcel;
using LCConfig.Excel.GenCode.Property;
using LCToolkit;
using OfficeOpenXml;
using UnityEngine;

namespace LCConfig.Excel.GenCode
{
    internal class ExcelGenCode
    {
        public const string Tb = "Tb";

        private string exportCodeStr;
        private string usingStrs = "\n";
        private List<string> usingCheckList = new List<string>();

        public void Init()
        {
            Clear();
        }

        public void Save()
        {
            exportCodeStr = exportCodeStr.Insert(exportCodeStr.IndexOf("#region AutoGenUsing")+"#region AutoGenUsing".Length, usingStrs);
            string exprotCodePath = GetExportCodePath();
            IOHelper.WriteText(exportCodeStr,exprotCodePath);
        }
        
        public void GenCommonExcelCode(GenConfigInfo pInfo)
        {
            ExcelPackage tPackage = null;

            if (!usingCheckList.Contains(pInfo.nameSpace))
            {
                usingStrs += "using "+pInfo.nameSpace + ";\n";
                usingCheckList.Add(pInfo.nameSpace);
            }
            
            List<ExcelWorksheet> sheets = new List<ExcelWorksheet>();
            foreach (string filePath in pInfo.filePaths)
                sheets.AddRange(ExcelReader.ReadAllSheets(filePath,out tPackage, pInfo.sheetName));
                
            List<BaseProperty> props = ExcelGenCode.GetPropsByCommonExcel(sheets[0], out var propDict);
            foreach (BaseProperty prop in props)
            {
                if (!usingCheckList.Contains(prop.NameSpace))
                {
                    usingStrs += prop.NameSpace + "\n";
                    usingCheckList.Add(prop.NameSpace);
                }
            }
                
            ExcelGenTbClassCode tbGen = new ExcelGenTbClassCode(pInfo.nameSpace, pInfo.className);
            CommonExcelGenClassCode classGen = new CommonExcelGenClassCode(pInfo.nameSpace, pInfo.className);
            ExcelGenExportCode exportGen = new ExcelGenExportCode(pInfo.className);

            string tbStr     = tbGen.GenTbCode(props);

            string classStr  = classGen.GenClassCode(props,pInfo);

            string exportStr = exportGen.GenExportCode(sheets, props, propDict);
            exportCodeStr = exportCodeStr.Insert(exportCodeStr.IndexOf("#region AutoRegExportFunc")+ "#region AutoRegExportFunc".Length, $"\n\t\t\texportFuncDict.Add(\"{pInfo.className}\",Export_{pInfo.className});\n");
            exportCodeStr = exportCodeStr.Insert(exportCodeStr.IndexOf("#region AutoGenCode")+ "#region AutoGenCode".Length, exportStr);
            
            IOHelper.WriteText(tbStr,ExcelReadSetting.Setting.GenCodeRootPath + "/Tb" +pInfo.className+".cs");
            IOHelper.WriteText(classStr,ExcelReadSetting.Setting.GenCodeRootPath + "/" +pInfo.className+".cs");
            
            tPackage.Dispose();
        }

        public void Clear()
        {
            string exprotCodePath = GetExportCodePath();
            string readStr = IOHelper.ReadText(exprotCodePath);

            exportCodeStr = RemMidStrEx(readStr, "#region AutoGenUsing", "#endregion AutoGenUsing");
            exportCodeStr = RemMidStrEx(exportCodeStr, "#region AutoRegExportFunc", "#endregion AutoRegExportFunc");
            exportCodeStr = RemMidStrEx(exportCodeStr, "#region AutoGenCode", "#endregion AutoGenCode");
        }

        public static string RemMidStrEx(string sourse, string startstr, string endstr)
        {
            string result = string.Empty;
            int startindex, endindex;
            
            startindex = sourse.IndexOf(startstr);
            if (startindex == -1)
                return result;
            string startStr = sourse.Substring(0,startindex + startstr.Length);

            endindex = sourse.IndexOf(endstr);
            if (endindex == -1)
                return result;
            string endStr = sourse.Substring(endindex);
            return startStr + endStr;
        }
        
        private string GetExportCodePath()
        {
            string exprotCodePath = ExcelReadSetting.EditorRootPath + "/Excel/Export/ExcelExportModule.cs";
            return exprotCodePath;
        }

        public static List<BaseProperty> GetPropsByCommonExcel(ExcelWorksheet pSheet,out Dictionary<string, List<int>> outPropColDict)
        {
            outPropColDict = null;
            if (pSheet == null || pSheet.Dimension == null)
            {
                return null;
            }
            //最大行
            int _MaxRowNum = pSheet.Dimension.End.Row;
            //最小行
            int _MinRowNum = pSheet.Dimension.Start.Row;
            
            //最大列
            int _MaxColumnNum = pSheet.Dimension.End.Column;
            //最小列
            int _MinColumnNum = pSheet.Dimension.Start.Column;

            List<string> keyPropNames = new List<string>();
            List<string> arrayProps = new List<string>();
            //字段名和列
            Dictionary<string, List<int>> propColDict = GetPropColDict(pSheet,out keyPropNames,out arrayProps);
            outPropColDict = propColDict;

            //是否有指定类型
            bool hasCoustomType = false;
            string checkType = ExcelReader.GetCellValue(pSheet, 2, 1).ToString();
            if (!string.IsNullOrEmpty(checkType) && checkType=="##type")
            {
                hasCoustomType = true;
            }
            else
            {
                hasCoustomType = false;
            }
            
            bool hasComment = false;
            string checkComment = ExcelReader.GetCellValue(pSheet,3, 1);
            if (!string.IsNullOrEmpty(checkComment) && checkComment=="##comment")
            {
                hasComment = true;
            }
            else
            {
                hasComment = false;
            }
            
            List<BaseProperty> propList = new List<BaseProperty>();
            foreach (string propName in propColDict.Keys)
            {
                List<int> valueColList = propColDict[propName];
                int col = valueColList[0];

                string typeName = null;
                if (hasCoustomType)
                {
                    string tName = ExcelReader.GetCellValue(pSheet, 2, col).ToString();
                    if (!string.IsNullOrEmpty(tName))
                        typeName = tName;
                }
                
                //单值
                if (valueColList.Count == 1 && !arrayProps.Contains(propName))
                {
                    //指定类型
                    if (typeName != null)
                    {
                        if (ExcelPropertyMap.CheckHasProperty(typeName))
                        {
                            BaseProperty prop = ExcelPropertyMap.GetPropertyByTypeName(typeName);
                            prop.name  = propName;
                            prop.isKey = keyPropNames.Contains(propName);
                            if (hasComment)
                            {
                                prop.comment = ExcelReader.GetCellValue(pSheet, 3, col);
                            }
                            propList.Add(prop);
                        }
                        else
                        {
                            Debug.LogError($"代码生成错误，该类型不支持{typeName}--->{pSheet.Name}");
                        }
                    }
                    else
                    {
                       
                        for (int j = 2; j <= _MaxRowNum; j++)
                        {
                            string firstValue = ExcelReader.GetCellValue(pSheet,j, 1).ToString();
                            //特殊标记
                            if (firstValue.Contains("##"))
                                continue;
                            string value =  ExcelReader.GetCellValue(pSheet,j, col).ToString();
                            if (string.IsNullOrEmpty(value))
                            {
                                StringProperty strProp = new StringProperty();
                                strProp.name = propName;
                                strProp.isKey = keyPropNames.Contains(propName);
                                if (hasComment)
                                {
                                    strProp.comment = ExcelReader.GetCellValue(pSheet, 3, col);
                                }
                                propList.Add(strProp);
                                break;
                            }
                            else
                            {
                                BaseProperty prop = ExcelPropertyMap.GetPropertyByValue(value);
                                prop.name = propName;
                                prop.isKey = keyPropNames.Contains(propName);
                                if (hasComment)
                                {
                                    prop.comment = ExcelReader.GetCellValue(pSheet, 3, col);
                                }
                                propList.Add(prop);
                                break;
                            }
                        }
                    }
                    
                }
                else
                {
                    if (typeName != null)
                    {
                        if (typeName.Contains("|"))
                        {
                            MapProperty prop = new MapProperty();
                            prop.name = propName;
                            prop.isKey = false;
                            if (hasComment)
                            {
                                prop.comment = ExcelReader.GetCellValue(pSheet, 3, col);
                            }
                            string[] mapTypes = typeName.Split("|");
                            if (!ExcelPropertyMap.CheckHasProperty(mapTypes[0]))
                            {
                                Debug.LogError($"代码生成错误，字典键类不支持{mapTypes[0]}--->{pSheet.Name}");
                                return propList;
                            }

                            prop.KeyTypeName = mapTypes[0];
                            prop.KeyProp = ExcelPropertyMap.GetPropertyByTypeName(mapTypes[0]);

                            if (!ExcelPropertyMap.CheckHasProperty(mapTypes[1]))
                            {
                                prop.ValueProp = new CustomClassProperty();
                                prop.ValueTypeName = prop.ValueProp.TypeName;
                            }
                            else
                            {
                                prop.ValueProp = ExcelPropertyMap.GetPropertyByTypeName(mapTypes[1]);
                                prop.ValueTypeName = prop.ValueProp.TypeName;
                            }
                            propList.Add(prop);
                        }
                        else
                        {
                            ListProperty prop = new ListProperty();
                            prop.name = propName;
                            prop.isKey = false;
                            if (hasComment)
                            {
                                prop.comment = ExcelReader.GetCellValue(pSheet, 3, col);
                            }
                            if (!ExcelPropertyMap.CheckHasProperty(typeName))
                            {
                                prop.ChildProp = new CustomClassProperty();
                                prop.ChildTypeName = prop.ChildProp.TypeName;
                            }
                            else
                            {
                                prop.ChildProp = ExcelPropertyMap.GetPropertyByTypeName(typeName);
                                prop.ChildTypeName = prop.ChildProp.TypeName;
                            }
                            propList.Add(prop);
                        }
                    }
                    else
                    {
                        BaseProperty prop = null;
                        for (int j = 2; j <= _MaxRowNum; j++)
                        {
                            string firstValue = ExcelReader.GetCellValue(pSheet,j, 1).ToString();
                            //特殊标记
                            if (firstValue.Contains("##"))
                                continue;
                            string value = ExcelReader.GetCellValue(pSheet,j, col).ToString();
                            if (string.IsNullOrEmpty(value))
                            {
                                Debug.LogWarning($"集合类型，没有填默认值，当作字符串处理{propName}--->{pSheet.Name}");
                                prop = new ListProperty();
                                prop.name = propName;
                                prop.isKey = false;
                                if (hasComment)
                                {
                                    prop.comment = ExcelReader.GetCellValue(pSheet, 3, col);
                                }
                                if (!prop.CanCatch(value))
                                {
                                    Debug.LogError($"代码生成错误，集合子类不支持{value}");
                                    return propList;
                                }
                                propList.Add(prop);
                                break;
                            }
                            else
                            {
                                if (value.Contains("|"))
                                {
                                    prop = new MapProperty();
                                    prop.name = propName;
                                    prop.isKey = false;
                                    if (hasComment)
                                    {
                                        prop.comment = ExcelReader.GetCellValue(pSheet, 3, col);
                                    }
                                    if (!prop.CanCatch(value))
                                    {
                                        Debug.LogError($"代码生成错误，字典子类不支持{value}");
                                        return propList;
                                    }
                                    propList.Add(prop);
                                    break;
                                }
                                else
                                {
                                    prop = new ListProperty();
                                    prop.name = propName;
                                    prop.isKey = false;
                                    if (hasComment)
                                    {
                                        prop.comment = ExcelReader.GetCellValue(pSheet, 3, col);
                                    }
                                    if (!prop.CanCatch(value))
                                    {
                                        Debug.LogError($"代码生成错误，集合子类不支持{value}");
                                        return propList;
                                    }
                                    propList.Add(prop);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            
            return propList;
        }

        private static Dictionary<string, List<int>> GetPropColDict(ExcelWorksheet pSheet, out List<string> pPropKeys,out List<string> pArrayProps)
        {
            pPropKeys = new List<string>();
            pArrayProps = new List<string>();
            if (pSheet == null || pSheet.Dimension == null)
                return null;
            
            Dictionary<string, List<int>> propColDict = new Dictionary<string, List<int>>();
            //最大行
            int _MaxRowNum = pSheet.Dimension.End.Row;
            //最小行
            int _MinRowNum = pSheet.Dimension.Start.Row;
            
            //最大列
            int _MaxColumnNum = pSheet.Dimension.End.Column;
            //最小列
            int _MinColumnNum = pSheet.Dimension.Start.Column;
            
            string lastPropName = null;
            for (int i = _MinColumnNum; i <= _MaxColumnNum; i++)
            {
                string propName = ExcelReader.GetCellValue(pSheet, 1, i);
                if (propName == "##")
                    continue;

                if (propName == "")
                {
                    if (!string.IsNullOrEmpty(lastPropName))
                        propColDict[lastPropName].Add(i);
                }
                else
                {
                    if (propColDict.ContainsKey(propName))
                    {
                        Debug.LogError($"表格读取失败，字段名重复>>>{propName}");
                        continue;
                    }
                    string newPropName = propName.Replace("#", "");
                    newPropName = newPropName.Replace("$", "");
                    propColDict.Add(newPropName,new List<int>());
                    propColDict[newPropName].Add(i);
                    
                    if (propName.Contains("#"))
                    {
                        pPropKeys.Add(newPropName);
                    }

                    if (propName.Contains("$"))
                    {
                        pArrayProps.Add(newPropName);
                    }

                    lastPropName = propName;
                }
            }

            return propColDict;
        }
        
    }
}