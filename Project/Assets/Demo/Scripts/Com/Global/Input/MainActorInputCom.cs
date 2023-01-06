using System;
using LCECS.Core;
using UnityEngine;

namespace Demo.Com
{
    public class MainActorInputCom : BaseCom
    {
        public KeyCode LeftMoveKey;
        public KeyCode RightMoveKey;
        public KeyCode Jump;
        public KeyCode Dash;
        public KeyCode Climb;
        
        /// <summary>
        /// 竖直输入
        /// </summary>
        public float v;
        
        /// <summary>
        /// 水平输入
        /// </summary>
        public float h;
        
        public int MoveDir;
        
        private int JumpFrame;
        
        /// <summary>
        /// 按下跳跃
        /// </summary>
        public bool JumpKeyDown {
            get
            {
                if(Input.GetKeyDown(Jump))
                {
                    return true;
                }
                else if(JumpFrame > 0)
                {
                    return true;
                }
                return false;
            }
        }
        
        public bool JumpKey { get { return Input.GetKey(Jump); } }
        
        public bool ClimbKey { get {return Input.GetKey(Climb); } }
        
        public bool DashKeyDown { get { return Input.GetKeyDown(Dash); } }

        protected override void OnAwake(Entity pEntity)
        {
            Jump = KeyCode.Space;
            Dash = KeyCode.E;
            Climb = KeyCode.Q;
            LeftMoveKey = KeyCode.A;
            RightMoveKey = KeyCode.D;
        }
        
        public void Update()
        {
            CheckHorzontalMove();
            v = Input.GetAxisRaw("Vertical");
            h = Input.GetAxisRaw("Horizontal");
            if (Input.GetKeyDown(Jump))
            {
                JumpFrame = 3;       //在落地前3帧按起跳仍然能跳
            }
        }

        public void FixedUpdate()
        {
            if(JumpFrame >= 0)
            {
                JumpFrame--;
            }
        }
        
        private void CheckHorzontalMove()
        {
            //按下右方向并且左键或者没有
            if (Input.GetKeyDown(RightMoveKey) && h <= 0)
            {
                MoveDir = 1;
            }
            else if (Input.GetKeyDown(LeftMoveKey) && h >= 0)
            {
		
                MoveDir = -1;
            }
            else if (Input.GetKeyUp(RightMoveKey))
            {
                if (Input.GetKey(LeftMoveKey))  //放开右键的时候仍按着左键
                {
                    MoveDir = -1;
                }
                else
                {
                    MoveDir = 0;
                }
            }
            else if (Input.GetKeyUp(LeftMoveKey))
            {
                if (Input.GetKey(RightMoveKey))
                {
                    MoveDir = 1;
                }
                else
                {
                    MoveDir = 0;
                }
            }
        }

        public void ClearJumpFrame()
        {
            JumpFrame = 0;
        }
    }
}