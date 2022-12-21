using LCNode.Model;
using UnityEditor;
using UnityEngine;
using LCToolkit;

namespace LCECS.Tree
{
    public class DecisionAsset : BaseGraphAsset<Decision>
    {
        [Header("决策树Id")]
        public int TreeId;
    }
}
