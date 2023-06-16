using System;
using System.Collections.Generic;
using Demo.Com;
using Demo.Help;
using UnityEngine;

namespace Demo
{
    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity) : base(capacity) { }

        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set
            {
                int index = this.FindIndex(x => x.Key.name.Equals(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }

    [Serializable]
    public class AnimOverrideInfo
    {
        public string name;
        public List<AnimationClip> clips;
    }

    /// <summary>
    /// 动画覆盖
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class AnimatorOverride : MonoBehaviour
    {
        [Header("动画覆盖片段")]
        public List<AnimOverrideInfo> AnimLayers = new List<AnimOverrideInfo>();

        [Header("当前动画层级")]
        [SerializeField]
        private string CurrLayerName;

        private Animator animator;
        private AnimatorOverrideController animatorOverrideController;
        private AnimationClipOverrides clipOverrides;

        public void Start()
        {
            animator = GetComponent<Animator>();
            animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = animatorOverrideController;

            clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
            animatorOverrideController.GetOverrides(clipOverrides);

            if (!string.IsNullOrEmpty(CurrLayerName))
            {
                SetAnimLayer(CurrLayerName);
            }
        }

        public void SetAnimLayer(string name)
        {
            AnimOverrideInfo selLayerInfo = null;
            foreach (var item in AnimLayers)
            {
                if (item.name == name)
                {
                    selLayerInfo = item;
                    break;
                }
            }
            if (selLayerInfo == null)
            {
                Debug.LogError($"设置层级失败>>>{name}");
                return;
            }
            CurrLayerName = name;
            Dictionary<string, AnimationClip> clipDict = GetAnimClipDict(selLayerInfo);
            foreach (var item in clipDict)
            {
                clipOverrides[item.Key] = item.Value;
            }
            animatorOverrideController.ApplyOverrides(clipOverrides);
        }

        public string GetAnimLayer()
        {
            return CurrLayerName;
        }

        public float GetClipTime(string animName, AnimLayer layer = AnimLayer.Side)
        {
            string checkName = layer.ToString().ToLower();
            AnimOverrideInfo selLayerInfo = null;
            foreach (var item in AnimLayers)
            {
                if (item.name == checkName)
                {
                    selLayerInfo = item;
                    break;
                }
            }
            
            if (selLayerInfo == null)
            {
                return AnimHelp.GetClipTime(animator,animName);
            }
            
            Dictionary<string, AnimationClip> clipDict = GetAnimClipDict(selLayerInfo);
            foreach (var item in clipDict)
            {
                if (item.Key == animName)
                {
                    return item.Value.length;
                }
            }
            
            return AnimHelp.GetClipTime(animator,animName);
        }

        private Dictionary<string, AnimationClip> GetAnimClipDict(AnimOverrideInfo layerInfo)
        {
            Dictionary<string, AnimationClip> dict = new Dictionary<string, AnimationClip>();
            foreach (var item in layerInfo.clips)
            {
                (string, string) layerState = CalcLayerAndStateName(item);
                foreach (var oldClip in clipOverrides)
                {
                    (string, string) oldPlayerState = CalcLayerAndStateName(oldClip.Key);
                    if (layerState.Item2 == oldPlayerState.Item2)
                    {
                        dict.Add(oldClip.Key.name, item);
                        break;
                    }
                }
            }
            return dict;
        }

        private (string, string) CalcLayerAndStateName(AnimationClip clip)
        {
            if (!clip.name.Contains("_"))
            {
                return new("", clip.name);
            }
            string[] strs = clip.name.Split("_");
            return new(strs[0], strs[1]);
        }
    }
}
