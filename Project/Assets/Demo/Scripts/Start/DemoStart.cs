using System;
using System.Collections.Generic;
using IAECS;
using IAECS.Config;
using IAECS.Core;
using IAECS.Data;
using IAFramework;
using IAFramework.Server;
using IAToolkit;
using LCMap;
using LCTask;
using LCUI;
using UnityEngine;
using YooAsset;
using DecisionCenter = IAECS.DecisionCenter;
using ECSLocate = IAECS.ECSLocate;

namespace Demo
{
    public class DemoStart : MonoBehaviour
    {
        [Header("游戏模式")]
        public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;
        
        [ReadOnly]
        [SerializeField]
        private bool StartSuccess;
        private GameStartServer gameStartServer = new GameStartServer();

        private void Awake()
        {
            DontDestroyOnLoad(this);
            
            InitGameStart();
        }

        private void Start()
        {
            GameStart();
        }

        private void Update()
        {
            if (!StartSuccess)
                return;
        }

        private void FixedUpdate()
        {
            if (!StartSuccess)
                return;
        }

        private void OnApplicationQuit()
        {
            StartSuccess = false;
            
            ClearGameContext();
            ClearGameStart();

            ClearLogic();
        }

        #region GameStart

        /// <summary>
        /// 初始化一些第三方插件
        /// </summary>
        private void InitGameStart()
        {
            //初始化资源系统
            YooAssets.Initialize();
            YooAssets.SetOperationSystemMaxTimeSlice(30);
            
            //游戏开始回调
            gameStartServer.OnStartSuccess += OnStartSuccess;
            gameStartServer.OnStartFail += OnStartFail;
            gameStartServer.Init();
        }
        
        /// <summary>
        /// 游戏开始
        /// </summary>
        private void GameStart()
        {
            //初始化资源系统
            YooAssets.Initialize();
            YooAssets.SetOperationSystemMaxTimeSlice(30);
            
            //开始游戏流程
            gameStartServer.Start(PlayMode);
        }
        
        private void ClearGameStart()
        {
            gameStartServer.Clear();
            YooAssets.Destroy();
        }
        
        
        #endregion
        
        #region 游戏启动回调

        private void OnStartSuccess()
        {
            Debug.Log("游戏开始------------>");
            InitGameContext();
            
            StartSuccess = true;
            InitLogic();
        }
        
        private void OnStartFail(string pReason)
        {
            
        }

        #endregion
        
        #region GameContext

        private void InitGameContext()
        {
            GameContext.Init();
        }
        
        private void ClearGameContext()
        {
            GameContext.Clear();
        }

        #endregion

        #region 逻辑初始化
        
        [Header("请求排序")]
        [SerializeField]
        private RequestSortAsset requestSortAsset;

        [Header("系统排序")]
        [SerializeField]
        private SystemSortAsset systemSortAsset;
        
        private DecisionCenter _DecCenter = new DecisionCenter();
        private ECSCenter _EcsCenter = new ECSCenter();
        private KeyDoubleClick LeftKey = new KeyDoubleClick(KeyCode.A);
        private KeyDoubleClick RightKey = new KeyDoubleClick(KeyCode.D);
        private ParamData paramData = new ParamData();

        private void InitLogic()
        {
            TaskLocate.Init();

            InitECS();
            
            MapLocate.Map.Enter(0, () =>
            {
                _DecCenter.Start_ThreadUpdate();

                //GameLocate.WorkServer.AfterMapInit();
                GameLocate.TimerServer.Init();
                //string uid = DialogLocate.Dialog.CreateDialog(new AddDialogInfo(DialogType.Bubble, 1001, 1));
                //DialogLocate.Dialog.Play(uid);

                UILocate.UI.Show(UIPanelDef.MainUIPanel);
            });
        }

        /// <summary>
        /// ECSC初始化
        /// </summary>
        public void InitECS()
        {
            _EcsCenter.Init(requestSortAsset, systemSortAsset);

            //创建世界实体
            Entity worldEntity = ECSLocate.ECS.CreateEntity("world_0", new List<BaseCom>());
            if (worldEntity.GetCom(out BindGoCom bindGoCom))
                bindGoCom.SetBindGo(new GameObject("<------------EntityWorld---------->"));
            worldEntity.Enable();
            ECSLocate.ECS.SetWorld(worldEntity);

            _DecCenter.Init();
        }

        private void ClearLogic()
        {
            ECSLocate.Clear();
        }
        
        #endregion
    }
}