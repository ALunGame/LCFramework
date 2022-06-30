using DG.Tweening;
using LCMap;
using UnityEngine;

namespace Demo
{
    public class CollectInteractive : ActorInteractive
    {
        /// <summary>
        /// 采集的物品
        /// </summary>
        public int collectId;

        /// <summary>
        /// 采集的数量
        /// </summary>
        public int collectCnt;

        protected override void OnExecute(ActorObj executeActor)
        {
            BagCom owerBagCom = actor.Entity.GetCom<BagCom>();
            BagCom executeBagCom = executeActor.Entity.GetCom<BagCom>();

            if (executeBagCom.CheckItemIsOutMax(collectId))
            {
                GameLocate.Log.LogError("采集失败，超过上限", collectId, collectCnt);
                ExecuteFinish();
                return;
            }

            if (!owerBagCom.RemoveItem(collectId,collectCnt))
            {
                GameLocate.Log.LogError("采集失败，库存不足",collectId,collectCnt);
                ExecuteFinish();
                return;
            }

            executeBagCom.AddItem(collectId,collectCnt);
            actor.GetStateGo().transform.DOComplete(false);
            actor.GetStateGo().transform.DOPunchPosition(new Vector3(-0.2f * actor.GetDirValue(), 0, 0), 0.1f, 1, 0);
            ExecuteFinish();
        }
    }
}
