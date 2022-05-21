using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;

namespace Demo.System
{
    //属性（数值）系统
    //1,对其他组件的值进行更新
    public class AttributeSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(AttributeCom),typeof(SpeedCom)};
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            AttributeCom attributeCom   = GetCom<AttributeCom>(comList[0]);
            SpeedCom speedCom           = GetCom<SpeedCom>(comList[1]);
            UpdateSpeedValue(attributeCom, speedCom);
        }

        private void UpdateSpeedValue(AttributeCom attributeCom,SpeedCom speedCom)
        {
            float moveSpeed = attributeCom.AttrDict["MaxMoveSpeed"];
            speedCom.MaxMoveSpeed = moveSpeed;

            float jumpSpeed = attributeCom.AttrDict["MaxJumpSpeed"];
            speedCom.MaxJumpSpeed = jumpSpeed;

            float dashSpeed = attributeCom.AttrDict["DashSpeed"];
            speedCom.DashSpeed = dashSpeed;
            
            float climbSpeed = attributeCom.AttrDict["ClimbSpeed"];
            speedCom.ClimbSpeed = climbSpeed;
        }
    }
}
