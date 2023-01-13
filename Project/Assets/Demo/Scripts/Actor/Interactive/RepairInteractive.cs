using Config;
using Demo.Com;
using Demo.Config;
using DG.Tweening;
using LCMap;
using UnityEngine;

namespace Demo
{
    public class RepairInteractive : ActorInteractive
    {
        public int repairValue = 1;

        private int _type = (int)InteractiveType.Repair;
        public override int Type { get => _type; }

        protected override InteractiveState OnExecute(Actor executeActor, params object[] pParams)
        {
            BasePropertyCom owerPropertyCom = actor.GetCom<BasePropertyCom>();

            if (owerPropertyCom.Hp.Curr >= owerPropertyCom.Hp.Total)
            {
                GameLocate.Log.Log("修复失败，生命值已满", actor);
                return InteractiveState.Success;
            }

            return ExecuteRepair(executeActor);
        }

        private InteractiveState ExecuteRepair(Actor executeActor)
        {
            //1，扣除修复道具
            ItemRepairCnf itemRecipeCnf = LCConfig.Config.ItemRepairCnf[actor.Id];
            executeActor.GetCom(out BagCom bagCom);
            for (int i = 0; i < itemRecipeCnf.repairs.Count; i++)
            {
                ItemInfo repairCnf = itemRecipeCnf.repairs[i];
                bagCom.RemoveItem(repairCnf.itemId, repairCnf.itemCnt);
            }
            
            //2，增加血量
            BasePropertyCom owerPropertyCom = actor.GetCom<BasePropertyCom>();
            owerPropertyCom.Hp.Curr += itemRecipeCnf.addHp;
            
            //3，表现
            actor.GetStateGo().transform.DOComplete(false);
            actor.GetStateGo().transform.DOPunchPosition(new Vector3(-0.2f * actor.GetDirValue(), 0, 0), 0.1f, 1, 0);
            return InteractiveState.Success;
        }
    }
}