using LCNode;
using LCNode.View;
using LCSkill.BuffGraph;
using UnityEngine.UIElements;

namespace LCSkill.SkillGraph.View
{
    [CustomNodeView(typeof(Skill_LearnBuffNode))]
    public class Skill_LearnBuffNodeView : BaseNodeView
    {
        Button btnOpenBuff;
        public Skill_LearnBuffNodeView()
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
            Skill_LearnBuffNode node = Model as Skill_LearnBuffNode;
            GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BuffGraphGroupAsset>();
            BuffGraphAsset asset = GraphSetting.Setting.GetAsset<BuffGraphAsset>(path.searchPath, "buff_" + node.id);
            if (asset != null)
            {
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}