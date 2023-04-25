using System;
using System.Collections.Generic;
using Demo.Com.MainActor.NewMove;
using LCECS.Core;
using UnityEngine;

namespace Demo.System.MainActor
{
    public class NewMainActorMoveSystem : BaseSystem
    {
        protected override List<Type> RegContainListenComs()
        {
            return new List<Type>() {typeof(NewMainActorMoveCom)};
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            NewMainActorMoveCom moveCom = GetCom<NewMainActorMoveCom>(comList[0]);
            moveCom.Update(Time.deltaTime);
        }

        protected override void FixedUpdateHandleComs(List<BaseCom> comList)
        {
            NewMainActorMoveCom moveCom = GetCom<NewMainActorMoveCom>(comList[0]);
            moveCom.FixeUpdate(Time.fixedDeltaTime);
            //moveCom.Update(Time.fixedDeltaTime);
        }
    }
}