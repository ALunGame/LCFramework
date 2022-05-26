using LCTimeline;
using LCTimeline.Inspector;
using LCToolkit;
using LCToolkit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using LCToolkit;

namespace Demo
{
    [CustomInspectorDrawer(typeof(Skill_Tl_PlayAnimClip))]
    public class Skill_Tl_PlayAnimClip_Inspector : BaseTimelineClipInspector
    {
        public override void DrawFields(FieldInfo fieldInfo)
        {
            if (fieldInfo.Name == "anim")
            {
                GUIContent lable = new GUIContent(fieldInfo.Name);
                float tmpHeight  = GUIExtension.GetHeight(fieldInfo.FieldType, lable);

                UnityObjectAsset oldObj = (UnityObjectAsset)fieldInfo.GetValue(Target);
                ObjectDrawer objectDrawer = ObjectDrawer.CreateEditor(oldObj);
                objectDrawer.OnGUI(EditorGUILayout.GetControlRect(true, tmpHeight), lable);
                if (GUILayout.Button("设置为动画时长"))
                {
                    AnimationClip clip = (AnimationClip)oldObj.GetObj();
                    if (clip != null)
                    {
                        ClipModel clipModel = Target as ClipModel;
                        clipModel.SetEnd(clipModel.StartTime + clip.length);
                    }
                }
            }
            else
            {
                base.DrawFields(fieldInfo);
            }
        }
    }
}
