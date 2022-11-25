using Demo.Com;
using Demo.Config;
using LCMap;
using System.Collections.Generic;

namespace Demo
{
    public class ProduceInteractive : ActorInteractive
    {
        private int _type = (int)InteractiveType.Produce;
        public override int Type { get => _type; }

        protected override bool OnExecute(Actor executeActor)
        {
           
            ManagerCom managerCom   = actor.GetCom<ManagerCom>();
            ProduceCom produceCom   = actor.GetCom<ProduceCom>();

            BagCom bagCom           = managerCom.buildingBagCom;

            if (produceCom.currProduces.Count != 0)
            {
                GameLocate.Log.LogError("生产失败，当前还有生产的物品", actor);
                return true;
            }

            List<int> resProduces = produceCom.GetCanMakeProduceIds(bagCom);
            if (resProduces.Count == 0)
            {
                GameLocate.Log.LogError("生产失败，当前没有可以生产的物品", actor);
                return true;
            }

            //扣除物品
            foreach (var produceId in resProduces)
            {
                ProduceCnf produceCnf = LCConfig.Config.ProduceCnf[produceId];
                foreach (var item in produceCnf.costItems)
                {
                    bagCom.RemoveItem(item.id, item.cnt);
                }
            }

            //执行生产
            foreach (var item in resProduces)
            {
                int produceId = item;
                ProduceCnf produceCnf = LCConfig.Config.ProduceCnf[produceId];
                produceCom.currProduces.Add(produceId);
                GameLocate.TimerServer.WaitForSeconds(produceCnf.time, () =>
                {
                    foreach (var item in produceCnf.produceItems)
                    {
                        bagCom.AddItem(item.id, item.cnt);
                    }
                    produceCom.currProduces.Remove(produceId);
                    if (produceCom.currProduces.Count == 0)
                    {
                        Finish();
                    }
                });
            }

            return false;
        }
    }
}
