using LCToolkit;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CreateAssetMenu(fileName = "GE组", menuName = "GAS/GE组", order = 2)]
    internal class GEGroupAsset : GroupAsset<GEAsset>
    {
        public override string DisplayName { get => "GE"; }
        
        public override void OpenChildAsset(GroupChildAsset pAsset)
        {
            Selection.activeObject = pAsset;
        }

        public override string ExportChildAsset(GroupChildAsset pAsset)
        {
            return null;
        }
    }
}