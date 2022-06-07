using LCNode;
using LCNode.Model;
using System.Collections.Generic;

namespace LCMap
{
    public enum MapNodeId
    {
        所有地图    = 1,           
    }

    public class MapActorData { }
    public class Map_ActorNode : BaseNode
    {
        public override string Title { get => "演员"; set => base.Title = value; }

        public List<int> actorIds = new List<int>();

        [NodeValue("地图Id")]
        public MapNodeId mapId = MapNodeId.所有地图;

        public List<int> GetActorIds()
        {
            List<int> ids = new List<int>();
            for (int i = 0; i < actorIds.Count; i++)
            {
                int tId = actorIds[i];
                if (!ids.Contains(tId))
                {
                    ids.Add(tId);
                }
            }
            return ids;
        }
    }
}
