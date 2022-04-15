using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Data;
using System.Collections.Generic;

namespace LCECS.Layer.Behavior
{
    /// <summary>
    /// 行为树
    /// </summary>
    public class BehaviorTree
    {
        private int treeId;
        private Node tree;

        public Node Tree
        {
            get
            {
                return tree;
            }
        }

        /// <summary>
        /// 行为树Id
        /// </summary>
        public int BevId
        {
            get => treeId;
        }

        //需要处理的工作数据
        private List<EntityWorkData> HandleList = new List<EntityWorkData>();

        public BehaviorTree()
        {

        }

        public BehaviorTree(int treeId, Node tree)
        {
            this.tree = tree;
            this.treeId = treeId;
        }

        /// <summary>
        /// 添加工作（现在不允许重复添加）
        /// </summary>
        public void AddWorkData(EntityWorkData workData)
        {
            if (HandleList.Contains(workData))
            {
                return;
            }
            HandleList.Add(workData);
        }

        /// <summary>
        /// 删除工作（在新的行为过来前，删除）
        /// </summary>
        public void RemoveWorkData(EntityWorkData workData)
        {
            if (!HandleList.Contains(workData))
            {
                return;
            }

            workData.CurrReqId = 0;
            //清理
            Tree.Transition(workData);
            HandleList.Remove(workData);
        }

        /// <summary>
        /// 执行行为
        /// </summary>
        public void Execute()
        {
            if (tree == null)
            {
                return;
            }
            for (int i = 0; i < HandleList.Count; i++)
            {
                EntityWorkData data = HandleList[i];

                //行为改变
                if (data.CurrReqId != treeId)
                {
                    RemoveWorkData(data);
                    continue;
                }
                if (tree.Evaluate(data))
                {
                    int treeState = tree.Execute(data);
                    //节点运行完成
                    //工作结束（删除数据）
                    if (treeState == NodeState.FINISHED)
                    {
                        RemoveWorkData(data);
                    }
                }
                else
                {
                    RemoveWorkData(data);
                }
            }
        }
    }
}
