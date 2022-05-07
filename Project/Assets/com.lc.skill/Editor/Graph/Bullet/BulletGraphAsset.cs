using LCNode.Model;
using LCToolkit;
using UnityEngine;

namespace LCSkill.BulletGraph
{
    public class BulletGraphAsset : BaseGraphAsset<BulletGraph>
    {
        [EDReadOnly]
        [Header("子弹Id")]
        public int bulletId;
    }
}
