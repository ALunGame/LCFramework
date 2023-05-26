using System.Collections.Generic;
using Demo.AStar.Com;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo.AStar
{
    [CustomEditor(typeof(PathGridCom))]
    public class PathGridComInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PathGridCom gridCom = (PathGridCom)target;
            if (GUILayout.Button("创建连接点", GUILayout.Height(30)))
            {
                GameObject connectGo = new GameObject("connect");
                connectGo.transform.SetParent(gridCom.transform.parent);
                connectGo.AddComponent<PathGridConnectCom>();
            }
        }
    }
}