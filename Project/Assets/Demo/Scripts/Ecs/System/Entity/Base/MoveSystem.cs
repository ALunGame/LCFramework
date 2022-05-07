using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.System
{
    public class MoveSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(PropertyCom), typeof(MoveCom), typeof(AnimCom), typeof(Collider2DCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            PropertyCom propertyCom = GetCom<PropertyCom>(comList[0]);
            MoveCom moveCom = GetCom<MoveCom>(comList[1]);

            if (moveCom.HasNoReqMove)
                return;

            //移动
            moveCom.Rig.velocity = new Vector2(moveCom.ReqMoveSpeed, moveCom.Rig.velocity.y);
            if (moveCom.ReqJumpSpeed != 0)
                moveCom.Rig.velocity = new Vector2(moveCom.Rig.velocity.x, moveCom.ReqJumpSpeed);

            HandleGravitySpeed(moveCom, propertyCom);
            Debug.LogFormat("MoveSystem>>>>>velocity：{0}", moveCom.Rig.velocity);
            if (moveCom.ReqJumpSpeed != 0)
            {
                Debug.LogFormat("MoveSystem>>>>>velocity：{0}", moveCom.Rig.velocity);
            }
            HandleMoveAnim(comList);
        }

        private void HandleGravitySpeed(MoveCom moveCom, PropertyCom propertyCom)
        {
            if (moveCom.Rig.velocity.y < 0)
            {
                moveCom.Rig.velocity += Definition.Gravity * 1.5f * propertyCom.Mass * Time.deltaTime;
            }
            else if (moveCom.Rig.velocity.y > 0)
            {
                moveCom.Rig.velocity += Definition.Gravity * 1 * propertyCom.Mass * Time.deltaTime;
            }
        }

        private void HandleMoveAnim(List<BaseCom> comList)
        {
            MoveCom moveCom = GetCom<MoveCom>(comList[1]);
            AnimCom animCom = GetCom<AnimCom>(comList[2]);
            Collider2DCom collider2DCom = GetCom<Collider2DCom>(comList[3]);

            //地面
            if (collider2DCom.Collider.Down)
            {
                if (moveCom.Rig.velocity.y > 0)
                {
                    animCom.ReqAnimName = "jumpUp";
                    return;
                }
                if (moveCom.Rig.velocity.x != 0)
                {
                    animCom.ReqAnimName = "run";
                    return;
                }
                else
                {
                    animCom.ReqAnimName = "idle";
                    return;
                }
            }
            else
            {
                if (collider2DCom.Collider.Left || collider2DCom.Collider.Right)
                {
                    animCom.ReqAnimName = "climb";
                    return;
                }
                //空中
                if (collider2DCom.Collider.IsNull() || collider2DCom.Collider.Up)
                {
                    if (moveCom.Rig.velocity.y > 0)
                    {
                        animCom.ReqAnimName = "jumpUp";
                        return;
                    }
                    else
                    {
                        animCom.ReqAnimName = "jumpDown";
                        return;
                    }
                }
            }
        }
    }
}