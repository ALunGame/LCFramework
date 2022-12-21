using Demo.AutoCreate;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Demo
{
    [CustomEditor(typeof(AnimatorOverride), true)]
    public class AnimatorOverrideInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            AnimatorOverride actorAnim = (AnimatorOverride)target;
            if (GUILayout.Button("收集覆盖动画", GUILayout.Height(30)))
            {
                HandleAnimLayers(actorAnim);
            }

            if (GUILayout.Button("设置覆盖动画", GUILayout.Height(30)))
            {
                actorAnim.SetAnimLayer(actorAnim.GetAnimLayer());
            }
        }

        private void HandleAnimLayers(AnimatorOverride actorAnim)
        {
            Animator animator = actorAnim.GetComponent<Animator>();
            if (animator == null || animator.runtimeAnimatorController == null)
                return;

            string path = AssetDatabase.GetAssetPath(animator.runtimeAnimatorController);
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
            List<AnimationClip> clips = new List<AnimationClip>();
            foreach (var item in objs)
            {
                if (item is AnimationClip)
                {
                    clips.Add(item as AnimationClip);
                }
            }

            Dictionary<string,List<AnimationClip>> clipDict = new Dictionary<string, List<AnimationClip>>();
            foreach (var clip in clips)
            {
                (string, string) layerStateStr = AutoSetAnimController.CalcLayerAndStateName(clip);
                if (!clipDict.ContainsKey(layerStateStr.Item1))
                    clipDict.Add(layerStateStr.Item1, new List<AnimationClip>());
                clipDict[layerStateStr.Item1].Add(clip);
            }

            List<AnimOverrideInfo> layerInfos = new List<AnimOverrideInfo>();
            foreach (var item in clipDict)
            {
                layerInfos.Add(new AnimOverrideInfo() { name = item.Key,clips = item.Value });
            }
            actorAnim.AnimLayers = layerInfos;
        }
    }
}
