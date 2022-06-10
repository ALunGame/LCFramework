using LCNode;
using LCNode.View;
using LCToolkit;
using UnityEngine.UIElements;

namespace LCTask.TaskGraph
{
    [CustomNodeView(typeof(Task_Node))]
    public class Task_NodeView : BaseNodeView
    {
        public Task_NodeView()
        {
            SetDeletable(false);
        }

        public override void CreateSelectMenu(DropdownMenu dropdownMenu)
        {
            Task_Node node = Model as Task_Node;
            dropdownMenu.AppendAction("设置任务Id", delegate
            {
                MiscHelper.Input("输入新任务Id", (x) =>
                {
                    int newTaskId = int.Parse(x);
                    node.taskId = newTaskId;
                    RefreshTitle();
                });   
            });
        }

        private void RefreshTitle()
        {
            Task_Node node = Model as Task_Node;
            if (node is Task_AcceptNode)
            {
                node.Title = $"接受{node.taskId}任务";
            }
            if (node is Task_ExecuteNode)
            {
                node.Title = $"提交{node.taskId}任务";
            }
        }
    } 
}
