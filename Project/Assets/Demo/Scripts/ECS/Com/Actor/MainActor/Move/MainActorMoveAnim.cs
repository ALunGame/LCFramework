
using UnityEngine;

namespace Demo.Com.MainActor
{
    public class MainActorMoveAnim
    {
        private MainActorMoveCom moveCom;
        private AnimCom animCom;
        
        public MainActorMoveAnim(MainActorMoveCom pMoveCom, AnimCom pAnimCom)
        {
            moveCom = pMoveCom;
            animCom = pAnimCom;
        }
        
        /// <summary>
        /// 处理移动动画
        /// </summary>
        public void ExecutePlayAnim()
        {
            MainActorMoveState moveState = moveCom.MoveState;
            Vector2 velocity = moveCom.Velocity;

            string animName = "";
            if (moveState == MainActorMoveState.Normal)
            {
                if (velocity == Vector2.zero)
                {
                    animName = "idle";
                }
                else
                {
                    animName = "run";
                }
            }
            else if (moveState == MainActorMoveState.Jump)
            {
                animName = "jumpUp";
            }
            else if (moveState == MainActorMoveState.Climb)
            {
                animName = "climb";
            }
            else if (moveState == MainActorMoveState.Dash)
            {
                animName = "idle";
            }
            else if (moveState == MainActorMoveState.Fall)
            {
                animName = "jumpDown";
            }
            
            animCom.PlayAnim(animName);
        }
    }
}