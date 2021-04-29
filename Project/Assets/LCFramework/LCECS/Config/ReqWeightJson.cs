using System.Collections.Generic;

namespace LCECS.Data
{
    public class WeightJson
    {
        public int Key;
        public int Weight;
    }

    public class ReqWeightJson
    {
        public List<WeightJson> ReqWeights = new List<WeightJson>();
    }
}