using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;

namespace Demo.System
{
    //玩家体力，能量系统
    public class PlayerEnergySystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(PlayerCom), typeof(PlayerPhysicsCom),typeof(ColliderCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            PlayerCom playerCom = GetCom<PlayerCom>(comList[0]);
            PlayerPhysicsCom physicsCom = GetCom<PlayerPhysicsCom>(comList[1]);
            ColliderCom colliderCom = GetCom<ColliderCom>(comList[2]);
            RecoverEnergy(playerCom, physicsCom,colliderCom);
        }

        //恢复体力
        private void RecoverEnergy(PlayerCom playerCom, PlayerPhysicsCom physicsCom,ColliderCom colliderCom)
        {
            if (physicsCom.Rig2D.velocity.y > 0)
            {
                //上墙
                if (colliderCom.CollideDir == ColliderDir.Left || colliderCom.CollideDir == ColliderDir.Right)
                {
                    playerCom.CurrEnergy -= playerCom.ReplyEnergy * Definition.DeltaTime * 2;
                    playerCom.CurrEnergy  = playerCom.CurrEnergy <= 0 ? 0 : playerCom.CurrEnergy;
                }
            }

            if (colliderCom.CollideDir == ColliderDir.Down)
            {
                playerCom.CurrEnergy += playerCom.ReplyEnergy * Definition.DeltaTime;
                playerCom.CurrEnergy = playerCom.CurrEnergy >= playerCom.MaxEnergy ? playerCom.MaxEnergy : playerCom.CurrEnergy;
            }
        }
    }
}
