using LCNode;
using LCNode.View;
using LCSkill.BuffGraph;
using UnityEngine.UIElements;

namespace LCSkill.AoeGraph
{
    [CustomNodeView(typeof(Aoe_LifeCycleAddAoeNode))]
    public class Aoe_LifeCycleAddAoeNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Aoe_LifeCycleAddAoeNodeView()
        {
            btnOpenBuff = new Button();
            btnOpenBuff.text = "打开Aoe";
            controlsContainer.Add(btnOpenBuff);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            btnOpenBuff.clicked += OnClickOpenBuff;
        }

        private void OnClickOpenBuff()
        {
            Aoe_LifeCycleAddAoeNode node = Model as Aoe_LifeCycleAddAoeNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<AoeGraphGroupAsset>();
            AoeGraphAsset asset = GraphSetting.Setting.GetAsset<AoeGraphAsset>(path.searchPath, "aoe_" + node.addAoe.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }

    [CustomNodeView(typeof(Aoe_ActorEnterAddAoeNode))]
    public class Aoe_ActorEnterAddAoeNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Aoe_ActorEnterAddAoeNodeView()
        {
            btnOpenBuff = new Button();
            btnOpenBuff.text = "打开Aoe";
            controlsContainer.Add(btnOpenBuff);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            btnOpenBuff.clicked += OnClickOpenBuff;
        }

        private void OnClickOpenBuff()
        {
            Aoe_ActorEnterAddAoeNode node = Model as Aoe_ActorEnterAddAoeNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<AoeGraphGroupAsset>();
            AoeGraphAsset asset = GraphSetting.Setting.GetAsset<AoeGraphAsset>(path.searchPath, "aoe_" + node.addAoe.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }

    [CustomNodeView(typeof(Aoe_ActorLeaveAddAoeNode))]
    public class Aoe_ActorLeaveAddAoeNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Aoe_ActorLeaveAddAoeNodeView()
        {
            btnOpenBuff = new Button();
            btnOpenBuff.text = "打开Aoe";
            controlsContainer.Add(btnOpenBuff);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            btnOpenBuff.clicked += OnClickOpenBuff;
        }

        private void OnClickOpenBuff()
        {
            Aoe_ActorLeaveAddAoeNode node = Model as Aoe_ActorLeaveAddAoeNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<AoeGraphGroupAsset>();
            AoeGraphAsset asset = GraphSetting.Setting.GetAsset<AoeGraphAsset>(path.searchPath, "aoe_" + node.addAoe.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}