using LCECS.Model;
using LCHelp;
using LCNode.Model;
using LCNode.View;
using LCToolkit;
using LCToolkit.Extension;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using LCJson;

using Object = UnityEngine.Object;
using System.IO;

namespace LCECS.Tree
{
    [CustomEditor(typeof(DecisionList))]
    public class DecisionListInspector : Editor
    {
        private int ItemWidth = 350;
        private Object[] trees;

        private void OnEnable()
        {
            DecisionList list = (DecisionList)target;
            trees = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(list));
        }

        public override void OnInspectorGUI()
        {
            EDLayout.CreateVertical("box", ItemWidth, 10, () =>
            {
                for (int i = 0; i < trees.Length; i++)
                {
                    if (trees[i] is DecisionAsset)
                    {
                        DecisionAsset decision = trees[i] as DecisionAsset;
                        DrawDecision(decision);
                    }
                }
            });

            GUIHelper.Btn("添加", ItemWidth, 50, () =>
            {
                EDPopPanel.PopWindow("输入决策树Id", (string Id) =>
                {
                    DecisionAsset decisionAsset = CreateInstance<DecisionAsset>();
                    decisionAsset.TreeId = int.Parse(Id);
                    decisionAsset.name = Id;
                    AssetDatabase.AddObjectToAsset(decisionAsset, (DecisionList)target);
                    EditorUtility.SetDirty((DecisionList)target);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                });
            });

            GUIHelper.Btn("导出配置", ItemWidth, 50, () =>
            {
                Export();
            });
        }

        private void DrawDecision(DecisionAsset decisionAsset)
        {
            GUILayoutExtension.HorizontalGroup(() =>
            {
                EditorGUILayout.LabelField(decisionAsset.TreeId.ToString(), GUILayout.Width(150), GUILayout.Width(50));
                GUIHelper.Btn("打开", 100, 50, () =>
                {
                    BaseGraphWindow.Open(decisionAsset);
                });

                GUIHelper.Btn("删除", 100, 50, () =>
                {
                    DestroyImmediate(decisionAsset, true);
                    EditorUtility.SetDirty((DecisionList)target);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                });

                GUIHelper.Btn("重命名", 100, 50, () =>
                {
                    EDPopPanel.PopWindow("输入决策树Id", (string Id) =>
                    {
                        decisionAsset.name = Id;
                        decisionAsset.TreeId = int.Parse(Id);
                        EditorUtility.SetDirty(decisionAsset);
                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    });
                });
            }, GUILayout.Width(ItemWidth), GUILayout.Height(50));
        }

        private void Export()
        {
            DecisionList list = (DecisionList)target;
            Object[] trees = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(list));

            if (Directory.Exists(ECSDefPath.DecTreeRootPath))
            {
                Directory.Delete(ECSDefPath.DecTreeRootPath);
            }
            Directory.CreateDirectory(ECSDefPath.DecTreeRootPath);

            List<int> treeIds = new List<int>();
            for (int i = 0; i < trees.Length; i++)
            {
                if (trees[i] is DecisionAsset)
                {
                    DecisionAsset decision = trees[i] as DecisionAsset;
                    if (treeIds.Contains(decision.TreeId))
                    {
                        Debug.LogError($"决策树Id重复>>>>{decision.TreeId}");
                        return;
                    }
                    BaseGraph graph = decision.DeserializeGraph();

                    DecisionModel model = new DecisionModel();
                    model.id = decision.TreeId;
                    model.tree = SerializeHelp.SerializeToTree(graph);
                    string filePath = ECSDefPath.GetDecTreePath(decision.name);
                    LCIO.WriteText(JsonMapper.ToJson(model), filePath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    Debug.Log($"决策树生成成功>>>>{filePath}");
                }
            }
        }
    }
}
