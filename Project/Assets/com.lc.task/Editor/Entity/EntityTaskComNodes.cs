using System;
using Demo.Com;
using LCECS.Core;
using LCECS.EntityGraph;
using LCNode;
using LCTask;

namespace com.lc.task.Editor.Entity
{
    [NodeMenuItem("全局/任务组件")]
    public class Entity_TaskCom : Entity_ComNode
    {
        public override string Title { get => "全局任务组件"; set => base.Title = value; }
        public override string Tooltip { get => "全局任务组件"; set => base.Tooltip = value; }
        public override Type RuntimeNode => typeof(TaskCom);

        public override BaseCom CreateRuntimeNode()
        {
            TaskCom com = new TaskCom();
            return com;
        }
    }
}