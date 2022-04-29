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
    public class ECSCenter : MonoBehaviour
    {
        [SerializeField]
        private RequestSortAsset requestSortAsset;

        [SerializeField]
        private SystemSortAsset systemSortAsset;

        private bool DecThreadRun = false;

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            //更新树时间
            NodeTime.UpdateTime(Time.deltaTime, 1);

            //执行行为树
            ECSLayerLocate.Behavior.Execute();

            //系统处理
            ECSLocate.ECS.ExcuteUpdateSystem();
        }

        private void FixedUpdate()
        {
            //系统处理
            ECSLocate.ECS.ExcuteFixedUpdateSystem();
        }

        private void OnDestroy()
        {
            DecThreadRun = false;
            ECSLocate.Clear();
        }

        //线程执行决策层（只是读取数据操作）
        private void ThreadExcuteDec()
        {
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

        #region 初始化

        private void Init()
        {
            DecThreadRun = true;

            InitServer();
            InitConf();
            RegSystems();
            ThreadExcuteDec();
        }

        //设置服务
        private void InitServer()
        {
            ECSLocate.InitServer(this);
            ECSLayerLocate.InitLayerServer();
        }

        //初始化配置
        private void InitConf()
        {
        }

        //注册系统
        private void RegSystems()
        {
            foreach (var item in systemSortAsset.GetSystemSorts(SystemType.Update))
            {
                Type systemType = ReflectionHelper.GetType(item.typeFullName);
                BaseSystem system = ReflectionHelper.CreateInstance(systemType) as BaseSystem;
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
