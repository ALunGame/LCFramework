using System;
using System.IO;
using IAToolkit;
using LCToolkit;
using UnityEditor;
using UnityEngine;
using IOHelper = LCToolkit.IOHelper;

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
    }
}