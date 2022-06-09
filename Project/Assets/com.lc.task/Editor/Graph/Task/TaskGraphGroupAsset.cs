using LCNode.Model;
using LCNode.Model.Internal;
using System.Collections.Generic;
using UnityEngine;

namespace LCTask.TaskGraph
{
    [CreateAssetMenu(fileName = "Task×é", menuName = "ÅäÖÃ×é/Task×é", order = 1)]
    public class TaskGraphGroupAsset : BaseGraphGroupAsset<TaskGraphAsset>
    {
        public override string DisplayName => "Task";

        public override void ExportGraph(List<InternalBaseGraphAsset> assets)
        {
            throw new System.NotImplementedException();
        }
    } 
}
