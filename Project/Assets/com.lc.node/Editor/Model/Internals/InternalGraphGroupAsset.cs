using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCNode.Model.Internal
{
    public abstract class InternalGraphGroupAsset : ScriptableObject
    {
        public abstract string DisplayName { get; }

        /// <summary>
        /// 获得所有视图
        /// </summary>
        /// <returns></returns>
        public abstract List<InternalBaseGraphAsset> GetAllGraph();

        /// <summary>
        /// 检测存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract bool CheckHasGraph(string name);

        /// <summary>
        /// 创建视图
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract bool CreateGraph(string name);  

        /// <summary>
        /// 移除视图
        /// </summary>
        /// <param name="name"></param>
        public abstract void RemoveGraph(InternalBaseGraphAsset graph);

        /// <summary>
        /// 导出视图
        /// </summary>
        /// <param name="graph"></param>
        public abstract void ExportGraph(InternalBaseGraphAsset graph);
    }
}
