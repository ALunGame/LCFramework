using LCECS.Server;
using LCECS.Server.ECS;
using LCECS.Server.Player;
using LCToolkit.Server;

namespace LCECS
{
    /// <summary>
    /// ECS服务定位器
    /// </summary>
    public static class ECSLocate
    {
        public static ECSCenter Center;

        /// <summary>
        /// 日志
        /// </summary>
        public static ILogServer Log { get; set; }
        public static IECSServer ECS { get; set; }
        public static IPlayerServer Player { get; set; }

        public static void InitServer(ECSCenter center)
        {
            Center      = center;
            Log         = new ECSLogServer();
            ECS         = new ECSServer();
            Player      = new PlayerServer();
        }

        public static void Clear()
        {
            Log       = null;
            ECS       = null;
            Player    = null;
        }
    }
}
