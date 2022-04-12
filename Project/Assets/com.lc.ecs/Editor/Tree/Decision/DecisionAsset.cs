using LCNode.Model;
using UnityEditor;
using UnityEngine;

namespace LCECS.Tree
{
    public class DecisionAsset : BaseGraphAsset<Decision>
    {
        [HideInInspector]
        public int TreeId;
    }
}
