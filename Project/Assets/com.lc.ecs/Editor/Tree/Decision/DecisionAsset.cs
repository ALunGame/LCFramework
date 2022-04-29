using LCNode.Model;
using UnityEditor;
using UnityEngine;
using LCToolkit;

namespace LCECS.Tree
{
    public class DecisionAsset : BaseGraphAsset<Decision>
    {
        [EDReadOnly]
        [HideInInspector]
        public int TreeId;
    }
}
