using LCNode;
using UnityEditor;
using UnityEngine;

namespace LCTask.TaskGraph
{
    [CustomNodeView(typeof(Task_AcceptNode))]
    public class Task_ExecuteNodeView : Task_NodeView
    {
    }
}