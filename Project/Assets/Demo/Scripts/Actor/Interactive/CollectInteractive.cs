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

        private int _type = (int)InteractiveType.Collect;
        public override int Type { get => _type;}

        protected override bool OnExecute(Actor executeActor)
        {
            BagCom owerBagCom = actor.GetCom<BagCom>();
            BagCom executeBagCom = executeActor.GetCom<BagCom>();

            if (executeBagCom.CheckItemIsOutMax(collectId))
            {
                GameLocate.Log.LogError("采集失败，超过上限", collectId, collectCnt);
                return true;
            }

            if (!owerBagCom.RemoveItem(collectId,collectCnt))
            {
                GameLocate.Log.LogError("采集失败，库存不足",collectId,collectCnt);
                return true;
            }

            executeBagCom.AddItem(collectId,collectCnt);
            actor.GetStateGo().transform.DOComplete(false);
            actor.GetStateGo().transform.DOPunchPosition(new Vector3(-0.2f * actor.GetDirValue(), 0, 0), 0.1f, 1, 0);

            return true;
        }
    }
}
