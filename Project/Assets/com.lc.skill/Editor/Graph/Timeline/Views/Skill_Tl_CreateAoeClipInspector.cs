using LCNode;
using LCNode.View;
using LCSkill.AoeGraph;
using LCTimeline.Inspector;
using LCToolkit.Core;
using UnityEngine;

namespace LCSkill
{
    [CustomInspectorDrawer(typeof(Skill_Tl_CreateAoeClip))]
    public class Skill_Tl_CreateAoeClipInspector : BaseTimelineClipInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Skill_Tl_CreateAoeClip clip = Target as Skill_Tl_CreateAoeClip;
            if (GUILayout.Button("Open", GUILayout.Height(30)))
            {
                GraphGroupPath path = GraphSetting.Setting.GetSearchPath<AoeGraphGroupAsset>();
                AoeGraphAsset asset = GraphSetting.Setting.GetAsset<AoeGraphAsset>(path.searchPath,"aoe_" + clip.aoeId);
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}
