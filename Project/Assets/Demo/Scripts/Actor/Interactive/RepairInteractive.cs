using Demo.Com;
using DG.Tweening;
using LCMap;
using UnityEngine;

namespace Demo
{
    public class RepairInteractive : ActorInteractive
    {
        public int repairValue = 1;

        protected override void OnExecute(ActorObj executeActor)
        {
            BasePropertyCom owerPropertyCom = actor.Entity.GetCom<BasePropertyCom>();

            if (owerPropertyCom.Hp.Curr >= owerPropertyCom.Hp.Max)
            {
                GameLocate.Log.LogError("修复失败，生命值已满", actor);
                ExecuteFinish();
                return;
            }

            ExecuteRepair(executeActor);
        }

        private void ExecuteRepair(ActorObj executeActor)
        {
            BasePropertyCom owerPropertyCom = actor.Entity.GetCom<BasePropertyCom>();
            if (owerPropertyCom.Hp.Curr >= owerPropertyCom.Hp.Max)
            {
                ExecuteFinish();
                return;
            }

            owerPropertyCom.Hp.Curr = owerPropertyCom.Hp.Curr + repairValue;
            actor.GetStateGo().transform.DOComplete(false);
            actor.GetStateGo().transform.DOPunchPosition(new Vector3(-0.2f * actor.GetDirValue(), 0, 0), 0.1f, 1, 0);
            GameLocate.TimerServer.WaitForSeconds(1, () =>
            {
                ExecuteRepair(executeActor);
            });
        }
    }
}