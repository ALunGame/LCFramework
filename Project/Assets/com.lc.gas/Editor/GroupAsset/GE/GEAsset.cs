using System;
using IAToolkit;
using UnityEditor;
using UnityEngine;
using IOHelper = LCToolkit.IOHelper;

namespace LCGAS
{
    internal class GEAsset : GroupChildAsset
    {
        public Action clickBackFunc = null;
        
        public override void Open(object pOwner = null, Action _clickBackFunc = null)
        {
            clickBackFunc = _clickBackFunc;
            base.Open(pOwner, _clickBackFunc);
        }
        
        public GameplayEffect GetAsset()
        {
            string filePath = GetExportFilePath();
            if (string.IsNullOrEmpty(filePath))
            {
                return new GameplayEffect();
            }

            string text = IOHelper.ReadText(filePath);
            var asset = LCJson.JsonMapper.ToObject<GameplayEffect>(text);
            if (asset == null)
                asset = new GameplayEffect();
            return asset;
        }
    }
}