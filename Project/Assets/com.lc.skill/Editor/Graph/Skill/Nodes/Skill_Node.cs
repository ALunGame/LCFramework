using LCECS.Core;
using LCNode;
using LCNode.Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill.SkillGraph
{
    public class SkillCostData { }

    public class SkillConditionData { }

    public class SkillLearnBuffData { }

    /// <summary>
    /// 技能消耗
    /// </summary>
    public abstract class Skill_CostNode : BaseNode
    {
        public override Color TitleColor { get => Color.magenta; set => base.TitleColor = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public SkillCostData parentNode;

        public abstract Type RuntimeNode { get; }

        public abstract SkillCost CreateRuntimeNode();
    }

    /// <summary>
    /// 技能条件
    /// </summary>
    public abstract class Skill_ConditionNode : BaseNode
    {
        public override Color TitleColor { get => Color.magenta; set => base.TitleColor = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public SkillConditionData parentNode;

        [OutputPort("子节点", BasePort.Capacity.Single)]
        public SkillConditionData node;

        public abstract Type RuntimeNode { get; }

        public abstract SkillCondition CreateRuntimeNode();

        public SkillCondition GetSkillCondition()
        {
            SkillCondition skillCondition = CreateRuntimeNode();
            List<Skill_ConditionNode> nodes = NodeHelper.GetNodeOutNodes<Skill_ConditionNode>(Owner, this);
            if (nodes.Count > 0)
            {
                skillCondition.nextCondition = nodes[0].GetSkillCondition();
            }

            return skillCondition;
        }
    }

    /// <summary>
    /// 学会技能时，获得的Buff
    /// </summary>
    public abstract class Skill_LearnBuffNode : BaseNode
    {
        public override Color TitleColor { get => Color.magenta; set => base.TitleColor = value; }

        [InputPort("父节点", BasePort.Capacity.Single)]
        public SkillLearnBuffData parentNode;

        public abstract Type RuntimeNode { get; }

        public abstract AddBuffModel CreateRuntimeNode();
    }

    [NodeMenuItem("技能配置")]
    public class Skill_Node : BaseNode
    {
        public override string Title { get => "技能配置"; set => base.Title = value; }
        public override string Tooltip { get => "技能配置"; set => base.Tooltip = value; }
        public override Color TitleColor { get => Color.white; set => base.TitleColor = value; }

        [OutputPort("条件", BasePort.Capacity.Single)]
        public SkillConditionData condition;

        [OutputPort("消耗", BasePort.Capacity.Multi)]
        public SkillConditionData costs;

        [OutputPort("学会技能获得的Buff", BasePort.Capacity.Multi)]
        public SkillLearnBuffData addBuffs;

        [NodeValue("技能Timeline")]
        public string timeline = "";

        public SkillCondition GetCondition()
        {
            SkillCondition condition = null;
            //组件节点
            List<Skill_ConditionNode> nodes = NodeHelper.GetNodeOutNodes<Skill_ConditionNode>(Owner, this);
            if (nodes.Count > 0)
            {
                condition = nodes[0].GetSkillCondition();
            }
            return condition;
        }

        public List<SkillCost> GetSkillCosts()
        {
            List<SkillCost> costs = new List<SkillCost>();
            //组件节点
            List<Skill_CostNode> nodes = NodeHelper.GetNodeOutNodes<Skill_CostNode>(Owner, this);
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    costs.Add(nodes[i].CreateRuntimeNode());
                }
            }
            return costs;
        }

        public List<AddBuffModel> GetAddBuffs()
        {
            List<AddBuffModel> buffs = new List<AddBuffModel>();
            //组件节点
            List<Skill_LearnBuffNode> nodes = NodeHelper.GetNodeOutNodes<Skill_LearnBuffNode>(Owner, this);
            if (nodes.Count > 0)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    buffs.Add(nodes[i].CreateRuntimeNode());
                }
            }
            return buffs;
        }
    }
}
