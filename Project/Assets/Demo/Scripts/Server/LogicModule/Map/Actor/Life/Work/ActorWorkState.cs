using Cnf;
using Demo.Life.State.Content;
using LCECS.Core;
using LCMap;
using LCToolkit.FSM;

namespace Demo.Life.State
{
    public class ActorWorkState : ActorLifeState
    {
        private ActorWorkContent currContent;
        private bool needWait = false;

        protected internal override void OnInit()
        {
            base.OnInit();
        }

        public override bool OnEvaluate()
        {
            if (!lifeCom.WorkContents.IsLegal())
            {
                return false;
            }

            ActorWorkContent content = lifeCom.WorkContents.Peek();
            return content.CanDoWork();
        }

        protected internal override void OnEnter()
        {
            currContent = lifeCom.WorkContents.Peek();
            currContent.DoWork();
        }

        protected internal override void OnUpdate(float pDeltaTime, float pRealElapseSeconds)
        {
            if (currContent.State != WorkState.Doing)
            {
                AutoChangeState();
            }
        }

        protected internal override void OnLeave()
        {
            if (currContent.State == WorkState.Success)
            {
                lifeCom.WorkSuccess();
            }

            currContent = null;
        }
    }
}