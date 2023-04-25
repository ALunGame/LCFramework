using System;
using LCMap;
using LCToolkit.FSM;
using UnityEngine;

namespace Demo.Com.MainActor.NewMove
{
    public class MainActorClimbState: BaseMainActorMoveState
    {
        public ActorDir HopWaitDir;
        public float HopWaitXSpeed;
        
        public override bool OnEvaluate()
        {
            if (!moveCom.Collider.CollideCheck((int)moveCom.CurrDir*Vector2.right))
            {
                return false;
            }

            if (moveCom.IsGround)
            {
                return false;
            }

            return true;
        }

        protected internal override void OnEnter()
        {
            HopWaitDir = ActorDir.None;
            HopWaitXSpeed = 0;
            
            moveCom.Speed.x = 0;
            moveCom.Input.MoveX = 0;
            moveCom.Speed.y *= MainActorConst.ClimbGrabYMult;

            moveCom.Collider.ClimbSnap();
        }

        protected internal override void OnUpdate(float pDeltaTime, float pRealElapseSeconds)
        {
            //检测跳跃
            if (moveCom.Input.ClickJump)
            {
                moveCom.Input.ClearJump();

                if (moveCom.Input.MoveX == (int)moveCom.CurrDir * -1)
                {
                    moveCom.WallJump((ActorDir)((int)moveCom.CurrDir * -1));
                }
                else
                {
                    moveCom.WallJump(ActorDir.None);
                }
                return;
            }
            
            //翻越墙角
            if (HopWaitDir != ActorDir.None)
            {
                if (Math.Sign(moveCom.Speed.x) == -(int)HopWaitDir || moveCom.Speed.y < 0)
                    HopWaitDir = ActorDir.None;
                else if (!moveCom.Collider.CollideCheck(Vector2.right * (int)HopWaitDir))
                {
                    Debug.LogError("翻越墙角0001");
                    moveCom.Speed.x = HopWaitXSpeed;
                    HopWaitDir = ActorDir.None;
                    AutoChangeState();
                }
                return;
            }
            
            //检测面前墙壁
            if (!moveCom.Collider.CollideCheck((int)moveCom.CurrDir*Vector2.right))
            {
                if (moveCom.Speed.y > 0 && moveCom.Collider.ForwardUpCheck())
                {
                    ClimbHop();
                }
                else
                {
                    AutoChangeState();
                }
                return;
            }
            
            //攀爬
            float upSpeed = MainActorConst.ClimbUpSpeed;
            bool trySlip = false;
            if (moveCom.Input.MoveX != 0)
            {
                //上爬
                if (moveCom.Input.MoveX == (int)moveCom.CurrDir)
                {
                    //上方阻挡
                    if (moveCom.Collider.CollideCheck(Vector2.up))
                    {
                        moveCom.Speed.y = Mathf.Min(moveCom.Speed.y, 0);
                        upSpeed = 0;
                        trySlip = true;
                    }
                    //前上方没有阻挡，翻越
                    else if (moveCom.Collider.ForwardUpCheck()){
                        ClimbHop();
                        return;
                    }
                }
                else
                {
                    trySlip = true;
                }
            }
            else
            {
                trySlip = true;
            }

            if (trySlip)
            {
                if (moveCom.IsGround)
                {
                    
                    AutoChangeState();
                    return;
                }
            }
            
            //滑行
            if (trySlip && moveCom.Collider.ForwardUpCheck())
            {
                Debug.Log("=======ClimbSlip_Type4");
                upSpeed = MainActorConst.ClimbSlipSpeed;
            }
            moveCom.Speed.y = Mathf.MoveTowards(moveCom.Speed.y, upSpeed, MainActorConst.ClimbAccel * pDeltaTime);
            moveCom.Speed.x = 0;
            moveCom.Input.MoveX = 0;

            //下滑碰到底部
            if (moveCom.Speed.y < 0 && !moveCom.Collider.CollideCheck(new Vector2((int)moveCom.CurrDir, -1)))
            {
                moveCom.Speed.y = 0;
            }
        }

        /// <summary>
        /// 翻越墙角
        /// </summary>
        public void ClimbHop()
        {
            Debug.LogWarning("翻越墙角>>>>>>");
            
            bool hit = moveCom.Collider.CollideCheck(Vector2.right * (int)moveCom.CurrDir);
            if (hit)
            {
                HopWaitDir = moveCom.CurrDir;
                HopWaitXSpeed = (int) moveCom.CurrDir * MainActorConst.ClimbHopX;
            }
            else
            {
                HopWaitDir = ActorDir.None;
                moveCom.Speed.x = (int) moveCom.CurrDir * MainActorConst.ClimbHopX;
            }

            moveCom.Speed.y = Math.Max(moveCom.Speed.y, MainActorConst.ClimbHopY);
            moveCom.Input.ForceMoveX = 0;
            moveCom.Input.ForceMoveXTimer = MainActorConst.ClimbHopForceTime;
        }
    }
}