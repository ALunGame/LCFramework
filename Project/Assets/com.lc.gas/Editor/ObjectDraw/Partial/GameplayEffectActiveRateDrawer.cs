using System.Collections.Generic;
using LCToolkit;
using LCToolkit.Core;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(GameplayEffectActiveRate))]
    public class GameplayEffectActiveRateDrawer : ObjectDrawer
    {
        public override object OnGUI(Rect _position, GUIContent _label)
        {
            GameplayEffectActiveRate rate = (GameplayEffectActiveRate) Target;
            rate.conditionTag = (CommonConditionTag)GUILayoutExtension.DrawField(typeof(CommonConditionTag), rate.conditionTag, "激活的条件标签","");
            rate.checkFuns = (List<string>)GUILayoutExtension.DrawField(typeof(List<string>), rate.checkFuns, "激活的检测函数","");
            rate.rateValue = (int)GUILayoutExtension.DrawField(typeof(int), rate.rateValue, "概率","（0-100）");
            return rate;
        }
        
        
        public override float GetHeight()
        {
            return 0;
        }
    }
}