using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCTimeline
{
    public abstract class InternalTimelineGraphGroupAsset : ScriptableObject
    {
        public abstract string DisplayName { get; }

        /// <summary>
        /// 获得所有视图
        /// </summary>
        /// <returns></returns>
        public abstract List<InternalTimelineGraphAsset> GetAllGraph();

        /// <summary>
        /// 检测存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract bool CheckHasGraph(string name);

        /// <summary>
        /// 当点击创建按钮
        /// </summary>
        public abstract void OnClickCreateBtn();

        /// <summary>
        /// 创建视图
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract InternalTimelineGraphAsset CreateGraph(string name);

        /// <summary>
        /// 移除视图
        /// </summary>
        /// <param name="name"></param>
        public abstract void RemoveGraph(InternalTimelineGraphAsset graph);

        /// <summary>
        /// 导出
        /// </summary>
        public virtual void OnClickExport()
        {
            TimelineGroupPath path = TimelineSetting.Setting.GetSearchPath(this.GetType().FullName);
            List<InternalTimelineGraphGroupAsset> groups = TimelineSetting.Setting.GetGroups(path.searchPath);
            List<InternalTimelineGraphAsset> assets = new List<InternalTimelineGraphAsset>();

            foreach (InternalTimelineGraphGroupAsset group in groups)
            {
                if (group.GetType() == GetType())
                {
                    assets.AddRange(group.GetAllGraph());
                }
            }

            this.ExportGraph(assets);
        }

        /// <summary>
        /// 导出视图
        /// </summary>
        /// <param name="graph"></param>
        public abstract void ExportGraph(List<InternalTimelineGraphAsset> assets);
    }
}