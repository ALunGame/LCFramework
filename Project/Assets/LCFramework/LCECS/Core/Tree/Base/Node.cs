﻿using LCECS.Data;
using LCECS.Help;
using LCHelp;
using System;
using System.Collections.Generic;

namespace LCECS.Core.Tree.Base
{
    /// <summary>
    /// 节点特性 类可用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeAttribute : Attribute
    {
        /// <summary>
        /// 编辑器显示名
        /// </summary>
        public string ViewName { get; set; } = "";
        /// <summary>
        /// 子节点数量
        /// </summary>
        public int MaxChildCnt { get; set; } = 1;
        /// <summary>
        /// 基础行为节点
        /// </summary>
        public bool IsCommonAction { get; set; } = false;
        /// <summary>
        /// 行为层节点
        /// </summary>
        public bool IsBevNode { get; set; } = true;
    }

    /// <summary>
    /// 节点字段特性 字段可用
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class NodeValueAttribute : Attribute
    {
        /// <summary>
        /// 可以在编辑器模式下编辑
        /// </summary>
        public bool ViewEditor { get; set; } = false;
    }


    /// <summary>
    /// 节点环境
    /// </summary>
    public class NodeContext
    {

    }

    /// <summary>
    /// 节点基类
    /// </summary>
    public class Node
    {
        //节点唯一Id
        public string Uid;

        //最大节点数
        protected int maxChildCount;

        //子节点列表
        public List<Node> childNodes = new List<Node>();

        //节点前提
        public NodePremise nodePremise;

        public Node()
        {

        }

        /// <summary>
        /// 获取数据
        /// </summary>
        protected T GetContext<T>(NodeData wData) where T : NodeContext, new()
        {
            int uniqueKey = GetHashCode();
            T thisContext;
            if (wData.Context.ContainsKey(uniqueKey) == false)
            {
                thisContext = new T();
                wData.Context.Add(uniqueKey, thisContext);
            }
            else
            {
                try
                {
                    thisContext = (T)wData.Context[uniqueKey];
                }
                catch (Exception e)
                {
                    thisContext = new T();
                    ECSLocate.ECSLog.LogError("节点转换失败》》》》》》", uniqueKey, typeof(T).FullName);
                }
            }
            return thisContext;
        }

        /// <summary>
        /// 设置前提
        /// </summary>
        public void SetPremise(NodePremise premise)
        {
            nodePremise = premise;
        }

        #region 添加删除获取子节点

        //添加
        public bool AddChild(Node node)
        {
            childNodes.Add(node);
            return true;
        }

        //删除
        public void RemoveChild(Node node)
        {
            childNodes.Remove(node);
        }

        //数量
        public int GetChildCount()
        {
            return childNodes.Count;
        }

        //检测子节点索引是否合法
        public bool IsIndexValid(int index)
        {
            return index >= 0 && index < childNodes.Count;
        }

        //获取子节点
        public T GetChild<T>(int index) where T : Node
        {
            if (index < 0 || index >= childNodes.Count)
            {
                return null;
            }
            return (T)childNodes[index];
        }

        #endregion

        #region 生命周期函数
        //评估 （评估是否可执行）
        public bool Evaluate(NodeData wData)
        {
            return (nodePremise == null || nodePremise.IsTrue(wData)) && OnEvaluate(wData);
        }

        //子类重写 （是一个节点评估成功就执行，还是啥）
        protected virtual bool OnEvaluate(NodeData wData)
        {
            return true;
        }

        //执行
        public int Execute(NodeData wData)
        {
#if UNITY_EDITOR
            NodeRunSelEntityHelp.SetRunningNode(wData.Id, GetHashCode(), GetType().FullName);
#endif
            return OnExcute(wData);
        }

        //子类重写  （返回执行结果）
        protected virtual int OnExcute(NodeData wData)
        {
            return NodeState.FINISHED;
        }

        //节点转换 （执行下一个节点，这个时候做一些数据清理操作）
        public void Transition(NodeData wData)
        {
            OnTransition(wData);
        }

        //子类重写  （基本上执行数据清理）
        protected virtual void OnTransition(NodeData wData)
        {

        }
        #endregion
    }
}
