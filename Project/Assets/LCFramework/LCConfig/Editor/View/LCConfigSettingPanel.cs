using LCHelp;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LCConfig
{
    /// <summary>
    /// 配置基础设置
    /// </summary>
    public class LCConfigSettingPanel
    {
        private ConfigSetting configSetting;
        private ConfigJson configJson;

        public void SetData(ConfigSetting json, ConfigJson cfJson)
        {
            configSetting = json;
            configJson = cfJson;
        }

        public void Refresh()
        {
            if (configSetting == null)
            {
                return;
            }
            EDLayout.CreateVertical("", 1000, 800, () =>
            {
                EDLayout.CreateVertical("box", 400, 30, () =>
                {
                    configSetting.ExcelRootPath = EditorGUILayout.TextField("表格导出目录:", configSetting.ExcelRootPath, GUILayout.Width(350), GUILayout.Height(28));
                });

                EditorGUILayout.Space();
                EDLayout.CreateVertical("box", 400, 30, () =>
                {
                    configSetting.LuaRootPath = EditorGUILayout.TextField("Lua生成目录:", configSetting.LuaRootPath, GUILayout.Width(350), GUILayout.Height(28));
                });

                EditorGUILayout.Space();
                EDLayout.CreateVertical("box", 400, 30, () =>
                {
                    configSetting.JsonPath = EditorGUILayout.TextField("编辑器Json生成目录:", configSetting.JsonPath, GUILayout.Width(350), GUILayout.Height(28));
                });

                EditorGUILayout.Space();
                EDLayout.CreateVertical("box", 400, 30, () =>
                {
                    configSetting.AutoCreateLuaConfig = EditorGUILayout.Toggle("自动生成Lua配置文件", configSetting.AutoCreateLuaConfig, GUILayout.Width(250), GUILayout.Height(28));
                });

                EditorGUILayout.Space();
                EDLayout.CreateVertical("box", 400, 30, () =>
                {
                    configSetting.AutoCreateJson = EditorGUILayout.Toggle("自动生成Json文件", configSetting.AutoCreateJson, GUILayout.Width(250), GUILayout.Height(28));
                });

                EditorGUILayout.Space();
                EDLayout.CreateVertical("box", 400, 30, () =>
                {
                    EDButton.CreateBtn("导出所有Excel", 200, 28, () =>
                    {
                        LCConfigToExcel.ExcelRootPath = configSetting.ExcelRootPath;
                        foreach (var item in configJson.ConfGroup.Values)
                        {
                            LCConfigToExcel.ConfigGroupToExcel(item);
                        }
                        EditorUtility.DisplayDialog("所有Excel导出完毕", string.Format("Excel导出路径为 {0}", Path.GetFullPath(configSetting.ExcelRootPath)), "确认");
                    });
                });

            });
        }
    }
}
