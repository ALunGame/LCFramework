using LCMap;
using LCToolkit.FSM;

namespace Demo.Com.MainActor.NewMove
{
    public class MainActorFullState : BaseMainActorMoveState
    {
        public override bool OnEvaluate()
        {
            return !moveCom.IsGround;
        }

        protected internal override void OnUpdate(float pDeltaTime, float pRealElapseSeconds)
        {
            if (moveCom.IsGround)
            {
                AutoChangeState();
                return;
            }

            moveCom.UpdateSpeedX(pDeltaTime);
            moveCom.UpdateFullSpeedY(pDeltaTime);
            
            //检测跳跃
            if (moveCom.Input.ClickJump)
            {
                if (moveCom.Collider.CheckWallJump(ActorDir.Left))
                {
                    moveCom.WallJump(ActorDir.Right);
                }
                else if (moveCom.Collider.CheckWallJump(ActorDir.Right))
                {
                    moveCom.WallJump(ActorDir.Left);
                }
                else
                {
                    moveCom.Jump();
                }
            }
        }
    }
}