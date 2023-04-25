using LCToolkit;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;

namespace LCGAS
{
    [CustomInspectorDrawer(typeof(InternalGameplayAbility))]
    public class GameplayAbilityDrawer : ObjectInspectorDrawer
    {
        private bool OpenTag = false;
        private bool OpenConditionTag = false;
        private bool OpenActiveOwnedTags = false;
        private bool OpenCancelTags = false;
        private bool OpenBlockTags = false;
        private bool OpenTriggerTags = false;
        
        public override void OnInspectorGUI()
        {
            InternalGameplayAbility ability = (InternalGameplayAbility)Target;
            
            GUILayoutExtension.FoldoutGroup("标签",OpenTag, () =>
            {
                OpenTag = true;
                ability.tags = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer), ability.tags);
            }, () =>
            {
                OpenTag = false;
            });
            
            GUILayoutExtension.FoldoutGroup("激活该GA的标签",OpenTriggerTags, () =>
            {
                OpenTriggerTags = true;
                ability.triggerTags = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer), ability.triggerTags);
            }, () =>
            {
                OpenTriggerTags = false;
            });
            
            
            GUILayoutExtension.FoldoutGroup("激活条件标签",OpenConditionTag, () =>
            {
                OpenConditionTag = true;
                ability.conditionTag = (CommonConditionTag)GUILayoutExtension.DrawField(typeof(CommonConditionTag), ability.conditionTag);
            }, () =>
            {
                OpenConditionTag = false;
            });
            
            
            GUILayoutExtension.FoldoutGroup("激活GA时，赋予ASC标签",OpenActiveOwnedTags, () =>
            {
                OpenActiveOwnedTags = true;
                ability.activeOwnedTags = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer), ability.activeOwnedTags);
            }, () =>
            {
                OpenActiveOwnedTags = false;
            });
            
            GUILayoutExtension.FoldoutGroup("激活该GA时，打断其他拥有该标签的GA",OpenCancelTags, () =>
            {
                OpenCancelTags = true;
                ability.cancelTags = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer), ability.cancelTags);
            }, () =>
            {
                OpenCancelTags = false;
            });
            
            GUILayoutExtension.FoldoutGroup("激活该GA时，阻止激活其他拥有该标签的GA",OpenBlockTags, () =>
            {
                OpenBlockTags = true;
                ability.blockTags = (GameplayTagContainer)GUILayoutExtension.DrawField(typeof(GameplayTagContainer), ability.blockTags);
            }, () =>
            {
                OpenBlockTags = false;
            });
            
            
            ability.cost = (GameplayEffectName)GUILayoutExtension.DrawField(typeof(GameplayEffectName), ability.cost,"消耗GE");
            ability.cooldown = (GameplayEffectName)GUILayoutExtension.DrawField(typeof(GameplayEffectName), ability.cooldown,"冷却GE");

            OnHandleEvent(Event.current);
        }
        
        public void OnHandleEvent(Event evt)
        {
            if (evt == null)
                return;
            //保存Ctrl+S
            if (Event.current.Equals(Event.KeyboardEvent("^S")))
            {
                SaveAsset();
                Event.current.Use();
            }
        }
        
        private void SaveAsset()
        {
            InternalGameplayAbility ability = (InternalGameplayAbility)Target;

            GAAsset asset = Owner as GAAsset;
            asset.Export(ability);
            
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            UpdateTarget(asset.GetAsset());
        }
    }
}