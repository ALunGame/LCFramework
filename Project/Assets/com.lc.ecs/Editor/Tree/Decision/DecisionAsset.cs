using LCNode.Model;
using UnityEditor;
using UnityEngine;
using LCToolkit;

namespace LCECS.Tree
{
    public class DecisionAsset : BaseGraphAsset<Decision>
    {
        [ReadOnly]
        [HideInInspector]
        public int TreeId;
    }
}
