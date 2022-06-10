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
            dropdownMenu.AppendAction("��������Id", delegate
            {
                MiscHelper.Input("����������Id", (x) =>
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
                node.Title = $"����{node.taskId}����";
            }
            if (node is Task_ExecuteNode)
            {
                node.Title = $"�ύ{node.taskId}����";
            }
        }
    } 
}
