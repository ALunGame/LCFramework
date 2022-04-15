using LCECS.Core.Tree.Base;
using LCECS.Data;
using System.Collections.Generic;

namespace LCECS.Layer.Decision
{
    /// <summary>
    /// 决策树
    /// </summary>
    public sealed class DecisionTree
    {
        //Id
        private int treeId;
        //决策树
        private Node tree;
        //决策实体列表
        private List<EntityWorkData> EntityList = new List<EntityWorkData>();

        public DecisionTree()
        {

        }
        public DecisionTree(int treeId,Node tree)
        {
            this.treeId = treeId;
            this.tree = tree;
        }

        /// <summary>
        /// 添加决策实体
        /// </summary>
        public void AddEntity(EntityWorkData workData)
        {
            if (EntityList.Contains(workData))
            {
                return;
            }
            EntityList.Add(workData);
        }

        /// <summary>
        /// 获得决策实体
        /// </summary>
        public EntityWorkData GetEntity(int entityId)
        {
            for (int i = 0; i < EntityList.Count; i++)
            {
                EntityWorkData workData = EntityList[i];
                if (workData.MEntity.GetHashCode() == entityId)
                {
                    return workData;
                }
            }
            return null;
        }

        /// <summary>
        /// 删除决策实体
        /// </summary>
        public void RemoveEntity(int entityId)
        {
            for (int i = 0; i < EntityList.Count; i++)
            {
                EntityWorkData workData = EntityList[i];
                if (workData.MEntity.GetHashCode() == entityId)
                {
                    EntityList.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// 执行决策
        /// </summary>
        public void Execute()
        {
            for (int i = 0; i < EntityList.Count; i++)
            {
                EntityWorkData workData = EntityList[i];
                if (tree.Evaluate(workData))
                {
                    tree.Execute(workData);
                }
                else
                {
                    tree.Transition(workData);
                }
            }
        }
    }
}
