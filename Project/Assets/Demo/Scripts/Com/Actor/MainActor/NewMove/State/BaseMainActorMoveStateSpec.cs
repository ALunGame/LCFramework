using LCMap;
using LCToolkit.FSM;
using UnityEngine;

namespace Demo.Com.MainActor.NewMove
{
    public class BaseMainActorMoveState : FsmState
    {
        internal NewMainActorMoveCom moveCom;
        
        protected internal override void OnInit()
        {
            moveCom = Owner.GetCom<NewMainActorMoveCom>();
        }
    }
}