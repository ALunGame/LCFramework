using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCToolkit.Core
{
    /// <summary>
    /// �Զ���Inspector����
    /// </summary>
    public class CustomInspectorDrawer : Attribute
    {
        public Type targetType;

        public CustomInspectorDrawer(Type _targetType) { targetType = _targetType; }
    }

    /// <summary>
    /// Inspector��������
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

        private static List<Type> GetBaseTypes(Type objectType)
        {
            //ֻ���Ĳ�
            List<Type> baseTypes = new List<Type>();
            if (objectType.BaseType != null)
            {
                baseTypes.Add(objectType.BaseType);
                if (objectType.BaseType.BaseType != null)
                {
                    baseTypes.Add(objectType.BaseType.BaseType);
                    if (objectType.BaseType.BaseType.BaseType != null)
                    {
                        baseTypes.Add(objectType.BaseType.BaseType.BaseType);
                    }
                }
            }
            return baseTypes;
        }

        //��ö��������
        private static Type GetEditorType(Type objectType)
        {
            if (ObjectEditorTypeCache.TryGetValue(objectType, out Type editorType))
                return editorType;

            //�ӿ�
            Type[] interfaces = objectType.GetInterfaces();
            if (interfaces != null && interfaces.Length > 0)
            {
                for (int i = 0; i < interfaces.Length; i++)
                {
                    if (ObjectEditorTypeCache.TryGetValue(interfaces[i], out Type interfacesEditorType))
                        return interfacesEditorType;
                }
            }

            //����
            List<Type> baseTypes = GetBaseTypes(objectType);
            foreach (Type type in baseTypes)
            {
                if (ObjectEditorTypeCache.TryGetValue(type, out Type baseEditorType))
                    return baseEditorType;
            }
            return typeof(ObjectInspectorDrawer);
        }

        private static ObjectInspectorDrawer InternalCreateEditor(object _targetObject)
        {
            if (_targetObject == null) return null;
            return Activator.CreateInstance(GetEditorType(_targetObject.GetType()), true) as ObjectInspectorDrawer;
        }

        //�������������
        public static ObjectInspectorDrawer CreateEditor(object _targetObject)
        {
            ObjectInspectorDrawer objectEditor = InternalCreateEditor(_targetObject);
            if (objectEditor == null) return null;

            objectEditor.Init(_targetObject);
            return objectEditor;
        }

        public static ObjectInspectorDrawer CreateEditor(object _targetObject, object _owner)
        {
            ObjectInspectorDrawer objectEditor = InternalCreateEditor(_targetObject);
            if (objectEditor == null) return null;

            objectEditor.Init(_targetObject, _owner);
            return objectEditor;
        }

        public static ObjectInspectorDrawer CreateEditor(object _targetObject, object _owner, Editor _editor)
        {
            ObjectInspectorDrawer objectEditor = InternalCreateEditor(_targetObject);
            if (objectEditor == null) return null;

            objectEditor.Init(_targetObject, _owner, _editor);
            return objectEditor;
        }

        #endregion

        protected IReadOnlyList<FieldInfo> Fields { get; private set; }

        public object Target { get; private set; }
        public object Owner { get; private set; }
        public Editor Editor { get; private set; }
        public MonoScript Script { get; private set; }

        protected ObjectInspectorDrawer() { }

        #region ��ʼ��

        void Init(object _target)
        {
            Target = _target;
            Script = EditorExtension.FindScriptFromType(Target.GetType());
            Fields = ReflectionHelper.GetFieldInfos(Target.GetType()).Where(field => GUILayoutExtension.CanDraw(field)).ToList();
        }

        void Init(object _target, object _owner)
        {
            Owner = _owner;
            Init(_target);
        }

        void Init(object _target, object _owner, Editor _editor)
        {
            Owner = _owner;
            Editor = _editor;
            Init(_target);
        }

        #endregion

        /// <summary>
        /// ����
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
                GUILayoutExtension.DrawField(field, Target);
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
