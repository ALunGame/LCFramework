using LCHelp;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using XPToolchains.Json;

namespace LCConfig
{
    public enum LCConfigEditorWindowShowType
    {
        Set,
        Search,
        ShowConfig,
    }

    public class LCConfigEditorWindow : EditorWindow
    {
        private static string settingPath = "";
        public static ConfigSetting configSetting = new ConfigSetting();

        private static ConfigJson configJson = null;

        [MenuItem("LCConfig/配置表编辑")]
        private static void OpenWindow()
        {
            LCConfigEditorWindow window = GetWindow<LCConfigEditorWindow>("配置表编辑");
            window.minSize = new Vector2(1250, 800);
        }

        private void OnEnable()
        {
            EditorUtility.ClearProgressBar();
            Init();
        }

        private void Init()
        {
            settingPath = LCConfigLocate.SettingJsonPath;
            configSetting = new ConfigSetting();
            configJson = null;

            //setting
            if (!File.Exists(settingPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(settingPath));
                File.Create(settingPath).Dispose();
            }
            else
            {
                string dataJson = EDTool.ReadText(settingPath);
                configSetting = JsonMapper.ToObject<ConfigSetting>(dataJson);
            }

            //多线程加载避免卡顿
            TaskHelp.AddTaskOneParam(configSetting, (ConfigSetting setConf) =>
            {
                ConfigJson tmpjson = new ConfigJson();
                if (!Directory.Exists(configSetting.JsonPath))
                {
                    Directory.CreateDirectory(configSetting.JsonPath);
                    return tmpjson;
                }

                List<string> allFilePaths = EDTool.GetAllFilePath(configSetting.JsonPath);
                for (int i = 0; i < allFilePaths.Count; i++)
                {
                    FileInfo fileInfo = new FileInfo(allFilePaths[i]);
                    string dataJson = EDTool.ReadText(allFilePaths[i]);
                    ConfigGroup cfgGroup = JsonMapper.ToObject<ConfigGroup>(dataJson);
                    if (!string.IsNullOrEmpty(cfgGroup.Name))
                    {
                        tmpjson.ConfGroup.Add(fileInfo.Name, cfgGroup);
                    }
                }
                return tmpjson;
            }, (ConfigJson json) =>
            {
                configJson = json;
                AssetDatabase.Refresh();
            });

            AssetDatabase.Refresh();
        }

        private void OnDestroy()
        {
            EditorUtility.ClearProgressBar();
            Save(true);
            LCConfigLocate.ReLoadData = true;
        }

        private void Save(bool clearCache)
        {
            if (configSetting.AutoCreateJson)
            {
                //多线程写入
                TaskHelp.AddTaskTwoParam(configJson, configSetting, (ConfigJson configJson, ConfigSetting setJson) =>
                {
                    List<string> allFile = EDTool.GetAllFilePath(setJson.JsonPath);
                    for (int i = 0; i < allFile.Count; i++)
                    {
                        File.Delete(allFile[i]);
                    }

                    foreach (var item in configJson.ConfGroup.Values)
                    {
                        string str = JsonMapper.ToJson(item);
                        EDTool.WriteText(str, setJson.JsonPath + "/" + item.Name);
                    }

                    return true;
                }, (bool x) =>
                {
                    string jsonData = JsonMapper.ToJson(configSetting);
                    EDTool.WriteText(jsonData, settingPath);
                    OnClose(clearCache);
                });
            }
            else
            {
                string jsonData = JsonMapper.ToJson(configSetting);
                EDTool.WriteText(jsonData, settingPath);
                OnClose(clearCache);
            }
        }

        int currCreateLuaIndex = 0;
        private List<ConfigGroup> currCreateLuaConfigGroupList = new List<ConfigGroup>();
        private void OnClose(bool clearCache)
        {
            if (configSetting.AutoCreateLuaConfig)
            {
                OnCreateLuaFile(configJson, clearCache);
            }
            else
            {
                if (clearCache)
                {
                    ClearCache();
                }
            }

            /**自动检测引用表
             if (configSetting.AutoCheckRelateIsLegal)
            {
                //自动检测引用表（目测很耗 ------ 多线程出马）
                TaskHelp.AddTaskTwoParam(configJson, configSetting, (ConfigJson json, ConfigSetting set) =>
                {
                    List<ConfigItem> configItems = LCConfigHelp.GetAllRelateConfigItems(json);
                    for (int i = 0; i < configItems.Count; i++)
                    {
                        ConfigItem configItem = configItems[i];
                        for (int j = 0; j < configItem.DataList.Count; j++)
                        {
                            string value = configItem.DataList[j];
                            if (string.IsNullOrEmpty(value))
                            {
                                continue;
                            }
                            if (!LCConfigHelp.CheckRelateDataIsLegal(json, configItem, value))
                            {
                                configItem.DataList[j] = "";
                            }
                        }
                    }
                    if (set.AutoCreateLuaConfig)
                    {
                        return configJson;
                    }
                    else
                    {
                        return null;
                    }

                }, (ConfigJson json) =>
                {
                    if (json != null)
                    {
                        OnCreateLuaFile(json);
                    }
                });
            }
            else
            {
                if (configSetting.AutoCreateLuaConfig)
                {
                    OnCreateLuaFile(configJson);
                }
            }

            **/
        }

        private void ClearCache()
        {
            EditorUtility.ClearProgressBar();
            configJson = null;
            configSetting = new ConfigSetting();
            AssetDatabase.Refresh();
        }

