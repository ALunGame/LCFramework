using System;
using System.IO;
using LCNode.Model;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace LCToolkit
{
    public abstract class GroupChildAsset : ScriptableObject
    {
        public virtual void Open(object pOwner = null, Action _clickBackFunc = null)
        {
            Selection.activeObject = this;
        }

        public InternalGroupAsset GetGroupAsset()
        {
            string path = AssetDatabase.GetAssetPath(this);
            path = path.Replace(name + ".asset", "");
            
            string[] guids = AssetDatabase.FindAssets("t:InternalGroupAsset", new []{path});
            if (guids == null || guids.Length <= 0)
            {
                return null;
            }
            
            string groupAssetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            InternalGroupAsset groupAsset = AssetDatabase.LoadAssetAtPath<InternalGroupAsset>(groupAssetPath);
            return groupAsset;
        }
        
        public virtual string FileName(InternalGroupAsset pGroupAsset = null)
        {
            InternalGroupAsset groupAsset = pGroupAsset??GetGroupAsset();
            return groupAsset == null? "" : $"{groupAsset.name}_{name}";
        }

        /// <summary>
        /// 导出
        /// </summary>
        public virtual void Export(object pData, InternalGroupAsset pGroupAsset = null)
        {
            InternalGroupAsset groupAsset = pGroupAsset ?? GetGroupAsset();
            if (groupAsset == null)
            {
                Debug.LogError("没有在组里，无法导出"+name);
                return;
            }
            
            GroupPath groupPath = GroupAssetSetting.Setting.GetSearchPath(groupAsset.GetType().FullName);
            
            string fileName = $"{groupAsset.name}_{name}.{groupPath.exportExName}";
            string filePath = Path.Combine(groupPath.exportPath, fileName);
            string txt = ExportAsset(pData);
            
            IOHelper.WriteText(txt,filePath);
            
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 获得导出路径
        /// </summary>
        /// <returns></returns>
        public virtual string GetExportFilePath()
        {
            InternalGroupAsset groupAsset = GetGroupAsset();
            if (groupAsset == null)
            {
                Debug.LogError("没有在组里，无法导出"+name);
                return "";
            }
            
            GroupPath groupPath = GroupAssetSetting.Setting.GetSearchPath(groupAsset.GetType().FullName);
            string fileName = $"{groupAsset.name}_{name}.{groupPath.exportExName}";
            string filePath = Path.Combine(groupPath.exportPath, fileName);
            return filePath;
        }

        public virtual string ExportAsset(object pData)
        {
            return LCJson.JsonMapper.ToJson(pData);
        }

        /// <summary> 双击资源 </summary>
        [OnOpenAsset(0)]
        public static bool OnOpen(int instanceID, int line)
        {
            UnityObject go = EditorUtility.InstanceIDToObject(instanceID);
            if (go == null) return false;
            GroupChildAsset asset = go as GroupChildAsset;
            if (asset == null)
                return false;
            asset.Open();
            return true;
        }
    }
}