using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomObjectDrawer(typeof(GameplayEffectPeriod))]
    public class GameplayEffectPeriodDrawer : ObjectDrawer
    {
        public override object OnGUI(Rect _position, GUIContent _label)
        {
            GameplayEffectPeriod period = (GameplayEffectPeriod) Target;
            period.period = EditorGUILayout.FloatField("周期(秒)",period.period);
            period.executeOnActive = EditorGUILayout.Toggle("激活立即执行",period.executeOnActive);
            return period;
        }

        public override float GetHeight()
        {
            return 0;
        }
    }
}