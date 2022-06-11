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

        public static void Init()
        {
            Task = new TaskServer();
        }

        public static void Clear()
        {

        }
    } 
}
