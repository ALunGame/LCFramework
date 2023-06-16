using Demo.GAS.Attribute;
using LCECS.Core;
using LCMap;

namespace Demo.Com.Model
{
    public class MainActorModelCom : BaseCom
    {
        protected override void OnAwake(Entity pEntity)
        {
            Actor actor = pEntity as Actor;
            
            actor.Ability.Attr.AddAttr(new Attr_MainActorSpeedRatio());
            actor.Ability.Attr.AddAttr(new Attr_MainActorEndurance());
        }
    }
}