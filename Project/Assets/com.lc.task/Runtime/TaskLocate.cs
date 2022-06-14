using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCTask
{
    public static class TaskLocate
    {
        public static TaskLogServer Log = new TaskLogServer();
        public static TaskConfigServer Config = new TaskConfigServer();
        public static TaskServer Task;

        private static TaskGameData gameData;
        public static TaskGameData GameData 
        { 
            get 
            { 
                if (gameData == null)
                    gameData = LCToolkit.ToolkitLocate.GameData.GetGameData<TaskGameData>();
                return gameData; 
            } 
        }

        public static void Init()
        {
            Task = new TaskServer();
        }

        public static void Clear()
        {
            gameData = null;
        }
    } 
}
