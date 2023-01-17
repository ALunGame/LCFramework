using LCMap;

namespace Demo
{
    public class ActorMoveRequest : ActorRequest
    {
        public override int RequestId { get => (int)ActorRequestType.Move; }
        public override int Weight { get => (int)ActorRequestType.Move; }

        public override void OnEnter(Actor pActor, params object[] pParams)
        {
            base.OnEnter(pActor, pParams);
        }

        public override void OnExit(Actor pActor)
        {
            base.OnExit(pActor);
        }
    }
}