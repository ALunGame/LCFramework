namespace Demo.Com.MainActor.NewMove
{
    /// <summary>
    /// 方向
    /// </summary>
    public enum ActorDir
    {
        None = 0,
        Left = -1,
        Right = 1,
    }
    
    
    /// <summary>
    /// 主角固定变量
    /// </summary>
    public static class MainActorConst
    {
        public static float Gravity = 90f;
        
        //最大移动速度
        public static float MaxRunSpeed = 9f;
        //移动加速度
        public static float RunAccel = 100f;
        //移动减速度
        public static float RunReduce = 40f;
        
        //空气阻力
        public static float AirMult = 0.65f;
        //地面阻力
        public static float GroundMult = 1f;
        
        //普通最大下落速度
        public static float MaxFall = -16; 


        #region Climb

        public const float ClimbCheckDist = 0.2f;           //攀爬检查距离
        public const float ClimbGrabYMult = .2f;       //攀爬时抓取导致的Y轴速度衰减
        public static float ClimbHopY = 12f;            //Hop的Y轴速度
        public static float ClimbHopX = 10f;            //Hop的X轴速度
        public static float ClimbHopForceTime = .2f;    //Hop时间
        
        public static float ClimbUpSpeed = 4.5f;        //上爬速度
        public static float ClimbSlipSpeed = -3f;       //下滑速度
        public static float ClimbAccel = 90f;           //下滑加速度

        #endregion

        #region Jump

        public static float GroundJumpAddSpeed = 4f; //地上起跳添加的水平方向的速度

        #endregion
        
        #region WallJump
        
        public static float WallJumpCheckDist = 0.3f;
        public static float WallJumpForceTime = .16f; //墙上跳跃强制时间
        public static float WallJumpHSpeed = 13f;

        #endregion
        
        public static int UpwardCornerCorrection = 4;       //向上移动，X轴上边缘校正的最大距离（/10）


    }
}