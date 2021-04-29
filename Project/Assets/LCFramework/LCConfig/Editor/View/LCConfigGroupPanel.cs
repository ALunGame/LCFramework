using LCHelp;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XPToolchains.Json;

namespace LCConfig
{
    /// <summary>
    /// 配置显示界面
    /// </summary>
    public class LCConfigGroupPanel
    {
        private ConfigJson configJson;
        private ConfigGroup configGroup;

        private string currExcelName;
        private string selConfigName;

        //树状显示
        private string searchItem = "";
        private string searchData = "";
        private string selConfigItem = "";

        public void SetData(ConfigJson json, ConfigGroup group, string selExcelName)
        {
            configJson = json;
            configGroup = group;
            currExcelName = selExcelName;
            searchItem = "";
            searchData = "";
            selConfigItem = "";
        }

        public void Refresh()
        {
            if (configGroup == null)
            {
                return;
            }
            EDLayout.CreateVertical("", 1000, 800, () =>
            {
                ShowConfInfo(1000, 30);
                ShowConfigList(1000, 120);
                ShowConfigDataTab(1000, 800 - 150);
            });
        }

        #region 上方
        //顶部信息
        private void ShowConfInfo(float width, float height)
        {
            EDLayout.CreateHorizontal("box", width, height, () =>
            {
                EditorGUILayout.LabelField("当前配置分组:" + currExcelName, GUILayout.Width(250), GUILayout.Height(25));

                EDButton.CreateBtn("删除配置分组", 250, 25, () =>
                {
                    configJson.ConfGroup.Remove(configGroup.Name);
                    configGroup = null;
                });
            });
        }
        #endregion

        #region 中间
        //中间配置列表
        private Vector2 pos01 = Vector2.zero;
        private void ShowConfigList(float width, float height)
        {
            EDLayout.CreateScrollView(ref pos01, "box", 900, 90, (float wid, float hei) =>
            {
                EDLayout.CreateHorizontal("", wid - 20, 75, () =>
                {
                    ShowOperateJson(120, 50);

                    ShowOperateExcel(120, 50);

                    ShowOperateConfigList(120, 50);

                    if (configGroup == null)
                    {
                        return;
                    }

                    for (int i = 0; i < configGroup.Configs.Count; i++)
                    {
                        int index = i;
                        Config config = configGroup.Configs[i];
                        EDLayout.CreateVertical("box", 120, 50, () =>
                        {
                            EDColor.DrawColorArea(selConfigName == config.Name ? Color.green : Color.white, () =>
                            {
                                EDButton.CreateBtn(config.Name, 160, 25, () =>
                                {
                                    selConfigName = config.Name;
                                    columnPage = 0;
                                    rowPage = 0;
                                });
                            });

                            EDLayout.CreateHorizontal("", 120, 25, () =>
                            {
                                EDButton.CreateBtn("重命名", 50, 20, () =>
                                {
                                    EDPopPanel.PopWindow("输入新配置名：", (string x) =>
                                    {
                                        Config tmp = LCConfigHelp.GetConfig(configGroup, x);
                                        if (tmp != null)
                                        {
                                            Debug.LogError("新配置名重复" + x);
                                            return;
                                        }
                                        config.Name = x;
                                    });
                                });

                                EDButton.CreateBtn("删除", 50, 20, () =>
                                {
                                    configGroup.Configs.RemoveAt(index);
                                });
                            });

                        });
                    }
                });
            });
        }

        private void ShowOperateJson(float width, float height)
        {
            EDLayout.CreateVertical("box", width, height, () =>
            {
                EDButton.CreateBtn("生成Json文件", 120, height, () =>
                {
                    string str = JsonMapper.ToJson(configGroup);
                    EDTool.WriteText(str, LCConfigEditorWindow.configSetting.JsonPath + "/" + configGroup.Name);
                });
            });
        }

        private void ShowOperateExcel(float width, float height)
        {
            EDLayout.CreateVertical("box", width, height, () =>
            {
                EDButton.CreateBtn("导入Excel", 120, 25, () =>
                {
                    string filePath = EDOpenFloder.OpenFilePanelWithExtName("选择Excel", LCConfigEditorWindow.configSetting.ExcelRootPath, "xlsx");
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        bool cover = EditorUtility.DisplayDialog("导入模式选择", "是否覆盖导入Excel，这将会删除配置中所有数据，以Excel中的数据为准？", "确认", "取消");

                        LCConfigToExcel.ExcelRootPath = LCConfigEditorWindow.configSetting.ExcelRootPath;
                        LCConfigToExcel.ExcelToConfigGroup(filePath, configGroup, cover);
                    }
                });

                EDButton.CreateBtn("导出Excel", 120, 25, () =>
                {
                    LCConfigToExcel.ExcelRootPath = LCConfigEditorWindow.configSetting.ExcelRootPath;
                    LCConfigToExcel.ConfigGroupToExcel(configGroup);
                });
            });
        }

