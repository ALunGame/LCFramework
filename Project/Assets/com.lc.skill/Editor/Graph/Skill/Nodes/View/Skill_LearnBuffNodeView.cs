using LCNode;
using LCNode.View;
using UnityEngine.UIElements;

namespace LCSkill.SkillGraph.View
{
    [CustomNodeView(typeof(Skill_Node))]
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

        }
    }
}