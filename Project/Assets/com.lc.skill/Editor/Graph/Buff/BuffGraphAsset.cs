using LCNode.Model;
using LCToolkit;
using UnityEngine;

namespace LCSkill.BuffGraph
{
    public class BuffGraphAsset : BaseGraphAsset<BuffGraph>
    {
        [EDReadOnly]
        [Header("技能Id")]
        public int buffId;
    }
}
