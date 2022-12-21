using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Condition;
using Demo.Event;
using Demo.Task;

namespace Demo
{
    public class FuncModule
    {
        public void Init()
        {
            OnInit();
        }

        public virtual void OnInit() { }

        public void Clear()
        {
            OnClear();
        }

        public virtual void OnClear() { }
    }


    public class FuncModuleCtrl
    {
        private List<FuncModule> modules = new List<FuncModule>();
        private void AddModule(FuncModule pFuncModule)
        {
            modules.Add(pFuncModule);
            pFuncModule.Init();
        }

        public ConditionFuncModule Condition { get; private set; }
        public EventFuncModule Event { get; private set; }
        public WorkFuncModule Work { get; private set; }
        
        public void Init()
        {
            Condition = new ConditionFuncModule();
            AddModule(Condition);
            Event = new EventFuncModule();
            AddModule(Event);
            Work = new WorkFuncModule();
            AddModule(Work);
        }

        public void Clear()
        {

        }
    }
}
