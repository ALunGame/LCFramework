using Scenes.MoveTest;

namespace Scenes.Move
{
    /// <summary>
    /// 地面移动
    /// </summary>
    public partial class PlayerMove
    {
        /// <summary>
        /// 土狼时间
        /// </summary>
        private int CoyotetimeFram = 0;
        
        private void Normal()
        {
            if (!IsGround())
            {
                CoyotetimeFram = 4;
                ChangeState(PlayState.Fall);
                return;
            }

            Velocity.y = 0;
            if (input.JumpKeyDown)
            {
                Jump();
                return;
            }
        }
    }
}