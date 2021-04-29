using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Core.Tree.Nodes.Control;
using LCECS.Data;
using LCECS.Help;
using LCHelp;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XPToolchains.Json;

namespace LCECS.Tree
{
    /// <summary>
    /// 节点编辑器
    /// 1，通过数据来显示对应的节点
    /// 2，绘制节点
    /// 3，保存数据
    /// 4，派发操作事件
    /// </summary>
    public class LogicLayerEditorWindow : EditorWindow
    {
        enum ShowType
        {
            Dec,
            Bev,
            ReqWeight,
        }
        
        private const string DecPath = "/Resources/Config/Temp/DecTree.txt";
        private const string BevPath = "/Resources/Config/Temp/BevTree.txt";
        private const string RequestWeightPath = "/" + ECSDefinitionPath.LogicReqWeightPath;
        
        private static DecTrees MDecTrees;
        private static BevTrees MBevTrees;
        private static ReqWeightJson MReqWeightJson = new ReqWeightJson();
        
        private static float TimeRefreshRunningNode = 0.1f;
        private static float Timer;

        [MenuItem("LCECS/逻辑层配置")]
        private static void OpenWindow()
        {
            if (EDTool.CheckFileInPath(Application.dataPath + DecPath))
            {
                string dataJson = EDTool.ReadText(Application.dataPath + DecPath);
                MDecTrees =JsonMapper.ToObject<DecTrees>(dataJson);
            }
            else
            {
                MDecTrees = new DecTrees();
            }

            if (EDTool.CheckFileInPath(Application.dataPath + BevPath))
            {
                string dataJson = EDTool.ReadText(Application.dataPath + BevPath);
                MBevTrees = JsonMapper.ToObject<BevTrees>(dataJson);
            }
            else
            {
                MBevTrees = new BevTrees();
            }
            
            if (EDTool.CheckFileInPath(Application.dataPath + RequestWeightPath))
            {
                string dataJson = EDTool.ReadText(Application.dataPath + RequestWeightPath);
                MReqWeightJson = JsonMapper.ToObject<ReqWeightJson>(dataJson);
            }
            else
            {
                MReqWeightJson = new ReqWeightJson();
            }
            
            LogicLayerEditorWindow window = GetWindow<LogicLayerEditorWindow>();
            window.titleContent = new GUIContent("逻辑层配置");

            if (EditorApplication.isPlaying == true)
            {
                Timer = Time.realtimeSinceStartup;
            }
        }
        
        //背景格偏移
        private Vector2 offset;
        //背景格范围
        private Rect bgGridRect;
        //鼠标拖拽
        private Vector2 drag;
        //左侧滚动条
        private Vector2 leftPos = Vector2.zero;
        //当前鼠标位置
        private Vector2 curMousePos;

        private NodeDataJson selTree;
        private List<NodeEditor> showNodeEditor = new List<NodeEditor>();
        private NodeEditor selNode = null;
        private ShowType CurShowType = ShowType.Dec;

        //连接逻辑
        public static NodeEditor curConnectNode = null;

        private void Update()
        {
            if (Application.isPlaying)
            {
                float dlTime = Time.realtimeSinceStartup - Timer;
                if (dlTime >= TimeRefreshRunningNode)
                {
                    NodeRunSelEntityHelp.RefreshRunningNode();
                    Timer = Time.realtimeSinceStartup + dlTime;
                }
            }
        }

        private void OnDestroy()
        {
            SaveJson();
            MDecTrees = null;
            MBevTrees = null;
            curConnectNode = null;
        }

        private void OnGUI()
        {
            Refresh();
            
            ProcessNodeEvents(Event.current);
            
            ProcessEvents(Event.current);
            
            curMousePos = Event.current.mousePosition;
            bgGridRect = new Rect(new Vector2(350, 75), new Vector2(position.width-350, position.height-75));
            Repaint();
        }

        #region 按钮事件

        private void ProcessNodeEvents(Event e)
        {
            for (int i = 0; i < showNodeEditor.Count; i++)
            {
                bool childChange = showNodeEditor[i].ProcessEvents(e);
                if (childChange)
                {
                    GUI.changed = true;
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        ShowCreateNodeMenu();
                        Debug.LogError("创建节点>>>>>>>");
                    }
                    else if (e.button == 0)
                    {
                        if (curConnectNode != null)
                        {
                            curConnectNode = null;
                        }
                    }
                    break;
                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        if (bgGridRect.Contains(e.mousePosition))
                        {
                            OnDrag(e.delta);
                        }
                    }
                    break;
            }
        }

