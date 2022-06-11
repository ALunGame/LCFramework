using System.Collections;
using UnityEngine;

namespace LCMap
{
    public static class MapLocate
    {
        public const int AllMapId = -1;

        public static MapLogServer Log = new MapLogServer();
        public static MapServer Map = new MapServer();
        public static ActorServer Actor = new ActorServer();
    }
}