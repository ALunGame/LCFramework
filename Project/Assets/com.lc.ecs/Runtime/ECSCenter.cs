using LCECS.Config;
using LCECS.Core;
using LCECS.Core.Tree;
using LCToolkit;
using System;
using System.Threading;
using UnityEngine;

namespace LCECS
{
    /// <summary>
    /// ECS启动中心
    /// </summary>
    public class ECSCenter
    {
        private RequestSortAsset requestSortAsset;
        private SystemSortAsset systemSortAsset;
        private bool _Init = false;
        private bool DecThreadRun = false;

        #region 初始化

        public void Init(RequestSortAsset requestSort, SystemSortAsset systemSortAsset)
        {
            this.requestSortAsset = requestSort;
            this.systemSortAsset = systemSortAsset;
            _Init = true;
            DecThreadRun = true;
            InitServer();
            RegSystems();
            Execute_Thread();
        }

        //设置服务
        private void InitServer()
        {
            ECSLocate.InitServer(this);
            ECSLayerLocate.InitLayerServer();
        }

        //注册系统
        private void RegSystems()
        {
            foreach (var item in systemSortAsset.GetSystemSorts(SystemType.Update))
            {
                Type systemType = ReflectionHelper.GetType(item.typeFullName);
                if (systemType == null)
                {
                    LCECS.ECSLocate.Log.LogError("系统创建失败>>>", systemType, item.typeFullName);
                    continue;
                }
                object systemObj = ReflectionHelper.CreateInstance(systemType);
                BaseSystem system = systemObj as BaseSystem;
                system.Init();
                ECSLocate.ECS.RegUpdateSystem(system);
            }
            foreach (var item in systemSortAsset.GetSystemSorts(SystemType.FixedUpdate))
            {
                Type systemType = ReflectionHelper.GetType(item.typeFullName);
                BaseSystem system = ReflectionHelper.CreateInstance(systemType) as BaseSystem;
                system.Init();
                ECSLocate.ECS.RegFixedUpdateSystem(system);
            }
        }

        public void Clear()
        {
            _Init = false;
            DecThreadRun = false;
            ECSLocate.Clear();
        }

        #endregion

        #region Execute

        public void Execute_Update()
        {
            if (!_Init)
                return;

            //更新树时间
            NodeTime.UpdateTime(Time.deltaTime, 1);

            //执行行为树
            ECSLayerLocate.Behavior.Execute();

            //系统处理
            ECSLocate.ECS.ExcuteUpdateSystem();
        }

        public void Execute_FixedUpdate()
        {
            if (!_Init)
                return;

            //系统处理
            ECSLocate.ECS.ExcuteFixedUpdateSystem();
        }

        public void Execute_Thread()
        {
            if (!_Init)
                return;
            TaskHelper.AddTask(() =>
            {
                while (DecThreadRun)
                {
                    Thread.Sleep(100);
                    //更新决策
                    ECSLayerLocate.Decision.Execute();
                }
            }, () =>
            {
                ECSLocate.Log.LogWarning("决策更新结束");
            });
        }

        #endregion

        #region Get

        public RequestSort GetRequestSort(RequestId requestId)
        {
            for (int i = 0; i < requestSortAsset.requests.Count; i++)
            {
                RequestSort sort = requestSortAsset.requests[i];
                if (sort.key.Equals(requestId.ToString()))
                {
                    return sort;
                }
            }
            ECSLocate.Log.LogError("请求没有配置排序", requestId);
            return null;
        }

        #endregion
    }
}
