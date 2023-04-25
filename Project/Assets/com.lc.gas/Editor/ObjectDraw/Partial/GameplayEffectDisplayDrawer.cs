using System.Collections.Generic;
using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(GameplayEffectDisplay))]
    public class GameplayEffectDisplayDrawer : ObjectDrawer
    {
        public override object OnGUI(Rect _position, GUIContent _label)
        {
            GameplayEffectDisplay display = (GameplayEffectDisplay) Target;
            display.tags = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer),
                display.tags, "激活GC的标签");
            return display;
        }

        public override float GetHeight()
        {
            return 0;
        }
    }
}