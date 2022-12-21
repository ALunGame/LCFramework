using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCTask
{
    public static class TaskLocate
    {
        public static TaskLogServer Log = new TaskLogServer();
        public static TaskConfigServer Config;
        public static TaskServer Task;

        public static void Init()
        {
            Config = new TaskConfigServer();
            Config.Init();
            
            Task = new TaskServer();
            Task.Init();
        }

        public static void Clear()
        {
        }
    } 
}
