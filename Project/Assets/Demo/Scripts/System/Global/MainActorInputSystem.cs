using System;
using System.Collections.Generic;
using Demo.Com;
using LCECS.Core;

namespace Demo.System
{
    public class MainActorInputSystem : BaseSystem
    {
        protected override List<Type> RegContainListenComs()
        {
            return new List<Type>() {typeof(MainActorInputCom)};
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            MainActorInputCom inputCom = GetCom<MainActorInputCom>(comList[0]);
            inputCom.Update();
        }

        protected override void FixedUpdateHandleComs(List<BaseCom> comList)
        {
            MainActorInputCom inputCom = GetCom<MainActorInputCom>(comList[0]);
            inputCom.FixedUpdate();
        }
    }
}