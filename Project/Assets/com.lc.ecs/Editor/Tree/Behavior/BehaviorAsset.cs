using LCNode.Model;
using UnityEditor;
using UnityEngine;

namespace LCECS.Tree
{
    public class BehaviorAsset : BaseGraphAsset<Behavior>
    {
        [HideInInspector]
        public int TreeId;
    }
}
