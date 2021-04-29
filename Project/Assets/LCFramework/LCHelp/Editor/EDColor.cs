using System;
using UnityEngine;

namespace LCHelp
{
    public class EDColor
    {
        public static void DrawColorArea(Color color, Action callBack)
        {
            GUI.color = color;
            callBack?.Invoke();
            GUI.color = Color.white;
        }
    }
}
