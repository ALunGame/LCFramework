using LCECS.Core.Tree.Base;
using LCECS.Data;
using System.Collections.Generic;

namespace LCECS.Layer.Decision
{
    /// <summary>
    /// 实体决策
    /// </summary>
    public class BaseDecision
    {
        //决策树
        private Node Tree;
        private List<EntityWorkData> EntityList = new List<EntityWorkData>();

        public BaseDecision(Node tree)
        {
            Tree = tree;
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
        public virtual void Execute()
        {
            for (int i = 0; i < EntityList.Count; i++)
            {
                EntityWorkData workData = EntityList[i];
                if (Tree.Evaluate(workData))
                {
                    Tree.Execute(workData);
                }
                else
                {
                    Tree.Transition(workData);
                }
            }
        }
    }
}
