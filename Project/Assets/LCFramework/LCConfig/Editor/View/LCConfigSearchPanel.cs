using LCHelp;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCConfig
{
    public class SearchShowConfigGroup
    {
        public string Name = "";
        //config名字 和 数据索引
        public Dictionary<string, Dictionary<string, List<int>>> ConfigDataDict = new Dictionary<string, Dictionary<string, List<int>>>();

        public SearchShowConfigGroup(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// 配置搜索
    /// </summary>
    public class LCConfigSearchPanel
    {
        private ConfigJson configJson;
        private List<string> SearchStr = new List<string> { "全局搜索", "配置组搜索", "配置搜索" };

        public void SetData(ConfigJson json)
        {
            configJson = json;
        }

        public void Refresh()
        {
            if (configJson == null)
            {
                return;
            }
            EDLayout.CreateVertical("", 1000, 800, () =>
            {
                ShowTop(1000, 70);
                ShowMiddle(1000, 600);
            });
        }

        #region Top

        private string searchData = "";
        private ConfigDataType searchDataType = ConfigDataType.Int;
        private void ShowTop(float width, float height)
        {
            EDLayout.CreateVertical("", width, height, () =>
            {
                EDLayout.CreateHorizontal("box", width, 60, () =>
                {
                    EDLayout.CreateVertical("box", 200, 30, () =>
                    {
                        EditorGUILayout.LabelField("输入查询值：", GUILayout.Width(200), GUILayout.Height(25));
                        searchData = EditorGUILayout.TextField(searchData, GUILayout.Width(200), GUILayout.Height(25));
                    });

                    EDLayout.CreateVertical("box", 200, 30, () =>
                    {
                        EditorGUILayout.LabelField("选择值类型：", GUILayout.Width(200), GUILayout.Height(25));
                        searchDataType = (ConfigDataType)EditorGUILayout.EnumPopup(searchDataType, GUILayout.Width(100), GUILayout.Height(25));
                    });

                    EDButton.CreateBtn("查询", 200, 60, () =>
                    {
                        OnClickSearch();
                    });
                });
            });
        }

        #endregion

        #region Middle

        private SearchShowConfigGroup curSelGroup = null;
        private Config curSelConfig = null;
        private string curSelConfigItemName = "";
        private List<int> curSelSearchDataIndex = null;
        private List<bool> curSelOpenSearchData = null;

        private List<SearchShowConfigGroup> curShowSeachData = null;
        private void OnClickSearch()
        {
            curSelGroup = null;
            curSelConfig = null;
            curSelConfigItemName = "";
            curShowSeachData = null;
            curSelSearchDataIndex = null;
            curSelOpenSearchData = null;

            curShowSeachData = LCConfigHelp.GetSearchData(configJson, searchData, searchDataType);
        }

        private Vector2 pos01 = Vector2.zero;
        private Vector2 pos02 = Vector2.zero;
        private Vector2 pos03 = Vector2.zero;
        private Vector2 pos04 = Vector2.zero;
        private void ShowMiddle(float width, float height)
        {
            if (curShowSeachData == null)
            {
                EditorGUILayout.LabelField("没有查询的数据>>>>>>>>  ", GUILayout.Width(width), GUILayout.Height(50));
                return;
            }

            EDLayout.CreateHorizontal("", width, height, () =>
            {
                //分组框
                EDLayout.CreateScrollView(ref pos01, "box", 230, height, () =>
                {
                    EditorGUILayout.LabelField("查询结果的分组", GUILayout.Width(180), GUILayout.Height(25));

                    for (int i = 0; i < curShowSeachData.Count; i++)
                    {
                        SearchShowConfigGroup showConfigGroup = curShowSeachData[i];
                        EDColor.DrawColorArea(curSelGroup != null && curSelGroup.Name == showConfigGroup.Name ? Color.green : Color.white, () =>
                        {
                            EDButton.CreateBtn(showConfigGroup.Name, 180, 25, () =>
                            {
                                curSelGroup = showConfigGroup;
                                curSelConfig = null;
                                curSelConfigItemName = "";
                                curSelSearchDataIndex = null;
                                curSelOpenSearchData = null;
                            });
                        });
                    }
                });

                if (curSelGroup == null)
                {
                    return;
                }

                //配置列表
                EDLayout.CreateScrollView(ref pos02, "box", 230, height, () =>
                {
                    foreach (string item in curSelGroup.ConfigDataDict.Keys)
                    {
                        EDColor.DrawColorArea(curSelConfig != null && curSelConfig.Name == item ? Color.green : Color.white, () =>
                        {
                            EDButton.CreateBtn(item, 180, 25, () =>
                            {
                                curSelConfig = LCConfigHelp.GetConfig(configJson.ConfGroup[curSelGroup.Name], item);
                                curSelConfigItemName = "";
                                curSelSearchDataIndex = null;
                                curSelOpenSearchData = null;
                            });
                        });
                    }
                });

                if (curSelConfig == null)
                {
                    return;
                }

                //配置项列表
                EDLayout.CreateScrollView(ref pos03, "box", 230, height, () =>
                {
                    foreach (string itemName in curSelGroup.ConfigDataDict[curSelConfig.Name].Keys)
                    {
                        EDColor.DrawColorArea(curSelConfigItemName != "" && curSelConfigItemName == itemName ? Color.green : Color.white, () =>
                        {
                            EDButton.CreateBtn(itemName, 180, 25, () =>
                            {
                                curSelConfigItemName = itemName;
                                curSelSearchDataIndex = curSelGroup.ConfigDataDict[curSelConfig.Name][itemName];
                                curSelOpenSearchData = new List<bool>();
                                for (int i = 0; i < curSelSearchDataIndex.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        curSelOpenSearchData.Add(true);
                                    }
                                    else
                                    {
                                        curSelOpenSearchData.Add(false);
                                    }
                                }
                            });
                        });
                    }
                });

                if (curSelConfigItemName == "" || curSelSearchDataIndex == null || curSelOpenSearchData == null)
                {
                    return;
                }

                //最终的数据
                //配置项列表
                EDLayout.CreateScrollView(ref pos04, "box", 230, height, () =>
                {
                    for (int i = 0; i < curSelOpenSearchData.Count; i++)
                    {
                        curSelOpenSearchData[i] = EditorGUILayout.Foldout(curSelOpenSearchData[i], string.Format("第{0}条", i + 1));
                        if (curSelOpenSearchData[i])
                        {
                            int showDataIndex = curSelSearchDataIndex[i];

                            EditorGUILayout.Space();
                            EditorGUILayout.LabelField(string.Format("配置项 {0} 是 {1} 数据：", curSelConfigItemName, searchData), GUILayout.Width(180), GUILayout.Height(25));
                            EditorGUILayout.Space();

                            for (int j = 0; j < curSelConfig.Items.Count; j++)
                            {
                                if (curSelConfig.Items[j].Name != curSelConfigItemName)
                                {
                                    ConfigItem item1 = curSelConfig.Items[j];
                                    string value = item1.DataList[showDataIndex];
                                    string showName = string.Format("{0}：", item1.Name);
                                    EDLayout.CreateVertical("box", 190, 60, () =>
                                    {
                                        GUILayout.Label(showName, GUILayout.Width(180), GUILayout.Height(25));
                                        GUILayout.Label(value, GUILayout.Width(180), GUILayout.Height(25));
                                    });
                                }
                            }
                        }
                    }
                });
            });
        }

        #endregion
    }
}
