using LCNode.Model;
using LCNode.View;
using LCTask.TaskGraph;
using LCToolkit;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCTask
{
    public class TaskGraphView : BaseGraphView
    {
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            if (evt.target is GraphView)
            {
                evt.menu.AppendAction("创建任务",OnClickCreateTask);
                evt.menu.AppendSeparator();
            }
        }

        private void OnClickCreateTask(DropdownMenuAction menuAction)
        {
            MiscHelper.Input("输入任务Id", (x) =>
            {
                int taskId = int.Parse(x);

                Vector2 pos = GetMousePosition();

                CommandDispacter.BeginGroup();

                BaseNodeVM createAcceptNode = Model.NewNode(typeof(Task_AcceptNode), pos);
                ((Task_AcceptNode)createAcceptNode.Model).taskId = taskId;
                CommandDispacter.Do(new AddNodeCommand(Model, createAcceptNode));

                BaseNodeVM createExecuteNode = Model.NewNode(typeof(Task_ExecuteNode), pos + new Vector2(0,250));
                ((Task_ExecuteNode)createExecuteNode.Model).taskId = taskId;
                CommandDispacter.Do(new AddNodeCommand(Model, createExecuteNode));

                CommandDispacter.EndGroup();
            });
        }
    }
}