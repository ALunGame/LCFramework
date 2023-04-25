using System.Collections.Generic;
using System;
using LCToolkit;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using UnityEngine;

namespace LCSkill.Timeline
{
    public static class TimelineUtil
    {
        private static Dictionary<Type, Type> groupElementDict = new Dictionary<Type, Type>();
        private static Dictionary<Type, Type> trackElementDict = new Dictionary<Type, Type>();
        private static Dictionary<Type, Type> clipElementDict = new Dictionary<Type, Type>();
        
        private static bool isInit = false;

        private static void Init()
        {
            if (isInit)
            {
                return;
            }

            isInit = true;
            
            groupElementDict.Clear();
            foreach (var type in ReflectionHelper.GetChildTypes<InternalTrackGroup_Element>())
            {
                if (type.IsAbstract)
                    continue;
                if (type == typeof(InternalTrackGroup_Element))
                    continue;
                if (AttributeHelper.TryGetTypeAttribute(type, out TimlineGroupElementAttribute attr))
                {
                    groupElementDict.Add(attr.targetModelType,type);
                }
            }
            
            trackElementDict.Clear();
            foreach (var type in ReflectionHelper.GetChildTypes<InternalTrack_Element>())
            {
                if (type.IsAbstract)
                    continue;
                if (type == typeof(InternalTrack_Element))
                    continue;
                if (AttributeHelper.TryGetTypeAttribute(type, out TimlineTrackElementAttribute attr))
                {
                    trackElementDict.Add(attr.targetModelType,type);
                }
            }
            
            clipElementDict.Clear();
            foreach (var type in ReflectionHelper.GetChildTypes<InternalClip_Element>())
            {
                if (type.IsAbstract)
                    continue;
                if (type == typeof(InternalClip_Element))
                    continue;
                if (AttributeHelper.TryGetTypeAttribute(type, out TimlineClipElementAttribute attr))
                {
                    clipElementDict.Add(attr.targetModelType,type);
                }
            }
        }
        
        public static Type GetTrackGroupModelByElementType(Type pGroupElementType)
        {
            Init();
            foreach (var info in groupElementDict)
            {
                if (info.Value == pGroupElementType)
                {
                    return info.Key;
                }
            }
            Debug.LogError($"没有对应轨道组视图{pGroupElementType}");
            return null;
        }
        
        public static Type GetTrackGroupElementType(BaseTrackGroup pGroup)
        {
            Init();
            if (!groupElementDict.ContainsKey(pGroup.GetType()))
            {
                Debug.LogError($"没有对应轨道组视图{pGroup.GetType()}");
                return null;
            }

            return groupElementDict[pGroup.GetType()];
        }
        
        public static Type GetTrackElementType(BaseTrack pTrack)
        {
            Init();
            if (!trackElementDict.ContainsKey(pTrack.GetType()))
            {
                Debug.LogError($"没有对应轨道视图{pTrack.GetType()}");
                return null;
            }

            return trackElementDict[pTrack.GetType()];
        }
        
        public static Type GetClipElementType(BaseClip pClip)
        {
            Init();
            if (!clipElementDict.ContainsKey(pClip.GetType()))
            {
                Debug.LogError($"没有对应片段视图{pClip.GetType()}");
                return null;
            }

            return clipElementDict[pClip.GetType()];
        }
    }
}