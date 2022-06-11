using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCTask
{
    public class TaskBranch
    {
        public int branchId;
        public List<TaskConditionFunc> conditionFuncs = new List<TaskConditionFunc>();
        public List<TaskActionFunc> actionFuncs       = new List<TaskActionFunc>();
    }
}
