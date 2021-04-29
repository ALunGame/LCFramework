using System;
using System.Collections.Generic;
using System.Reflection;
using LCECS.Core.ECS;
using LCECS.Data;
using LCECS.Help;
using LCECS.Scene;
using LCHelp;
using UnityEditor;
using UnityEngine;

namespace LCECS.Inspector
{
    /// <summary>
    /// 组件键值预览
    /// </summary>
    public class ComKeyValueView
    {
        public string KeyName;

        public FieldInfo Info;

        public BaseCom Com;

        public ComKeyValueView(string name, FieldInfo info, BaseCom com)
        {
            KeyName = name;
            Info = info;
            Com = com;
        }
    }

    /// <summary>
    /// 组件预览
    /// </summary>
    public class EntityComView
    {
        public string ComName;
        public BaseCom Com;
        public List<ComKeyValueView> ValueList = new List<ComKeyValueView>();
        public List<ComKeyValueView> EditorValueList = new List<ComKeyValueView>();

        public EntityComView(string comName, FieldInfo[] fieldInfos, BaseCom baseCom)
        {
            ComName = comName;
            Com = baseCom;
            ComAttribute comAttribute = LCReflect.GetTypeAttr<ComAttribute>(baseCom.GetType());
            if (comAttribute != null)
            {
                ComName = comAttribute.ViewName;
            }
            for (int i = 0; i < fieldInfos.Length; i++)
            {
                FieldInfo info = fieldInfos[i];
                ComKeyValueView keyValue = new ComKeyValueView(info.Name, info, baseCom);

                ComValueAttribute comValueAttribute = LCReflect.GetFieldAttr<ComValueAttribute>(info);
                if (comValueAttribute != null)
                {
                    if (comValueAttribute.ViewEditor)
                    {
                        EditorValueList.Add(keyValue);
                    }
                    else
                    {
                        if (comValueAttribute.ShowView)
                        {
                            ValueList.Add(keyValue);
                        }
                    }
                }

            }
        }
    }
    
    [CustomEditor(typeof(EntityEditorViewHelp))]
    public class EntityEditorViewHelpInspector : Editor
    {
        private int ItemWidth = 350;
        private EntityWorkData workData = null;
        
        private void OnEnable()
        {
            EntityEditorViewHelp help = target as EntityEditorViewHelp;
            EntityWorkData workData = LCECS.ECSLayerLocate.Info.GetEntityWorkData(help.EntityId);
            this.workData = workData;
            CollectAllComExDraw(workData);
            EditorApplication.update += Update;
        }

        private void OnDisable()
        {
            EditorApplication.update -= Update;
        }

        private void OnDestroy()
        {
            EditorApplication.update -= Update;
        }

        private void Update()
        {
            this.Repaint();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            Refresh();
        }

        private void Refresh()
        {
            ItemWidth=EditorGUILayout.IntField("Width：", ItemWidth);
            EDLayout.CreateVertical("box",ItemWidth,10,() =>
            {
                EntityEditorViewHelp help = target as EntityEditorViewHelp;
                EDButton.CreateBtn("监视实体决策执行", ItemWidth, 25, () =>
                {
                    NodeRunSelEntityHelp.SelEntityId = help.EntityId;
                    NodeRunSelEntityHelp.RefreshRunState = false;
                });
                
                if (workData == null)
                    return;

                DrawComList(workData, ItemWidth, 10);
            });
        }
        
        //渲染实体组件列表
        private static Vector2 ScrollPos = Vector2.zero;
        private static bool ShowComs = false;
        private static bool ShowSystems = false;
        private static Dictionary<string, bool> ComTogDict = new Dictionary<string, bool>();
        
        private void DrawComList(EntityWorkData workData, float width, float height)
        {
            ShowComs = EditorGUILayout.Foldout(ShowComs, "组件列表");
            if (ShowComs==false)
                return;
            List<EntityComView> comViews = HandleEntityComs(workData.MEntity);
            DrawEntityComs(comViews, width);
        }

