using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCMap
{
    /// <summary>
    /// 演员控制
    /// 1，演员的创建
    /// 2，演员系统的初始化
    /// 3，演员的初始化和清理
    /// </summary>
    public class ActorCtrl
    {
        private Dictionary<string, Actor> actorMap = new Dictionary<string, Actor>();
        private Dictionary<int,List<Actor>> actorDict = new Dictionary<int, List<Actor>>();

        

        public void Init()
        {

        }

        public void Clear()
        {

        }
    }
}
