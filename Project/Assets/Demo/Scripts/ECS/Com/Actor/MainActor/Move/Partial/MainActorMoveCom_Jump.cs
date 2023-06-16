using System;
using System.Collections;
using UnityEngine;

namespace Demo.Com.MainActor
{
    public partial class MainActorMoveCom
    {
        [NonSerialized] private float startJumpPos;             //开始跳跃时的位置
        [NonSerialized] private bool isIntroJump;               //是否是刚进入跳跃的状态
        [NonSerialized] private bool secondJump = false;		//二段跳
        
        //一段跳最大距离
        public float JumpDis = 0.5f;					
        //一段跳跳跃速度
        public float JumpSpeed = 18;
        
        //可以二段跳最小距离
        public float JumpSecondCanDis = 0.5f;			   
        //二段跳最大距离
        public float JumpSecondDis = 2.5f;            
        //二段跳跳跃速度
        public float JumpSecondSpeed = 18;
        
        /// <summary>
        /// 记录初始位置和计算最高能跳到的位置，根据按键时间进行跳跃高度判断
        /// </summary>
        private void Jump(Vector2 vel, Vector2 maxVel)
        {
            ChangeState(MainActorMoveState.Jump);
            startJumpPos = rig2D.position.y;
            isIntroJump  = true;
            secondJump   = false;
            if (vel.y >= 0)
                velocity.y = vel.y;
            monoHelper.BeginCoroutine(ExecuteJump(vel, maxVel,JumpSpeed,JumpDis));
        }
        
        /// <summary>
        /// 跳跃
        /// </summary>
        private void Jump()
        {
	        ChangeState(MainActorMoveState.Jump);
	        startJumpPos = rig2D.position.y;
	        isIntroJump = true;
	        secondJump  = false;
	        input.ClearJumpFrame();
	        monoHelper.BeginCoroutine(ExecuteJump(Vector2.zero, Vector2.zero,JumpSpeed,JumpDis));
        }

        /// <summary>
        /// 二段跳
        /// </summary>
        /// <returns></returns>
        private bool SecondJump()
        {
	        if (secondJump)
	        {
		        return false;
	        }
	        
	        Debug.Log("二段跳！！！！！！！");
	        secondJump  = true;
	        ChangeState(MainActorMoveState.Jump);
	        startJumpPos = rig2D.position.y;
	        isIntroJump = true;
	        input.ClearJumpFrame();
	        monoHelper.BeginCoroutine(ExecuteJump(Vector2.zero, Vector2.zero,JumpSecondSpeed,JumpSecondDis));
	        return true;
        }
        
        private bool CheckSecondJump()
        {
	        if (input.JumpKeyDown)
	        {
		        //蹬墙跳
		        if (BoxCheckCanClimb() && !CheckIsClimb())
		        {
			        velocity.y = 0;
			        isIntroJump = false;
			        Jump(new Vector2(4 * -GetDirInt, 0), new Vector2(24, 0));
			        return true;
		        }
		        //二段跳
		        else
		        {
			        if (SecondJump())
				        return true;
		        }
	        }
	        return false;
        }
        
        private IEnumerator ExecuteJump(Vector2 pCurrVel, Vector2 pMaxVel, float pJumpSpeed, float pJumpDis)
        {
	        float jumpDis   = pJumpDis * (pCurrVel.y + pJumpSpeed) / pJumpSpeed;
	        float jumpSpeed = pJumpSpeed + pCurrVel.y;
	        
	        float dis = 0;
	        //1,还没有到达最小跳跃高度
	        while (moveState == MainActorMoveState.Jump && dis <= jumpDis)
	        {
		        //赋值横向加速度
		        if (pCurrVel.x != 0 && Mathf.Abs(velocity.x) < pMaxVel.x)
		        {
			        isMove = false;
			        velocity.x += pCurrVel.x;
			        if(Mathf.Abs(velocity.x) > pMaxVel.x)
			        {
				        velocity.x = pMaxVel.x * GetDirInt;
			        }
		        }
		        //检测头顶碰撞
		        if (!CheckFixedUpMove())   //返回false说明撞到墙，结束跳跃
		        {
			        velocity.y = 0;
			        isIntroJump = false;
			        isMove = true;
			        yield break;
		        }

		        //可以二段跳
		        if (dis >= JumpSecondCanDis)
		        {
			        if (CheckSecondJump())
			        {
				        yield break;
			        }
		        }
		        
		        //获取当前角色相对于初始跳跃时的高度
		        dis = rig2D.position.y - startJumpPos;
		        if (pCurrVel.y <= 0 && velocity.y < jumpSpeed)
		        {
			        velocity.y += 240 * Time.fixedDeltaTime;
		        }
		        yield return new WaitForFixedUpdate();
	        }
	        
	        velocity.y = jumpSpeed;
	        isMove = true;
	        
	        //2，到达最大高度，减速
	        while (moveState == MainActorMoveState.Jump && velocity.y > 0 )
	        {
		        //检测头顶碰撞
		        if (!CheckFixedUpMove())
		        {
			        break;
		        }

		        if (CheckSecondJump())
		        {
			        yield break;
		        }
		        
		        //跳跃到最大高度时，减速速度是其他的一半，为了玩家在最大高度时更好控制
		        if (dis >= jumpDis)
		        {
			        velocity.y -= 100 * Time.fixedDeltaTime;
		        }
		        else
		        {
			        velocity.y -= 200 * Time.fixedDeltaTime;
		        }
		        yield return new WaitForFixedUpdate();
	        }
	        
	        //3，跳跃结束
	        velocity.y = 0;
	        yield return 0.1f;
	        isIntroJump = false;
	        ChangeState(MainActorMoveState.Fall);
        }
    }
}