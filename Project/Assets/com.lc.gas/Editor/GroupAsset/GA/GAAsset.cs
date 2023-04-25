using System;
using System.IO;
using LCToolkit;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    internal class GAAsset : GroupChildAsset
    {
        public string typeFullName = "";
        public Action clickBackFunc = null;
        
        public override void Open(object pOwner = null, Action _clickBackFunc = null)
        {
            clickBackFunc = _clickBackFunc;
            base.Open(pOwner, _clickBackFunc);
        }

        public override string FileName(InternalGroupAsset pGroupAsset = null)
        {
            return $"{typeFullName}";
        }

        public override string GetExportFilePath()
        {
            InternalGroupAsset groupAsset = GetGroupAsset();
            if (groupAsset == null)
            {
                Debug.LogError("没有在组里，无法导出"+name);
                return "";
            }
            
            GroupPath groupPath = GroupAssetSetting.Setting.GetSearchPath(groupAsset.GetType().FullName);
            string fileName = $"{typeFullName}.{groupPath.exportExName}";
            string filePath = Path.Combine(groupPath.exportPath, fileName);
            return filePath;
        }

        public InternalGameplayAbility GetAsset()
        {
            string filePath = GetExportFilePath();
            if (string.IsNullOrEmpty(filePath))
            {
                return new InternalGameplayAbility();
            }

            string text = IOHelper.ReadText(filePath);
            var asset = LCJson.JsonMapper.ToObject(text,ReflectionHelper.GetType(typeFullName));
            if (asset == null)
                asset = new InternalGameplayAbility();
            return asset as InternalGameplayAbility;
        }

        public override void Export(object pData, InternalGroupAsset pGroupAsset = null)
        {
            InternalGroupAsset groupAsset = pGroupAsset ?? GetGroupAsset();
            if (groupAsset == null)
            {
                Debug.LogError("没有在组里，无法导出"+name);
                return;
            }
            
            GroupPath groupPath = GroupAssetSetting.Setting.GetSearchPath(groupAsset.GetType().FullName);
            
            string fileName = $"{typeFullName}.{groupPath.exportExName}";
            string filePath = Path.Combine(groupPath.exportPath, fileName);
            string txt = ExportAsset(pData);
            
            IOHelper.WriteText(txt,filePath);
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}