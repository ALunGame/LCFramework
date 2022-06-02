using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using LCToolkit;

namespace Demo.System
{
    public class PlayerMoveSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(PropertyCom), typeof(PlayerMoveCom), typeof(AnimCom), typeof(Collider2DCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            PropertyCom propertyCom = GetCom<PropertyCom>(comList[0]);
            PlayerMoveCom moveCom = GetCom<PlayerMoveCom>(comList[1]);

            if (moveCom.ReqMove != Vector3.zero)
            {
                moveCom.Rig.MovePosition(moveCom.Rig.position + moveCom.ReqMove.ToVector2());
                moveCom.ReqMove = Vector3.zero;
            }

            if (moveCom.HasNoReqMove)
                return;

            //移动
            moveCom.Rig.velocity = new Vector2(moveCom.ReqMoveSpeed, moveCom.Rig.velocity.y);
            if (moveCom.ReqJumpSpeed != 0)
                moveCom.Rig.velocity = new Vector2(moveCom.Rig.velocity.x, moveCom.ReqJumpSpeed);

            HandleGravitySpeed(moveCom, propertyCom);
            HandleMoveAnim(comList);
        }

        private void HandleGravitySpeed(PlayerMoveCom moveCom, PropertyCom propertyCom)
        {
            float massValue = 0;
            switch (moveCom.CurrMoveType)
            {
                case Behavior.MoveType.None:
                case Behavior.MoveType.Run:
                case Behavior.MoveType.Jump:
                    massValue = 1;
                    break;
                case Behavior.MoveType.Climb:
                case Behavior.MoveType.ClimbJump:
                case Behavior.MoveType.GrabWall:
                    massValue = 0.6f;
                    break;
                default:
                    break;
            }
            Vector2 gVector = new Vector2(0, GravitySystem._G);

            if (moveCom.Rig.velocity.y < 0)
            {
                moveCom.Rig.velocity += gVector * 1.5f * massValue * Time.deltaTime;
            }
            else if (moveCom.Rig.velocity.y > 0)
            {
                moveCom.Rig.velocity += gVector * 1 * massValue * Time.deltaTime;
            }
        }

        private void HandleMoveAnim(List<BaseCom> comList)
        {
            PlayerMoveCom moveCom = GetCom<PlayerMoveCom>(comList[1]);
            AnimCom animCom = GetCom<AnimCom>(comList[2]);
            Collider2DCom collider2DCom = GetCom<Collider2DCom>(comList[3]);

            //地面
            if (collider2DCom.Collider.Down)
            {
                if (moveCom.Rig.velocity.y > 0)
                {
                    animCom.SetReqAnim("jumpUp");
                    return;
                }
                if (moveCom.Rig.velocity.x != 0)
                {
                    animCom.SetReqAnim("run");
                    return;
                }
                else
                {
                    animCom.SetReqAnim("idle");
                    return;
                }
            }
            else
            {
                if (collider2DCom.Collider.Left || collider2DCom.Collider.Right)
                {
                    animCom.SetReqAnim("climb");
                    return;
                }
                //空中
                if (collider2DCom.Collider.IsNull() || collider2DCom.Collider.Up)
                {
                    if (moveCom.Rig.velocity.y > 0)
                    {
                        animCom.SetReqAnim("jumpUp");
                        return;
                    }
                    else
                    {
                        animCom.SetReqAnim("jumpDown");
                        return;
                    }
                }
            }
        }
    }
}