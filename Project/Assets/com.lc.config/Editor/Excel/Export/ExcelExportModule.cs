using System;
using System.Collections.Generic;
using com.lc.config.Editor.Excel.Core;
using LCConfig.Excel.GenCode;
using LCConfig.Excel.GenCode.Property;
using LCMap;
using LCTask;
using LCToolkit;
using OfficeOpenXml;
using UnityEngine;

#region AutoGenUsing
using TT;
using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System;
using System;
using Cnf;
using System.Collections.Generic;
using Cnf;
using System.Collections.Generic;
using Cnf;
using Cnf;
using Cnf;
using LCMap;
using LCMap;
using Demo;

using LCUI;
#endregion AutoGenUsing

namespace LCConfig.Excel.Export
{
    internal class ExcelExportModule
    {
        private Dictionary<string, Action<GenConfigInfo, List<BaseProperty>, List<Dictionary<string, List<string>>>>>
            exportFuncDict =
                new Dictionary<string,
                    Action<GenConfigInfo, List<BaseProperty>, List<Dictionary<string, List<string>>>>>();

#region AutoGenCode
        private void Export_UIPanelCnf(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<UIPanelCnf> cnfs = new List<UIPanelCnf>();
            foreach (var propDict in propValuelist)
            {
                UIPanelCnf cnf = new UIPanelCnf();
				cnf.id = (UIPanelDef)GetProp(pProps,"id").Parse(propDict["id"][0]);
				cnf.prefab = (string)GetProp(pProps,"prefab").Parse(propDict["prefab"][0]);
				cnf.script = (string)GetProp(pProps,"script").Parse(propDict["script"][0]);
				cnf.layer = (UILayer)GetProp(pProps,"layer").Parse(propDict["layer"][0]);
				cnf.canvas = (UICanvasType)GetProp(pProps,"canvas").Parse(propDict["canvas"][0]);
				cnf.showRule = (UIShowRule)GetProp(pProps,"showRule").Parse(propDict["showRule"][0]);

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }

        private void Export_EventCnf(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<EventCnf> cnfs = new List<EventCnf>();
            foreach (var propDict in propValuelist)
            {
                EventCnf cnf = new EventCnf();
				cnf.id = (int)GetProp(pProps,"id").Parse(propDict["id"][0]);
				cnf.name = (string)GetProp(pProps,"name").Parse(propDict["name"][0]);
				cnf.des = (string)GetProp(pProps,"des").Parse(propDict["des"][0]);
				cnf.cond = (ConditionType)GetProp(pProps,"cond").Parse(propDict["cond"][0]);
				cnf.condParam = (string)GetProp(pProps,"condParam").Parse(propDict["condParam"][0]);
				cnf.startTaskId = (int)GetProp(pProps,"startTaskId").Parse(propDict["startTaskId"][0]);
				cnf.nextSuccess = (int)GetProp(pProps,"nextSuccess").Parse(propDict["nextSuccess"][0]);

                cnf.successReward = new List<ItemInfo>();
                for (int i = 0; i < propDict["successReward"].Count; i++)
                {
                    string value = propDict["successReward"][i];
                    if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                    cnf.successReward.Add((ItemInfo)GetProp(pProps,"successReward").Parse(value));
                }; 
            
				cnf.nextFail = (int)GetProp(pProps,"nextFail").Parse(propDict["nextFail"][0]);

                cnf.failReward = new List<ItemInfo>();
                for (int i = 0; i < propDict["failReward"].Count; i++)
                {
                    string value = propDict["failReward"][i];
                    if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                    cnf.failReward.Add((ItemInfo)GetProp(pProps,"failReward").Parse(value));
                }; 
            

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }

        private void Export_WeaponCnf(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<WeaponCnf> cnfs = new List<WeaponCnf>();
            foreach (var propDict in propValuelist)
            {
                WeaponCnf cnf = new WeaponCnf();
				cnf.id = (int)GetProp(pProps,"id").Parse(propDict["id"][0]);
				cnf.name = (string)GetProp(pProps,"name").Parse(propDict["name"][0]);
				cnf.prefab = (string)GetProp(pProps,"prefab").Parse(propDict["prefab"][0]);
				cnf.useAnim = (string)GetProp(pProps,"useAnim").Parse(propDict["useAnim"][0]);
				cnf.useSkillId = (int)GetProp(pProps,"useSkillId").Parse(propDict["useSkillId"][0]);

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }

        private void Export_ActorBasePropertyCnf(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<ActorBasePropertyCnf> cnfs = new List<ActorBasePropertyCnf>();
            foreach (var propDict in propValuelist)
            {
                ActorBasePropertyCnf cnf = new ActorBasePropertyCnf();
				cnf.id = (int)GetProp(pProps,"id").Parse(propDict["id"][0]);
				cnf.hp = (int)GetProp(pProps,"hp").Parse(propDict["hp"][0]);
				cnf.moveSpeed = (float)GetProp(pProps,"moveSpeed").Parse(propDict["moveSpeed"][0]);
				cnf.jumpSpeed = (float)GetProp(pProps,"jumpSpeed").Parse(propDict["jumpSpeed"][0]);
				cnf.attack = (float)GetProp(pProps,"attack").Parse(propDict["attack"][0]);
				cnf.defense = (float)GetProp(pProps,"defense").Parse(propDict["defense"][0]);
				cnf.actionSpeed = (float)GetProp(pProps,"actionSpeed").Parse(propDict["actionSpeed"][0]);

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }

        private void Export_ActorCollectCnf(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<ActorCollectCnf> cnfs = new List<ActorCollectCnf>();
            foreach (var propDict in propValuelist)
            {
                ActorCollectCnf cnf = new ActorCollectCnf();
				cnf.id = (int)GetProp(pProps,"id").Parse(propDict["id"][0]);

                cnf.workeCollect = new List<ItemInfo>();
                for (int i = 0; i < propDict["workeCollect"].Count; i++)
                {
                    string value = propDict["workeCollect"][i];
                    if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                    cnf.workeCollect.Add((ItemInfo)GetProp(pProps,"workeCollect").Parse(value));
                }; 
            
				cnf.time = (float)GetProp(pProps,"time").Parse(propDict["time"][0]);
				cnf.anim = (AnimInfo)GetProp(pProps,"anim").Parse(propDict["anim"][0]);

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }

        private void Export_ActorProduceCnf(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<ActorProduceCnf> cnfs = new List<ActorProduceCnf>();
            foreach (var propDict in propValuelist)
            {
                ActorProduceCnf cnf = new ActorProduceCnf();
				cnf.id = (int)GetProp(pProps,"id").Parse(propDict["id"][0]);

                cnf.workeOutput = new List<int>();
                for (int i = 0; i < propDict["workeOutput"].Count; i++)
                {
                    string value = propDict["workeOutput"][i];
                    if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                    cnf.workeOutput.Add((int)GetProp(pProps,"workeOutput").Parse(value));
                }; 
            
				cnf.time = (float)GetProp(pProps,"time").Parse(propDict["time"][0]);
				cnf.anim = (AnimInfo)GetProp(pProps,"anim").Parse(propDict["anim"][0]);

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }

        private void Export_ActorLifeCnf(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<ActorLifeCnf> cnfs = new List<ActorLifeCnf>();
            foreach (var propDict in propValuelist)
            {
                ActorLifeCnf cnf = new ActorLifeCnf();
				cnf.id = (int)GetProp(pProps,"id").Parse(propDict["id"][0]);
				cnf.produceId = (int)GetProp(pProps,"produceId").Parse(propDict["produceId"][0]);
				cnf.collectId = (int)GetProp(pProps,"collectId").Parse(propDict["collectId"][0]);

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }

        private void Export_ActorCnf(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<ActorCnf> cnfs = new List<ActorCnf>();
            foreach (var propDict in propValuelist)
            {
                ActorCnf cnf = new ActorCnf();
				cnf.id = (int)GetProp(pProps,"id").Parse(propDict["id"][0]);
				cnf.name = (string)GetProp(pProps,"name").Parse(propDict["name"][0]);
				cnf.type = (ActorType)GetProp(pProps,"type").Parse(propDict["type"][0]);
				cnf.entityId = (int)GetProp(pProps,"entityId").Parse(propDict["entityId"][0]);
				cnf.prefab = (string)GetProp(pProps,"prefab").Parse(propDict["prefab"][0]);
				cnf.interactiveRange = (int)GetProp(pProps,"interactiveRange").Parse(propDict["interactiveRange"][0]);
				cnf.moveSpeed = (int)GetProp(pProps,"moveSpeed").Parse(propDict["moveSpeed"][0]);

                cnf.defaultSkills = new List<int>();
                for (int i = 0; i < propDict["defaultSkills"].Count; i++)
                {
                    string value = propDict["defaultSkills"][i];
                    if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                    cnf.defaultSkills.Add((int)GetProp(pProps,"defaultSkills").Parse(value));
                }; 
            

                cnf.defaultBuffs = new List<int>();
                for (int i = 0; i < propDict["defaultBuffs"].Count; i++)
                {
                    string value = propDict["defaultBuffs"][i];
                    if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                    cnf.defaultBuffs.Add((int)GetProp(pProps,"defaultBuffs").Parse(value));
                }; 
            

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }

        private void Export_SkillCnf(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<SkillCnf> cnfs = new List<SkillCnf>();
            foreach (var propDict in propValuelist)
            {
                SkillCnf cnf = new SkillCnf();
				cnf.id = (int)GetProp(pProps,"id").Parse(propDict["id"][0]);
				cnf.name = (string)GetProp(pProps,"name").Parse(propDict["name"][0]);
				cnf.cd = (string)GetProp(pProps,"cd").Parse(propDict["cd"][0]);
				cnf.timeline = (string)GetProp(pProps,"timeline").Parse(propDict["timeline"][0]);

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }

        private void Export_ItemRepairCnf(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<ItemRepairCnf> cnfs = new List<ItemRepairCnf>();
            foreach (var propDict in propValuelist)
            {
                ItemRepairCnf cnf = new ItemRepairCnf();
				cnf.id = (int)GetProp(pProps,"id").Parse(propDict["id"][0]);
				cnf.addHp = (int)GetProp(pProps,"addHp").Parse(propDict["addHp"][0]);

                cnf.repairs = new List<ItemInfo>();
                for (int i = 0; i < propDict["repairs"].Count; i++)
                {
                    string value = propDict["repairs"][i];
                    if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                    cnf.repairs.Add((ItemInfo)GetProp(pProps,"repairs").Parse(value));
                }; 
            

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }

        private void Export_ItemRecipeCnf(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<ItemRecipeCnf> cnfs = new List<ItemRecipeCnf>();
            foreach (var propDict in propValuelist)
            {
                ItemRecipeCnf cnf = new ItemRecipeCnf();
				cnf.id = (int)GetProp(pProps,"id").Parse(propDict["id"][0]);

                cnf.recipes = new List<ItemInfo>();
                for (int i = 0; i < propDict["recipes"].Count; i++)
                {
                    string value = propDict["recipes"][i];
                    if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                    cnf.recipes.Add((ItemInfo)GetProp(pProps,"recipes").Parse(value));
                }; 
            

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }

        private void Export_ItemCnf(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<ItemCnf> cnfs = new List<ItemCnf>();
            foreach (var propDict in propValuelist)
            {
                ItemCnf cnf = new ItemCnf();
				cnf.id = (int)GetProp(pProps,"id").Parse(propDict["id"][0]);
				cnf.name = (string)GetProp(pProps,"name").Parse(propDict["name"][0]);

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }

        private void Export_Test(GenConfigInfo pGenInfo,List<BaseProperty> pProps,List<Dictionary<string, List<string>>> propValuelist)
        {
            List<Test> cnfs = new List<Test>();
            foreach (var propDict in propValuelist)
            {
                Test cnf = new Test();
				cnf.id = (int)GetProp(pProps,"id").Parse(propDict["id"][0]);
				cnf.id2 = (int)GetProp(pProps,"id2").Parse(propDict["id2"][0]);
				cnf.name = (string)GetProp(pProps,"name").Parse(propDict["name"][0]);
				cnf.age = (int)GetProp(pProps,"age").Parse(propDict["age"][0]);

                cnf.itemlist = new List<int>();
                for (int i = 0; i < propDict["itemlist"].Count; i++)
                {
                    string value = propDict["itemlist"][i];
                    if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                    cnf.itemlist.Add((int)GetProp(pProps,"itemlist").Parse(value));
                }; 
            

                cnf.itemMap = new Dictionary<int,int>();
                for (int i = 0; i < propDict["itemMap"].Count; i++)
                {
                    string value = propDict["itemMap"][i];
                    if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                    MapProperty prop = (MapProperty)GetProp(pProps,"itemMap");
                    string[] values = value.Split("|");
                    string key = values[0];
                    string va  = values[1];
                    cnf.itemMap.Add((int)prop.GetKey(key),(int)prop.GetValue(va));
                }; 
            
				cnf.type = (ItemType)GetProp(pProps,"type").Parse(propDict["type"][0]);
				cnf.info = (ItemInfo)GetProp(pProps,"info").Parse(propDict["info"][0]);

                cnf.infos = new List<ItemInfo>();
                for (int i = 0; i < propDict["infos"].Count; i++)
                {
                    string value = propDict["infos"][i];
                    if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                    cnf.infos.Add((ItemInfo)GetProp(pProps,"infos").Parse(value));
                }; 
            

                cnf.infomap = new Dictionary<ItemType,ItemInfo>();
                for (int i = 0; i < propDict["infomap"].Count; i++)
                {
                    string value = propDict["infomap"][i];
                    if (string.IsNullOrEmpty(value))
                    {
                        break;
                    }
                    MapProperty prop = (MapProperty)GetProp(pProps,"infomap");
                    string[] values = value.Split("|");
                    string key = values[0];
                    string va  = values[1];
                    cnf.infomap.Add((ItemType)prop.GetKey(key),(ItemInfo)prop.GetValue(va));
                }; 
            

                cnfs.Add(cnf);
            }
                
            pGenInfo.SaveJson(cnfs);    
        }
#endregion AutoGenCode

        private bool _init = false;
        private void init()
        {
            if (_init)
            {
                return;
            }
#region AutoRegExportFunc
			exportFuncDict.Add("UIPanelCnf",Export_UIPanelCnf);

			exportFuncDict.Add("EventCnf",Export_EventCnf);

			exportFuncDict.Add("WeaponCnf",Export_WeaponCnf);

			exportFuncDict.Add("ActorBasePropertyCnf",Export_ActorBasePropertyCnf);

			exportFuncDict.Add("ActorCollectCnf",Export_ActorCollectCnf);

			exportFuncDict.Add("ActorProduceCnf",Export_ActorProduceCnf);

			exportFuncDict.Add("ActorLifeCnf",Export_ActorLifeCnf);

			exportFuncDict.Add("ActorCnf",Export_ActorCnf);

			exportFuncDict.Add("SkillCnf",Export_SkillCnf);

			exportFuncDict.Add("ItemRepairCnf",Export_ItemRepairCnf);

			exportFuncDict.Add("ItemRecipeCnf",Export_ItemRecipeCnf);

			exportFuncDict.Add("ItemCnf",Export_ItemCnf);

			exportFuncDict.Add("Test",Export_Test);
#endregion AutoRegExportFunc

            _init = true;
        }
        
        private BaseProperty GetProp(List<BaseProperty> pProps, string pPropName)
        {
            foreach (BaseProperty baseProperty in pProps)
            {
                if (baseProperty.name == pPropName)
                {
                    return baseProperty;
                }
            }

            return null;
        }

        private List<Dictionary<string, List<string>>> GetAllPropValuelist(List<BaseProperty> pProps,List<ExcelWorksheet> pSheets,Dictionary<string, List<int>> pPropColDict)
        {
            List<Dictionary<string, List<string>>> propValuelist = new List<Dictionary<string, List<string>>>();
            foreach (ExcelWorksheet pSheet in pSheets)
            {
                if (pSheet == null || pSheet.Dimension == null)
                {
                    continue;
                }
                //最大行
                int _MaxRowNum = pSheet.Dimension.End.Row;
                //最小行
                int _MinRowNum = pSheet.Dimension.Start.Row;
            
                //最大列
                int _MaxColumnNum = pSheet.Dimension.End.Column;
                //最小列
                int _MinColumnNum = pSheet.Dimension.Start.Column;

                bool hasDefault = false;
                int defaultRow = 0;
                for (int row = 2; row <= _MaxRowNum; row++)
                {
                    string firstValue = ExcelReader.GetCellValue(pSheet,row, 1).ToString();
                    //特殊标记
                    if (firstValue.Contains("##"))
                    {
                        if (firstValue == "##default")
                        {
                            hasDefault = true;
                            defaultRow = row;
                        }
                        continue;
                    }

                    bool isSuccess = true;
                    
                    Dictionary<string, List<string>> propValueDict = new Dictionary<string, List<string>>();
                    foreach (string propName in pPropColDict.Keys)
                    {
                        List<int> colList = pPropColDict[propName];
                        List<string> values = new List<string>();
                        BaseProperty prop = GetProp(pProps, propName);
                        for (int i = 0; i < colList.Count; i++)
                        {
                            int col = colList[i];
                            string value =  ExcelReader.GetCellValue(pSheet,row, col).ToString();
                            if (hasDefault && string.IsNullOrEmpty(value))
                            {
                                value = ExcelReader.GetCellValue(pSheet,defaultRow, col).ToString();
                            }
                            if (string.IsNullOrEmpty(value))
                            {
                                if (prop.isKey)
                                {
                                    isSuccess = false;
                                    break;
                                }
                                else
                                {
                                    if (!prop.CanCatch(value))
                                    {
                                        isSuccess = false;
                                        Debug.LogWarning($"表格导出出错，类型不匹配{value}--->{prop.TypeName}:{prop.name} Sheet:{pSheet.Name} Col:{col} Row:{row}");
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (!prop.CanCatch(value))
                                {
                                    isSuccess = false;
                                    Debug.LogWarning($"表格导出出错，类型不匹配{value}--->{prop.TypeName}:{prop.name} Sheet:{pSheet.Name} Col:{col} Row:{row}");
                                    break;
                                } 
                            }
                            values.Add(value);
                        }

                        if (isSuccess)
                        {
                            propValueDict.Add(propName,values);
                        }
                        else
                        {
                            break;
                        }
                        
                    }

                    if (isSuccess)
                    {
                        propValuelist.Add(propValueDict);
                    }
                }
            }
            return propValuelist;
        }

        public void ExportAll(List<GenConfigInfo> pConfigs)
        {
            init();
            foreach (GenConfigInfo info in pConfigs)
            {
                ExcelPackage tPackage = null;
                
                List<ExcelWorksheet> sheets = new List<ExcelWorksheet>();
                foreach (string filePath in info.filePaths)
                    sheets.AddRange(ExcelReader.ReadAllSheets(filePath,out tPackage, info.sheetName));

                if (!sheets.IsLegal())
                {
                    Debug.LogError($"导出失败，没有对应工作簿:{info.filePaths[0]}-->{info.sheetName}");
                }
                List<BaseProperty> props = ExcelGenCode.GetPropsByCommonExcel(sheets[0], out var propDict);
                exportFuncDict[info.className].Invoke(info,props,GetAllPropValuelist(props,sheets,propDict));
                
                tPackage.Dispose();
            }
        }

        public List<T> Export<T>(GenConfigInfo pInfo)
        {
            init();
            ExcelPackage tPackage = null;
            
            List<ExcelWorksheet> sheets = new List<ExcelWorksheet>();
            foreach (string filePath in pInfo.filePaths)
                sheets.AddRange(ExcelReader.ReadAllSheets(filePath,out tPackage, pInfo.sheetName));
            
            List<BaseProperty> props = ExcelGenCode.GetPropsByCommonExcel(sheets[0], out var propDict);
            exportFuncDict[pInfo.className].Invoke(pInfo,props,GetAllPropValuelist(props,sheets,propDict));
            
            tPackage.Dispose();

            return pInfo.resValue as List<T>;
        }
    }
}





























































































































