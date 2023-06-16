using System.Collections.Generic;
using Cnf;
using Demo.Com;
using Demo.Life;
using Demo.Life.State.Content;
using LCMap;
using LCToolkit;

namespace Demo
{
    /// <summary>
    /// 地图演员生活逻辑
    /// 1，休息
    /// 2，工作
    ///
    ///
    ///
    /// 1,还是不能用状态机写，有点别扭，因为演员的上层就有个行为树了
    /// 2，新的思路，决策层判断工资和休息，通知行为层
    /// </summary>
    public class MapActorLifeLogic : BaseServerLogicModule<MapServer>
    {
        private List<ActorWorkContent> waitWorkContents = new List<ActorWorkContent>();
        private List<ActorLifeCom> workers = new List<ActorLifeCom>();

        public override void OnInit()
        {
            
        }

        public override void OnClear()
        {
            
        }
        
        public void AddWorker(ActorLifeCom pCom)
        {
            if (!workers.Contains(pCom))
            {
                workers.Add(pCom);
            }
        }
        
        public void RemoveWorker(ActorLifeCom pCom)
        {
            workers.Remove(pCom);
        }
        
        /// <summary>
        /// 获得生产者
        /// </summary>
        /// <param name="pItemId">生产的物品Id</param>
        /// <returns></returns>
        public List<ActorLifeCom> GetProducersByItemId(int pItemId)
        {
            List<ActorLifeCom> resComs = new List<ActorLifeCom>();
            
            for (int i = 0; i < workers.Count; i++)
            {
                if (workers[i].WorkerType == ActorWorker.Producer)
                {
                    int cnfId = workers[i].CnfId;
                    if (LCConfig.Config.ActorProduceCnf.TryGetValue(cnfId,out ActorProduceCnf cnf))
                    {
                        if (cnf.workeOutput.Contains(pItemId))
                        {
                            resComs.Add(workers[i]);
                        }
                    }
                }
            }

            return resComs;
        }
    }
}