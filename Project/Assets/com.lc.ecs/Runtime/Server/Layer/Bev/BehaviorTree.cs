using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Data;
using System.Collections.Generic;
using UnityEngine;

namespace LCECS.Layer.Behavior
{
    /// <summary>
    /// 行为树
    /// </summary>
    public class BehaviorTree
    {
        private RequestId reqId;
        private Node tree;

        public Node Tree
        {
            get
            {
                return tree;
            }
        }

        //需要处理的工作数据
        private List<EntityWorkData> HandleList = new List<EntityWorkData>();

        public BehaviorTree()
        {

        }

        public BehaviorTree(RequestId reqId, Node tree)
        {
            this.tree = tree;
            this.reqId = reqId;
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
        /// 删除工作
        /// </summary>
        public void RemoveWorkData(EntityWorkData workData)
        {
            if (!HandleList.Contains(workData))
                return;

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
                if (data.CurrReqId != reqId)
                {
                    //清理
                    RemoveWorkData(data);
                    continue;
                }

                if (tree.Evaluate(data))
                {
                    int treeState = tree.Execute(data);
                    //节点运行完成
                    if (treeState == NodeState.FINISHED)
                    {
                        data.RemoveCurrReqCnt();
                        if (data.CurrReqCnt <= 0)
                        {
                            OnBehaviorFinish(data);
                        }
                    }
                }
                else
                {
                    RemoveWorkData(data);
                }
            }
        }

        private void OnBehaviorFinish(EntityWorkData workData)
        {
            if (!HandleList.Contains(workData))
            {
                return;
            }
            workData.ChangeRequestId(RequestId.None);
            //清理
            Tree.Transition(workData);
            HandleList.Remove(workData);
        }
    }
}
