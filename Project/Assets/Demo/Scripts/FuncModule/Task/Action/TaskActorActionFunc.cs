using System.Collections.Generic;
using Demo.Com;
using LCECS.Core;
using LCMap;
using LCTask;
using LCToolkit;

namespace Demo.Task
{
    /// <summary>
    /// 设置演员基础属性
    /// </summary>
    public class TaskSetActorBasePropertyFunc : TaskActionFunc
    {
        public int actorId;
        public BasePropertyType propertyType;
        public string value;

        protected override TaskActionState OnStart(TaskObj taskObj)
        {
            List<Actor> actors = LCMap.ActorMediator.GetActors(actorId);
            if (!actors.IsLegal())
            {
                return TaskActionState.Fail;
            }

            for (int i = 0; i < actors.Count; i++)
            {
                Actor actor = actors[i];
                ExecuteProperty(actor);
            }

            return TaskActionState.Finished;
        }

        private void ExecuteProperty(Actor pActor)
        {
            BasePropertyCom propertyCom = pActor.GetCom<BasePropertyCom>();
            switch (propertyType)
            {
                case BasePropertyType.HP:
                    propertyCom.Hp.Curr = int.Parse(value);
                    break;
                case BasePropertyType.MoveSpeed:
                    propertyCom.MoveSpeed.Curr = float.Parse(value);
                    break;
                case BasePropertyType.JumpSpeed:
                    propertyCom.JumpSpeed.Curr = float.Parse(value);
                    break;
                case BasePropertyType.Attack:
                    propertyCom.Attack.Curr = float.Parse(value);
                    break;
                case BasePropertyType.Defense:
                    propertyCom.Defense.Curr = float.Parse(value);
                    break;
                case BasePropertyType.ActionSpeed:
                    propertyCom.ActionSpeed.Curr = float.Parse(value);
                    break;
            }
        }

        protected override void OnClear(TaskObj taskObj)
        {
        }

        private void WaitCmdFinish(WorkCommand pCmd)
        {
            Finish();
        }
    }
}