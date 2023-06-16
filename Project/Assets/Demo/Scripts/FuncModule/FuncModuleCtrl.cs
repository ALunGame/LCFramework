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


        public void Clear()
        {
            OnClear();
        }

        public virtual void OnInit() { }
        
        public virtual void OnEnterMap(int pMapId) { }
        
        public virtual void OnClear() { }
        
        public virtual void OnExitMap(int pMapId) { }

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
        
        public void Init()
        {
            Condition = new ConditionFuncModule();
            AddModule(Condition);
            Event = new EventFuncModule();
            AddModule(Event);
        }

        public void Clear()
        {
            for (int i = 0; i < modules.Count; i++)
            {
                modules[i].Clear();
            }
        }
    }
}
