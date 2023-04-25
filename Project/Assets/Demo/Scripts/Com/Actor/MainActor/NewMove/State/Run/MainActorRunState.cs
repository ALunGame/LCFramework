using System;
using LCMap;
using LCToolkit.FSM;
using UnityEngine;

namespace Demo.Com.MainActor.NewMove
{
    public class MainActorRunState : BaseMainActorMoveState
    {
        public override bool OnEvaluate()
        {
            if (!moveCom.IsGround)
            {
                return false;
            }

            return true;
        }

        protected internal override void OnEnter()
        {
            moveCom.ClearJumpStep();
            moveCom.Speed.y = 0;
        }

        protected internal override void OnUpdate(float pDeltaTime, float pRealElapseSeconds)
        {
            if (!moveCom.IsGround)
            {
                AutoChangeState();
                return;
            }
            
            //水平速度计算
            moveCom.UpdateSpeedX(pDeltaTime);

            //检测跳跃
            if (moveCom.Input.ClickJump)
            {
                moveCom.Input.ClearJump();
                moveCom.Jump();
            }

            //墙壁
            if (moveCom.Input.MoveX!=0)
            {
                if (moveCom.Collider.CollideCheck((int)moveCom.CurrDir*Vector2.right))
                {
                    if (moveCom.Input.MoveX == (int)moveCom.CurrDir)
                    {
                        moveCom.Climb();
                    }
                }
            }
        }
    }
}