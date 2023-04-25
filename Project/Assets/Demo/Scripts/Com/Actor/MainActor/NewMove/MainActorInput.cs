using UnityEngine;

namespace Demo.Com.MainActor.NewMove
{
    public class MainActorInput
    {
        private MainActorVirtualBtn leftMoveBtn;
        private MainActorVirtualBtn rightMoveBtn;
        private MainActorVirtualBtn jumpBtn;

        /// <summary>
        /// 强制移动方向
        /// </summary>
        public int ForceMoveX { get; set; }
        /// <summary>
        /// 强制移动计时器
        /// </summary>
        public float ForceMoveXTimer { get; set; }
        
        /// <summary>
        /// 水平移动
        /// </summary>
        public int MoveX { get; set; }
        
        /// <summary>
        /// 点击跳跃
        /// </summary>
        public bool ClickJump { get; set; }

        public void ClearJump()
        {
            jumpBtn.ClearWaitFrames();
        }

        /// <summary>
        /// 键盘模式
        /// </summary>
        public void KeyCodeMode()
        {
            leftMoveBtn = new MainActorKeyCodeBtn(KeyCode.A, 0);
            rightMoveBtn = new MainActorKeyCodeBtn(KeyCode.D, 0);
            jumpBtn = new MainActorKeyCodeBtn(KeyCode.Space, 4);
        }
        
        /// <summary>
        /// UI模式
        /// </summary>
        public void UIMode()
        {
            //TODO
        }

        public void Update(float pDeltaTime)
        {
            ClickJump = jumpBtn.Pressed();
            
            if (ForceMoveXTimer > 0)
            {
                ForceMoveXTimer -= pDeltaTime;
                MoveX = ForceMoveX;
            }
            else
            {
                if (leftMoveBtn.Hold())
                {
                    MoveX = -1;
                }
                else if (rightMoveBtn.Hold())
                {
                    MoveX = 1;
                }
                else
                {
                    MoveX = 0;
                }
            }

            leftMoveBtn.Update();
            rightMoveBtn.Update();
            jumpBtn.Update();
        }
        
    }
}