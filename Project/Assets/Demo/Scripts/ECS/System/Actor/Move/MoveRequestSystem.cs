using System;
using System.Collections.Generic;
using Demo.Com;
using LCECS.Core;
using UnityEngine;

namespace Demo
{
    /// <summary>
    /// 移动请求系统
    /// </summary>
    public class MoveRequestSystem : BaseSystem
    {
        protected override List<Type> RegContainListenComs()
        {
            return new List<Type>() { typeof(MoveRequestCom),typeof(MoveCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            MoveRequestCom requestCom = GetCom<MoveRequestCom>(comList[0]);
            if (requestCom.IsNull())
                return;
            
            if (requestCom.IsFinish())
            {
                requestCom.Finsih();
            }
            else
            {
                MoveCom moveCom = GetCom<MoveCom>(comList[1]);
                float xDelta = requestCom.GetXDeltaPos();
                moveCom.CurrentMoveInfo.xDelta = xDelta;
            }
        }
    }
}