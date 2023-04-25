using Demo.GAS.Attribute;
using LCECS.Core;
using LCMap;

namespace Demo.Com.Model
{
    public class BaseActorModelCom : BaseCom
    {
        protected override void OnAwake(Entity pEntity)
        {
            Actor actor = pEntity as Actor;
            
            actor.Ability.Attr.AddAttr(new Attr_ActorCurrHP());
            actor.Ability.Attr.AddAttr(new Attr_ActorMaxHP());
        }
    }
}