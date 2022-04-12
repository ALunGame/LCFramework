using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.System
{
    public class AnimSystem : BaseSystem
    {
        public  Vector2 MoveMinVector = new Vector2(0.001f,0.001f);
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(AnimCom), typeof(SpeedCom),typeof(ColliderCom), typeof(StateCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            HandlePlayAnim(comList);
        }

        private void HandlePlayAnim(List<BaseCom> comList)
        {
            AnimCom animCom = GetCom<AnimCom>(comList[0]);
            SpeedCom speedCom = GetCom<SpeedCom>(comList[1]);
            ColliderCom colliderCom = GetCom<ColliderCom>(comList[2]);

            AnimDefaultState curState = HandleAnimDefaultState(comList);
            
            animCom.Animtor.SetBool("Idle", curState == AnimDefaultState.Idle);
            animCom.Animtor.SetBool("Run", curState  == AnimDefaultState.Run);
            animCom.Animtor.SetBool("Dead", curState == AnimDefaultState.Dead);
            animCom.Animtor.SetBool("JumpUp", curState == AnimDefaultState.JumpUp);
            animCom.Animtor.SetBool("JumpDown", curState == AnimDefaultState.JumpDown);
            animCom.Animtor.SetBool("Dash", curState == AnimDefaultState.Dash);
            animCom.Animtor.SetBool("Climb", curState == AnimDefaultState.Climb);

            //方向
            HandleAnimDir(curState, speedCom, colliderCom, animCom);
        }

        //处理动画方向
        private void HandleAnimDir(AnimDefaultState state,SpeedCom speedCom,ColliderCom colliderCom,AnimCom animCom)
        {
            if (state == AnimDefaultState.Climb)
            {
                if (colliderCom.CollideDir == ColliderDir.Left || colliderCom.SubCollideDir == ColliderDir.Left)
                {
                    animCom.SpriteRender.flipX = true;
                }
                if (colliderCom.CollideDir == ColliderDir.Right || colliderCom.SubCollideDir == ColliderDir.Right)
                {
                    animCom.SpriteRender.flipX = false;
                }
                return;
            }
            if (speedCom.CurVelocity.x > 0)
            {
                animCom.SpriteRender.flipX = false;
            }
            else if (speedCom.CurVelocity.x < 0)
            {
                animCom.SpriteRender.flipX = true;
            }
        }

        private AnimDefaultState HandleAnimDefaultState(List<BaseCom> comList)
        {
            AnimCom animCom = GetCom<AnimCom>(comList[0]);
            SpeedCom speedCom = GetCom<SpeedCom>(comList[1]);
            ColliderCom colliderCom = GetCom<ColliderCom>(comList[2]);
            StateCom stateCom = GetCom<StateCom>(comList[3]);

            //死亡
            if (stateCom.CurState == EntityState.Dead)
                return AnimDefaultState.Dead;

            //执行触发动画
            if (animCom.DoTrigger)
            {
                return AnimDefaultState.DoTrigger;
            }
            
            //冲刺
            if (speedCom.ReqDash)
            {
                return AnimDefaultState.Dash;
            }
            
            //攀爬
            if (CheckEntityIsClimbing(comList))
            {
                return AnimDefaultState.Climb;
            }
            
            //跳跃
            if (colliderCom.CollideDir == ColliderDir.None)
            {
                if (speedCom.CurVelocity.y > 0)
                {
                    return AnimDefaultState.JumpUp;
                }
                else
                {
                    return AnimDefaultState.JumpDown;
                }
            }
            
            //移动
            if (speedCom.CurVelocity.x!=0)
            {
                return AnimDefaultState.Run;
            }
            
            return AnimDefaultState.Idle;
        }

        //检测是否在攀爬
        private bool CheckEntityIsClimbing(List<BaseCom> comList)
        {
            AnimCom animCom = GetCom<AnimCom>(comList[0]);
            SpeedCom speedCom = GetCom<SpeedCom>(comList[1]);
            ColliderCom colliderCom = GetCom<ColliderCom>(comList[2]);
            
            if (speedCom.CurVelocity.y > 0)
            {
                if (colliderCom.CollideDir == ColliderDir.Left || colliderCom.CollideDir == ColliderDir.Right)
                {
                    return true;
                }
                
                if (colliderCom.SubCollideDir == ColliderDir.Left || colliderCom.SubCollideDir == ColliderDir.Right)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
