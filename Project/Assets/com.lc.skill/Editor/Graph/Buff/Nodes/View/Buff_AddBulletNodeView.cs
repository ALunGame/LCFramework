using LCNode;
using LCNode.View;
using UnityEngine.UIElements;
using LCSkill.AoeGraph;
using LCSkill.BulletGraph;

namespace LCSkill.BuffGraph
{
    [CustomNodeView(typeof(Buff_LifeCycleAddBulletNode))]
    public class Buff_LifeCycleAddBulletNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Buff_LifeCycleAddBulletNodeView()
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
            Buff_LifeCycleAddBulletNode node = Model as Buff_LifeCycleAddBulletNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BulletGraphGroupAsset>();
            BulletGraphAsset asset = GraphSetting.Setting.GetAsset<BulletGraphAsset>(path.searchPath, "bullet_" + node.addBullet.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}