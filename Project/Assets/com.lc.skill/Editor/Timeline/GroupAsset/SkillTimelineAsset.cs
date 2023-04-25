using System;
using LCToolkit;
using UnityEditor;
using UnityEngine;

namespace LCSkill.Timeline
{
    public class SkillTimelineAsset : GroupChildAsset
    {
        public GameObject previewGo;
        public Action clickBackFunc = null;

        public override void Open(object pOwner = null, Action _clickBackFunc = null)
        {
            SkillTimelineWindow.Open(this);
        }

        public BaseTimeline GetAsset()
        {
            string filePath = GetExportFilePath();
            if (string.IsNullOrEmpty(filePath))
            {
                return new BaseTimeline();
            }

            string text = IOHelper.ReadText(filePath);
            var asset = LCJson.JsonMapper.ToObject<BaseTimeline>(text);
            if (asset == null)
                asset = new BaseTimeline();
            return asset;
        }
    }
}