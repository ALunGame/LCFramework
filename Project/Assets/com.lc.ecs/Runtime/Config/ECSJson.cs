using System.Collections.Generic;

namespace LCECS.Data
{
    public class EntityJsonList
    {
        public List<EntityJson> List = new List<EntityJson>();
    }

    public class EntityJson
    {
        public int EntityId = 0;
        public string TipStr = "";
        public int Group = 0;
        public string PrefabPath = "";
        public List<EntityComJson> Coms = new List<EntityComJson>();
    }

    public class EntityComJson
    {
        public string ComName = "";
        public List<EntityComValueJson> Values = new List<EntityComValueJson>();
    }

    public class EntityComValueJson
    {
        public string Name = "";
        public string Type = "";
        public string Value = "";
    }
}