using LCNode;
using LCNode.View;
using LCTimeline;
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
            Skill_Node node = Model as Skill_Node;
            TimelineGroupPath path = TimelineSetting.Setting.GetSearchPath<SkillTimelineGraphGroupAsset>();
            SkillTimelineGraphAsset asset = TimelineSetting.Setting.GetAsset<SkillTimelineGraphAsset>(path.searchPath, "timeline_" + node.timeline);
            if (asset != null)
            {
                TimelineWindow.Open(asset);
            }
        }
    }
}
