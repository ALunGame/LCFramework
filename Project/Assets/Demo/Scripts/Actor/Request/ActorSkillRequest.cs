using Demo.Mediator;
using LCMap;

namespace Demo
{
    public class ActorSkillRequest : ActorRequest
    {
        public override int RequestId { get => (int)ActorRequestType.Skill; }
        public override int Weight { get => (int)ActorRequestType.Skill; }

        public override void OnEnter(Actor pActor, params object[] pParams)
        {
            int skillId = (int)pParams[0];
            ActorMediator.ReleaseSkill(pActor, skillId);
        }

        public override void OnExit(Actor pActor)
        {
            base.OnExit(pActor);
        }
    }
}