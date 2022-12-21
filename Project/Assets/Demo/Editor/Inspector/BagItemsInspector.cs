using LCToolkit;
using LCToolkit.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    [CustomInspectorDrawer(typeof(List<BagItem>))]
    public class BagItemsInspector : ObjectInspectorDrawer
    {
        static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();

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

            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label("物品列表", bigLabel.value);
            });

            List<BagItem> bagItems = Target as List<BagItem>;

            if (GUILayout.Button("创建物品", GUILayout.Height(35)))
            {
                bagItems.Add(new BagItem());
            }

            for (int i = 0; i < bagItems.Count; i++)
            {
                DrawInteractive(bagItems[i]);
            }
        }

        private void DrawInteractive(BagItem bagItem)
        {
            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayoutExtension.DrawField(bagItem.GetType(), bagItem);
                if (GUILayout.Button("删除", GUILayout.Height(35)))
                {
                    List<BagItem> bagItems = Target as List<BagItem>;
                    bagItems.Remove(bagItem);
                }
            });
        }
    }
}
