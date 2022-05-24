using LCNode;
using LCNode.View;
using LCSkill.BuffGraph;
using LCTimeline.Inspector;
using LCToolkit.Core;
using UnityEngine;

namespace LCSkill
{
    [CustomInspectorDrawer(typeof(Skill_Tl_CreateBuffClip))]
    public class Skill_Tl_CreateBuffClipInspector : BaseTimelineClipInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Skill_Tl_CreateBuffClip clip = Target as Skill_Tl_CreateBuffClip;
            if (GUILayout.Button("Open", GUILayout.Height(30)))
            {
                GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BuffGraphGroupAsset>();
                BuffGraphAsset asset = GraphSetting.Setting.GetAsset<BuffGraphAsset>(path.searchPath, "buff_" + clip.addBuff.id);
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}
