using LCToolkit.Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCToolkit
{
    public static class ToolkitLocate
    {
        public static ILogServer Log = new ToolkitLogServer();

        public static IGoPoolServer GoPool = new GoPoolServer();

        public static GameDataServer GameData = new GameDataServer();

        public static void Init()
        {

        }

        public static void InitGameData(string gameDataUid)
        {
            GameData.Init(gameDataUid);
        }

        public static void Clear()
        {
            GameData.Clear();
        }
    } 
}
