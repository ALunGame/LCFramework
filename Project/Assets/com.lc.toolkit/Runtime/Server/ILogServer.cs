
namespace LCToolkit.Server
{
    /// <summary>
    /// 日志服务
    /// </summary>
    public interface ILogServer
    {
        /// <summary>
        /// 日志标签
        /// </summary>
        string LogTag { get; }

        /// <summary>
        /// 日志（真机不输出）
        /// </summary>
        void Log(string log, params object[] args);

        /// <summary>
        /// 报错日志（真机不输出）
        /// </summary>
        void LogR(string log, params object[] args);

        /// <summary>
        /// 警告日志（真机不输出）
        /// </summary>
        void LogWarning(string log, params object[] args);

        /// <summary>
        /// 报错日志（真机输出）
        /// </summary>
        void LogError(string log, params object[] args);
    }
}
