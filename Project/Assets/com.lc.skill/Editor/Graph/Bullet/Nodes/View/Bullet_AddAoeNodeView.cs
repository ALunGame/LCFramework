using LCNode;
using LCNode.View;
using LCSkill.BuffGraph;
using UnityEngine.UIElements;
using LCSkill.AoeGraph;

namespace LCSkill.BulletGraph
{
    [CustomNodeView(typeof(Bullet_HitAddAoeNode))]
    public class Bullet_HitAddAoeNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Bullet_HitAddAoeNodeView()
        {
            btnOpenBuff = new Button();
            btnOpenBuff.text = "打开Buff";
            controlsContainer.Add(btnOpenBuff);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            btnOpenBuff.clicked += OnClickOpenBuff;
        }

        private void OnClickOpenBuff()
        {
            Bullet_HitAddAoeNode node = Model as Bullet_HitAddAoeNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<AoeGraphGroupAsset>();
            AoeGraphAsset asset = GraphSetting.Setting.GetAsset<AoeGraphAsset>(path.searchPath, "aoe_" + node.addAoe.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}