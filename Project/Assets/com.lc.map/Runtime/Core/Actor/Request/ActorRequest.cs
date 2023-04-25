namespace LCMap
{
    public abstract class ActorRequest
    {
        /// <summary>
        /// 请求Id
        /// </summary>
        public abstract int RequestId { get; }
        
        /// <summary>
        /// 请求权重
        /// </summary>
        public abstract int Weight { get; }
        
        public virtual void OnEnter(ActorRequestSpec pSpec, params object[] pParams)
        {
            
        }
        
        public virtual void OnExit(ActorRequestSpec pSpec)
        {
            
        }

        public virtual ActorRequestSpec CreateSpec(Actor pActor)
        {
            ActorRequestSpec spec = new ActorRequestSpec(pActor, this);
            return spec;
        }
    }

    public class NullActorRequest : ActorRequest
    {
        public const int NullRequestId = -999;
        public const int NullWeight = -999;
            
        public override int RequestId { get => NullRequestId; }
        public override int Weight { get => NullWeight; }
    }

    public class CustomActorRequest : ActorRequest
    {
        public override int RequestId { get => -1;}
        public override int Weight { get => 1; }
    }
}