using LCNode;
using LCNode.View;
using LCSkill.BulletGraph;
using LCTimeline.Inspector;
using LCToolkit.Core;
using UnityEngine;

namespace LCSkill
{
    [CustomInspectorDrawer(typeof(Skill_Tl_CreateBulletClip))]
    public class Skill_Tl_CreateBulletClipInspector : BaseTimelineClipInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Skill_Tl_CreateBulletClip clip = Target as Skill_Tl_CreateBulletClip;
            if (GUILayout.Button("Open", GUILayout.Height(30)))
            {
                GraphGroupPath path = GraphSetting.Setting.GetSearchPath<BulletGraphGroupAsset>();
                BulletGraphAsset asset = GraphSetting.Setting.GetAsset<BulletGraphAsset>(path.searchPath, "bullet_" + clip.bulletId);
                BaseGraphWindow.JumpTo(asset);
            }
        }
    }
}

