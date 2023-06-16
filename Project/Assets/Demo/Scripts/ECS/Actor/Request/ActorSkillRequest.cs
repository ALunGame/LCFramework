using System.Collections.Generic;
using LCGAS;
using LCMap;
using LCSkill;
using LCToolkit;

namespace Demo
{
    public class ActorSkillRequestSpec : ActorRequestSpec
    {
        public ActorSkillRequestSpec(Actor pOwnerActor, ActorRequest pModel) : base(pOwnerActor, pModel)
        {
        }
        
        private void OnActorTagChange()
        {
            ActorSkillRequest skillRequest = Model as ActorSkillRequest;
            if (!Owner.Ability.Tag.HasAny(skillRequest.SkillTag))
            {
                ActorLocate.ActorRequest.FinishRequest(Owner,skillRequest.RequestId);
            }
        }
        
        public override void OnEnter(params object[] pParams)
        {
            Owner.Ability.RegTagChangeEvent(OnActorTagChange);
            base.OnEnter(pParams);
        }

        public override void OnExit()
        {
            Owner.Ability.RemoveTagChangeEvent(OnActorTagChange);
            base.OnExit();
        }
    }
    
    public class ActorSkillRequest : ActorRequest
    {
        public override int RequestId { get => (int)ActorRequestType.Skill; }
        public override int Weight { get => (int)ActorRequestType.Skill; }

        public GameplayTagContainer SkillTag = new GameplayTagContainer(new List<string>(){"GA.Skill"});

        public override void OnEnter(ActorRequestSpec pSpec, params object[] pParams)
        {
            if (!pSpec.Owner.Ability.TryActiveGameplayAbility(AbilityNameDef.Skill, pParams))
                ActorLocate.ActorRequest.FinishRequest(pSpec.Owner,RequestId);
        }

        public override void OnExit(ActorRequestSpec pSpec)
        {
        }

        public override ActorRequestSpec CreateSpec(Actor pActor)
        {
            return new ActorSkillRequestSpec(pActor, this);
        }
    }
}