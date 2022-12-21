using LCNode;
using LCNode.View;
using LCSkill.BuffGraph;
using UnityEngine.UIElements;
using LCSkill.AoeGraph;
using LCSkill.BulletGraph;

namespace LCSkill.BulletGraph
{
    [CustomNodeView(typeof(Bullet_HitAddBulletNode))]
    public class Bullet_HitAddBulletNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Bullet_HitAddBulletNodeView()
        {
            btnOpenBuff = new Button();
            btnOpenBuff.text = "打开Bullet";
            controlsContainer.Add(btnOpenBuff);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            btnOpenBuff.clicked += OnClickOpenBuff;
        }

        private void OnClickOpenBuff()
        {
            Bullet_HitAddBulletNode node = Model as Bullet_HitAddBulletNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BulletGraphGroupAsset>();
            BulletGraphAsset asset = GraphSetting.Setting.GetAsset<BulletGraphAsset>(path.searchPath, "bullet_" + node.addBullet.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}