namespace Demo.Com.MainActor.NewMove
{
    public abstract class MainActorVirtualBtn
    {
        //等待帧数
        private int waitFrames;
        //当前帧
        private int currFrames;

        public MainActorVirtualBtn(int pWaitFrames)
        {
            waitFrames = pWaitFrames;
        }

        /// <summary>
        /// 按下
        /// </summary>
        /// <returns></returns>
        public bool Pressed()
        {
            return CheckPressed() || currFrames > 0;
        }

        /// <summary>
        /// 检测按钮按下
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckPressed();

        /// <summary>
        /// 按住
        /// </summary>
        /// <returns></returns>
        public abstract bool Hold();

        /// <summary>
        /// 更新等待帧
        /// </summary>
        public void Update()
        {
            if (currFrames > 0)
            {
                currFrames--;
            }

            bool flag = false;
            if (CheckPressed())
            {
                currFrames = waitFrames;
                flag = true;
            }
            else if (Hold())
            {
                flag = true;
            }

            if (!flag)
            {
                ClearWaitFrames();
            }
        }
        
        /// <summary>
        /// 清除等待帧
        /// </summary>
        public void ClearWaitFrames()
        {
            currFrames = 0;
        }
    }
}