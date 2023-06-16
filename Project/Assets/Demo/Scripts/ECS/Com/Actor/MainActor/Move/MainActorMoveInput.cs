namespace Demo.Com.MainActor
{
    public class MainActorMoveInput
    {
        /// <summary>
        /// 竖直输入
        /// </summary>
        public float v;
        
        /// <summary>
        /// 水平输入
        /// </summary>
        public float h;
        
        /// <summary>
        /// 移动方向
        /// </summary>
        public int MoveDir;
        
        //跳跃等待帧数
        private int jumpWaitFrame = 0;
        
        public void FixedUpdate()
        {
            if(jumpWaitFrame >= 0)
            {
                jumpWaitFrame--;
            }
        }

        #region 移动


        public void Move()
        {
            
        }

        #endregion

        #region 跳跃
        
        public bool CheckClickJumpBtn()
        {
            if (jumpWaitFrame>0)
            {
                return true;
            }

            return false;
        }
        
        public void ClickJump()
        {
            jumpWaitFrame = 3;
        }

        #endregion
        
    }
}