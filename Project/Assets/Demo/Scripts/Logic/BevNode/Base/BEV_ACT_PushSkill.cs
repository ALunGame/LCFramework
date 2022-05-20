using Demo.Com;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using LCSkill;

namespace Demo.BevNode
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

            SkillCom skillCom = workData.MEntity.GetCom<SkillCom>();
            MoveCom moveCom = workData.MEntity.GetCom<MoveCom>();
            moveCom.HasNoReqMove = true;
            skillCom.ReleaseSkill(skillId);

            LCECS.ECSLocate.Log.LogWarning("释放技能>>>>>", skillId);
        }

        protected override int OnRunning(NodeData wData)
        {
            EntityWorkData workData     = wData as EntityWorkData;
            NodeActionContext context   = GetContext<NodeActionContext>(wData);
            PushSkillData userData = context.GetUserData<PushSkillData>();

            SkillCom skillCom = workData.MEntity.GetCom<SkillCom>();
            if (skillCom.CheckSkillIsFinish(userData.skillId))
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
            MoveCom moveCom = workData.MEntity.GetCom<MoveCom>();
            moveCom.HasNoReqMove = false;
        }

    }
}