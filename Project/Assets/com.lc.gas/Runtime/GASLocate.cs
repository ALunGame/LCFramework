using LCGAS.Server;

namespace LCGAS
{
    public static class GASLocate
    {
        public static GASLogServer Log { get; private set; }

        public static void Init()
        {
            Log = new GASLogServer();
        }

        public static void Clear()
        {
            
        }
    }
}