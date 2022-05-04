using Demo.Com;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using UnityEngine;

namespace Demo.BevNode
{
    public class BEV_ACT_Jump : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            MoveCom moveCom = workData.MEntity.GetCom<MoveCom>();
            PropertyCom propertyCom = workData.MEntity.GetCom<PropertyCom>();
            Collider2DCom collider2DCom = workData.MEntity.GetCom<Collider2DCom>();

            moveCom.ReqJumpSpeed = 5;
            ResetJumpStep(moveCom, collider2DCom);

            //爬墙跳不算跳跃阶段
            if (CheckCanWallJump(collider2DCom))
            {
                WallJump(moveCom, collider2DCom);
            }
            else
            {
                if (CheckCanJump(moveCom, collider2DCom))
                {
                    moveCom.JumpStep++;
                }
            }
        }

        private void ResetJumpStep(MoveCom moveCom, Collider2DCom collider2DCom)
        {
            if (!collider2DCom.Collider.Down)
                return;
            moveCom.JumpStep = 0;
        }

        private bool CheckCanJump(MoveCom moveCom, Collider2DCom collider2DCom)
        {
            if (moveCom.JumpStep >= 2)
                return false;
            if (collider2DCom.Collider.Down)
                return true;
            if (moveCom.JumpStep < 2)
                return true;
            return true;
        }

        private bool CheckCanWallJump(Collider2DCom collider2DCom)
        {
            if (!collider2DCom.Collider.Left && !collider2DCom.Collider.Right)
            {
                return false;
            }
            if (collider2DCom.Collider.Down)
            {
                return false;
            }
            return true;
        }

        //跳墙
        private void WallJump(MoveCom moveCom, Collider2DCom collider2DCom)
        {
            if (!collider2DCom.Collider.Left && !collider2DCom.Collider.Right)
            {
                return;
            }

            if (collider2DCom.Collider.Left)
            {
                moveCom.Rig.AddForce(new Vector2(1, 1), ForceMode2D.Impulse);
                return;
            }
            if (collider2DCom.Collider.Right)
            {
                moveCom.Rig.AddForce(new Vector2(-10, 10), ForceMode2D.Impulse);
                return;
            }
        }
    }
}