using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Demo
{
    public static class TempAssetHandle
    {
        [MenuItem("Assets/重命名切图")]
        public static void Create2DAnim()
        {
            Object[] guidArray = Selection.objects;
            if (guidArray == null || guidArray.Length != 1)
            {
                return;
            }
            Object selGuid = guidArray[0];

            Dictionary<string, string> nameDict = new Dictionary<string, string>();

            string filePath = AssetDatabase.GetAssetPath(selGuid);

            Object[] allAsset = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(selGuid));
            for (int i = 0; i < allAsset.Length; i++)
            {
                Object asset = allAsset[i];
                if (asset != null && asset != selGuid)
                {
                    Debug.Log(">>>>>" + asset.name);
                    string name = asset.name;
                    name = name.Replace("TX Village Props", "");
                    string[] endNames = name.Split(' ');
                    string endName = selGuid.name;
                    foreach (var item in endNames)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            endName = endName + "_" + item.ToLower();
                        }
                    }
                    Debug.Log("endName>>>>>" + endName);
                    //asset.name = endName;

                    nameDict.Add(asset.name, endName);
                }
            }

            string metaFile = Path.Combine(Path.GetDirectoryName(filePath), selGuid.name + ".png.meta");
            string metaStr = File.ReadAllText(metaFile);
            foreach (var item in nameDict)
            {
                metaStr = metaStr.Replace(item.Key, item.Value);
            }
            Debug.Log(">>>>>" + metaStr);
            File.WriteAllText(metaFile, metaStr);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }



        [MenuItem("GameObject/重新引用图片")]
        public static void RefreshSprite()
        {
            GameObject selGo = Selection.activeGameObject;
            if (selGo == null)
                return;
            List<SpriteRenderer> sprites = new List<SpriteRenderer>();
            if (selGo.GetComponent<SpriteRenderer>()!=null)
            {
                sprites.Add(selGo.GetComponent<SpriteRenderer>());
            }
            foreach (var sprite in selGo.GetComponentsInChildren<SpriteRenderer>())
            {
                sprites.Add(sprite);
            }

            Object[] allAsset = AssetDatabase.LoadAllAssetsAtPath("Assets/Demo/Asset/Actors/Item/Sprite/tx_item.png");
            foreach (var sprite in sprites)
            {
                string newName = GetAssetName(sprite.sprite.name);
                foreach (var asset in allAsset)
                {
                    if (asset.name == newName)
                    {
                        sprite.name = asset.name;
                        sprite.sprite = (Sprite)asset;
                        break;
                    }
                }
            }
        }

        private static string GetAssetName(string oldName)
        {
            string name = oldName;
            name = name.Replace("TX Village Props", "");
            string[] endNames = name.Split(' ');
            string endName = "tx_item";
            foreach (var item in endNames)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    endName = endName + "_" + item.ToLower();
                }
            }
            return endName;
        }
    }
}