        private void NodeEventCallBack(int index, NodeEditor node)
        {
            //选择节点
            if (index == -1)
            {
                selNode = node;
            }
            //连接父节点
            if (index == 0)
            {
                if (curConnectNode == null)
                {
                    Debug.Log("连接中》》》》》");
                    curConnectNode = node;
                }
                else
                {
                    Debug.Log("连接》》》》》");
                    if (node.ConnectNode(curConnectNode))
                    {
                        curConnectNode = null;
                        GUI.changed = true;
                    }

                }

            }
            //删除节点
            if (index == 1)
            {
                //删除显示
                showNodeEditor.Remove(node);
                //删除数据
                if (node.ParEditor != null)
                {
                    node.ParEditor.Json.ChildNodes.Remove(node.Json);
                }
                selNode = null;
            }
            //删除连接
            if (index == 2)
            {
                //删除数据
                if (node.ParEditor == null)
                {
                    Debug.Log("没有连接》》》》");
                    return;
                }

                node.ParEditor.Json.ChildNodes.Remove(node.Json);
                node.ParEditor = null;
            }
        }

        private void OnDrag(Vector2 delta)
        {
            drag = delta;
            for (int i = 0; i < showNodeEditor.Count; i++)
            {
                showNodeEditor[i].Drag(delta);
            }
            GUI.changed = true;
        }

        #endregion

        private void Refresh()
        {
            //左侧列表
            EDLayout.CreateVertical("box", position.width, position.height, () =>
            {
                DrawTop(position.width, 30);
                DrawLeft(300, position.height - 30);
            });

            //右侧节点
            RefreshRightPanel();
           
        }

        #region 上方

        private void DrawTop(float width, float height)
        {
            //编辑界面切换
            EDLayout.CreateHorizontal("box", width, height, (() =>
            {
                EDColor.DrawColorArea(CurShowType == ShowType.Dec?Color.green:Color.white, () =>
                {
                    EDButton.CreateBtn("决策层", 100, height, (() => { CurShowType = ShowType.Dec; SaveJson(); showNodeEditor.Clear(); }));
                });
                EDColor.DrawColorArea(CurShowType == ShowType.Bev?Color.green:Color.white, () =>
                {
                    EDButton.CreateBtn("行为层", 100, height, (() => { CurShowType = ShowType.Bev; SaveJson(); showNodeEditor.Clear(); }));
                });
                EDColor.DrawColorArea(CurShowType == ShowType.ReqWeight?Color.green:Color.white, () =>
                {
                    EDButton.CreateBtn("请求权重", 100, height, (() => { CurShowType = ShowType.ReqWeight; SelTreeChange(null); SetAllReqWeight(); SaveJson(); showNodeEditor.Clear(); }));
                });
            }));
        }

        #endregion

        #region 左侧

        private void DrawLeft(float width, float height)
        {
            EDLayout.CreateHorizontal("",width,height, () =>
            {
                //列表
                if (CurShowType== ShowType.Dec)
                {
                    DrawDecTreeList(100, height);
                }
                else if (CurShowType== ShowType.Bev)
                {
                    DrawBevTreeList(100, height);
                }
                else if (CurShowType== ShowType.ReqWeight)
                {
                    DrawRequestWeight(400, position.height);
                }
            
                //节点编辑界面
                DrawOperateTree(200, height);
            });
        }
        
        //决策层
        private void DrawDecTreeList(float width, float height)
        {
            //列表
            EDLayout.CreateScrollView(ref leftPos, "box", width, position.height, () =>
            {
                EDButton.CreateBtn("新建决策",width,25, () =>
                {
                    EDPopPanel.PopWindow("输入新的决策名:", (string name) =>
                    {
                        NodeDataJson decTree = CreateNodeJson(NodeType.Root, typeof(NodeRoot).FullName, MDecTrees.CurDecId,"根节点");
                        decTree.DesName = name;
                        MDecTrees.Trees.Add(decTree);
                        MDecTrees.CurDecId++;
                    });
                });

                foreach (NodeDataJson tree in MDecTrees.Trees)
                {
                    EDColor.DrawColorArea(selTree==tree?Color.green : Color.white, () =>
                    {
                        EDButton.CreateBtn(string.Format("{0}({1})",tree.TreeId,tree.DesName), width, 25, () =>
                        {
                            SelTreeChange(tree);
                        });
                    });
                }
            });
        }

