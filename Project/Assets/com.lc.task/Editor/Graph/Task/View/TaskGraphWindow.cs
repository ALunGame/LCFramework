using LCNode;
using LCNode.Model;
using LCNode.View;
using UnityEditor;
using UnityEngine;

namespace LCTask.TaskGraph
{
    [CustomGraphWindow(typeof(TaskGraph))]
    public class TaskGraphWindow : BaseGraphWindow
    {
        protected override BaseGraphView NewGraphView(BaseGraph graph)
        {
            return new TaskGraphView();
        }
    }
}