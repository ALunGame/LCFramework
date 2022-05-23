using Demo.Com;
using Demo.System;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using UnityEngine;

namespace Demo.Behavior
{
    public class BEV_ACT_Wander : NodeAction
    {
        class WanderData
        {
            public DirType reqMoveDir = DirType.None;
        }

        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            //参数
            float wanderRange = workData.GetParam().GetFloat();

            //获取环境数据
            NodeActionContext context = GetContext<NodeActionContext>(wData);
            WanderData userData = context.GetUserData<WanderData>();

            //计算方向
            userData.reqMoveDir = CalcMoveDir(wData);

            //组件
            WanderCom wanderCom = workData.MEntity.GetCom<WanderCom>();
            TransformCom transCom = workData.MEntity.GetCom<TransformCom>();
            Collider2DCom collider2DCom = workData.MEntity.GetCom<Collider2DCom>();
            PropertyCom propertyCom = workData.MEntity.GetCom<PropertyCom>();

            //更新方向
            transCom.ReqDir = userData.reqMoveDir;

        }

        private DirType CalcMoveDir(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            //获取环境数据
            NodeActionContext context = GetContext<NodeActionContext>(wData);
            WanderData userData = context.GetUserData<WanderData>();

            if (userData.reqMoveDir == DirType.None)
            {
                return (DirType)Random.Range((int)DirType.Left, (int)DirType.Right);
            }
            else
            {
                Collider2DCom collider2DCom = workData.MEntity.GetCom<Collider2DCom>();
                return WanderSystem.CalcWanderMoveDir(collider2DCom, userData.reqMoveDir);
            }
        }
    }
}
