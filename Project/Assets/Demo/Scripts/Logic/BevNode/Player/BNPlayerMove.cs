using Demo.Com;
using LCECS;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using UnityEngine;

namespace Demo.BevNode
{
    //玩家移动类型
    public enum BNPlayerMoveType
    {
        None,        //静止
        Run,         //跑
        Jump,        //跳
        Dash,        //冲刺
        Climb,       //爬墙
        ClimbJumpLeft,   //爬墙时候左跳
        ClimbJumpRight,   //爬墙时候右跳
    }
    
    public class BNPlayerMove : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData     = wData as EntityWorkData;
            
            //参数
            bool doDash                 = workData.GetReqParam(workData.CurrReqId).GetBool();
            Vector2 inputMove           = workData.GetReqParam(workData.CurrReqId).GetVect2();

            //组件
            PlayerCom playerCom         = workData.MEntity.GetCom<PlayerCom>();
            SpeedCom speedCom           = workData.MEntity.GetCom<SpeedCom>();
            PlayerPhysicsCom physicsCom = workData.MEntity.GetCom<PlayerPhysicsCom>();
            ColliderCom colliderCom     = workData.MEntity.GetCom<ColliderCom>();

            //跳跃索引重置
            CheckResetJumpIndex(inputMove, workData);
            
            //移动类型
            BNPlayerMoveType reqMoveType = GetPlayerReqMoveType(workData,inputMove,doDash);
            
            //根据类型赋值速度
            if (reqMoveType== BNPlayerMoveType.None)
            {
                SetEntityComValue(workData, 0, 0, false);
            }
            else if (reqMoveType == BNPlayerMoveType.Run)
            {
                SetEntityComValue(workData, speedCom.MaxMoveSpeed*inputMove.x, 0, false);
            }
            else if (reqMoveType == BNPlayerMoveType.Jump)
            {
                playerCom.CurrJumpIndex++;
                SetEntityComValue(workData, speedCom.MaxMoveSpeed*inputMove.x, speedCom.MaxJumpSpeed * inputMove.y, false);
            }
            else if (reqMoveType == BNPlayerMoveType.Dash)
            {
                SetEntityComValue(workData, speedCom.MaxMoveSpeed*inputMove.x, 0, true);
            }
            else if (reqMoveType == BNPlayerMoveType.Climb)
            {
                inputMove.x = inputMove.x < 0 ? -inputMove.x : inputMove.x;
                SetEntityComValue(workData, 0, speedCom.ClimbSpeed*inputMove.x, false);
            }
            else if (reqMoveType == BNPlayerMoveType.ClimbJumpLeft)
            {
                SetEntityComValue(workData, -speedCom.MaxMoveSpeed, speedCom.ClimbSpeed* inputMove.y, false);
            }
            else if (reqMoveType == BNPlayerMoveType.ClimbJumpRight)
            {
                SetEntityComValue(workData, speedCom.MaxMoveSpeed, speedCom.ClimbSpeed* inputMove.y, false);
            }
            
