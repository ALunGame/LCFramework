using System.Collections.Generic;
using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(GameplayEffectExpiration))]
    public class GameplayEffectExpirationDrawer : ObjectDrawer
    {
        public override object OnGUI(Rect _position, GUIContent _label)
        {
            GameplayEffectExpiration expiration = (GameplayEffectExpiration) Target;
            expiration.prematureEffectTags = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer),
                expiration.prematureEffectTags, "打断时添加的GE");
            
            expiration.routineEffectTags = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer),
                expiration.routineEffectTags, "正常结束时Apply的GE");
            
            return expiration;
        }

        public override float GetHeight()
        {
            return 0;
        }
    }
}