using System.Collections.Generic;
using Config;
using Demo.Com;
using Demo.Config;
using Demo.Scripts.Com.Work;
using LCECS;
using LCECS.Data;
using LCMap;

namespace Demo
{
    /// <summary>
    /// 修复物品命令
    /// </summary>
    public class RepairActorWorkCmd : WorkCommand
    {
        public Actor repairActor;
        private BasePropertyCom repairProperty;

        public RepairActorWorkCmd(Actor pRepairActor)
        {
            repairActor = pRepairActor;
            repairProperty = repairActor.GetCom<BasePropertyCom>();
        }
        
        public override bool CanTakeCommand(Actor pWorkActor)
        {
            if (pWorkActor.GetCom(out RepairItemCom repairItemCom))
            {
                if (repairItemCom.itemIds.Contains(repairActor.Id))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        protected override void OnCommandTook()
        {
            if (repairProperty.Hp.CheckOutTotal())
                return;


            SendRepairNeedCmds();
        }

        public override bool CanExecute()
        {
            if (repairProperty.Hp.CheckOutTotal())
                return true;
            ItemRepairCnf itemRecipeCnf =  LCConfig.Config.ItemRepairCnf[repairActor.Id];
            executor.GetCom(out BagCom bagCom);
            for (int i = 0; i < itemRecipeCnf.repairs.Count; i++)
            {
                ItemInfo repairCnf = itemRecipeCnf.repairs[i];
                if (bagCom.GetItemCnt(repairCnf.itemId) < repairCnf.itemCnt)
                    return false;
            }
            return true;
        }

        protected override void OnExecute()
        {
            if (repairProperty.Hp.CheckOutTotal())
            {
                ExecuteFinish();
                return;
            }

            RepairActor();
        }

        private void SendRepairNeedCmds()
        {
            //计算需要多少材料
            ItemRepairCnf itemRecipeCnf =  LCConfig.Config.ItemRepairCnf[repairActor.Id];
            
            executor.GetCom(out BagCom bagCom);
            List<BagItem> bagItems = new List<BagItem>();
            for (int i = 0; i < itemRecipeCnf.repairs.Count; i++)
            {
                ItemInfo repairCnf = itemRecipeCnf.repairs[i];
                int currCnt = bagCom.GetItemCnt(repairCnf.itemId);
                if (currCnt < repairCnf.itemCnt)
                {
                    bagItems.Add(new BagItem(repairCnf.itemId,repairCnf.itemCnt-currCnt));
                }
            }

            //发送生产命令
            for (int i = 0; i < bagItems.Count; i++)
            {
                ProduceItemWorkCmd cmd = new ProduceItemWorkCmd(bagItems[i].id,bagItems[i].cnt);
                cmd.SetOriginator(executor);
                GameLocate.FuncModule.Work.SendWorkCommand(cmd);
            }
        }

        private void RepairActor()
        {
            MoveRequestCom.MoveToActorInteractiveRange(executor,repairActor, () =>
            {
                repairActor.ExecuteInteractive(executor, InteractiveType.Repair, (InteractiveState state) =>
                {
                    //3，判断血量是否已经满了
                    if (repairProperty.Hp.CheckOutTotal())
                    {
                        ExecuteFinish();
                    }
                    //4，没满发送命令
                    else
                    {
                        ExecuteFail();
                        SendRepairNeedCmds();
                    }
                });
            });
        }
    }
}