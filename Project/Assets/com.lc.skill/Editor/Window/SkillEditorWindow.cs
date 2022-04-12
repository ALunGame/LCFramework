using LCSkill.Serialize;
using System;
using System.IO;
using System.Text;
using LCTimeline;
using UnityEditor;
using UnityEngine;
using LCJson;

namespace LCSkill
{
    public class SkillEditorWindow : TimelineEditorWindow
    {
        public override string SavePath => "Assets/SkillSystem/Editor/EDData/";

        public string RunningTimeSavePath => "Assets/SkillSystem/Data/";

        public override Action<TimelineData> OnSaveTimeline => SaveTimeline;

        [MenuItem("Skill/¾ŽÝ‹", false, 1)]
        public static void ShowWindow()
        {
            SkillEditorWindow window = GetWindow<SkillEditorWindow>(true, "¼¼ÄÜ¾ŽÝ‹", true);
            window.TimeInFrames = true;
            window.minSize = new Vector2(400, 300);
        }

        private void SaveTimeline(TimelineData timelineData)
        {
            //string filePath = string.Format("{0}/{1}", RunningTimeSavePath, timelineData.Name + ".txt");
            //SK_TimelineModel model = SkillSerializeHelp.CreateSkillTimelineModel(timelineData);
            //string jsonStr = JsonMapper.ToJson(model);
            //string dirPath = Path.GetDirectoryName(filePath);
            //if (!Directory.Exists(dirPath))
            //    Directory.CreateDirectory(dirPath);
            //File.WriteAllText(filePath, jsonStr, Encoding.UTF8);
        }
    }
}