using System.Collections.Generic;
using System.Linq;
using Demo.Com;
using LCECS;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;

namespace Demo.BevNode
{
    [Node(ViewName = "玩家普通攻击节点", IsBevNode = true)]
    public class BNPlayerNormalAttack : NodeAction
    {
        private Dictionary<int,int> NormalSkillDict=new Dictionary<int, int>(){{1,2001},{2,2002},{3,2003}};
        
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            //参数
            bool doAttack = workData.GetReqParam(workData.CurrReqId).GetBool();
            if (doAttack == false)
            {
                return;
            }

            //组件
            SkillCom skillCom = workData.MEntity.GetCom<SkillCom>();
            skillCom.ReqSkillId = GetCurrNeedPlaySkillId(skillCom);
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
            return NodeState.FINISHED;
        }

        private int GetCurrNeedPlaySkillId(SkillCom skillCom)
        {
            int skillIndex = 0;
            foreach (var item in NormalSkillDict)
            {
                if (item.Value==skillCom.LastShowSkillId)
                {
                    skillIndex = item.Key+1;
                    break;
                }
            }

            if (!NormalSkillDict.ContainsKey(skillIndex))
            {
                return NormalSkillDict[1];
            }
            return NormalSkillDict[skillIndex];
        }
    }

}