using System;
using System.Collections.Generic;
using Cnf;
using Demo.Com.MainActor.NewMove;
using Demo.Life.State;
using Demo.Life.State.Content;
using LCECS.Core;
using LCMap;
using LCToolkit.FSM;

namespace Demo.Life
{
    public static class ActorLifeStateDef
    {
        public static Type Fight;
        public static Type Reset;
        public static Type Work;
        
        static ActorLifeStateDef()
        {
            Fight = typeof(ActorFightState);
            Reset = typeof(ActorResetState);
            Work = typeof(ActorWorkState);
        }
    }
    
    public class ActorLifeState : FsmState
    {
        internal ActorLifeCom lifeCom;
        
        protected internal override void OnInit()
        {
            lifeCom = Owner.GetCom<ActorLifeCom>();
        }
    }

    public enum ActorWorker
    {
        Manager,
        Collecter,
        Producer,
    }
    
    public class ActorLifeCom : BaseCom
    {
        
        public Actor Owner { get; private set; }
        
        /// <summary>
        /// 工种
        /// </summary>
        public ActorWorker WorkerType { get; private set; }
        
        /// <summary>
        /// 工作策略Id
        /// </summary>
        public int CnfId { get; private set; }
        
        private Stack<ActorWorkContent> workContents = new Stack<ActorWorkContent>();
        /// <summary>
        /// 工作内容
        /// </summary>
        public Stack<ActorWorkContent> WorkContents
        {
            get => workContents;
        }
        
        private Fsm fsm;
        private ActorLifeState[] states = 
        {
            new ActorFightState(),
            new ActorResetState(),
            new ActorWorkState(),
        };

        protected override void OnAwake(Entity pEntity)
        {
            Actor actor = pEntity as Actor;

            if (LCConfig.Config.ActorLifeCnf.TryGetValue(actor.Id, out ActorLifeCnf cnf))
            {
                if (cnf.produceId != 0)
                {
                    WorkerType = ActorWorker.Producer;
                    CnfId = cnf.produceId;
                }
                if (cnf.collectId != 0)
                {
                    WorkerType = ActorWorker.Collecter;
                    CnfId = cnf.collectId;
                }
            }
            
            fsm = Fsm.Create(actor,states);
            fsm.Start(ActorLifeStateDef.Reset);
            
        }
        
        public void WorkSuccess()
        {
            workContents.Pop();
        }
    }
}