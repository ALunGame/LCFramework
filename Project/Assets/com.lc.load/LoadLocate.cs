using LCToolkit.Server;

namespace LCLoad
{
    public static class LoadLocate
    {
        public static ILogServer Log = new LoadLogServer();

        public static ILoadServer Load = new LoadServer();
    }
}
