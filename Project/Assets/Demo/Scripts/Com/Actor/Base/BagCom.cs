using LCECS.Core;
using System.Collections.Generic;

namespace Demo
{
    public class BagItem
    {
        public int id;
        public int cnt;
    }

    public class BagCom : BaseCom
    {
        public List<BagItem> itemlist = new List<BagItem>();
    }
}