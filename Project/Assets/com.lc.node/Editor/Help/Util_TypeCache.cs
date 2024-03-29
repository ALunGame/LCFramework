﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace LCNode
{
    public static class Util_TypeCache
    {
        private static readonly List<Type> s_AllTypes = new List<Type>(512);

        public static IReadOnlyList<Type> AllTypes
        {
            get { return s_AllTypes; }
        }

        static Util_TypeCache()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.FullName.StartsWith("UnityEngine.CoreModule")) continue;
                if (!assembly.FullName.Contains("Version=0.0.0")) continue;
                s_AllTypes.AddRange(assembly.GetTypes());
            }
        }

        public static IEnumerable<Type> GetTypesWithAttribute(Type attributeType, bool inherit = true)
        {
            foreach (var type in AllTypes)
            {
                if (!type.IsDefined(attributeType, inherit))
                    continue;
                yield return type;
            }
        }

        public static IEnumerable<Type> GetTypesWithAttribute<T>(bool inherit = true) where T : Attribute
        {
            return GetTypesWithAttribute(typeof(T), inherit);
        }

        public static IEnumerable<Type> GetTypesDerivedFrom(Type parentType)
        {
            foreach (var type in AllTypes)
            {
                if (type == parentType)
                    continue;
                if (!parentType.IsAssignableFrom(type))
                    continue;
                yield return type;
            }
        }

        public static IEnumerable<Type> GetTypesDerivedFrom<T>()
        {
            return GetTypesDerivedFrom(typeof(T));
        }

        public static IEnumerable<MethodInfo> GetMethodsWithAttribute(Type attributeType, bool inherit = true)
        {
            foreach (var type in AllTypes)
            {
                foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))
                {
                    if (!method.IsDefined(attributeType, inherit))
                        continue;
                    yield return method;
                }
            }
        }

        public static IEnumerable<MethodInfo> GetMethodsWithAttribute<T>(bool inherit = true) where T : Attribute
        {
            return GetMethodsWithAttribute(typeof(T), inherit);
        }
    }
}