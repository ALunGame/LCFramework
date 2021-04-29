using Demo.Com;
using LCECS.Scene;
using LCHelp;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Demo
{
    [ComEditorView(typeof(AttributeCom))]
    public class AttributeComExView : ComEditorView
    {
        private Vector2 scrollPos = Vector2.zero;
        public override void OnDrawScene(float width, float height)
        {
            AttributeCom targetCom = TargetCom as AttributeCom;

            EDLayout.CreateScrollView(ref scrollPos, "box", width, height, () =>
            {
                foreach (string key in targetCom.AttrDict.Keys.ToArray())
                {

                    EDLayout.CreateHorizontal("box", width, 25, () =>
                    {
                        EditorGUILayout.LabelField("属性：" + key, GUILayout.Width(width), GUILayout.Height(25));

                        targetCom.AttrDict[key] = EditorGUILayout.FloatField("值：", targetCom.AttrDict[key], GUILayout.Width(width), GUILayout.Height(25));
                    });
                }
            });

        }
    }
}
