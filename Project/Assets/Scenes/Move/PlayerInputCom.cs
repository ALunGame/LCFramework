using System;
using UnityEngine;

namespace Scenes.MoveTest
{
    public class PlayerInputCom : MonoBehaviour
    {
        [Header("左移动")]
        public KeyCode LeftMoveKey;
        [Header("右移动")]
        public KeyCode RightMoveKey;
        [Header("跳跃按键")]
        public KeyCode Jump;
        [Header("冲刺按键")]
        public KeyCode Dash;
        [Header("爬墙按键")]
        public KeyCode Climb;
        [HideInInspector]

        /// <summary>
        /// 竖直输入
        /// </summary>
        public float v;
        /// <summary>
        /// 水平输入
        /// </summary>
        public float h;
        
        [SerializeField]
        [Header("移动方向")]
        public int MoveDir;

        [SerializeField]
        [Header("跳跃有效帧")]
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

        
        /// <summary>
        /// 按下冲刺
        /// </summary>
        public bool DashKeyDown { get { return Input.GetKeyDown(Dash); } }

        private void Awake()
        {
            Jump = KeyCode.Space;
            Dash = KeyCode.E;
            Climb = KeyCode.Q;
            LeftMoveKey = KeyCode.A;
            RightMoveKey = KeyCode.D;
        }

        private void Update()
        {
            CheckHorzontalMove();
            v = Input.GetAxisRaw("Vertical");
            h = Input.GetAxisRaw("Horizontal");
            if (Input.GetKeyDown(Jump))
            {
                JumpFrame = 3;       //在落地前3帧按起跳仍然能跳
            }
        }

        private void FixedUpdate()
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