using LCECS.Core.Tree.Base;
using LCECS.Data;
using LCECS.Layer.Decision;
using LCJson;
using System.Collections.Generic;
using UnityEngine;

namespace LCECS.Server.Layer
{
    public class DecisionServer : IDecisionServer
    {
        private Dictionary<int, DecisionTree> DesDict = new Dictionary<int, DecisionTree>();
       
        public void Init()
        {
        }
        
        public void AddDecisionEntity(int decId, EntityWorkData workData)
        {
            DecisionTree tree = GetDecision(decId);
            if (tree == null)
            {
                ECSLocate.Log.LogR("添加决策实体错误，没有对应决策树>>>>>>>", decId);
                return;
            }

            //删除已经存在的
            int entityId = workData.MEntity.GetHashCode();
            List<DecisionTree> trees = DecisionHasEntity(entityId);
            for (int i = 0; i < trees.Count; i++)
            {
                trees[i].RemoveEntity(entityId);
            }

            //加入新的
            DecisionTree decision = DesDict[decId];
            decision.AddEntity(workData);
        }

        public void RemoveDecisionEntity(int decId, int entityId)
        {
            if (!DesDict.ContainsKey(decId))
            {
                return;
            }
            DecisionTree decision = DesDict[decId];
            decision.RemoveEntity(entityId);
        }

        public void Execute()
        {
            foreach (DecisionTree item in DesDict.Values)
            {
                if (item != null)
                {
                    item.Execute();
                }
            }
        }

        /// <summary>
        /// 获得决策树
        /// </summary>
        /// <returns></returns>
        private DecisionTree GetDecision(int treeId)
        {
            if (!DesDict.ContainsKey(treeId))
            {
                DecisionTree tree = LoadDecision(treeId);
                if (tree != null)
                {
                    DesDict.Add(treeId, tree);
                }
                else
                {
                    return null;
                }
            }
            return DesDict[treeId];
        }

        /// <summary>
        /// 加载决策树
        /// </summary>
        /// <returns></returns>
        private DecisionTree LoadDecision(int treeId)
        {
            string jsonStr = LCLoad.LoadHelper.LoadString(ECSDefPath.GetDecTreeCnfName(treeId));
            DecisionTree decision = JsonMapper.ToObject<DecisionTree>(jsonStr);
            if (decision == null)
                return null;
            return decision;
        }

        /// <summary>
        /// 获得其他包含此实体的决策树
        /// </summary>
        /// <returns></returns>
        private List<DecisionTree> DecisionHasEntity(int entityId)
        {
            List<DecisionTree> trees = new List<DecisionTree>();
            foreach (DecisionTree item in DesDict.Values)
            {
                if (item.GetEntity(entityId) != null)
                {
                    trees.Add(item);
                }
            }
            return trees;
        }
    }
}
