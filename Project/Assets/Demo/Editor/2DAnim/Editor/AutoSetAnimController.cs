using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Demo.AutoCreate
{
    public class AutoSetAnimController
    {
        private static List<AnimatorController>  selAnimControllers = new List<AnimatorController>();

        [MenuItem("Assets/自动设置动画状态机")]
        public static void Create2DAnim()
        {
            foreach (var item in selAnimControllers)
            {
                SetAnimatorController(item);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [MenuItem("Assets/自动设置动画状态机", true)]
        public static bool Create2DAnimValidate()
        {
            Object[] guidArray = Selection.objects;
            selAnimControllers.Clear();
            foreach (var obj in guidArray)
            {
                if (obj is AnimatorController)
                {
                    selAnimControllers.Add(obj as AnimatorController);
                }
            }
            return selAnimControllers.Count > 0;
        }

        class AnimLayerInfo
        {
            public AnimationClip defaultClip;
            public List<AnimationClip> otherClips;

            private AnimatorState defaultState;

            public AnimLayerInfo(List<AnimationClip> clips)
            {
                defaultClip = clips.FirstOrDefault(x => GetStateName(x) == "idle");
                if (defaultClip == null)
                    defaultClip = clips.FirstOrDefault(x => GetStateName(x).Contains("idle"));
                if (defaultClip == null)
                {
                    Debug.LogError("没有默认状态");
                    return;
                }
                otherClips = clips.Where(x => x.name != defaultClip.name).ToList();
            }

            public void CreateAnimParameter(AnimatorController controller)
            {
                controller.AddParameter(GetStateName(defaultClip), AnimatorControllerParameterType.Bool);
                foreach (var item in otherClips)
                {
                    string stateName = GetStateName(item);
                    controller.AddParameter(stateName, AnimatorControllerParameterType.Trigger);

                    if (item.isLooping)
                        controller.AddParameter(stateName + "_state", AnimatorControllerParameterType.Bool);
                }
            }

            public void ConnectAnimClips(AnimatorControllerLayer layer)
            {
                AnimatorStateMachine sm = layer.stateMachine;
                sm.anyStatePosition     = new Vector2(sm.entryPosition.x + 600, sm.entryPosition.y - 200);
                sm.anyStateTransitions  = null;
                sm.entryTransitions     = null;
                sm.states               = null;

                ConnectDefaultClip(sm);

                for (int i = 0; i < otherClips.Count; i++)
                {
                    AnimationClip clip = otherClips[i];
                    int stateIndex = i + 1;
                    if (clip.isLooping)
                    {
                        ConnectLoopClip(sm, clip, stateIndex);
                    }
                    else
                    {
                        ConnectTriggerClip(sm, clip, stateIndex);
                    }
                }
            }

            private void ConnectDefaultClip(AnimatorStateMachine sm)
            {
                float posX = sm.entryPosition.x;
                float posY = sm.entryPosition.y - 200;
                Vector2 statePos = new Vector2(posX, posY);

                string stateName = GetStateName(defaultClip);
                AnimatorState defaultState = sm.AddState(stateName, statePos);
                defaultState.motion = defaultClip;

                ConnectAnyStateByBool(sm, defaultState, stateName);

                this.defaultState = defaultState;
            }

            private void ConnectLoopClip(AnimatorStateMachine sm, AnimationClip clip, int stateIndex)
            {
                float posX = sm.anyStatePosition.x - 300;
                float posY = sm.anyStatePosition.y + 100 * stateIndex;
                Vector2 statePos = new Vector2(posX, posY);

                string stateName = GetStateName(clip);
                AnimatorState newState = sm.AddState(stateName, statePos);
                newState.motion = clip;

                AnimatorStateTransition stateTransition = ConnectAnyStateByTrigger(sm, newState, stateName + "_state");
                stateTransition.AddCondition(AnimatorConditionMode.If, 0, stateName);
                ConnectIdleStateByBool(newState, defaultState, stateName + "_state");
            }

            private void ConnectTriggerClip(AnimatorStateMachine sm, AnimationClip clip, int stateIndex)
            {
                float posX = sm.anyStatePosition.x - 300;
                float posY = sm.anyStatePosition.y + 100 * stateIndex;
                Vector2 statePos = new Vector2(posX, posY);

                string stateName = GetStateName(clip);
                AnimatorState newState = sm.AddState(stateName, statePos);
                newState.motion = clip;

                ConnectAnyStateByTrigger(sm, newState, stateName);
                ConnectIdleStateByTrigger(newState, defaultState);
            }

            private AnimatorStateTransition ConnectAnyStateByBool(AnimatorStateMachine sm, AnimatorState newState, string condName)
            {
                AnimatorStateTransition _transition = sm.AddAnyStateTransition(newState);
                _transition.AddCondition(AnimatorConditionMode.If, 0, condName);
                _transition.hasExitTime = false;
                _transition.canTransitionToSelf = false;
                _transition.offset = 0;
                _transition.duration = 0;
                return _transition;
            }

            private AnimatorStateTransition ConnectAnyStateByTrigger(AnimatorStateMachine sm, AnimatorState newState, string condName)
            {
                AnimatorStateTransition _transition = sm.AddAnyStateTransition(newState);
                _transition.AddCondition(AnimatorConditionMode.If, 0, condName);
                _transition.hasExitTime = false;
                _transition.canTransitionToSelf = true;
                _transition.offset = 0;
                _transition.duration = 0;
                return _transition;
            }

            private void ConnectIdleStateByBool(AnimatorState newState, AnimatorState entryState, string condName)
            {
                if (newState == null || entryState == null)
                    return;
                AnimatorStateTransition _transition = newState.AddTransition(entryState);
                _transition.AddCondition(AnimatorConditionMode.IfNot, 0, condName);
                _transition.hasExitTime = true;
                _transition.offset = 0;
                _transition.duration = 0;
            }

            private void ConnectIdleStateByTrigger(AnimatorState newState, AnimatorState entryState)
            {
                if (newState == null || entryState == null)
                    return;
                AnimatorStateTransition _transition = newState.AddTransition(entryState);
                _transition.hasExitTime = true;
                _transition.offset = 0;
                _transition.duration = 0;
            }

            private string GetStateName(AnimationClip clip)
            {
                (string, string) data = CalcLayerAndStateName(clip);
                return data.Item2;
            }
        }

        private static void SetAnimatorController(AnimatorController animatorController)
        {
            string controllerPath = AssetDatabase.GetAssetPath(animatorController);
            Object[] objs = AssetDatabase.LoadAllAssetsAtPath(controllerPath);

            List<AnimationClip> clips = new List<AnimationClip>();
            foreach (var item in objs)
            {
                if (item is AnimationClip)
                {
                    clips.Add(item as AnimationClip);
                }
            }
            animatorController.parameters = null;

            AnimatorControllerLayer layer = animatorController.layers[0];
            AnimatorStateMachine sm = layer.stateMachine;
            sm.anyStatePosition = new Vector2(0, 0);
            sm.entryPosition = new Vector2(sm.anyStatePosition.x + 1200, -200);
            sm.exitPosition = new Vector2(0, -200);
            sm.anyStateTransitions = null;
            sm.entryTransitions = null;
            sm.states = null;

            List<AnimationClip> layerClips = new List<AnimationClip>();
            for (int i = 0; i < clips.Count; i++)
            {
                AnimationClip clip = clips[i];
                (string, string) layerStateStr = CalcLayerAndStateName(clip);
                if (layerStateStr.Item1 == "" || layerStateStr.Item1 == "side")
                {
                    layerClips.Add(clip);
                }
            }
            AnimLayerInfo animLayer = new AnimLayerInfo(layerClips);
            animLayer.CreateAnimParameter(animatorController);
            animLayer.ConnectAnimClips(layer);
        }

        public static (string, string) CalcLayerAndStateName(AnimationClip clip)
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