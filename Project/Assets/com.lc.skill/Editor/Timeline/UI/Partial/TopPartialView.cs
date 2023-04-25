using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Timeline
{
    public class TopPartialView : PartialView
    {
        public VisualElement RootElement { get; private set; }

        private ObjectField previewGo;
        
        private ObjectField assetField;
        
        public void Init()
        {
            RootElement = window.rootVisualElement.Q<VisualElement>("Top");

            previewGo = RootElement.Q<ObjectField>("PreviewGo");
            previewGo.value = SkillTimelineWindow.TimelineAsset.previewGo;
            previewGo.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                SkillTimelineWindow.TimelineAsset.previewGo = (GameObject)evt.newValue;
                EditorUtility.SetDirty(SkillTimelineWindow.TimelineAsset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            });

            assetField = RootElement.Q<ObjectField>("AssetObj");
            assetField.objectType = typeof(SkillTimelineAsset);
            assetField.value = SkillTimelineWindow.TimelineAsset;
            assetField.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                SkillTimelineAsset newAsset = evt.newValue as SkillTimelineAsset;
                if (!newAsset.Equals(SkillTimelineWindow.TimelineAsset))
                {
                    SkillTimelineWindow.Open(newAsset);
                }
            });
            
        }

        public override void OnFocuseWindow()
        {
        }
    }
}