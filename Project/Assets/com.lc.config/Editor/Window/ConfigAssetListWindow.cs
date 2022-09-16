using LCToolkit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCConfig
{
    public class ConfigAssetListWindow : BaseEditorWindow
    {
        public static void Open()
        {
            var window = GetWindow<ConfigAssetListWindow>();
            window.titleContent = new GUIContent("配置列表");
            window.Init();
        }

        private Dictionary<string, ConfigAssetGroup> configDict = new Dictionary<string, ConfigAssetGroup>();
        private ConfigAssetGroup selGroup;
        private List<ConfigAsset> configs = new List<ConfigAsset>();

        public void Init()
        {
            configDict = ConfigSetting.GetAllConfigGroups();
        }

        private void OnGUI()
        {
            GUILayoutExtension.HorizontalGroup(() =>
            {
                GUILayoutExtension.VerticalGroup(() =>
                {
                    MiscHelper.Btn("刷新", 200, 35, () =>
                    {
                        Refresh();
                    });

                    foreach (var item in configDict)
                    {
                        GUIExtension.SetColor(item.Value == selGroup ? Color.green : Color.white, () =>
                        {
                            MiscHelper.Btn(item.Key, 200, 35, () =>
                            {
                                OnSelConfigGroup(item.Value);
                            });
                        });
                    }
                });

                GUILayoutExtension.VerticalGroup(() =>
                {
                    for (int i = 0; i < configs.Count; i++)
                    {
                        MiscHelper.Btn(configs[i].name, 200, 35, () =>
                        {
                            ConfigAssetWindow.Open(configs[i]);
                        });
                    }
                });
            });
        }

        private void OnSelConfigGroup(ConfigAssetGroup group)
        {
            selGroup = group;
            configs = selGroup.GetAllAsset();
        }

        private void Refresh()
        {
            Init();
            selGroup = null;
        }
    }
}