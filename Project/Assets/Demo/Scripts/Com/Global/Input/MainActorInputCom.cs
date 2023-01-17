using System;
using LCECS.Core;
using UnityEngine;

namespace Demo.Com
{
    public class MainActorInputKey
    {
        public Func<bool> CheckKeyDown;
        public Func<bool> CheckKey;
        public Func<bool> CheckKeyUp;

        public bool KeyDown()
        {
            return CheckKeyDown == null ? false : CheckKeyDown();
        }
        
        public bool Key()
        {
            return CheckKey == null ? false : CheckKey();
        }
        
        public bool KeyUp()
        {
            return CheckKeyUp == null ? false : CheckKeyUp();
        }
    }
    
    public class MainActorInputCom : BaseCom
    {
        // public KeyCode LeftMoveKey;
        // public KeyCode RightMoveKey;
        // public KeyCode Jump;
        [NonSerialized] public KeyCode Dash;
        [NonSerialized] public KeyCode Climb;


        [NonSerialized] public MainActorInputKey LeftMoveKey = new MainActorInputKey();
        [NonSerialized] public MainActorInputKey RightMoveKey = new MainActorInputKey();
        [NonSerialized] public MainActorInputKey Jump = new MainActorInputKey();

        /// <summary>
        /// 竖直输入
        /// </summary>
        [NonSerialized] public float v;
        
        /// <summary>
        /// 水平输入
        /// </summary>
        [NonSerialized] public float h;
        
        [NonSerialized] public int MoveDir;
        
        [NonSerialized] private int JumpFrame;
        
        /// <summary>
        /// 按下跳跃
        /// </summary>
        public bool JumpKeyDown {
            get
            {
                if(Jump.KeyDown())
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
        
        
        public bool ClimbKey { get {return Input.GetKey(Climb); } }
        
        public bool DashKeyDown { get { return Input.GetKeyDown(Dash); } }

        protected override void OnAwake(Entity pEntity)
        {
            // Jump = KeyCode.Space;
            // Dash = KeyCode.E;
            // Climb = KeyCode.Q;
            // LeftMoveKey = KeyCode.A;
            // RightMoveKey = KeyCode.D;
        }
        
        public void Update()
        {
            CheckHorzontalMove();
            v = Input.GetAxisRaw("Vertical");
            h = Input.GetAxisRaw("Horizontal");
            if (Jump.KeyDown())
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
            if (RightMoveKey.KeyDown() && h <= 0)
            {
                MoveDir = 1;
            }
            else if (LeftMoveKey.KeyDown() && h >= 0)
            {
		
                MoveDir = -1;
            }
            else if (RightMoveKey.KeyUp())
            {
                if (LeftMoveKey.Key())  //放开右键的时候仍按着左键
                {
                    MoveDir = -1;
                }
                else
                {
                    MoveDir = 0;
                }
            }
            else if (LeftMoveKey.KeyUp())
            {
                if (RightMoveKey.Key())
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