using LCMap;

namespace Demo
{
    public class ActorMoveRequest : ActorRequest
    {
        public override int RequestId { get => (int)ActorRequestType.Move; }
        public override int Weight { get => (int)ActorRequestType.Move; }
    }
}