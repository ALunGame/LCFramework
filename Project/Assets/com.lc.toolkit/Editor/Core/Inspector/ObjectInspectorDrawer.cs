using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using LCToolkit.Extension;
using LCToolkit.Help;
using UnityObject = UnityEngine.Object;

namespace LCToolkit.Core
{
    /// <summary>
    /// 自定义Inspector绘制
    /// </summary>
    public class CustomInspectorDrawer : Attribute
    {
        public Type targetType;

        public CustomInspectorDrawer(Type _targetType) { targetType = _targetType; }
    }

    /// <summary>
    /// Inspector面板绘制器
    /// </summary>
    public class ObjectInspectorDrawer
    {
        #region Static

        static Dictionary<Type, Type> ObjectEditorTypeCache;

        static ObjectInspectorDrawer()
        {
            BuildCache();
        }

        private static void BuildCache()
        {
            ObjectEditorTypeCache = new Dictionary<Type, Type>();

            foreach (var type in TypeCache.GetTypesDerivedFrom<ObjectInspectorDrawer>())
            {
                foreach (var att in AttributeHelper.GetTypeAttributes(type, true))
                {
                    if (att is CustomInspectorDrawer sAtt)
                        ObjectEditorTypeCache[sAtt.targetType] = type;
                }
            }
        }

        //获得对象绘制类
        private static Type GetEditorType(Type objectType)
        {
            if (ObjectEditorTypeCache.TryGetValue(objectType, out Type editorType))
                return editorType;
            if (objectType.BaseType != null)
                return GetEditorType(objectType.BaseType);
            else
                return typeof(ObjectInspectorDrawer);
        }

        private static ObjectInspectorDrawer InternalCreateEditor(object _targetObject)
        {
            if (_targetObject == null) return null;
            return Activator.CreateInstance(GetEditorType(_targetObject.GetType()), true) as ObjectInspectorDrawer;
        }

        //创建对象绘制类
        public static ObjectInspectorDrawer CreateEditor(object _targetObject)
        {
            ObjectInspectorDrawer objectEditor = InternalCreateEditor(_targetObject);
            if (objectEditor == null) return null;

            objectEditor.Init(_targetObject);
            return objectEditor;
        }

        public static ObjectInspectorDrawer CreateEditor(object _targetObject, UnityObject _owner)
        {
            ObjectInspectorDrawer objectEditor = InternalCreateEditor(_targetObject);
            if (objectEditor == null) return null;

            objectEditor.Init(_targetObject, _owner);
            return objectEditor;
        }

        public static ObjectInspectorDrawer CreateEditor(object _targetObject, UnityObject _owner, Editor _editor)
        {
            ObjectInspectorDrawer objectEditor = InternalCreateEditor(_targetObject);
            if (objectEditor == null) return null;

            objectEditor.Init(_targetObject, _owner, _editor);
            return objectEditor;
        }

        #endregion

        protected IReadOnlyList<FieldInfo> Fields { get; private set; }

        public object Target { get; private set; }
        public UnityObject Owner { get; private set; }
        public Editor Editor { get; private set; }
        public MonoScript Script { get; private set; }

        protected ObjectInspectorDrawer() { }

        #region 初始化

        void Init(object _target)
        {
            Target = _target;
            Script = EditorExtension.FindScriptFromType(Target.GetType());
            Fields = ReflectionHelper.GetFieldInfos(Target.GetType()).Where(field => GUILayoutExtension.CanDraw(field)).ToList();
        }

        void Init(object _target, UnityObject _owner)
        {
            Owner = _owner;
            Init(_target);
        }

        void Init(object _target, UnityObject _owner, Editor _editor)
        {
            Owner = _owner;
            Editor = _editor;
            Init(_target);
        }

        #endregion

        /// <summary>
        /// 标题
        /// </summary>
        public virtual string GetTitle() { return string.Empty; }

        public virtual void OnEnable() { }

        public virtual void OnHeaderGUI() { }

        public virtual void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", Script, typeof(MonoScript), false);
            EditorGUI.EndDisabledGroup();
            foreach (var field in Fields)
            {
                GUILayoutExtension.DrawField(field, field.GetValue(Target));
            }
        }

        public virtual bool HasPreviewGUI() { return false; }

        public virtual GUIContent GetPreviewTitle() { return null; }

        public virtual void OnPreviewSettings() { }

        public virtual void DrawPreview(Rect previewArea) { }

        public virtual void OnPreviewGUI(Rect _r, GUIStyle _background) { }

        public virtual void OnInteractivePreviewGUI(Rect _r, GUIStyle _background) { }

        public virtual void OnValidate() { }

        public virtual void OnSceneGUI() { }

        public virtual void OnDisable() { }

        public VisualElement CreateInspectorGUI() { return null; }
    } 
}
