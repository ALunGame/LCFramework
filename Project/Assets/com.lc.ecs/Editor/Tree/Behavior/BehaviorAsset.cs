using LCNode.Model;
using UnityEditor;
using UnityEngine;
using LCToolkit;

namespace LCECS.Tree
{
    public class BehaviorAsset : BaseGraphAsset<Behavior>
    {
        [EDReadOnly]
        [Header("ÐÐÎªÇëÇó")]
        public RequestId ReqId;
    }
}
