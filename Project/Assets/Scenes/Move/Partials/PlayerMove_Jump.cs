using System.Collections;
using Scenes.MoveTest;
using UnityEngine;

namespace Scenes.Move
{
    public partial class PlayerMove
    {
        //开始跳跃时的位置
        private float startJumpPos;             //开始跳跃时的位置
        private bool isIntroJump;               //是否是刚进入跳跃的状态
        private bool secondJump = false;		//二段跳
        
        [Header("一段跳最大距离")]
        public float JumpDis = 0.5f;					//一段跳高度
        [Header("一段跳跳跃速度")]
        public float JumpSpeed = 18;
        
        [Header("可以二段跳最小距离")]
        public float JumpSecondCanDis = 0.5f;			   //可以二段跳高度
        [Header("二段跳最大距离")]
        public float JumpSecondDis = 2.5f;             //二段跳高度
        [Header("二段跳跳跃速度")]
        public float JumpSecondSpeed = 18;
        
        /// <summary>
        /// 记录初始位置和计算最高能跳到的位置，根据按键时间进行跳跃高度判断
        /// </summary>
        private void Jump(Vector2 vel, Vector2 maxVel)
        {
	        ChangeState(PlayState.Jump);
	        startJumpPos = transform.position.y;
	        isIntroJump = true;
	        secondJump  = false;
	        if (vel.y >= 0)
		        Velocity.y = vel.y;
	        StartCoroutine(ExecuteJump(vel, maxVel,JumpSpeed,JumpDis));
        }
        
        private void Jump()
        {
	        ChangeState(PlayState.Jump);
            startJumpPos = transform.position.y;
            isIntroJump = true;
            secondJump  = false;
            input.ClearJumpFrame();
            StartCoroutine(ExecuteJump(Vector2.zero, Vector2.zero,JumpSpeed,JumpDis));
        }

        private bool SecondJump()
        {
	        if (secondJump)
	        {
		        return false;
	        }
	        
	        Debug.Log("二段跳！！！！！！！");
	        secondJump  = true;
	        ChangeState(PlayState.Jump);
	        startJumpPos = transform.position.y;
	        isIntroJump = true;
	        input.ClearJumpFrame();
	        StartCoroutine(ExecuteJump(Vector2.zero, Vector2.zero,JumpSecondSpeed,JumpSecondDis));
	        return true;
        }

        private IEnumerator ExecuteJump(Vector2 pCurrVel, Vector2 pMaxVel, float pJumpSpeed, float pJumpDis)
        {
	        float jumpDis   = pJumpDis * (pCurrVel.y + pJumpSpeed) / pJumpSpeed;
	        float jumpSpeed = pJumpSpeed + pCurrVel.y;
	        
	        float dis = 0;
	        //1,还没有到达最小跳跃高度
	        while (playState == PlayState.Jump && dis <= jumpDis)
	        {
		        //赋值横向加速度
		        if (pCurrVel.x != 0 && Mathf.Abs(Velocity.x) < pMaxVel.x)
		        {
			        isMove = false;
			        Velocity.x += pCurrVel.x;
			        if(Mathf.Abs(Velocity.x) > pMaxVel.x)
			        {
				        Velocity.x = pMaxVel.x * GetDirInt;
			        }
		        }
		        //检测头顶碰撞
		        if (!CheckFixedUpMove())   //返回false说明撞到墙，结束跳跃
		        {
			        Velocity.y = 0;
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
		        dis = transform.position.y - startJumpPos;
		        if (pCurrVel.y <= 0 && Velocity.y < jumpSpeed)
		        {
			        Velocity.y += 240 * Time.fixedDeltaTime;
		        }
		        yield return new WaitForFixedUpdate();
	        }
	        
	        Velocity.y = jumpSpeed;
	        isMove = true;
	        
	        //2，到达最大高度，减速
	        while (playState == PlayState.Jump && Velocity.y > 0 )
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
			        Velocity.y -= 100 * Time.fixedDeltaTime;
		        }
		        else
		        {
			        Velocity.y -= 200 * Time.fixedDeltaTime;
		        }
		        yield return new WaitForFixedUpdate();
	        }
	        
	        //3，跳跃结束
	        Velocity.y = 0;
	        yield return 0.1f;
	        isIntroJump = false;
	        ChangeState(PlayState.Fall);
        }

        private bool CheckSecondJump()
        {
	        if (input.JumpKeyDown)
	        {
		        //蹬墙跳
		        if (BoxCheckCanClimb() && !CheckIsClimb())
		        {
			        Velocity.y = 0;
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
    }
}