        private void ShowOperateConfigList(float width, float height)
        {
            EDLayout.CreateVertical("box", width, height, () =>
            {
                EDButton.CreateBtn("新建", 120, 25, () =>
                {
                    EDPopPanel.PopWindow("请输入新的配置名：", (string x) =>
                    {
                        LCConfigHelp.AddConfig(configJson, configGroup, x);
                    });
                });

                EDButton.CreateBtn("粘贴", 120, 25, () =>
                {
                    if (string.IsNullOrEmpty(selConfigName))
                    {
                        Debug.LogError("请选择配置");
                        return;
                    }
                    Config copyConfig = null;
                    for (int i = 0; i < configGroup.Configs.Count; i++)
                    {
                        if (configGroup.Configs[i].Name == selConfigName)
                        {
                            copyConfig = LCDeepCopy.DeepCopyByBin<Config>(configGroup.Configs[i]);
                            copyConfig.Name = selConfigName + "_copy";
                            configGroup.Configs.Add(copyConfig);
                        }
                    }
                });
            });
        }
        #endregion

        #region 下方

        //显示下方配置信息
        private Vector2 pos03 = Vector2.zero;
        private bool ShowTab = false;
        private void ShowConfigDataTab(float width, float height)
        {
            Config config = LCConfigHelp.GetConfig(configGroup, selConfigName);
            if (config == null)
            {
                return;
            }
            EDLayout.CreateHorizontal("", width, height, () =>
            {
                ShowOperateConfig(150, height, config);
                if (ShowTab)
                {
                    ShowTabData(width - 150, height, config);
                }
                else
                {
                    ShowTreeData(width - 150, height, config);
                }
            });
        }

        //操作配置
        private void ShowOperateConfig(float width, float height, Config config)
        {
            EDLayout.CreateVertical("box", width, height, () =>
            {
                EditorGUILayout.Space();
                EDButton.CreateBtn(ShowTab ? "表格显示" : "树状显示", width, 25, () =>
                {
                    ShowTab = !ShowTab;
                });

                EditorGUILayout.Space();
                EDButton.CreateBtn("新建配置项", width, 25, () =>
                {
                    EDPopPanel.PopWindow("请输入新的配置项名：", (string x) =>
                    {
                        Config selConf = LCConfigHelp.GetConfig(configGroup, selConfigName);
                        LCConfigHelp.AddConfigItem(selConf, x);
                    });
                });

                EditorGUILayout.Space();
                EDButton.CreateBtn("导入Excel Sheet", 120, 25, () =>
                {
                    string filePath = EDOpenFloder.OpenFilePanelWithExtName("选择Excel", LCConfigEditorWindow.configSetting.ExcelRootPath, "xlsx");
                    if (!string.IsNullOrEmpty(filePath))
                    {
                        bool cover = EditorUtility.DisplayDialog("导入模式选择", "是否覆盖导入Excel，这将会删除配置中所有数据，以Excel中的数据为准？", "确认", "取消");

                        LCConfigToExcel.ExcelRootPath = LCConfigEditorWindow.configSetting.ExcelRootPath;
                        LCConfigToExcel.ExcelToConfig(filePath, configGroup, config, cover);
                    }
                });

                EDButton.CreateBtn("导出Excel Sheet", 120, 25, () =>
                {
                    LCConfigToExcel.ExcelRootPath = LCConfigEditorWindow.configSetting.ExcelRootPath;
                    LCConfigToExcel.ConfigToExcel(configGroup, config);
                });

                EDButton.CreateBtn("生成Lua文件", 120, 25, () =>
                {
                    LCConfigToLua.LuaRootPath = LCConfigEditorWindow.configSetting.LuaRootPath;
                    LCConfigToLua.ConfigToLua(config);
                });

                if (ShowTab)
                {
                    EDLayout.CreateHorizontal("", 120, 100, () =>
                    {
                        EDLayout.CreateVertical("box", 55, 100, () =>
                        {
                            EDButton.CreateBtn("最前列", 50, 25, () =>
                            {
                                columnPage = 0;
                            });
                            EDButton.CreateBtn("最后列", 50, 25, () =>
                            {
                                columnPage = 999999;
                            });
                            EDButton.CreateBtn("上列", 50, 25, () =>
                            {
                                columnPage--;
                            });
                            EDButton.CreateBtn("下列", 50, 25, () =>
                            {
                                columnPage++;
                            });
                        });

                        EDLayout.CreateVertical("box", 55, 50, () =>
                        {
                            EDButton.CreateBtn("第一行", 50, 25, () =>
                            {
                                rowPage = 0;
                            });
                            EDButton.CreateBtn("最后行", 50, 25, () =>
                            {
                                rowPage = 999999;
                            });
                            EDButton.CreateBtn("上行", 50, 25, () =>
                            {
                                rowPage--;
                            });
                            EDButton.CreateBtn("下行", 50, 25, () =>
                            {
                                rowPage++;
                            });
                        });
                    });
                }
            });

        }

