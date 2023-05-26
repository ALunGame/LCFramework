using System.Collections;
using LCToolkit;
using UnityEngine;

namespace LCMap
{
    public static class MapLocate
    {
        public const int AllMapId = 0;

        public static MapLogServer Log = new MapLogServer();
        public static MapServer Map = new MapServer();

        public static void Init()
        {
            Log = new MapLogServer();
            Map = new MapServer();
        }

        public static void Clear()
        {
        }
    }
}