using Demo.Com;
using Demo.System;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using LCSkill;
using UnityEngine;

namespace Demo.Behavior
{
    /// <summary>
    /// 释放技能
    /// </summary>
    public class BEV_ACT_PushSkill : NodeAction
    {
        class PushSkillData
        {
            public string skillId;
        }

        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            string skillId = workData.GetParam().GetString();

            //获取环境数据
            NodeActionContext context = GetContext<NodeActionContext>(wData);
            PushSkillData userData = context.GetUserData<PushSkillData>();
            userData.skillId = skillId;

            //打断移动
            TransformCom transformCom = workData.MEntity.GetCom<TransformCom>();
            if (transformCom != null)
            {
                transformCom.ReqMove = Vector3.zero;
                transformCom.ReqDir  = DirType.None;
            }
            PlayerMoveCom moveCom = workData.MEntity.GetCom<PlayerMoveCom>();
            if (moveCom != null)
            {
                moveCom.HasNoReqMove = true;
                moveCom.Rig.velocity = Vector2.zero;
            }

            //释放技能
            SkillCom skillCom = workData.MEntity.GetCom<SkillCom>();
            if (skillCom.ReleaseSkill(skillId))
            {
                userData.skillId = skillId;
            }
            else
            {
                userData.skillId = "-1";
            }
            LCECS.ECSLocate.Log.LogWarning("释放技能>>>>>", skillId);
        }

        protected override int OnRunning(NodeData wData)
        {
            EntityWorkData workData     = wData as EntityWorkData;
            NodeActionContext context   = GetContext<NodeActionContext>(wData);
            PushSkillData userData = context.GetUserData<PushSkillData>();

            SkillCom skillCom = workData.MEntity.GetCom<SkillCom>();
            if (userData.skillId == "-1" || skillCom.CheckSkillIsFinish(userData.skillId))
            {
                return NodeState.FINISHED;
            }
            else
            {
                return NodeState.EXECUTING;
            }
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
            EntityWorkData workData = wData as EntityWorkData;
            NodeActionContext context = GetContext<NodeActionContext>(wData);
            PushSkillData userData = context.GetUserData<PushSkillData>();
            userData.skillId = "-1";

            //打断移动
            AnimCom animCom = workData.MEntity.GetCom<AnimCom>();
            if (animCom != null)
            {
                animCom.SetReqAnim(AnimSystem.IdleState);
            }
        }

    }
}