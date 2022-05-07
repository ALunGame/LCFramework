using System.Collections.Generic;
using UnityEngine;

namespace Demo.Help
{
    public static class AnimHelp
    {
        private static Dictionary<string, List<string>> animParameterCache = new Dictionary<string, List<string>>();

        private static List<string> GetParameterList(Animator animator)
        {
            if (animator.runtimeAnimatorController == null)
            {
                Debug.LogWarning("没有动画控制器》》" + animator.transform.parent.name);
                return null;
            }
            string key = animator.runtimeAnimatorController.name;
            if (animParameterCache.ContainsKey(key))
            {
                return animParameterCache[key];
            }

            List<string> list = new List<string>();
            int parameterCount = animator.parameterCount;
            for (int i = 0; i < parameterCount; ++i)
            {
                AnimatorControllerParameter info = animator.GetParameter(i);
                list.Add(info.name);
            }

            if (list.Count > 0)
                animParameterCache.Add(key, list);

            return list;
        }

        public static bool CheckAnimatorHasParam(Animator animator, string name)
        {
            if (animator == null || string.IsNullOrEmpty(name))
                return false;
            List<string> list = GetParameterList(animator);
            if (list == null || list.Count <= 0)
                return false;
            for (int i = 0; i < list.Count; i++)
            {
                string info = list[i];
                if (info != null && info.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<string> GetAllParamNames(Animator animator)
        {
            if (animator == null)
                return null;

            int parameterCount = animator.parameterCount;
            List<string> nameList = new List<string>();
            for (int i = 0; i < parameterCount; ++i)
            {
                AnimatorControllerParameter info = animator.GetParameter(i);
                if (info != null)
                {
                    nameList.Add(info.name);
                }
            }
            return nameList;
        }

        //检测当前控制器是不是处在指定状态
        public static bool CheckIsInState(Animator animator, string stateName, int layerIndex = 0)
        {
            if (animator == null || animator.runtimeAnimatorController == null || string.IsNullOrEmpty(stateName))
                return false;
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
            return stateInfo.IsName(stateName);
        }

        public static float GetClipTime(Animator animator, string clipName)
        {
            if (animator.runtimeAnimatorController == null || string.IsNullOrEmpty(clipName))
                return 0;

            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            if (clips == null || clips.Length <= 0)
                return 0;

            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name == clipName)
                {
                    return clips[i].length;
                }
            }
            return 0;
        }
    }
}