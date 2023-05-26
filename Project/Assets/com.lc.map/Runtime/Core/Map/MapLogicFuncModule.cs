using System.Collections.Generic;

namespace LCMap
{
    public class MapLogicFuncModuleMapping
    {
        /// <summary>
        /// 地图进入之前逻辑
        /// </summary>
        public List<MapLogicFuncModule> BeforeEnterLogics { get; private set; }
        
        /// <summary>
        /// 地图进入之后逻辑
        /// </summary>
        public List<MapLogicFuncModule> AfterEnterLogics { get; private set; }

        public MapLogicFuncModuleMapping()
        {
            BeforeEnterLogics = new List<MapLogicFuncModule>();
            AfterEnterLogics = new List<MapLogicFuncModule>();
        }

        public void ExecuteEnter(int pMapId, bool pBefore)
        {
            List<MapLogicFuncModule> logics = pBefore ? BeforeEnterLogics : AfterEnterLogics;
            for (int i = 0; i < logics.Count; i++)
            {
                logics[i].OnEnterMap(pMapId);
            }
        }
        
        public void ExecuteExit(int pMapId, bool pBefore)
        {
            List<MapLogicFuncModule> logics = pBefore ? BeforeEnterLogics : AfterEnterLogics;
            for (int i = 0; i < logics.Count; i++)
            {
                logics[i].OnExitMap(pMapId);
            }
        }
    }
    
    
    /// <summary>
    /// 地图逻辑模块，依赖地图
    /// </summary>
    public class MapLogicFuncModule
    {
        public virtual void OnEnterMap(int pMapId) { }
        
        public virtual void OnExitMap(int pMapId) { }
    }
}