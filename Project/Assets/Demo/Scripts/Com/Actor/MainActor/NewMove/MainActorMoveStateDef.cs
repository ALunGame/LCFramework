using System;

namespace Demo.Com.MainActor.NewMove
{
    public static class MainActorMoveStateDef
    {
        public static Type Run;
        public static Type Jump;
        public static Type Climb;
        
        static MainActorMoveStateDef()
        {
            Run = typeof(MainActorRunState);
            Jump = typeof(MainActorJumpState);
            Climb = typeof(MainActorClimbState);
        }
    }
}