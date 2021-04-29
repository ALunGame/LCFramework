using LCECS.Server.ECS;
using LCECS.Server.Factory;
using LCECS.Server.Log;
using LCECS.Server.Player;
using System;

namespace LCECS
{
    /// <summary>
    /// ECS服务定位器
    /// </summary>
    public static class ECSLocate
    {
        public static IECSLogServer ECSLog { get; set; }
        public static IECSServer ECS { get; set; }
        public static IFactoryServer Factory { get; set; }
        public static IPlayerServer Player { get; set; }

        private static ECSCenter ECSMono;

        public static void InitServer(ECSCenter eCSCenter)
        {
            ECSLog      = new ECSLogServer();
            ECS         = new ECSServer();
            Factory     = new FactoryServer();
            Player      = new PlayerServer();
            ECSMono     = eCSCenter;
        }

        public static void Clear()
        {
            ECSLog    = null;
            ECS       = null;
            Factory   = null;
            Player    = null;
            ECSMono   = null;
        }


        public static void AddDrawGizmosFunc(Action drawFunc)
        {
            if (ECSMono==null)
            {
                return;
            }
            ECSMono.AddDrawGizmosFunc(drawFunc);
        }
    }
}
