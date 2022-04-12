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
    [CustomEditor(typeof(BehaviorList))]
    public class BehaviorListInspector : Editor
    {
        private int ItemWidth = 350;
        private Object[] trees;

        private void OnEnable()
        {
            BehaviorList list = (BehaviorList)target;
            trees = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(list));
        }

        public override void OnInspectorGUI()
        {
            EDLayout.CreateVertical("box", ItemWidth, 10, () =>
            {
                for (int i = 0; i < trees.Length; i++)
                {
                    if (trees[i] is BehaviorAsset)
                    {
                        BehaviorAsset behavior = trees[i] as BehaviorAsset;
                        DrawBehavior(behavior);
                    }
                }
            });

            GUIHelper.Btn("添加", ItemWidth, 50, () =>
            {
                EDPopPanel.PopWindow("输入行为树Id", (string Id) =>
                {
                    BehaviorAsset behaviorAsset = CreateInstance<BehaviorAsset>();
                    behaviorAsset.TreeId = int.Parse(Id);
                    behaviorAsset.name = Id;
                    AssetDatabase.AddObjectToAsset(behaviorAsset, (BehaviorList)target);
                    EditorUtility.SetDirty((BehaviorList)target);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                });
            });

            GUIHelper.Btn("导出配置", ItemWidth, 50, () =>
            {
                Export();
            });
        }

        private void DrawBehavior(BehaviorAsset behaviorAsset)
        {
            GUILayoutExtension.HorizontalGroup(() =>
            {
                EditorGUILayout.LabelField(behaviorAsset.TreeId.ToString(), GUILayout.Width(150), GUILayout.Width(50));
                GUIHelper.Btn("打开", 100, 50, () =>
                {
                    BaseGraphWindow.Open(behaviorAsset);
                });

                GUIHelper.Btn("删除", 100, 50, () =>
                {
                    DestroyImmediate(behaviorAsset, true);
                    EditorUtility.SetDirty((BehaviorList)target);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                });

                GUIHelper.Btn("重命名", 100, 50, () =>
                {
                    EDPopPanel.PopWindow("输入行为树Id", (string Id) =>
                    {
                        behaviorAsset.name = Id;
                        behaviorAsset.TreeId = int.Parse(Id);
                        EditorUtility.SetDirty(behaviorAsset);
                        EditorUtility.SetDirty(target);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    });
                });
            }, GUILayout.Width(ItemWidth), GUILayout.Height(50));
        }

        private void Export()
        {
            BehaviorList list = (BehaviorList)target;
            Object[] trees = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(list));

            if (Directory.Exists(ECSDefPath.BevTreeRootPath))
            {
                Directory.Delete(ECSDefPath.BevTreeRootPath);
            }
            Directory.CreateDirectory(ECSDefPath.BevTreeRootPath);

            List<int> treeIds = new List<int>();
            for (int i = 0; i < trees.Length; i++)
            {
                if (trees[i] is BehaviorAsset)
                {
                    BehaviorAsset behavior = trees[i] as BehaviorAsset;
                    if (treeIds.Contains(behavior.TreeId))
                    {
                        Debug.LogError($"行为树Id重复>>>>{behavior.TreeId}");
                        return;
                    }
                    BaseGraph graph = behavior.DeserializeGraph();

                    BehaviorModel model = new BehaviorModel();
                    model.id = behavior.TreeId;
                    model.tree = SerializeHelp.SerializeToTree(graph);
                    string filePath = ECSDefPath.GetBevTreePath(behavior.name);
                    LCIO.WriteText(JsonMapper.ToJson(model), filePath);
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                    Debug.Log($"行为树生成成功>>>>{filePath}");
                }
            }
        }
    }
}
