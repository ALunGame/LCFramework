using System;
using System.Collections.Generic;
using IAToolkit;
using LCToolkit;
using UnityEditor;
using UnityEngine;
using MiscHelper = LCToolkit.MiscHelper;

namespace LCGAS
{
    [CreateAssetMenu(fileName = "GA组", menuName = "GAS/GA组", order = 1)]
    internal class GAGroupAsset : GroupAsset<GAAsset>
    {
        public override string DisplayName { get =>"能力"; }

        public override Type ChildType { get => typeof(GAAsset); }

        public override void OpenChildAsset(GroupChildAsset pAsset)
        {
            Selection.activeObject = pAsset;
        }

        public override void OnClickCreateBtn()
        {
            List<string> typeNames = new List<string>();

            foreach (Type type in  ReflectionHelper.GetChildTypes<GameplayAbility>())
            {
                if (type.IsAbstract)
                {
                    continue;
                }
                
                typeNames.Add(type.FullName);
            }
            
            MiscHelper.Menu(typeNames, (string str) =>
            {
                CreateChildAsset(str);
            });
        }

        public override GroupChildAsset CreateChildAsset(string pName)
        {
            GroupChildAsset childAsset = base.CreateChildAsset(pName);
            GAAsset gaAsset = childAsset as GAAsset;
            gaAsset.typeFullName = pName;
            return gaAsset;
        }

        public override string ExportChildAsset(GroupChildAsset pAsset)
        {
            throw new System.NotImplementedException();
        }
    }
}