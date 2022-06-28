using LCECS.Core;
using LCToolkit;
using LCToolkit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LCMap
{
    [CustomInspectorDrawer(typeof(List<ActorInteractive>))]
    public class ActorInteractivesInspector : ObjectInspectorDrawer
    {
        static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();
        static List<Type> CreateInteractives = new List<Type>();

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

            if (CreateInteractives.Count == 0)
            {
                foreach (var item in ReflectionHelper.GetChildTypes<ActorInteractive>())
                {
                    CreateInteractives.Add(item);
                }
            }

            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label("交互列表", bigLabel.value);
            });

            List<ActorInteractive> interactives = Target as List<ActorInteractive>;

            if (GUILayout.Button("创建交互", GUILayout.Height(35)))
            {
                List<Type> leftTypes = CreateInteractives.Where(x => interactives.All(value => value.GetType() != x)).ToList();
                MiscHelper.Menu<Type>(leftTypes, (Type selType) =>
                {
                    ActorInteractive interactive = (ActorInteractive)ReflectionHelper.CreateInstance(selType);
                    interactives.Add(interactive);
                },
                (Type type) =>
                {
                    return type.Name;
                });
            }

            for (int i = 0; i < interactives.Count; i++)
            {
                DrawInteractive(interactives[i]);
            }
        }

        private void DrawInteractive(ActorInteractive interactive)
        {
            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayoutExtension.DrawField(interactive.GetType(), interactive);
                if (GUILayout.Button("删除", GUILayout.Height(35)))
                {
                    List<ActorInteractive> interactives = Target as List<ActorInteractive>;
                    interactives.Remove(interactive);
                }
            });
        }
    }
}