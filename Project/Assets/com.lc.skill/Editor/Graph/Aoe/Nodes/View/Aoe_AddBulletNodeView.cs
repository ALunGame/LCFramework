using LCNode;
using LCNode.View;
using LCSkill.BuffGraph;
using UnityEngine.UIElements;
using LCSkill.BulletGraph;

namespace LCSkill.AoeGraph
{
    [CustomNodeView(typeof(Aoe_LifeCycleAddBulletNode))]
    public class Aoe_LifeCycleAddBulletNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Aoe_LifeCycleAddBulletNodeView()
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
            Aoe_LifeCycleAddBulletNode node = Model as Aoe_LifeCycleAddBulletNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BulletGraphGroupAsset>();
            BulletGraphAsset asset = GraphSetting.Setting.GetAsset<BulletGraphAsset>(path.searchPath, "bullet_" + node.addBullet.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }

    [CustomNodeView(typeof(Aoe_ActorEnterAddBulletNode))]
    public class Aoe_ActorEnterAddBulletNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Aoe_ActorEnterAddBulletNodeView()
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
            Aoe_ActorEnterAddBulletNode node = Model as Aoe_ActorEnterAddBulletNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BulletGraphGroupAsset>();
            BulletGraphAsset asset = GraphSetting.Setting.GetAsset<BulletGraphAsset>(path.searchPath, "bullet_" + node.addBullet.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }

    [CustomNodeView(typeof(Aoe_ActorLeaveAddBulletNode))]
    public class Aoe_ActorLeaveAddBulletNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Aoe_ActorLeaveAddBulletNodeView()
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
            Aoe_ActorLeaveAddBulletNode node = Model as Aoe_ActorLeaveAddBulletNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BulletGraphGroupAsset>();
            BulletGraphAsset asset = GraphSetting.Setting.GetAsset<BulletGraphAsset>(path.searchPath, "bullet_" + node.addBullet.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}