using System.Collections;
using UnityEngine;

namespace LCUI
{
    public static class UILocate
    {
        public static UIServer UI { get; private set; }

        public static UILogServer Log { get; private set; }

        static UILocate()
        {
            UI = new UIServer();
            Log = new UILogServer();
        }
    }
}