        private void OnCreateLuaFile(ConfigJson conf, bool clearCache)
        {
            Debug.LogError("OnCreateLuaFile》》》》》》》" + conf.ToString());

            if (conf.ConfGroup.Count <= 0)
            {
                if (clearCache)
                {
                    ClearCache();
                }
                return;
            }

            currCreateLuaIndex = 0;
            currCreateLuaConfigGroupList = new List<ConfigGroup>(conf.ConfGroup.Values);
            LCConfigToLua.LuaRootPath = configSetting.LuaRootPath;
            EditorApplication.update = delegate ()
            {
                if (currCreateLuaIndex >= currCreateLuaConfigGroupList.Count)
                {
                    EditorUtility.ClearProgressBar();
                    EditorApplication.update = null;
                    if (clearCache)
                    {
                        ClearCache();
                    }
                    return;
                }

                ConfigGroup currGroup = currCreateLuaConfigGroupList[currCreateLuaIndex];
                if (currGroup == null)
                {
                    if (clearCache)
                    {
                        ClearCache();
                    }
                    return;
                }

                currCreateLuaIndex++;
                EditorUtility.DisplayProgressBar("正在生成Lua文件", string.Format("正在生成配置分组{0}", currGroup.Name), (float)(currCreateLuaIndex) / currCreateLuaConfigGroupList.Count);
                LCConfigToLua.ConfigGroupToLua(currGroup);
            };
        }

        #region View

        private LCConfigGroupPanel configPanel = new LCConfigGroupPanel();
        private LCConfigSettingPanel settingPanel = new LCConfigSettingPanel();
        private LCConfigSearchPanel searchPanel = new LCConfigSearchPanel();
        private LCConfigEditorWindowShowType curShowType = LCConfigEditorWindowShowType.Search;

        private void OnGUI()
        {
            if (configJson == null)
            {
                return;
            }
            EDLayout.CreateHorizontal("", 1250, 800, () =>
            {
                ShowLeft(220, 800);
                ShowRight(position.width - 220, 800);
            });
        }

        private Vector2 LeftPos = Vector2.zero;
        private string SelConfigName = "";
        private void ShowLeft(float width, float height)
        {
            EDLayout.CreateScrollView(ref LeftPos, "box", width, height, () =>
            {
                //EDButton.CreateBtn("性能测试", 180, 35, () =>
                //{
                //    CreateTestConfigGroup();
                //});

                EDButton.CreateBtn("查找", 180, 35, () =>
                {
                    searchPanel.SetData(configJson);
                    curShowType = LCConfigEditorWindowShowType.Search;
                });

                EDButton.CreateBtn("设置", 180, 35, () =>
                {
                    settingPanel.SetData(configSetting, configJson);
                    curShowType = LCConfigEditorWindowShowType.Set;
                });

                //重载数据
                EDButton.CreateBtn("重载数据", 180, 35, () =>
                {
                    Save(false);
                    LCConfigLocate.ReLoadData = true;
                });

                EDButton.CreateBtn("新建配置", 180, 35, () =>
                {
                    EDPopPanel.PopWindow("输入配置名:", (string x) =>
                    {
                        if (configJson.ConfGroup.ContainsKey(x))
                        {
                            Debug.LogError("配置名重复" + x);
                            return;
                        }
                        ConfigGroup configGroup = new ConfigGroup(x);
                        configJson.ConfGroup.Add(x, configGroup);
                    });
                });

                foreach (ConfigGroup config in configJson.ConfGroup.Values)
                {
                    EDColor.DrawColorArea(SelConfigName == config.Name ? Color.green : Color.white, () =>
                    {
                        EDButton.CreateBtn(config.Name, 180, 35, () =>
                        {
                            SelConfigName = config.Name;
                            configPanel.SetData(configJson, config, SelConfigName);
                            curShowType = LCConfigEditorWindowShowType.ShowConfig;
                        });
                    });
                }
            });
        }

        private void ShowRight(float width, float height)
        {
            switch (curShowType)
            {
                case LCConfigEditorWindowShowType.Set:
                    settingPanel.Refresh();
                    break;
                case LCConfigEditorWindowShowType.Search:
                    searchPanel.Refresh();
                    break;
                case LCConfigEditorWindowShowType.ShowConfig:
                    if (!configJson.ConfGroup.ContainsKey(SelConfigName))
                    {
                        return;
                    }
                    ConfigGroup config = configJson.ConfGroup[SelConfigName];
                    configPanel.Refresh();
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region 性能测试

        private void CreateTestConfigGroup()
        {
            for (int i = 0; i < 30; i++)
            {
                ConfigGroup configGroup = new ConfigGroup("test" + i.ToString());
                for (int j = 0; j < 3; j++)
                {
                    Config config = new Config("sheetTest" + i.ToString() + j.ToString());

                    //数据
                    for (int z = 0; z < 5; z++)
                    {
                        ConfigItem configItem = new ConfigItem("itemName" + i.ToString() + j.ToString() + z.ToString());
                        configItem.Type = ConfigDataType.Int;
                        //记录
                        for (int a = 0; a < 2000; a++)
                        {
                            configItem.DataList.Add("0");
                        }

                        config.Items.Add(configItem);
                    }

                    configGroup.Configs.Add(config);
                }

                configJson.ConfGroup.Add("test" + i.ToString(), configGroup);
            }
        }

        #endregion
    }
}
