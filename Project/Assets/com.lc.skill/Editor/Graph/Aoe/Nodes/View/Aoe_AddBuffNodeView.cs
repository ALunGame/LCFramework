using LCNode;
using LCNode.View;
using LCSkill.BuffGraph;
using UnityEngine.UIElements;

namespace LCSkill.AoeGraph
{
    [CustomNodeView(typeof(Aoe_LifeCycleAddBuffNode))]
    public class Aoe_LifeCycleAddBuffNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Aoe_LifeCycleAddBuffNodeView()
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
            Aoe_LifeCycleAddBuffNode node = Model as Aoe_LifeCycleAddBuffNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BuffGraphGroupAsset>();
            BuffGraphAsset asset = GraphSetting.Setting.GetAsset<BuffGraphAsset>(path.searchPath, "buff_" + node.addBuff.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }

    [CustomNodeView(typeof(Aoe_ActorEnterAddBuffNode))]
    public class Aoe_ActorEnterAddBuffNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Aoe_ActorEnterAddBuffNodeView()
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
            Aoe_ActorEnterAddBuffNode node = Model as Aoe_ActorEnterAddBuffNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BuffGraphGroupAsset>();
            BuffGraphAsset asset = GraphSetting.Setting.GetAsset<BuffGraphAsset>(path.searchPath, "buff_" + node.addBuff.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }

    [CustomNodeView(typeof(Aoe_ActorLeaveAddBuffNode))]
    public class Aoe_ActorLeaveAddBuffNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Aoe_ActorLeaveAddBuffNodeView()
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
            Aoe_ActorLeaveAddBuffNode node = Model as Aoe_ActorLeaveAddBuffNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BuffGraphGroupAsset>();
            BuffGraphAsset asset = GraphSetting.Setting.GetAsset<BuffGraphAsset>(path.searchPath, "buff_" + node.addBuff.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}