using System.Collections.Generic;
using LCMap;

namespace LCToolkit
{
    
    public abstract class ServerLogicModuleMapping
    {

        private List<IServerLogicModule> beforeServerInitLogics = null;
        /// <summary>
        /// 服务初始化之前调用逻辑
        /// </summary>
        public List<IServerLogicModule> BeforeServerInitLogics
        {
            get
            {
                if (beforeServerInitLogics == null)
                {
                    beforeServerInitLogics = RegBeforeServerInitLogics();
                }

                return beforeServerInitLogics;
            }
        }
        
        /// <summary>
        /// 注册服务初始化之前调用逻辑
        /// </summary>
        /// <returns></returns>
        public abstract List<IServerLogicModule> RegBeforeServerInitLogics();

        
        private List<IServerLogicModule> afterServerInitLogics = null;
        /// <summary>
        /// 服务初始化之后调用逻辑
        /// </summary>
        public List<IServerLogicModule> AfterServerInitLogics {
            get
            {
                if (afterServerInitLogics == null)
                {
                    afterServerInitLogics = RegAfterServerInitLogics();
                }

                return afterServerInitLogics;
            }
        }

        /// <summary>
        /// 注册服务初始化之后调用逻辑
        /// </summary>
        /// <returns></returns>
        public abstract List<IServerLogicModule> RegAfterServerInitLogics();


        public void ExecuteInit(BaseServer pServer, bool pBefore)
        {
            List<IServerLogicModule> logics = pBefore ? BeforeServerInitLogics : AfterServerInitLogics;
            for (int i = 0; i < logics.Count; i++)
            {
                logics[i].Init(pServer);
            }
        }
        
        public void ExecuteClear(bool pBefore)
        {
            List<IServerLogicModule> logics = pBefore ? BeforeServerInitLogics : AfterServerInitLogics;
            for (int i = 0; i < logics.Count; i++)
            {
                logics[i].Clear();
            }
        }
    }


    public interface IServerLogicModule
    {
        void Init(BaseServer pServer);

        void Clear();
    }
    
    
    /// <summary>
    /// 服务逻辑模块
    /// 1，对服务逻辑的抽离
    /// 2，提供业务层对服务的扩展
    /// </summary>
    public abstract class BaseServerLogicModule<T> : IServerLogicModule where T : BaseServer
    {
        protected T server;
        
        public void Init(BaseServer pServer)
        {
            server = (T)pServer;
            OnInit();
        }

        public void Clear()
        {
            OnClear();
        }

        public virtual void OnInit()
        {
            
        }
        
        public virtual void OnClear()
        {
            
        }
    }
}