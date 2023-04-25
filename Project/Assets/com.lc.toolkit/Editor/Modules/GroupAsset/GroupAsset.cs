using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LCToolkit
{
    public abstract class InternalGroupAsset : ScriptableObject
    {
        public abstract string DisplayName { get; }

        public abstract string GetRootPath();
        
        public abstract List<GroupChildAsset> GetAllAssets();

        public abstract bool CheckHasAsset(string pName);

        public abstract void OnClickCreateBtn();

        public abstract GroupChildAsset CreateChildAsset(string pName);

        public abstract void RemoveChildAsset(string pName);

        public virtual void OpenChildAsset(GroupChildAsset pAsset)
        {
            pAsset.Open(this);
        }

        public abstract void OnClickExport();
        
        public abstract string ExportChildAsset(GroupChildAsset pAsset);
    }
    
    
    public abstract class GroupAsset<T> : InternalGroupAsset where T : GroupChildAsset
    {
        public override string GetRootPath()
        {
            string path = AssetDatabase.GetAssetPath(this);
            return path.Replace(name + ".asset", "");
        }

        public override List<GroupChildAsset> GetAllAssets()
        {
            List<GroupChildAsset> assets = new List<GroupChildAsset>();
            string rootPath = GetRootPath();
            if (string.IsNullOrEmpty(rootPath))
            {
                return new List<GroupChildAsset>();
            }
            
            string[] guids = AssetDatabase.FindAssets("t:GroupChildAsset", new []{GetRootPath()});
            if (guids == null || guids.Length <= 0)
            {
                return new List<GroupChildAsset>();
            }
            
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GroupChildAsset asset = AssetDatabase.LoadAssetAtPath<GroupChildAsset>(path);
                assets.Add(asset);
            }
            
            return assets;
        }
        
        public override bool CheckHasAsset(string name)
        {
            List<GroupChildAsset> assets = GetAllAssets();
            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i].name == name)
                {
                    return true;
                }
            }
            return false;
        }


        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入{DisplayName}名：", (string name) =>
            {
                if (CheckHasAsset(name))
                {
                    MiscHelper.Dialog("创建失败","命名重复："+name);
                    return;
                }
                CreateChildAsset(name);
            });
        }

        public override GroupChildAsset CreateChildAsset(string pName)
        {
            string rootPath = GetRootPath();
            string filePath = rootPath + pName + ".asset";
            T asset = CreateInstance<T>();
            asset.name = name;
            AssetDatabase.CreateAsset(asset,filePath);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }
        
        public override void RemoveChildAsset(string pName)
        {
            List<GroupChildAsset> assets = GetAllAssets();
            for (int i = 0; i < assets.Count; i++)
            {
                if (assets[i].name == pName)
                {
                    DestroyImmediate(assets[i], true);
                }
            }
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public override void OnClickExport()
        {
            GroupPath groupPath = GroupAssetSetting.Setting.GetSearchPath(GetType().FullName);
            List<GroupChildAsset> assets = GetAllAssets();
            
            for (int i = 0; i < assets.Count; i++)
            {
                ExportChildAsset(assets[i]);
            }
        }
    }
}