        //树状显示
        private Vector2 pos04 = Vector2.zero;
        private Vector2 pos05 = Vector2.zero;
        private void ShowTreeData(float width, float height, Config config)
        {
            //首先配置项列表
            EDLayout.CreateScrollView(ref pos04, "box", 180, height, () =>
            {
                searchData = EDSearchInput.CreateSearch(searchData, "查询指定值", 160);
                searchItem = EDSearchInput.CreateSearch(searchItem, "筛选配置项", 160);

                EDButton.CreateBtn("新建配置项", 160, 25, () =>
                {
                    EDPopPanel.PopWindow("请输入新的配置项名：", (string x) =>
                    {
                        Config selConf = LCConfigHelp.GetConfig(configGroup, selConfigName);
                        LCConfigHelp.AddConfigItem(selConf, x);
                    });
                });

                //配置列表
                for (int i = 0; i < config.Items.Count; i++)
                {
                    if (searchItem == "筛选配置项")
                    {
                        EDColor.DrawColorArea(selConfigItem == config.Items[i].Name ? Color.green : Color.white, () =>
                             {
                                 EDButton.CreateBtn(config.Items[i].Name, 160, 25, () =>
                                 {
                                     selConfigItem = config.Items[i].Name;
                                 });
                             });
                    }
                    else
                    {
                        if (config.Items[i].Name.Contains(searchItem))
                        {
                            EDColor.DrawColorArea(selConfigItem == config.Items[i].Name ? Color.green : Color.white, () =>
                            {
                                EDButton.CreateBtn(config.Items[i].Name, 160, 25, () =>
                                {
                                    selConfigItem = config.Items[i].Name;
                                });
                            });
                        }
                    }
                }
            });

            ConfigItem item = LCConfigHelp.GetConfigItem(config, selConfigItem);
            if (item == null || !item.DataList.Contains(searchData))
            {
                EDColor.DrawColorArea(Color.red, () =>
                {
                    GUILayout.Label("没有查询数据>>>>>>>>>>>>>>>>>>>>>");
                });
                return;
            }

            int showIndex = 0;
            for (int i = 0; i < item.DataList.Count; i++)
            {
                if (item.DataList[i] == searchData)
                {
                    showIndex = i;
                }
            }
            //数据
            EDLayout.CreateScrollView(ref pos05, "box", 250, height, () =>
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField(string.Format("配置项 {0} 是 {1} 数据：", selConfigItem, searchData), GUILayout.Width(220), GUILayout.Height(30));
                EditorGUILayout.Space();

                for (int i = 0; i < config.Items.Count; i++)
                {
                    if (config.Items[i].Name != item.Name)
                    {
                        ConfigItem item1 = config.Items[i];
                        string value = item1.DataList[showIndex];
                        string showName = string.Format("{0}: ({1})", item1.Name, item1.Type);
                        ShowDataValue(showName, 220, 30, item1, showIndex, "box");
                    }
                }
            });
        }

        //表格显示
        private Vector2 pos06 = Vector2.zero;
        int columnPage = 0;
        int rowPage = 0;
        private void ShowTabData(float width, float height, Config config)
        {
            int itemWid = 110;
            int itemHei = 25;
            int columnCnt = config.Items.Count + 1;
            int rowCnt = LCConfigHelp.ConfigItemValueIndex;
            if (config.Items.Count > 0)
            {
                rowCnt = config.Items[0].DataList.Count + LCConfigHelp.ConfigItemValueIndex;
            }
            EDLayout.CreateTableView(ref pos06, ref columnPage, ref rowPage, width, height, 120, 80, columnCnt, rowCnt, (int column, int row) =>
            {
                //添加删除
                if (column == 0)
                {
                    if (row == 0)
                    {
                        EditorGUILayout.LabelField("配置名", GUILayout.Width(itemWid), GUILayout.Height(itemHei));
                    }
                    else if (row == 1)
                    {
                        EditorGUILayout.LabelField("类型名", GUILayout.Width(itemWid), GUILayout.Height(itemHei));
                    }
                    else if (row == 2)
                    {
                        EditorGUILayout.LabelField("规则类型", GUILayout.Width(itemWid), GUILayout.Height(itemHei));
                    }
                    else if (row == 3)
                    {
                        GUILayout.TextArea("规则数据(比如依赖那个配置)", GUILayout.Width(itemWid), GUILayout.Height(60));
                    }
                    else if (row == 4)
                    {
                        EDButton.CreateBtn("新建配置项", itemWid, itemHei, () =>
                        {
                            EDPopPanel.PopWindow("请输入新的配置项名：", (string x) =>
                            {
                                Config selConf = LCConfigHelp.GetConfig(configGroup, selConfigName);
                                LCConfigHelp.AddConfigItem(selConf, x);
                            });
                        });

                        EDButton.CreateBtn("添加数据", itemWid, itemHei, () =>
                        {
                            for (int i = 0; i < config.Items.Count; i++)
                            {
                                OnAddValue(config.Items[i]);
                            }
                        });
                    }
                    else
                    {
                        EDButton.CreateBtn("+", itemWid, itemHei, () =>
                        {
                            for (int i = 0; i < config.Items.Count; i++)
                            {
                                OnAddValue(config.Items[i]);
                            }
                        });

                        EDButton.CreateBtn("-", itemWid / 2, itemHei, () =>
                        {
                            for (int i = 0; i < config.Items.Count; i++)
                            {
                                config.Items[i].DataList.RemoveAt(row - LCConfigHelp.ConfigItemValueIndex);
                            }
                        });
                    }
                }
                //数据
                else
                {
                    int itemIndex = column - 1;
                    if (itemIndex >= config.Items.Count)
                    {
                        return;
                    }
                    ConfigItem item = config.Items[itemIndex];
                    //名字以及删除
                    if (row == 0)
                    {
                        EditorGUILayout.LabelField(item.Name, GUILayout.Width(itemWid), GUILayout.Height(itemHei));

                        EDButton.CreateBtn("删除", itemWid, itemHei, () =>
                        {
                            config.Items.RemoveAt(itemIndex);
                        });
                    }
                    //类型
                    else if (row == 1)
                    {
                        ConfigDataType tmpType = item.Type;
                        tmpType = (ConfigDataType)EditorGUILayout.EnumPopup(item.Type, GUILayout.Width(itemWid), GUILayout.Height(itemHei));
                        if (tmpType != item.Type)
                        {
                            item.Type = tmpType;
                            OnTypeChange(item);
                        }
                    }
                    //规则类型
                    else if (row == 2)
                    {
                        ConfigDataRuleType tmpRuleType = item.RuleType;
                        tmpRuleType = (ConfigDataRuleType)EditorGUILayout.EnumPopup(tmpRuleType, GUILayout.Width(itemWid), GUILayout.Height(itemHei));
                        if (tmpRuleType != item.RuleType)
                        {
                            item.RuleType = tmpRuleType;
                            OnRuleTypeChange(item);
                        }
                    }
                    //类型规则
                    else if (row == 3)
                    {
                        ShowRuleDataValue(itemWid, item);
                    }
                    //注释
                    else if (row == 4)
                    {
                        EditorGUILayout.LabelField("注释", GUILayout.Width(itemWid), GUILayout.Height(itemHei));
                        item.Desc = EditorGUILayout.TextField(item.Desc, GUILayout.Width(itemWid), GUILayout.Height(itemHei));
                    }
                    else
                    {
                        int dataIndex = row - LCConfigHelp.ConfigItemValueIndex;
                        ShowDataValue("", itemWid, itemHei, item, dataIndex);
                    }
                }
            });
        }

        //显示规则数据
        private void ShowRuleDataValue(float width, ConfigItem item)
        {
            if (item.RuleType == ConfigDataRuleType.Relate)
            {
                EditorGUILayout.TextArea("选择关联的配置", GUILayout.Width(width), GUILayout.Height(30));

                EDButton.CreateBtn(item.RuleData, width, 25, () =>
                {
                    List<string> resList = LCConfigHelp.GetAllConfigItemPathName(configJson);
                    EDPopMenu.CreatePopMenu(resList, (string x) =>
                    {
                        item.RuleData = x;
                    });
                });
            }
            else
            {
                EditorGUILayout.TextArea("不需要规则数据", GUILayout.Width(width), GUILayout.Height(60));
            }
        }

        //显示数据  新加的数据如果需要特别显示
        private void ShowDataValue(string showName, float width, float height, ConfigItem item, int dataIndex, string style = "")
        {
            if (dataIndex >= item.DataList.Count)
            {
                return;
            }

            Type valueType = typeof(string);
            switch (item.Type)
            {
                case ConfigDataType.Int:
                    valueType = typeof(int);
                    break;
                case ConfigDataType.Float:
                    valueType = typeof(float);
                    break;
                case ConfigDataType.String:
                    valueType = typeof(string);
                    break;
                case ConfigDataType.Vector3:
                    valueType = typeof(Vector3);
                    break;
                case ConfigDataType.Vector2:
                    valueType = typeof(Vector2);
                    break;
                case ConfigDataType.Vector4:
                    valueType = typeof(Vector4);
                    break;
                case ConfigDataType.Bool:
                    valueType = typeof(bool);
                    break;
                default:
                    valueType = typeof(string);
                    break;
            }

            
            string data = item.DataList[dataIndex];
            object value = LCConvert.StrChangeToObject(data, valueType.FullName);

            if (item.Type == ConfigDataType.Asset)
            {
                EDLayout.CreateVertical(style, width, height, () =>
                {
                    UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(value.ToString(), typeof(UnityEngine.Object));
                    asset = EditorGUILayout.ObjectField(asset, typeof(UnityEngine.Object), false, GUILayout.Width(width - 5), GUILayout.Height(25));
                    value = AssetDatabase.GetAssetPath(asset);
                    GUILayout.Label(data, GUILayout.Width(width - 5), GUILayout.Height(25));
                });
            }
            else
            {
                EDLayout.CreateVertical(style, width, height, () =>
                {
                    EDTypeField.CreateTypeField(showName, ref value, valueType, width - 5, 25);
                    GUILayout.Label(data, GUILayout.Width(width - 5), GUILayout.Height(25));
                });
            }



            string newValue = LCExtension.ToString(value, valueType.FullName);
            if (newValue != item.DataList[dataIndex])
            {
                OnValueChange(item, dataIndex, newValue);
            }
        }

        //当类型改变
        private void OnTypeChange(ConfigItem item)
        {
            for (int i = 0; i < item.DataList.Count; i++)
            {
                item.DataList[i] = LCConfigHelp.SetConfigItemDefaultValue(item);
            }
        }

        //当规则改变
        private void OnRuleTypeChange(ConfigItem item)
        {
            if (item.RuleType == ConfigDataRuleType.Relate)
            {
                for (int i = 0; i < item.DataList.Count; i++)
                {
                    item.DataList[i] = "";
                }
            }
            else if (item.RuleType == ConfigDataRuleType.NoSame)
            {
                for (int i = 0; i < item.DataList.Count; i++)
                {
                    if (LCConfigHelp.CheckNoSameDataIsLegal(item, item.DataList[i]))
                    {
                        item.DataList[i] = item.DataList[i];
                    }
                    else
                    {
                        item.DataList[i] = LCConfigHelp.SetConfigItemDefaultValue(item);
                    }
                }
            }
        }

        //当添加数据
        private void OnAddValue(ConfigItem item)
        {
            string lastValue = "";
            if (item.DataList.Count > 0)
            {
                lastValue = item.DataList[item.DataList.Count - 1];
            }

            string addValue = LCConfigHelp.SetConfigItemDefaultValue(item);
            if (item.Type == ConfigDataType.Int)
            {
                if (!string.IsNullOrEmpty(lastValue))
                {
                    addValue = (int.Parse(lastValue) + 1).ToString();
                }
            }

            item.DataList.Add(addValue);
        }

        //当数据改变
        private void OnValueChange(ConfigItem item, int dataIndex, string newValue)
        {
            item.DataList[dataIndex] = LCConfigHelp.SetConfigItemValue(configJson, item, newValue);
        }

        #endregion
    }
}
