using LCToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCToolkit.Core;
using LCToolkit.Inspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCGAS.Inspector
{
    [CustomEditor(typeof(GEAsset), true)]
    public class GEAssetInspector : Editor
    {
        static GUIHelper.ContextDataCache ContextDataCache = new GUIHelper.ContextDataCache();

        private GameplayEffectDrawer objectEditor;
        private void OnEnable()
        {
            GEAsset asset = target as GEAsset;
            GameplayEffect effect = asset.GetAsset();

            objectEditor = new GameplayEffectDrawer();
            objectEditor.Init(effect,asset, asset.clickBackFunc);
            
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