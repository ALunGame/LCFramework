using LCToolkit.Server;

namespace LCGAS.Server
{
    public class GASLogServer : BaseLogServer
    {
        private string logTag = "[GAS]";
        public override string LogTag { get=>logTag; }
    }
}