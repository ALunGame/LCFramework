using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using LCToolkit;

namespace LCToolkit
{
    [CustomEditor(typeof(LockTransCom), true)]
    public class LockTransComInspector : Editor
    {
        protected static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();

        public override void OnInspectorGUI()
        {
            if (!ContextDataCache.TryGetContextData<GUIStyle>("BigLabel", out var bigLabel))
            {
                bigLabel.value = new GUIStyle(GUI.skin.label);
                bigLabel.value.fontSize = 18;
                bigLabel.value.fontStyle = FontStyle.Bold;
                bigLabel.value.alignment = TextAnchor.MiddleLeft;
                bigLabel.value.stretchWidth = true;
            }

            LockTransCom lockTransCom = target as LockTransCom;

            GUILayoutExtension.VerticalGroup(() => {
                lockTransCom.lockPos = EditorGUILayout.Toggle("锁定位置", lockTransCom.lockPos);
                if (lockTransCom.lockPos)
                    lockTransCom.lockPosValue = EditorGUILayout.Vector3Field("位置:", lockTransCom.lockPosValue);
            });

            GUILayoutExtension.VerticalGroup(() => {
                lockTransCom.lockRoate = EditorGUILayout.Toggle("锁定旋转", lockTransCom.lockRoate);
                if (lockTransCom.lockRoate)
                    lockTransCom.lockRoateValue = EditorGUILayout.Vector3Field("旋转:", lockTransCom.lockRoateValue);
            });

            GUILayoutExtension.VerticalGroup(() => {
                lockTransCom.lockScale = EditorGUILayout.Toggle("锁定缩放", lockTransCom.lockScale);
                if (lockTransCom.lockScale)
                    lockTransCom.lockScaleValue = EditorGUILayout.Vector3Field("缩放:", lockTransCom.lockScaleValue);
            });
        }
    }
}
