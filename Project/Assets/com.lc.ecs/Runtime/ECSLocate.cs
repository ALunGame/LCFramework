using LCECS.Server.ECS;
using LCECS.Server.Factory;
using LCECS.Server;
using LCECS.Server.Player;
using System;

namespace LCECS
{
    /// <summary>
    /// ECS服务定位器
    /// </summary>
    public static class ECSLocate
    {
        /// <summary>
        /// 日志
        /// </summary>
        public static ILogServer Log { get; set; }
        public static IECSServer ECS { get; set; }
        public static IFactoryServer Factory { get; set; }
        public static IPlayerServer Player { get; set; }

        public static void InitServer()
        {
            Log         = new ECSLogServer();
            ECS         = new ECSServer();
            Factory     = new FactoryServer();
            Player      = new PlayerServer();
        }

        public static void Clear()
        {
            Log    = null;
            ECS       = null;
            Factory   = null;
            Player    = null;
        }
    }
}
