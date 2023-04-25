using LCGAS;
using LCToolkit;
using LCToolkit.Core;
using UnityEditor;

namespace LCSkill.Common
{
    [CustomInspectorDrawer(typeof(RaycastClip))]
    internal class RaycastClipInspector : ObjectInspectorDrawer
    {
        private bool OpenTag = false;
        private bool OpenShape = false;
        private bool OpenOnActorEnter = false;
        private bool OpenOnActorExit = false;
        
        public override void OnInspectorGUI()
        {
            RaycastClip raycastClip = (RaycastClip)Target;

            //标签
            GUILayoutExtension.FoldoutGroup("标签", OpenTag, () =>
            {
                OpenTag = true;
                raycastClip.tag = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer), raycastClip.tag);
            }, () =>
            {
                OpenTag = false;
            });
            
            //射线形状
            GUILayoutExtension.FoldoutGroup("射线形状", OpenShape, () =>
            {
                OpenShape = true;
                raycastClip.areaShape = (Shape)GUILayoutExtension.DrawField(typeof(Shape), raycastClip.areaShape);
                raycastClip.areaPos = EditorGUILayout.Vector2Field("射线位置", raycastClip.areaPos);
            }, () =>
            {
                OpenShape = false;
            });
            
            //演员进入
            GUILayoutExtension.FoldoutGroup("演员进入", OpenOnActorEnter, () =>
            {
                OpenOnActorEnter = true;
                raycastClip.onActorEnter = (RaycastClipEventData)GUILayoutExtension.DrawField(typeof(RaycastClipEventData), raycastClip.onActorEnter,"演员进入事件数据");
            }, () =>
            {
                OpenOnActorEnter = false;
            });
            
            //演员离开
            GUILayoutExtension.FoldoutGroup("演员离开", OpenOnActorExit, () =>
            {
                OpenOnActorExit = true;
                raycastClip.onActorExit = (RaycastClipEventData)GUILayoutExtension.DrawField(typeof(RaycastClipEventData), raycastClip.onActorExit,"演员离开事件数据");
            }, () =>
            {
                OpenOnActorExit = false;
            });
        }
    }
}