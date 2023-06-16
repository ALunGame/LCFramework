using Demo.Com;
using LCECS;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.System
{
    public class GazeSurroundSystem : BaseSystem
    {

        protected override List<Type> RegContainListenComs()
        {
            return new List<Type>() { typeof(GazeSurroundCom), typeof(TransCom), typeof(BasePropertyCom), };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            GazeSurroundCom gazeSurroundCom = GetCom<GazeSurroundCom>(comList[0]);
            TransCom transCom = GetCom<TransCom>(comList[1]);
            BasePropertyCom propertyCom = GetCom<BasePropertyCom>(comList[2]);

            if (gazeSurroundCom.gazeUid == "")
                return;
            Entity gazeEntity = ECSLocate.ECS.GetEntity(gazeSurroundCom.gazeUid);
            if (gazeEntity == null)
                return;

            Vector2 selfPos = transCom.Pos;
            Vector2 targetPos = gazeEntity.GetCom<TransCom>().Pos;

            //方向
            // if (selfPos.x - targetPos.x > 0)
            //     transCom.Roate(DirType.Left);
            // else
            //     transCom.Roate(DirType.Right);
            //
            // //移动
            // gazeSurroundCom.moveDir = CalcWanderMoveDir(selfPos, gazeSurroundCom, targetPos);
            // transCom.MoveDir(gazeSurroundCom.moveDir, propertyCom.MoveSpeed.Curr);
        }

        public DirType CalcWanderMoveDir(Vector2 selfPos, GazeSurroundCom gazeSurroundCom, Vector2 targetPos)
        {
            float distance = Vector2.Distance(selfPos, targetPos);

            //太近
            if (distance < gazeSurroundCom.gazeRange.x)
            {
                if (selfPos.x - targetPos.x > 0)
                {
                    return DirType.Right;
                }
                else
                {
                    return DirType.Left;
                }
            }

            //太远
            if (distance > gazeSurroundCom.gazeRange.y)
            {
                if (selfPos.x - targetPos.x > 0)
                {
                    return DirType.Left;
                }
                else
                {
                    return DirType.Right;
                }
            }

            return gazeSurroundCom.moveDir;
        }

    }
}