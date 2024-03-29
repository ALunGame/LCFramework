﻿using LCECS;
using LCECS.Config;
using LCECS.Data;
using LCSkill;
using System;
using UnityEngine;
using LCMap;
using LCDialog;
using LCECS.Core;
using LCTask;
using LCUI;

namespace Demo
{
    /// <summary>
    /// 按键双击
    /// </summary>
    public class KeyDoubleClick
    {
        public KeyCode key;
        public float lastClickTime;
        public float doubleClickTime = 0.3f;
        private Action doubleClickCallBack;

        public KeyDoubleClick(KeyCode key)
        {
            this.key = key;
        }

        public bool CheckEvent()
        {
            if (Input.GetKeyUp(key))
            {
                if (Time.realtimeSinceStartup - lastClickTime < doubleClickTime)
                {
                    doubleClickCallBack?.Invoke();
                    lastClickTime = Time.realtimeSinceStartup;
                    return true;
                }
                lastClickTime = Time.realtimeSinceStartup;
            }
            return false;
        }

        public void SetDoubleClickCallBack(Action doubleClickCallBack, float doubleTime = 0.3f)
        {
            this.doubleClickCallBack = doubleClickCallBack;
            doubleClickTime = doubleTime;
        }
    }

    /// <summary>
    /// 负责游戏流程初始化
    /// </summary>
    public class GameCenter : MonoBehaviour
    {
        [Header("请求排序")]
        [SerializeField]
        private RequestSortAsset requestSortAsset;

        [Header("系统排序")]
        [SerializeField]
        private SystemSortAsset systemSortAsset;

        [Header("测试地图")]
        [SerializeField]
        private int testMapId;

        private DecisionCenter _DecCenter = new DecisionCenter();
        private ECSCenter _EcsCenter = new ECSCenter();
        private KeyDoubleClick LeftKey = new KeyDoubleClick(KeyCode.A);
        private KeyDoubleClick RightKey = new KeyDoubleClick(KeyCode.D);
        private ParamData paramData = new ParamData();

        private void Awake()
        {
            GameLocate.Init(this);
            DontDestroyOnLoad(this);
            Init();
        }

        private void Start()
        {
            MapLocate.Map.Enter(testMapId, () =>
            {
                _DecCenter.Start_ThreadUpdate();

                //GameLocate.WorkServer.AfterMapInit();
                GameLocate.TimerServer.Init();
                //string uid = DialogLocate.Dialog.CreateDialog(new AddDialogInfo(DialogType.Bubble, 1001, 1));
                //DialogLocate.Dialog.Play(uid);

                UILocate.UI.Show(UIPanelDef.MainUIPanel);
            });
        }

        private void Update()
        {
            //Execute_KeyEvent();
            _DecCenter.Execute_Update();
            _EcsCenter.Execute_Update();
        }

        private void FixedUpdate()
        {
            _EcsCenter.Execute_FixedUpdate();
        }

        private void OnDestroy()
        {
            Clear();
        }

        #region 初始化

        public void Init()
        {
            InitLocate();
            InitECS();
            InitKeyEvent();
        }

        /// <summary>
        /// 初始化定位器
        /// </summary>
        public void InitLocate()
        {
            TaskLocate.Init();
        }

        /// <summary>
        /// ECSC初始化
        /// </summary>
        public void InitECS()
        {
            _EcsCenter.Init(requestSortAsset, systemSortAsset);

            //创建世界实体
            Entity worldEntity = ECSLocate.ECS.CreateEntity("world_0", 0);
            if (worldEntity.GetCom(out BindGoCom bindGoCom))
                bindGoCom.SetBindGo(new GameObject("<------------EntityWorld---------->"));
            worldEntity.Enable();
            ECSLocate.ECS.SetWorld(worldEntity);

            _DecCenter.Init();
        }

        /// <summary>
        /// 初始化按钮事件
        /// </summary>
        private void InitKeyEvent()
        {
            LeftKey.SetDoubleClickCallBack(() => {
                Debug.LogError("双击LeftKey》》》》");
                paramData.SetString("100102");
                ECSLocate.Player.PushPlayerReq(RequestId.PushSkill, paramData);
            });
            RightKey.SetDoubleClickCallBack(() => {
                Debug.LogError("双击RightKey》》》》");
                paramData.SetString("100102");
                ECSLocate.Player.PushPlayerReq(RequestId.PushSkill, paramData);
            });
        }

        #endregion

        #region 清理

        public void Clear()
        {
            ECSLocate.Clear();
        }

        #endregion

        #region 按钮事件

        private RequestInputMove intputMoveParam = new RequestInputMove();
        private void Execute_KeyEvent()
        {
            if (LeftKey.CheckEvent() || RightKey.CheckEvent())
                return;

            if (Input.GetMouseButtonDown(0))
            {
                // paramData.SetString("100101");
                // GameLocate.PushInputAction(Com.InputAction.Skill, paramData);
            }
            else
            {
                Input.GetKeyDown(KeyCode.A);
                Vector2 move = Vector2.zero;
                move.x = Input.GetAxisRaw("Horizontal");
                if (Input.GetButtonDown("Jump"))
                {
                    move.y = 1;
                }

                intputMoveParam.inputMove = move;
                GameLocate.PushInputAction(Com.InputAction.Move, intputMoveParam);
            }



        }

        #endregion

    }
}
