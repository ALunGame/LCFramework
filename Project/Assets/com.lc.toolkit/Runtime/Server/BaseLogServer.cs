using System.Collections;
using System.Text;
using UnityEngine;

namespace LCToolkit.Server
{
    public enum LogType
    {
        Log,
        LogWarn,
        LogError,
    }

    public abstract class BaseLogServer : ILogServer
    {
        public abstract string LogTag { get; }

        private StringBuilder stringBuilder = new StringBuilder();
        private void PrintLog(LogType logTag, string log, params object[] args)
        {
            stringBuilder.Length = 0;
            switch (logTag)
            {
                case LogType.Log:
                    stringBuilder.Append($"[{LogTag}][LOG][I]:");
                    break;
                case LogType.LogWarn:
                    stringBuilder.Append($"[{LogTag}][LOG][W]:");
                    break;
                case LogType.LogError:
                    stringBuilder.Append($"[{LogTag}][LOG][E]:");
                    break;
            }

            if (args != null && args.Length > 0)
            {
                stringBuilder.Append(log);
                for (int i = 0; i < args.Length; i++)
                {
                    stringBuilder.Append(" "+args[i]);
                }
            }
            else
                stringBuilder.Append(log);

            string message = stringBuilder.ToString();
            switch (logTag)
            {
                case LogType.Log:
                    Debug.Log(message);
                    break;
                case LogType.LogWarn:
                    Debug.LogWarning(message);
                    break;
                case LogType.LogError:
                    Debug.LogError(message);
                    break;
            }
        }

        public void Log(string log, params object[] args)
        {
            PrintLog(LogType.Log, log, args);
        }

        public void LogError(string log, params object[] args)
        {
            PrintLog(LogType.LogWarn, log, args);
        }

        public void LogR(string log, params object[] args)
        {
#if UNITY_EDITOR
            PrintLog(LogType.LogWarn, log, args);
#endif
        }

        public void LogWarning(string log, params object[] args)
        {
            PrintLog(LogType.LogWarn, log, args);
        }
    }
}