        //行为层
        private void DrawBevTreeList(float width, float height)
        {
            //列表
            EDLayout.CreateScrollView(ref leftPos, "box", width, position.height, () =>
            {
                EDButton.CreateBtn("新建行为", width, 25, () =>
                {
                    EDPopPanel.PopWindow("输入新的行为名:", (string name) =>
                    {
                        NodeDataJson bevTree = CreateNodeJson(NodeType.Root, typeof(NodeRoot).FullName, MBevTrees.CurBevId,"根节点");
                        bevTree.DesName = name;
                        MBevTrees.Trees.Add(bevTree);
                        MBevTrees.CurBevId++;
                    });
                    
                });
                
                foreach (NodeDataJson tree in MBevTrees.Trees)
                {
                    EDColor.DrawColorArea(selTree==tree?Color.green : Color.white, () =>
                    {
                        EDButton.CreateBtn(string.Format("{0}({1})",tree.TreeId,tree.DesName), width, 25, () =>
                        {
                            SelTreeChange(tree);
                        });
                    });
                }
            });
        }

        //树操作
        private bool ShowPremise = false;
        private bool ShowData    = false;
        private void DrawOperateTree(float width, float height)
        {
            if (selTree==null)
            {
                return;
            }
            EDLayout.CreateVertical("box",width,height, () =>
            {
                EDButton.CreateBtn("删除此节点树",180,25, () =>
                {
                    if (CurShowType== ShowType.Dec)
                    {
                        for (int i = 0; i < MDecTrees.Trees.Count; i++)
                        {
                            if (MDecTrees.Trees[i].TreeId==selTree.TreeId)
                            {
                                MDecTrees.Trees.RemoveAt(i);
                            }
                        }
                        
                        SelTreeChange(null);
                    }
                    else if (CurShowType== ShowType.Bev)
                    {
                        for (int i = 0; i < MBevTrees.Trees.Count; i++)
                        {
                            if (MBevTrees.Trees[i].TreeId==selTree.TreeId)
                            {
                                MBevTrees.Trees.RemoveAt(i);
                            }
                        }
                        
                        SelTreeChange(null);
                    }
                });

                if (selNode==null || selNode.Json==null)
                {
                    return;
                }
                EditorGUILayout.LabelField("当前选择的节点："+selNode.Json.Name,GUILayout.Width(width),GUILayout.Height(25));
                //前提
                if (selNode.Json.Premise!=null)
                {
                    ShowPremise = EditorGUILayout.Foldout(ShowPremise,"节点前提");
                    if (ShowPremise)
                    {
                        DrawNodePremise(width, selNode.Json.Premise);
                    }
                }
                //数据
                ShowData = EditorGUILayout.Foldout(ShowData,"节点数据");
                if (ShowData)
                {
                    DrawNodeDataEditor(width);
                }
            });
        }

        //请求权重
        private void SetAllReqWeight()
        {
            ReqWeightJson weightJson=new ReqWeightJson();
            for (int i = 0; i < MBevTrees.Trees.Count; i++)
            {
                WeightJson weight = new WeightJson();
                weight.Key=MBevTrees.Trees[i].TreeId;
                weight.Weight = GetReqWeightById(weight.Key);
                weightJson.ReqWeights.Add(weight);
            }

            MReqWeightJson = weightJson;
        }
        
