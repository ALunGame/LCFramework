#if UNITY_EDITOR
using LCECS.Core.ECS;
using LCECS.Data;
using LCECS.Help;
using LCHelp;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LCECS.Scene
{
    /// <summary>
    /// ECS 实体场景视图
    /// </summary>
    public class ECSEntityScene
    {
        [InitializeOnLoadMethod]  //unity初始化时调用
        private static void Init()
        {
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        public static EntityEditorViewHelp SelEntityView = null;
        private static Vector2 ScrollPos = Vector2.zero;

        private static bool ShowComs = false;
        private static bool ShowSystems = false;

        private static void OnSceneGUI(SceneView sceneView)
        {
            GameObject go = Selection.activeGameObject;
            if (go != null)
            {
                EntityEditorViewHelp entityView = go.GetComponent<EntityEditorViewHelp>();
                if (entityView != null)
                {
                    if (SelEntityView == null || SelEntityView.EntityId != entityView.EntityId)
                    {
                        OnInit(entityView);
                    }
                    SelEntityView = entityView;
                }
            }

            if (SelEntityView != null && SelEntityView.DrawEditor)
            {
                Draw(SelEntityView);
            }
        }

        private static void OnInit(EntityEditorViewHelp entityEditor)
        {
            EntityWorkData workData = LCECS.ECSLayerLocate.Info.GetEntityWorkData(entityEditor.EntityId);
            if (workData == null)
                return;
        }

        private static void Draw(EntityEditorViewHelp entityEditor)
        {
            if (Event.current.type == EventType.KeyDown || Event.current.type == EventType.KeyUp) return;
            EntityWorkData workData = LCECS.ECSLayerLocate.Info.GetEntityWorkData(entityEditor.EntityId);
            if (workData == null)
                return;
            //渲染
            Handles.BeginGUI();
            GUILayout.BeginArea(entityEditor.ShowRect);

            DrawTopInfo(workData, entityEditor.ShowRect.width, 25);
            DrawMiddle(workData, entityEditor.ShowRect.width, 200);

            GUILayout.EndArea();
            Handles.EndGUI();
        }

        #region Top
        private static string TopInfoStr = "  id:{0} name:{1} state:{2} comCnt:{3} systemCnt:{4}";
        //渲染上方实体基础信息
        private static void DrawTopInfo(EntityWorkData workData, float width, float height)
        {
            EditorGUILayout.Space();
            string showStr = string.Format(TopInfoStr, workData.Id, workData.MEntity.GetEntityConfId(), workData.MEntity.IsEnable, workData.MEntity.GetAllComStr().Count, workData.MEntity.Systems.Count);
            GUILayout.Label(showStr, GUILayout.Width(width), GUILayout.Height(height));
        }
        #endregion

        #region Middle
        //渲染中间操作按钮以及请求行为列表
        private static void DrawMiddle(EntityWorkData workData, float width, float height)
        {
            EDLayout.CreateHorizontal("GroupBox", width, height, () =>
            {
                //按钮
                EDLayout.CreateVertical("GroupBox", 50, height, () =>
                {
                    string activeBtnStr = "{0}";
                    activeBtnStr = string.Format(activeBtnStr, workData.MEntity.IsEnable ? "禁用" : "激活");
                    EDButton.CreateBtn(activeBtnStr, 50, 25, () =>
                    {
                        if (workData.MEntity.IsEnable)
                            workData.MEntity.Disable();
                        else
                            workData.MEntity.Enable();
                    });
                });

                DrawEntityReqList(workData.Id, 100, height);
                DrawEntityBevList(workData.Id, 200, height);
                DrawEntitySystem(workData.MEntity.Systems, 150,height);
            });
        }

        #region 实体请求列表
        private static int ReqEntityId = 0;
        private static Queue<string> ReqQue = new Queue<string>();

        //刷新实体请求列表
        public static void CheckNeedRefreshEntityReqList(int entityId, int reqId)
        {
            if (ReqEntityId <= 0)
            {
                return;
            }
            if (ReqEntityId == entityId)
            {
                string showStr = string.Format("{0}({1})", reqId.ToString(), ECSLayerLocate.Request.GetRequestWeight(reqId));
                ReqQue.Enqueue(showStr);
                if (ReqQue.Count >= 15)
                {
                    ReqQue.Dequeue();
                }
            }
        }

        //渲染实体请求列表
        private static void DrawEntityReqList(int entityId, float width, float height)
        {
            EDLayout.CreateVertical("", width, height, () =>
            {
                if (ReqEntityId == 0)
                {
                    ReqEntityId = entityId;
                }
                else
                {
                    if (ReqEntityId != entityId)
                    {
                        ReqQue.Clear();
                        ReqEntityId = entityId;
                    }
                }
                string showStr = "实体请求列表：\n";
                foreach (var item in ReqQue)
                {
                    showStr += item + "\n";
                }

                GUILayout.Label(showStr, GUILayout.Width(width), GUILayout.Height(height));
            });
        }
        #endregion

        #region 实体行为列表
        private static int BevEntityId = 0;
        private static Queue<string> BevQue = new Queue<string>();
        private static string BevStr = "1:{0},2:{1}";
        //刷新实体行为列表
        public static void CheckNeedRefreshEntityBevList(int entityId)
        {
            if (BevEntityId <= 0)
            {
                return;
            }
            if (BevEntityId == entityId)
            {
                EntityWorkData workData = ECSLayerLocate.Info.GetEntityWorkData(entityId);

                string curStr = string.Format("{0}({1})", (workData.CurrReqId).ToString(), ECSLayerLocate.Request.GetRequestWeight(workData.CurrReqId));

                string nextStr = string.Format("{0}({1})", (workData.NextReqId).ToString(), ECSLayerLocate.Request.GetRequestWeight(workData.NextReqId));

                string showStr = string.Format(BevStr, curStr, nextStr);
                //空请求
                if (BevQue.Count > 0 && BevQue.Peek() != "0" && workData.CurrReqId == 0)
                {

                    BevQue.Enqueue(showStr);
                }
                else
                {
                    if (workData.CurrReqId > 0)
                    {

                        BevQue.Enqueue(showStr);
                    }
                }

                if (BevQue.Count >= 15)
                {
                    BevQue.Dequeue();
                }
            }
        }

        //渲染实体行为列表
        private static void DrawEntityBevList(int entityId, float width, float height)
        {
            EDLayout.CreateVertical("", width, height, () =>
            {
                if (BevEntityId == 0)
                {
                    BevEntityId = entityId;
                }
                else
                {
                    if (BevEntityId != entityId)
                    {
                        BevQue.Clear();
                        BevEntityId = entityId;
                    }
                }
                string showStr = "实体行为列表：\n";
                foreach (var item in BevQue)
                {
                    showStr += item + "\n";
                }

                GUILayout.Label(showStr, "GroupBox", GUILayout.Width(width), GUILayout.Height(height));
            });
        }
        #endregion
        
        #region 观察系统列表

        private static Vector2 systemListPos = Vector2.zero;
        private static void DrawEntitySystem(List<string> systems, float width,float height)
        {
            EDLayout.CreateScrollView(ref systemListPos, "", width, height, () =>
            {
                EditorGUILayout.LabelField("被观察的系统列表:", GUILayout.Width(width), GUILayout.Height(20));

                for (int i = 0; i < systems.Count; i++)
                {
                    string systemName = systems[i];
                    //组件名
                    EditorGUILayout.LabelField(systemName, GUILayout.Width(width), GUILayout.Height(20));
                }
            });
        }

        #endregion
        
        #endregion
    }
}

#endif