            //冲刺特效
            if (playerCom.DoDash)
            {
                ECSLocate.ECS.SetGlobalSingleComData((EffectCom com) =>
                {
                    com.EffectId        = 5004;
                    com.EffectEntityId  = workData.Id;
                    com.EffectHideTime  = 0.35f;
                    com.EffectGapTime   = 0.08f;
                });
            }
        }
        
        //设置组件值
        private void SetEntityComValue(EntityWorkData workData,float reqMoveSpeed,float reqJumpSpeed,bool doDash)
        {
            PlayerCom playerCom         = workData.MEntity.GetCom<PlayerCom>();
            SpeedCom speedCom           = workData.MEntity.GetCom<SpeedCom>();
            playerCom.DoDash            = doDash;
            speedCom.ReqJumpSpeed       = reqJumpSpeed;
            speedCom.ReqMoveSpeed       = reqMoveSpeed;
            speedCom.ReqMoveSpeed       = speedCom.ReqMoveSpeed > speedCom.MaxMoveSpeed ? speedCom.MaxMoveSpeed : speedCom.ReqMoveSpeed;
        }

        //获得玩家请求移动的类型
        private BNPlayerMoveType GetPlayerReqMoveType(EntityWorkData workData,Vector2 inputMove,bool doDash)
        {
            //没有输入
            if (doDash==false && inputMove==Vector2.zero)
                return BNPlayerMoveType.None;
            
            if (CheckCanDash(doDash,workData))
            {
                return BNPlayerMoveType.Dash;
            }
            
            if (CheckCanClimb(inputMove,workData))
            {
                return BNPlayerMoveType.Climb;
            }
            
            if (CheckCanClimbJumpRight(inputMove,workData))
            {
                return BNPlayerMoveType.ClimbJumpRight;
            }
            
            if (CheckCanClimbJumpLeft(inputMove,workData))
            {
                return BNPlayerMoveType.ClimbJumpLeft;
            }
            
            if (CheckCanJump(inputMove,workData))
            {
                return BNPlayerMoveType.Jump;
            }
            
            return BNPlayerMoveType.Run;
        }

        #region 冲刺

        private bool CheckCanDash(bool reqDash,EntityWorkData workData)
        {
            if (reqDash==false)
            {
                return false;
            }
            
            AnimCom animCom = workData.MEntity.GetCom<AnimCom>();
            ColliderCom colliderCom     = workData.MEntity.GetCom<ColliderCom>();
            
            //判断下方向
            bool isRight = false;
            if (animCom.SpriteRender.flipX)
                isRight = true;
            else
                isRight = false;

            //没有右方碰撞
            if (isRight && colliderCom.CollideDir!= ColliderDir.Right && colliderCom.SubCollideDir!= ColliderDir.Right)
            {
                return true;
            }
            
            //没有左方碰撞
            if (isRight==false && colliderCom.CollideDir!= ColliderDir.Left && colliderCom.SubCollideDir!= ColliderDir.Left)
            {
                return true;
            }
            
            return false;
        }
        
        #endregion
        
        #region 跳跃
        
        private bool CheckCanJump(Vector2 inputMove,EntityWorkData workData)
        {
            if (inputMove.y==0)
            {
                return false;
            }
            PlayerCom playerCom         = workData.MEntity.GetCom<PlayerCom>();
            if (playerCom.CurrJumpIndex < playerCom.MaxJumpIndex)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //检测是否药重置跳跃索引
        private void CheckResetJumpIndex(Vector2 inputMove,EntityWorkData workData)
        {
            ColliderCom colliderCom     = workData.MEntity.GetCom<ColliderCom>();
            PlayerCom playerCom         = workData.MEntity.GetCom<PlayerCom>();
            //重置跳跃索引
            if (colliderCom.CollideDir == ColliderDir.Down)
            {
                playerCom.CurrJumpIndex = 0;
            }
        }

        #endregion

        #region 爬墙

        //检测是不是需要爬墙
        private bool CheckCanClimb(Vector2 inputMove,EntityWorkData workData)
        {
            ColliderCom colliderCom     = workData.MEntity.GetCom<ColliderCom>();
            //没有跳跃输入（爬墙）
            if (inputMove.y == 0)
            {
                //没有左右输入
                if (inputMove.x == 0)
                {
                    return false;
                }

                //右移动 并且有右侧碰撞
                if (inputMove.x > 0 && (colliderCom.CollideDir== ColliderDir.Right || colliderCom.SubCollideDir== ColliderDir.Right))
                {
                    return true;
                }
                
                //左移动 并且有左侧碰撞
                if (inputMove.x < 0 && (colliderCom.CollideDir== ColliderDir.Left || colliderCom.SubCollideDir== ColliderDir.Left))
                {
                    return true;
                }
            }
            return false;

        }
        

        #endregion

        #region 爬墙跳跃

        //右横跳
        private bool CheckCanClimbJumpRight(Vector2 inputMove,EntityWorkData workData)
        {
            ColliderCom colliderCom     = workData.MEntity.GetCom<ColliderCom>();
            //没有跳跃请求
            if (inputMove.y == 0)
                return false;
            //没有左右输入
            if (inputMove.x == 0)
                return false;
            
            if (colliderCom.CollideDir == ColliderDir.Left && colliderCom.SubCollideDir == ColliderDir.Left)
            {
                //右横跳
                if (inputMove.x > 0)
                {
                    return true;
                }
            }
            
            if (colliderCom.CollideDir == ColliderDir.Right && colliderCom.SubCollideDir == ColliderDir.Right)
            {
                //左横跳
                if (inputMove.x < 0)
                {
                    inputMove.x = -1;
                    inputMove.y = 1;
                }
            }
            
            return false;
        }
        
        //左横跳
        private bool CheckCanClimbJumpLeft(Vector2 inputMove,EntityWorkData workData)
        {
            ColliderCom colliderCom     = workData.MEntity.GetCom<ColliderCom>();
            //没有跳跃请求
            if (inputMove.y == 0)
                return false;
            //没有左右输入
            if (inputMove.x == 0)
                return false;
            
            if (colliderCom.CollideDir == ColliderDir.Right && colliderCom.SubCollideDir == ColliderDir.Right)
            {
                if (inputMove.x < 0)
                {
                    return true;
                }
            }
            
            return false;
        }
        

        #endregion
    }
}