        private Vector2 weightPos= Vector2.zero;
        private void DrawRequestWeight(float width, float height)
        {
            EDLayout.CreateScrollView(ref weightPos,"box",width,height, () =>
            {
                for (int i = 0; i < MReqWeightJson.ReqWeights.Count; i++)
                {
                    EDLayout.CreateVertical("GroupBox",width,75, () =>
                    {
                        WeightJson weightJson = MReqWeightJson.ReqWeights[i];
                        EditorGUILayout.LabelField("请求行为："+GetBevNameById(weightJson.Key),GUILayout.Width(width),GUILayout.Height(25));
                        weightJson.Weight=EditorGUILayout.IntField(weightJson.Weight,GUILayout.Width(width),GUILayout.Height(25));
                        
                        EDLayout.CreateHorizontal("",width,25, () =>
                        {
                            EDButton.CreateBtn("强制覆盖请求权重", width/2, 25, (() => { weightJson.Weight = ECSDefinition.REForceSwithWeight; }));
                            EDButton.CreateBtn("需要自身判断置换请求权重", width/2, 25, (() => { weightJson.Weight = ECSDefinition.RESwithRuleSelf; }));
                        });
                        
                    });
                    
                }
            });
        }
        
        
        #endregion

        #region Node

        //画节点
        private void RefreshRightPanel()
        {
            if (selTree == null)
            {
                return;
            }

            EDLayout.DrawGrid(20, 0.2f, Color.gray, bgGridRect, ref offset, drag);
            BeginWindows();
            for (int i = 0; i < showNodeEditor.Count; i++)
            {
                showNodeEditor[i].Draw();
            }
            EndWindows();

            DrawConnectLine();
            //RefreshNodeLines();
        }

        //节点线
        private void RefreshNodeLines()
        {
            if (selTree == null)
            {
                return;
            }
            DrawNodeLine(selTree);
        }

