using System.Collections.Generic;
using Demo.Logic;
using LCToolkit;
using NotImplementedException = System.NotImplementedException;

namespace Demo
{
    public class MapServerLogicMapping : ServerLogicModuleMapping
    {
        public override List<IServerLogicModule> RegBeforeServerInitLogics()
        {
            return new List<IServerLogicModule>()
            {
                
            };
        }

        public override List<IServerLogicModule> RegAfterServerInitLogics()
        {
            return new List<IServerLogicModule>()
            {
                new MapSeekPathLogic(),
            };
        }
    }
}