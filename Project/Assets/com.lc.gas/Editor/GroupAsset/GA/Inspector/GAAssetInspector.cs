using LCToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IAToolkit.Inspector;
using LCToolkit.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCGAS
{
    [CustomEditor(typeof(GAAsset), true)]
    internal class GAAssetInspector : Editor
    {
        private GameplayAbilityDrawer objectEditor;
        private void OnEnable()
        {
            GAAsset asset = target as GAAsset;
            InternalGameplayAbility ability = asset.GetAsset();

            objectEditor = new GameplayAbilityDrawer();
            objectEditor.Init(ability,asset, asset.clickBackFunc);
            
            objectEditor.OnEnable();
        }
        
        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
            if (objectEditor != null)
                objectEditor.OnHeaderGUI();
        }

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement v = null;
            if (objectEditor != null)
                v = objectEditor.CreateInspectorGUI();
            if (v == null)
                v = base.CreateInspectorGUI();
            return v;
        }

        public override void OnInspectorGUI()
        {
            if (objectEditor.onClickBackFunc != null)
            {
                MiscHelper.Btn("返回",100,15, () =>
                {
                    Action func = objectEditor.onClickBackFunc;
                    objectEditor.onClickBackFunc = null;
                    func?.Invoke();
                });
            }
            
            GroupChildAssetInspector.DrawFileName(target);
            
            if (objectEditor != null)
                objectEditor.OnInspectorGUI();
        }

        public override void DrawPreview(Rect previewArea)
        {
            base.DrawPreview(previewArea);
            if (objectEditor != null)
                objectEditor.DrawPreview(previewArea);
        }

        public override bool HasPreviewGUI()
        {
            if (objectEditor != null)
                return objectEditor.HasPreviewGUI();
            return base.HasPreviewGUI();
        }

        public override void OnPreviewSettings()
        {
            base.OnPreviewSettings();
            if (objectEditor != null)
                objectEditor.OnPreviewSettings();
        }

        public override GUIContent GetPreviewTitle()
        {
            GUIContent title = null;
            if (objectEditor != null)
                title = objectEditor.GetPreviewTitle();
            if (title == null)
                title = base.GetPreviewTitle();
            return title;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            base.OnPreviewGUI(r, background);
            if (objectEditor != null)
                objectEditor.OnPreviewGUI(r, background);
        }

        public override void OnInteractivePreviewGUI(Rect r, GUIStyle background)
        {
            base.OnInteractivePreviewGUI(r, background);
            if (objectEditor != null)
                objectEditor.OnInteractivePreviewGUI(r, background);
        }

        private void OnSceneGUI()
        {
            if (objectEditor != null)
                objectEditor.OnSceneGUI();
        }

        private void OnValidate()
        {
            if (objectEditor != null)
                objectEditor.OnValidate();
        }

        private void OnDisable()
        {
            ObjectInspector.objectEditor = null;

            if (objectEditor != null)
                objectEditor.OnDisable();
        }
    }
}