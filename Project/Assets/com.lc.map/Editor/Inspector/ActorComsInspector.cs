using LCECS.Core;
using LCToolkit;
using LCToolkit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LCMap
{
    [CustomInspectorDrawer(typeof(List<BaseCom>))]
    public class ActorComsInspector : ObjectInspectorDrawer
    {
        static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();
        static List<Type> CreateComs = new List<Type>();

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

            if (CreateComs.Count == 0)
            {
                foreach (var item in ReflectionHelper.GetChildTypes<BaseCom>())
                {
                    CreateComs.Add(item);
                }
            }

            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayout.Label("组件列表", bigLabel.value);
            });

            List<BaseCom> coms = Target as List<BaseCom>;

            if (GUILayout.Button("创建组件",GUILayout.Height(35)))
            {
                List<Type> leftTypes = CreateComs.Where(x => coms.All(value => value.GetType() != x)).ToList();
                MiscHelper.Menu<Type>(leftTypes, (Type selType) =>
                {
                    BaseCom baseCom = (BaseCom)ReflectionHelper.CreateInstance(selType);
                    coms.Add(baseCom);
                }, 
                (Type type) =>
                {
                    return type.Name;
                });
            }

            for (int i = 0; i < coms.Count; i++)
            {
                DrawCom(coms[i]);
            }
        }

        private void DrawCom(BaseCom baseCom)
        {
            GUILayoutExtension.VerticalGroup(() =>
            {
                GUILayoutExtension.DrawField(baseCom.GetType(), baseCom);
                if (GUILayout.Button("删除", GUILayout.Height(35)))
                {
                    List<BaseCom> coms = Target as List<BaseCom>;
                    coms.Remove(baseCom);
                }
            });
        }
    }
}