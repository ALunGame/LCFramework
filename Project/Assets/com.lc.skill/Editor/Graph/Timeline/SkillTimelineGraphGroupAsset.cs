using LCJson;
using LCTimeline;
using LCToolkit;
using SkillSystem.ED.Timeline;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCSkill
{
    [CreateAssetMenu(fileName = "Timeline组", menuName = "配置组/Timeline组", order = 1)]
    public class SkillTimelineGraphGroupAsset : BaseTimelineGraphGroupAsset<SkillTimelineGraphAsset>
    {
        public override string DisplayName => "Timeline";

        public override void OnClickCreateBtn()
        {
            MiscHelper.Input($"输入Timeline名：", (string x) =>
            {
                string assetName = "timeline_" + x;
                SkillTimelineGraphAsset asset = CreateGraph(assetName) as SkillTimelineGraphAsset;
                asset.timelineName = x;
            });
        }

        public override void ExportGraph(List<InternalTimelineGraphAsset> assets)
        {
            for (int i = 0; i < assets.Count; i++)
            {
                SkillTimelineGraphAsset asset = assets[i] as SkillTimelineGraphAsset;
                BaseTimelineGraph graphData = asset.DeserializeGraph();

                //运行时数据结构
                TimelineModel model = SerializeToTimelineModel(graphData, asset);

                string filePath = SkillDef.GetTimelineCnfPath(asset.timelineName);
                IOHelper.WriteText(JsonMapper.ToJson(model), filePath);
                Debug.Log($"实体配置生成成功>>>>{filePath}");
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private TimelineModel SerializeToTimelineModel(BaseTimelineGraph graph, SkillTimelineGraphAsset asset)
        {
            TimelineModel timelineModel = new TimelineModel();
            timelineModel.name          = asset.timelineName;
            timelineModel.duration      = graph.DurationTime;
            timelineModel.nodes = new List<TimelineFunc>();

            for (int i = 0; i < graph.Tracks.Count; i++)
            {
                TrackModel track = graph.Tracks[i];
                for (int j = 0; j < track.Clips.Count; j++)
                {
                    TLSK_ClipData clip = track.Clips[j] as TLSK_ClipData;
                    timelineModel.nodes.Add(clip.GetFunc());
                }
            }
            return timelineModel;
        }
    }
}