        private static void DrawEntityComs(List<EntityComView> comViews, float width)
        {
            for (int i = 0; i < comViews.Count; i++)
            {
                EntityComView comView = comViews[i];
                if (!ComTogDict.ContainsKey(comView.ComName))
                {
                    ComTogDict.Add(comView.ComName, false);
                }

                EditorGUILayout.Space();

                ComTogDict[comView.ComName] = EditorGUILayout.Foldout(ComTogDict[comView.ComName], comView.ComName);
                if (ComTogDict[comView.ComName])
                {
                    GUI.color = Color.green;
                    //组件名
                    EditorGUILayout.LabelField("组件: ", comView.ComName, GUILayout.Width(width), GUILayout.Height(20));

                    //可编辑键值
                    for (int j = 0; j < comView.EditorValueList.Count; j++)
                    {
                        GUI.color = Color.blue;
                        ComKeyValueView info = comView.EditorValueList[j];
                        object valueObj = info.Info.GetValue(info.Com);
                        EDTypeField.CreateTypeField(info.KeyName + "= ", ref valueObj, info.Info.FieldType, width, 20);
                        LCReflect.SetTypeFieldValue(info.Com, info.KeyName, valueObj);
                    }

                    //只读值
                    for (int j = 0; j < comView.ValueList.Count; j++)
                    {
                        GUI.color = Color.white;
                        ComKeyValueView info = comView.ValueList[j];
                        object valueObj = info.Info.GetValue(info.Com);
                        EDTypeField.CreateLableField(info.KeyName + "=", valueObj.ToString(), width, 20);
                    }
                    GUI.color = Color.white;

                    //扩展渲染
                    DrawComExView(comView.Com, width, 200);
                }
            }
        }
        
        private static List<EntityComView> HandleEntityComs(Entity entity)
        {
            List<EntityComView> comViews = new List<EntityComView>();
            //组件名
            HashSet<string> entityComs = entity.GetAllComStr();
            foreach (string comName in entityComs)
            {
                BaseCom com = entity.GetCom(comName);
                FieldInfo[] fields = LCReflect.GetTypeFieldInfos(com.GetType());

                EntityComView comView = new EntityComView(comName, fields, com);

                comViews.Add(comView);
            }

            return comViews;
        }
        
        #region 扩展
        private static Dictionary<BaseCom, ComEditorView> ComEditorDrawDict = new Dictionary<BaseCom, ComEditorView>();

        //收集所有组件的扩展渲染类
        private static void CollectAllComExDraw(EntityWorkData workData)
        {
            ComEditorDrawDict.Clear();
            //反射获得所有的扩展渲染类
            Dictionary<Type, Type> comViewTypeDict = new Dictionary<Type, Type>();
            List<Type> viewTyps = LCReflect.GetClassByType<ComEditorView>();
            for (int i = 0; i < viewTyps.Count; i++)
            {
                ComEditorViewAttribute comAttribute = LCReflect.GetTypeAttr<ComEditorViewAttribute>(viewTyps[i]);
                if (comAttribute == null)
                {
                    Debug.LogError("组件渲染类型没有声明属性>>>>>ComEditorViewAttribute");
                }
                else
                {
                    if (comViewTypeDict.ContainsKey(comAttribute.ComType))
                    {
                        Debug.LogError("重复的组件渲染类型>>>>>" + comAttribute.ComType);
                    }
                    else
                    {
                        comViewTypeDict.Add(comAttribute.ComType, viewTyps[i]);
                    }
                }
            }

            //组件名
            HashSet<string> entityComs = workData.MEntity.GetAllComStr();
            foreach (string comName in entityComs)
            {
                BaseCom com = workData.MEntity.GetCom(comName);
                if (comViewTypeDict.ContainsKey(com.GetType()))
                {
                    Type viewType = comViewTypeDict[com.GetType()];
                    ComEditorView comEditorView = LCReflect.CreateInstanceByType<ComEditorView>(viewType.FullName);
                    comEditorView.TargetCom = com;
                    ComEditorDrawDict.Add(com, comEditorView);
                }
            }
        }

        private static void DrawComExView(BaseCom baseCom, float width, float height)
        {
            if (!ComEditorDrawDict.ContainsKey(baseCom))
            {
                return;
            }
            ComEditorDrawDict[baseCom].OnDrawScene(width, height);
        }

        #endregion
    }
}
