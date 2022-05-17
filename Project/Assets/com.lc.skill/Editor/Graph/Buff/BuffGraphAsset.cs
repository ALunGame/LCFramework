using LCNode.Model;
using LCToolkit;
using UnityEngine;

namespace LCSkill.BuffGraph
{
    public class BuffGraphAsset : BaseGraphAsset<BuffGraph>
    {
        [ReadOnly]
        [Header("技能Id")]
        public string buffId;
    }
}
