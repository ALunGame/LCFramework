using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LCECS.Core
{
    public abstract class BaseSystem
    {
        //警告执行时间毫秒
        public static int ExcuteWarnTime = 10;
        public static bool OpenTest = true;

        //检测运行时长
        private Stopwatch stopwatch;

        //监听的组件列表
        private List<Type> ListenComs = new List<Type>();

        //需要处理的组件列表
        private Dictionary<int, List<BaseCom>> IdHandleComsDict = new Dictionary<int, List<BaseCom>>();

        public void Init()
        {
            ListenComs = RegListenComs();
            if (OpenTest)
            {
                stopwatch = new Stopwatch();
            }
        }

        //检测实体是否处理符合条件
        public bool CheckEntity(Entity entity)
        {
            //是否需要检测
            bool check = true;

            //需要监听的组件列表
            List<BaseCom> listenComs = new List<BaseCom>();

            //实体关闭
            if (entity.IsEnable == false)
                check = false;
            else
            {
                for (int i = 0; i < ListenComs.Count; i++)
                {
                    string typeName = ListenComs[i].FullName;
                    BaseCom com = entity.GetCom(typeName);

                    //没有这个组件直接返回（因为组件没有删除，只有激活于禁用）
                    if (com == null)
                        return false;
                    else
                    {
                        //组件被禁用了
                        if (com.IsActive == false)
                        {
                            check = false;
                            break;
                        }
                        else
                            listenComs.Add(com);
                    }
                }
            }

            int entityId = entity.GetHashCode();
            //需要检测
            if (check)
            {
                if (!IdHandleComsDict.ContainsKey(entityId))
                {
                    IdHandleComsDict.Add(entityId, listenComs);
                    OnAddCheckComs(IdHandleComsDict[entityId]);
                }
            }
            else
            {
                if (IdHandleComsDict.ContainsKey(entityId))
                {
                    OnRemoveCheckComs(IdHandleComsDict[entityId]);
                    IdHandleComsDict.Remove(entityId);
                }
            }
            return check;
        }

        public void Excute()
        {
            if (IdHandleComsDict.Count == 0)
                return;

            if (OpenTest)
                stopwatch.Restart();
            foreach (List<BaseCom> coms in IdHandleComsDict.Values)
            {
                HandleComs(coms);
            }
            if (OpenTest)
            {
                stopwatch.Stop();
                if (stopwatch.Elapsed.TotalMilliseconds >= ExcuteWarnTime)
                {
                    ECSLocate.Log.LogWarning("系统运行超时>>>>", this.GetType(), stopwatch.Elapsed.TotalMilliseconds);
                }
            }
        }

        public void Clear()
        {
            ListenComs.Clear();
            IdHandleComsDict.Clear();
        }

        protected static T GetCom<T>(BaseCom baseCom) where T : BaseCom
        {
            return baseCom as T;
        }

        //注册需要监听的组件列表
        protected abstract List<Type> RegListenComs();

        //处理组件
        protected abstract void HandleComs(List<BaseCom> comList);

        //当组件添加检测的时候
        protected virtual void OnAddCheckComs(List<BaseCom> comList)
        {

        }

        //当组件移除检测的时候
        protected virtual void OnRemoveCheckComs(List<BaseCom> comList)
        {

        }
    }
}
