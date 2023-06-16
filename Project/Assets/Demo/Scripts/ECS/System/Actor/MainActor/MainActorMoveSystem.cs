using System;
using System.Collections.Generic;
using Demo.Com.MainActor;
using LCECS.Core;

namespace Demo.System.MainActor
{
    public class MainActorMoveSystem : BaseSystem
    {
        protected override List<Type> RegContainListenComs()
        {
            return new List<Type>() {typeof(MainActorMoveCom)};
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            MainActorMoveCom moveCom = GetCom<MainActorMoveCom>(comList[0]);
            moveCom.Update();
        }

        protected override void FixedUpdateHandleComs(List<BaseCom> comList)
        {
            MainActorMoveCom moveCom = GetCom<MainActorMoveCom>(comList[0]);
            moveCom.FixedUpdate();
        }
    }
}