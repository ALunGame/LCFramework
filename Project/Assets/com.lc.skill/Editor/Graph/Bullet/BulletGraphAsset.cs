using LCNode.Model;
using LCToolkit;
using UnityEngine;

namespace LCSkill.BulletGraph
{
    public class BulletGraphAsset : BaseGraphAsset<BulletGraph>
    {
        [ReadOnly]
        [Header("子弹Id")]
        public string bulletId;
    }
}
