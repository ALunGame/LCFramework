using LCToolkit;
using UnityEditor;
using UnityEngine;

namespace LCMap
{
    [CustomEditor(typeof(ActorDisplay), true)]
    public class ActorDisplayInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ActorDisplay actorDisplay = (ActorDisplay)target;
            if (GUILayout.Button("刷新偏移",GUILayout.Height(30)))
            {
                actorDisplay.RefreshDisplayOffset();
            }
        }
    }
}
