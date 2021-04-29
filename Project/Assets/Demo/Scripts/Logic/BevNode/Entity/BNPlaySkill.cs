using Demo.Com;
using LCECS;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;

namespace Demo
{
    [Node(ViewName = "释放技能", IsBevNode = true)]
    public class BNPlaySkill : NodeAction
    {
        [NodeValue(ViewEditor = true)]
        public int SkillId = 0;

        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            //首先

            //组件
            SkillCom skillCom = workData.MEntity.GetCom<SkillCom>();
            skillCom.ReqSkillId = SkillId;
        }

        protected override int OnRunning(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            //组件
            SkillCom skillCom = workData.MEntity.GetCom<SkillCom>();
            if (skillCom.ReqSkillId > 0)
            {
                return NodeState.EXECUTING;
            }
            if (skillCom.CurShowSkillId== SkillId)
            {
                return NodeState.EXECUTING;
            }
            return NodeState.FINISHED;
        }
    }
}
