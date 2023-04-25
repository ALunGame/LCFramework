using System.Collections.Generic;
using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(AttributeName))]
    public class AttributeNameDrawer : ObjectDrawer
    {
        private static List<string> attrNameList = new List<string>();

        public override object OnGUI(Rect _position, GUIContent _label)
        {
            AttributeName attrName = (AttributeName)Target;

            GUILayoutExtension.HorizontalGroup(() =>
            {
                EditorGUILayout.LabelField(_label);
                MiscHelper.Dropdown(attrName.Name == "" ? "选择属性名":attrName.Name,GetAllAttrNames(), (string selName) =>
                {
                    attrName.Name = selName;
                });
            });
            
            
            return attrName;
        }

        public override float GetHeight()
        {
            return 0;
        }

        public List<string> GetAllAttrNames()
        {
            if (attrNameList.IsLegal())
            {
                return attrNameList;
            }
            foreach (var item in LCToolkit.ReflectionHelper.GetChildTypes<AttributeValue>())
            {
                if (item.IsAbstract)
                    continue;
                AttributeValue value = LCToolkit.ReflectionHelper.CreateInstance(item) as AttributeValue;
                if (attrNameList.Contains(value.Name))
                {
                    LCGAS.GASLocate.Log.LogError("属性名重复",value.Name);
                    continue;
                }
                attrNameList.Add(value.Name);
            }
            return attrNameList;
        }
    }
}