        private void DrawNodeLine(NodeDataJson node)
        {
            NodeEditor parNode = GetNodeEditorById(node.GetHashCode());
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                NodeEditor childNode = GetNodeEditorById(node.ChildNodes[i].GetHashCode());
                EDLine.CreateBezierLine(parNode.mRect.position, childNode.mRect.position, 10);

                DrawNodeLine(node.ChildNodes[i]);
            }
        }

        #endregion

        #region 节点前提
        
        private void DrawNodePremise(float width,NodePremiseJson premiseJson)
        {
            if (premiseJson == null)
                return;
            
            EDLayout.CreateVertical("GroupBox",width,50, () =>
            {
                string trueValue = premiseJson.TrueValue ? "是" : "否";
                EDButton.CreateBtn(string.Format("（{0}） {1}",trueValue,premiseJson.Name), width, 25, () =>
                {
                    EDPopMenu.CreatePopMenu(new List<string> { "删除前提", "改变真值","改变关系/并且","改变关系/或者","改变关系/异或" }, (int sel) =>
                    {
                        if (sel == 0)
                        {
                            RemovePremise(premiseJson);
                        }
                        if (sel == 1)
                        {
                            premiseJson.TrueValue = !premiseJson.TrueValue;
                        }
                        if (sel == 2)
                        {
                            premiseJson.Type = PremiseType.AND;
                        }
                        if (sel == 3)
                        {
                            premiseJson.Type = PremiseType.OR;
                        }
                        if (sel == 4)
                        {
                            premiseJson.Type = PremiseType.XOR;
                        }
                    });
                });
            
                //和其他前提的关系
                EditorGUILayout.LabelField("关系:"+premiseJson.Type,GUILayout.Width(width),GUILayout.Height(25));
            });
            
            EditorGUILayout.Space();
            
            if (premiseJson.OtherPremise != null)
            {
                DrawNodePremise(width,premiseJson.OtherPremise);
            }
        }

        private void RemovePremise(NodePremiseJson removeJson)
        {
            if (selNode == null || selNode.Json.Premise == null)
                return;
            NodePremiseJson parent = GetParentPremise(removeJson.Name, selNode.Json.Premise);
            if (parent == null)
            {
                selNode.Json.Premise = null;
            }
            else
            {
                parent.OtherPremise = null;
            }
        }

        private NodePremiseJson GetParentPremise(string typeName, NodePremiseJson json)
        {
            if (json == null)
                return null;
            if (json.OtherPremise == null)
                return null;
            if (json.OtherPremise.TypeFullName == typeName)
                return json;
            return GetParentPremise(typeName, json.OtherPremise);
        }

        private NodePremiseJson GetPremise(string typeName, NodePremiseJson json)
        {
            if (json == null)
                return null;
            if (json.TypeFullName == name)
                return json;
            if (json.OtherPremise == null)
                return null;
            return GetPremise(typeName, json.OtherPremise);
        }

        #endregion

        #region 节点编辑数据
        private void DrawNodeDataEditor(float width)
        {
            //可编辑键值
            for (int j = 0; j < selNode.Json.KeyValues.Count; j++)
            {
                NodeKeyValue keyValue = selNode.Json.KeyValues[j];

                object value = LCConvert.StrChangeToObject(keyValue.Value, keyValue.TypeFullName);
                Type ty = LCReflect.GetType(keyValue.TypeFullName);
                EDTypeField.CreateTypeField(keyValue.KeyName + "= ", ref value, ty, width - 10, 20);
                keyValue.Value = LCExtension.ToString(value, ty.FullName);
            }
        }

        #endregion

        #region 渲染连接线

        private void DrawConnectLine()
        {
            if (curConnectNode == null)
            {
                return;
            }

            Vector2 mousePosition = Event.current.mousePosition;
            EDLine.CreateBezierLine(curConnectNode.mRect.center, mousePosition, 2.5f, Color.gray);
            GUI.changed = true;
        }


        #endregion

        #region 创建节点菜单

        private void ShowCreateNodeMenu()
        {
            List<string> showStrs = new List<string>();

            //控制节点
            List<Type> controlTypes = LCReflect.GetClassByType<NodeControl>();
            for (int i = 0; i < controlTypes.Count; i++)
            {
                NodeAttribute nodeAttribute = LCReflect.GetTypeAttr<NodeAttribute>(controlTypes[i]);
                if (nodeAttribute != null)
                {
                    showStrs.Add("控制节点/" + nodeAttribute.ViewName);
                }
                else
                {
                    showStrs.Add("控制节点/" + controlTypes[i].FullName);
                }

            }

            //行为节点
            List<Type> actionTypes = LCReflect.GetClassByType<NodeAction>();
            List<Type> actionShowTypes = new List<Type>();
            for (int i = 0; i < actionTypes.Count; i++)
            {
                NodeAttribute nodeAttribute = LCReflect.GetTypeAttr<NodeAttribute>(actionTypes[i]);
                if (nodeAttribute == null)
                {
                    showStrs.Add("基础行为节点/" + nodeAttribute.ViewName);
                    actionShowTypes.Add(actionTypes[i]);
                }
                else
                {
                    if (nodeAttribute.IsCommonAction)
                    {
                        showStrs.Add("基础行为/" + nodeAttribute.ViewName);
                        actionShowTypes.Add(actionTypes[i]);
                    }
                    else
                    {
                        if (CurShowType == ShowType.Dec)
                        {
                            if (nodeAttribute.IsBevNode == false)
                            {
                                showStrs.Add("扩展决策/" + nodeAttribute.ViewName);
                                actionShowTypes.Add(actionTypes[i]);
                            }
                        }
                        else if (CurShowType == ShowType.Bev)
                        {
                            if (nodeAttribute.IsBevNode)
                            {
                                showStrs.Add("扩展行为/" + nodeAttribute.ViewName);
                                actionShowTypes.Add(actionTypes[i]);
                            }
                        }
                    }
                }
            }

            EDPopMenu.CreatePopMenu(showStrs, (int index) =>
            {
                Debug.Log("创建节点》》》》》》》》》" + index + "  " + controlTypes.Count + "   >>" + actionShowTypes.Count);
                //创建控制
                if (index <= controlTypes.Count - 1)
                {
                    NodeDataJson node = CreateNodeJson(NodeType.Control, controlTypes[index].FullName, selTree.TreeId,showStrs[index],curMousePos);
                    //创建显示
                    NodeEditor nodeEditor = new NodeEditor(new Rect(new Vector2((float)node.PosX, (float)node.PosY), new Vector2(200, 100)), node, null, NodeEventCallBack);
                    showNodeEditor.Add(nodeEditor);
                    GUI.changed = true;
                }
                //创建行为
                else
                {
                    Debug.Log("创建行为节点》》》》》》》》》" + (index - controlTypes.Count));
                    NodeDataJson node = CreateNodeJson(NodeType.Action, actionShowTypes[index - controlTypes.Count].FullName, selTree.TreeId, showStrs[index], curMousePos);
                    //创建显示
                    NodeEditor nodeEditor = new NodeEditor(new Rect(new Vector2((float)node.PosX, (float)node.PosY), new Vector2(200, 100)), node, null, NodeEventCallBack);
                    showNodeEditor.Add(nodeEditor);
                    GUI.changed = true;
                }
            });
        }

        #endregion

        #region Data

        private void SelTreeChange(NodeDataJson tree)
        {
            selTree = tree;
            showNodeEditor.Clear();
            
            if (selTree==null)
            {
                selNode = null;
                return;
            }
            
            //创建显示
            NodeEditor nodeEditor = new NodeEditor(new Rect(new Vector2((float)tree.PosX, (float)tree.PosY), new Vector2(200, 100)), tree, null, NodeEventCallBack);
            showNodeEditor.Add(nodeEditor);
            CreateNodeEditor(nodeEditor);
        }

        private void CreateNodeEditor(NodeEditor node)
        {
            for (int i = 0; i < node.Json.ChildNodes.Count; i++)
            {
                Type nodeType = LCReflect.GetType(node.Json.ChildNodes[i].TypeFullName);
                if (nodeType==null)
                {
                    node.Json.ChildNodes.RemoveAt(i);
                    continue;
                }
                NodeDataJson childNode = node.Json.ChildNodes[i];
                NodeEditor tmpEditor = new NodeEditor(new Rect(new Vector2((float)childNode.PosX, (float)childNode.PosY), new Vector2(200, 100)), childNode, node, NodeEventCallBack);
                showNodeEditor.Add(tmpEditor);
                CreateNodeEditor(tmpEditor);
            }
        }

        private NodeEditor GetNodeEditorById(int id)
        {
            for (int i = 0; i < showNodeEditor.Count; i++)
            {
                if (showNodeEditor[i].MId == id)
                {
                    return showNodeEditor[i];
                }
            }
            Debug.LogError("没有找到指定的节点>>>>" + id);
            return null;
        }

        private NodeDataJson CreateNodeJson(NodeType nodeType, string fullName, int treeId,  string name = "", Vector2 pos = default)
        {
            if (pos == default)
            {
                pos = new Vector2(300, 400);
            }

            int nodeId = 0;
            if (CurShowType== ShowType.Dec)
            {
                nodeId = MDecTrees.NodeId;
                MDecTrees.NodeId++;
            }
            else if (CurShowType== ShowType.Bev)
            {
                nodeId = MDecTrees.NodeId;
                MDecTrees.NodeId++;
            }
            NodeDataJson node = new NodeDataJson(nodeId, nodeType, fullName, treeId,pos.x, pos.y, name); ;
            return node;
        }
        
        private string GetBevNameById(int decId)
        {
            for (int i = 0; i < MBevTrees.Trees.Count; i++)
            {
                if (MBevTrees.Trees[i].TreeId==decId)
                {
                    return MBevTrees.Trees[i].DesName;
                }
            }
            
            return "";
        }
        
        private int GetReqWeightById(int decId)
        {
            for (int i = 0; i < MReqWeightJson.ReqWeights.Count; i++)
            {
                if (MReqWeightJson.ReqWeights[i].Key==decId)
                {
                    return MReqWeightJson.ReqWeights[i].Weight;
                }
            }
            
            return 1;
        }
        
        private void SaveJson()
        {
            SaveBevTreeJson();
            SaveDecTreeJson();
            SaveReqWeightJson();
        }

        private void SaveBevTreeJson()
        {
            string jsonData = JsonMapper.ToJson(MBevTrees);
            EDTool.WriteText(jsonData, Application.dataPath + BevPath);
            AssetDatabase.Refresh();
        }

        private void SaveDecTreeJson()
        {
            string jsonData = JsonMapper.ToJson(MDecTrees);
            EDTool.WriteText(jsonData, Application.dataPath + DecPath);
            AssetDatabase.Refresh();
        }
        
        private void SaveReqWeightJson()
        {
            string jsonData = JsonMapper.ToJson(MReqWeightJson);
            EDTool.WriteText(jsonData, Application.dataPath + RequestWeightPath);
            AssetDatabase.Refresh();
        }

        #endregion
        
    }
}
