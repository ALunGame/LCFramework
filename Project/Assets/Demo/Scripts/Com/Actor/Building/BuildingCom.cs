using LCECS.Core;
using LCMap;
using System;
using System.Collections.Generic;

namespace Demo.Com
{
    public class BuildingCom : BaseCom
    {
        public List<int> owerActorIds = new List<int>();

        [NonSerialized]
        public List<ActorObj> owerActors;
    }
}