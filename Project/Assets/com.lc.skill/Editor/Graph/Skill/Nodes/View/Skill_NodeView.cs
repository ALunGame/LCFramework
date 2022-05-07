using LCNode;
using LCNode.View;
using UnityEngine.UIElements;

namespace LCSkill.SkillGraph.View
{
    [CustomNodeView(typeof(Skill_Node))]
    public class Skill_NodeView : BaseNodeView
    {
        Button btnOpenTimeline;
        public Skill_NodeView()
        {
            btnOpenTimeline = new Button();
            btnOpenTimeline.text = "打开Timeline";
            controlsContainer.Add(btnOpenTimeline);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            btnOpenTimeline.clicked += OnClickOpenTimeline;
        }

        private void OnClickOpenTimeline()
        {
            
        }
    }
}
