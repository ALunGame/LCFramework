using LCECS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using XPToolchains.Help;
using XPToolchains.Json;

namespace LCECS.EDWindow
{
    public abstract class RequsetWeightEditorWindow : EditorWindow
    {
        private Vector2 listPos = Vector2.zero;
        private ReqWeightJson MReqWeightJson = new ReqWeightJson();
        private string RequestWeightPath = "/" + ECSDefinitionPath.LogicReqWeightPath;

        private void OnEnable()
        {
            if (EDTool.CheckFileInPath(Application.dataPath + RequestWeightPath))
            {
                string dataJson = EDTool.ReadText(Application.dataPath + RequestWeightPath);
                MReqWeightJson = JsonMapper.ToObject<ReqWeightJson>(dataJson);
            }
            else
            {
                MReqWeightJson = new ReqWeightJson();
            }
            LoadReqWeightJson();
        }

        private void OnGUI()
        {
            DrawRequestWeight(position.width, position.height);
        }

        private void OnDestroy()
        {
            string jsonData = JsonMapper.ToJson(MReqWeightJson);
            EDTool.WriteText(jsonData, Application.dataPath + RequestWeightPath);
            AssetDatabase.Refresh();
        }

        private void LoadReqWeightJson()
        {
            List<int> requests = GetAllRequests();
            for (int i = 0; i < MReqWeightJson.ReqWeights.Count; i++)
            {
                WeightJson weight = MReqWeightJson.ReqWeights[i];
                bool legal = requests.Any(x => x == weight.Key);
                if (legal == false)
                {
                    MReqWeightJson.ReqWeights.RemoveAt(i);
                }
            }

            for (int i = 0; i < requests.Count; i++)
            {
                int reqId = requests[i];
                bool hasReq = MReqWeightJson.ReqWeights.Any(x => x.Key == reqId);
                if (hasReq == false)
                {
                    MReqWeightJson.ReqWeights.Add(new WeightJson() { Key = reqId });
                }
            }
        }

        private void DrawRequestWeight(float width, float height)
        {
            EDLayout.CreateScrollView(ref listPos, "box", width, height, () =>
            {
                for (int i = 0; i < MReqWeightJson.ReqWeights.Count; i++)
                {
                    EDLayout.CreateVertical("GroupBox", width, 75, () =>
                    {
                        WeightJson weightJson = MReqWeightJson.ReqWeights[i];
                        EditorGUILayout.LabelField("请求行为：" + GetRequestDisplayName(weightJson.Key), GUILayout.Width(width), GUILayout.Height(25));
                        weightJson.Weight = EditorGUILayout.IntField(weightJson.Weight, GUILayout.Width(width), GUILayout.Height(25));

                        EDLayout.CreateHorizontal("", width, 25, () =>
                        {
                            EDButton.CreateBtn("强制覆盖请求权重", width / 2, 25, (() => { weightJson.Weight = ECSDefinition.REForceSwithWeight; }));
                            EDButton.CreateBtn("需要自身判断置换请求权重", width / 2, 25, (() => { weightJson.Weight = ECSDefinition.RESwithRuleSelf; }));
                        });

                    });
                }
            });
        }

        public abstract List<int> GetAllRequests();

        public abstract string GetRequestDisplayName(int reqId);
    }
}
