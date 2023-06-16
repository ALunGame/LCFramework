using LCMap;
using LCToolkit.FSM;
using UnityEngine;

namespace Demo.Com.MainActor.NewMove
{
    public class MainActorJumpStateContext : FsmStateContext
    {
        //目标跳跃高度
        public float targetJumpHeight;

        public MainActorJumpStateContext(float pTargetJumpHeight)
        {
            targetJumpHeight = pTargetJumpHeight;
        }
    }
    
    public class MainActorJumpState : BaseMainActorMoveState
    {
        public float startJumpHeight;               //开始跳跃时的高度
        public float currJumpHeight;                //当前跳跃高度
        public float targetJumpHeight;              //目标跳跃高度

        public override bool OnEvaluate()
        {
            return moveCom.Input.ClickJump;
        }

        protected internal override void OnEnter()
        {
            moveCom.Input.ClearJump();
            
            startJumpHeight = moveCom.Pos.y;
            
            targetJumpHeight = GetContext<MainActorJumpStateContext>().targetJumpHeight + startJumpHeight;
            currJumpHeight   = startJumpHeight;
        }

        protected internal override void OnUpdate(float pDeltaTime, float pRealElapseSeconds)
        {
            //检测目标高度
            if (currJumpHeight > targetJumpHeight)
            {
                AutoChangeState();
                return;
            }
            
            //检测头顶碰撞
            if (moveCom.Collider.CollideCheck(ColliderDirType.Up))
            {
                AutoChangeState();
                return;
            }
            
            //水平速度更新
            moveCom.UpdateSpeedX(pDeltaTime);
            
            //高度更新
            currJumpHeight = moveCom.Pos.y;
            
            //检测跳跃
            if (moveCom.Input.ClickJump)
            {
                moveCom.Input.ClearJump();
                if (moveCom.Collider.CheckWall(ActorDir.Left))
                {
                    moveCom.WallJump(ActorDir.Right);
                }
                else if (moveCom.Collider.CheckWall(ActorDir.Right))
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