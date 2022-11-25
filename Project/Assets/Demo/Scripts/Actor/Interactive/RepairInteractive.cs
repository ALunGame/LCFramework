using Demo.Com;
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

        protected override bool OnExecute(Actor executeActor)
        {
            BasePropertyCom owerPropertyCom = actor.GetCom<BasePropertyCom>();

            if (owerPropertyCom.Hp.Curr >= owerPropertyCom.Hp.Max)
            {
                GameLocate.Log.LogError("修复失败，生命值已满", actor);
                return true;
            }

            return ExecuteRepair(executeActor);
        }

        private bool ExecuteRepair(Actor executeActor)
        {
            BasePropertyCom owerPropertyCom = actor.GetCom<BasePropertyCom>();
            if (owerPropertyCom.Hp.Curr >= owerPropertyCom.Hp.Max)
            {
                return true;
            }

            owerPropertyCom.Hp.Curr = owerPropertyCom.Hp.Curr + repairValue;
            actor.GetStateGo().transform.DOComplete(false);
            actor.GetStateGo().transform.DOPunchPosition(new Vector3(-0.2f * actor.GetDirValue(), 0, 0), 0.1f, 1, 0);
            GameLocate.TimerServer.WaitForSeconds(1, () =>
            {
                ExecuteRepair(executeActor);
            });
            return false;
        }
    }
}