using LCNode.Model;
using LCToolkit;
using UnityEngine;

namespace LCSkill.AoeGraph
{
    public class AoeGraphAsset : BaseGraphAsset<AoeGraph>
    {
        [EDReadOnly]
        [Header("AoeId")]
        public string aoeId;
    }
}
