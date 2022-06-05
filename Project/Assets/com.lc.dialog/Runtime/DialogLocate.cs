using System.Collections;
using UnityEngine;

namespace LCDialog
{
    public static class DialogLocate
    {
        public static DialogLogServer Log = new DialogLogServer();

        public static IDialogConfigServer Config = new DialogConfigServer();

        public static IDialogDisplayServer Display;

        public static IDialogServer Dialog = new DialogServer();

        public static void Init()
        {
            Config.Init();
        }

        public static void SetDisplayServer(IDialogDisplayServer displayServer)
        {
            Display = displayServer;
        }

        public static void Clear()
        {
            Config.Clear();
        }
    }
}