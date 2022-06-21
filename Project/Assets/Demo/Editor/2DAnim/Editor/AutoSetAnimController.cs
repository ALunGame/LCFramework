using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Assets.Demo.Editor._2DAnim.Editor
{
    public class AutoSetAnimController
    {
        private static AnimatorController SelAnim = null;

        //默认入口状态
        private static string EntryStateName = "idle";



        //默认其他状态
        private static List<string> DefaultStateNames = new List<string>() { "idle", "walk", "run" };

        //循环动画片段
        private static List<string> LoopClipNames = new List<string>() { "idle", "walk", "run" };

        //自定义状态动画名
        private static List<string> CustomStateClipNames = new List<string>();

        [MenuItem("Assets/自动设置动画状态机")]
        public static void Create2DAnim()
        {
            CreateAnim();
        }

        [MenuItem("Assets/自动设置动画状态机", true)]
        public static bool Create2DAnimValidate()
        {
            return CheckCanOpenCreate2DAnimPanel();
        }

        private static bool CheckCanOpenCreate2DAnimPanel()
        {
            //只选中一个
            string[] guidArray = Selection.assetGUIDs;
            if (guidArray == null || guidArray.Length != 1)
            {
                return false;
            }

            //必须是AnimatorController
            string path = AssetDatabase.GUIDToAssetPath(guidArray[0]);
            AnimatorController animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
            if (animatorController == null)
            {
                SelAnim = null;
                return false;
            }

            SelAnim = animatorController;
            return true;
        }

        private static void CreateAnim()
        {
            string controllerPath = AssetDatabase.GetAssetPath(SelAnim);
            UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(controllerPath);
            CollectLoopAnim(objs);
            var clipList = new List<AnimationClip>();

            foreach (var o in objs)
            {
                if (o is AnimationClip)
                {
                    var clip = o as AnimationClip;
                    if (CheckIsLoopClip(clip))
                    {
                        SerializedObject serializedClip = new SerializedObject(clip);
                        AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
                        clipSettings.loopTime = true;
                        AnimationUtility.SetAnimationClipSettings(clip, clipSettings);

                        serializedClip.ApplyModifiedProperties();
                        EditorUtility.SetDirty(o);
                    }
                    clipList.Add(clip);
                }
            }

            CreateAnimatorController(SelAnim, clipList);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static bool CheckIsLoopClip(AnimationClip clip)
        {
            for (int i = 0; i < LoopClipNames.Count; i++)
            {
                if (clip.name.Contains(LoopClipNames[i]))
                {
                    return true;
                }
            }
            return false;
        }

        private static void CollectLoopAnim(UnityEngine.Object[] objs)
        {
            CustomStateClipNames.Clear();
            var clipList = new List<AnimationClip>();

            foreach (var o in objs)
            {
                if (o is AnimationClip)
                {
                    var clip = o as AnimationClip;
                    SerializedObject serializedClip = new SerializedObject(clip);
                    AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
                    if (clipSettings.loopTime)
                    {
                        CustomStateClipNames.Add(clip.name);
                    }
                }
            }
        }

        private static bool CheckIsStateClip(string clipName)
        {
            for (int i = 0; i < DefaultStateNames.Count; i++)
            {
                if (clipName.Contains(DefaultStateNames[i]))
                {
                    return true;
                }
            }

            if (CustomStateClipNames.Contains(clipName))
            {
                return true;
            }
            return false;
        }

        //创建动画控制器
        private static void CreateAnimatorController(AnimatorController animatorController, List<AnimationClip> clips)
        {
            AnimatorControllerLayer layer = animatorController.layers[0];
            AnimatorStateMachine sm = layer.stateMachine;
            sm.anyStatePosition = new Vector2(0, 0);
            sm.entryPosition = new Vector2(sm.anyStatePosition.x + 1200, -200);
            sm.exitPosition = new Vector2(0, -200);
            sm.anyStateTransitions = null;
            sm.entryTransitions = null;
            sm.states = null;

            Dictionary<string, List<AnimationClip>> groupClipDict = CollectAllClips(clips);
            Vector2 startPos = new Vector2(sm.anyStatePosition.x, sm.anyStatePosition.y + 300);
            int posIndex = 0;

            if (!groupClipDict.ContainsKey(EntryStateName))
            {
                Debug.LogError($"当前没有 {EntryStateName} 默认入口");
                return;
            }
            //参数
            CreateAnimatorControllerParameter(animatorController, groupClipDict);

            Dictionary<string, List<AnimationClip>> orderGroupDict = new Dictionary<string, List<AnimationClip>>();
            orderGroupDict.Add(EntryStateName, groupClipDict[EntryStateName]);
            foreach (var item in groupClipDict)
            {
                if (item.Key != EntryStateName)
                {
                    orderGroupDict.Add(item.Key, item.Value);
                }
            }

            //状态节点
            AnimatorState entryState = null;
            foreach (var item in orderGroupDict)
            {
                bool isState = CheckIsStateClip(item.Key);

                Vector2 statePos = startPos;
                if (posIndex != 0)
                {
                    statePos = new Vector2(startPos.x + 300, startPos.y + posIndex * 100);
                }

                if (item.Key == EntryStateName)
                {
                    statePos = new Vector2(sm.entryPosition.x, sm.anyStatePosition.y);
                    (AnimatorState, AnimatorState) itemStateGroup = CreateGroupState(sm, item.Key, item.Value, statePos, isState);
                    entryState = itemStateGroup.Item1;
                    sm.entryTransitions = null;
                    //sm.AddEntryTransition(entryState);
                    sm.defaultState = entryState;
                }
                else
                {
                    (AnimatorState, AnimatorState) itemStateGroup = CreateGroupState(sm, item.Key, item.Value, statePos, isState, entryState);
                }

                posIndex++;
            }

            //CreateAllState(sm, clips);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static Dictionary<string, List<AnimationClip>> CollectAllClips(List<AnimationClip> clips)
        {
            Dictionary<string, List<AnimationClip>> clipDict = new Dictionary<string, List<AnimationClip>>();

            for (int i = 0; i < clips.Count; i++)
            {
                AnimationClip clip = clips[i];
                //子状态
                if (clip.name.Contains("_"))
                    continue;
                if (clipDict.ContainsKey(clip.name))
                {
                    Debug.LogError("动画Clip重复或重名：" + clip.name);
                    continue;
                }
                clipDict.Add(clip.name, new List<AnimationClip>());
                clipDict[clip.name].Add(clip);

                //找到子
                IEnumerable<AnimationClip> childClips = clips.Where(x => x.name.Contains(clip.name) && !x.name.Equals(clip.name) && x.name.Contains("_"));
                if (childClips != null)
                {
                    AnimationClip[] tempClip = new AnimationClip[10];
                    foreach (var item in childClips)
                    {
                        AnimationClip childClip = item;
                        int index = 0;
                        if (int.TryParse(childClip.name.Replace(clip.name + "_", ""), out index))
                        {
                            tempClip[index] = childClip;
                        }
                    }
                    for (int j = 0; j < tempClip.Length; j++)
                    {
                        if (tempClip[j] != null)
                        {
                            clipDict[clip.name].Add(tempClip[j]);
                        }
                    }
                }
            }

            return clipDict;
        }

        private static void CreateAnimatorControllerParameter(AnimatorController animatorController, Dictionary<string, List<AnimationClip>> groupDict)
        {
            animatorController.parameters = null;
            foreach (var item in groupDict)
            {
                bool isState = CheckIsStateClip(item.Key);
                if (isState && item.Key.Equals(EntryStateName))
                {
                    animatorController.AddParameter(item.Key, AnimatorControllerParameterType.Bool);
                }
                else
                {
                    animatorController.AddParameter(item.Key, AnimatorControllerParameterType.Trigger);
                }
                if (isState && !item.Key.Equals(EntryStateName))
                {
                    animatorController.AddParameter(item.Key + "_state", AnimatorControllerParameterType.Bool);
                }
            }
        }

        private static (AnimatorState, AnimatorState) CreateGroupState(AnimatorStateMachine stateMachine, string groupName, List<AnimationClip> groupClips, Vector2 startPos, bool isBoolState, AnimatorState entryState = null)
        {
            AnimatorState startState = null;
            AnimatorState endState = null;
            List<AnimatorState> groupStates = new List<AnimatorState>();

            bool isLoop = isBoolState || groupName.Contains(EntryStateName);

            if (groupClips.Count == 1)
            {
                Vector2 statePos = new Vector2(startPos.x + 300, startPos.y);
                AnimationClip clip = groupClips[0];
                AnimatorState newState = stateMachine.AddState(clip.name, statePos);
                newState.motion = clip;

                //临时代码
                if (isLoop)
                {
                    string transName = groupName == EntryStateName ? groupName : groupName + "_state";
                    if (groupName == EntryStateName)
                    {
                        transName = groupName;
                    }
                    else
                    {
                        transName = groupName + "_state";
                    }

                    if (transName.Contains("_state"))
                    {
                        AnimatorStateTransition stateTransition = ConnectAnyStateByTrigger(stateMachine, newState, groupName);
                        stateTransition.AddCondition(AnimatorConditionMode.If, 0, transName);
                        ConnectIdleStateByBool(newState, entryState, transName);
                    }
                    else
                    {
                        ConnectAnyState(stateMachine, newState, transName);
                        ConnectIdleStateByBool(newState, entryState, transName);
                    }
                }
                else
                {
                    ConnectAnyStateByTrigger(stateMachine, newState, groupName);
                    ConnectIdleState(newState, entryState);
                }

                startState = newState;
            }
            else
            {
                for (int i = 0; i < groupClips.Count; i++)
                {
                    AnimationClip clip = groupClips[i];
                    if (clip == null)
                    {
                        continue;
                    }
                    Vector2 statePos = new Vector2(startPos.x + 300 * i, startPos.y);
                    AnimatorState newState = stateMachine.AddState(clip.name, statePos);
                    newState.motion = clip;
                    groupStates.Add(newState);

                    //与上一个相连
                    int lastIndex = i - 1;
                    if (lastIndex >= 0 && lastIndex < groupClips.Count - 1)
                    {
                        AnimatorState lastState = groupStates[i - 1];

                        //Tigger最后一个
                        if (!isBoolState && i == groupClips.Count - 1)
                        {
                            //ConnectOtherStateByBool(lastState, newState, groupName + "_state");
                            ConnectOtherState(lastState, newState);
                        }
                        else
                        {
                            ConnectOtherState(lastState, newState);
                        }

                        //Debug.LogError($" aaaaaaaa  {isBoolState} {lastIndex} {groupClips.Count - 1}");
                    }

                    //起始状态---连Any
                    if (i == 0)
                    {
                        if (isBoolState)
                        {
                            ConnectAnyState(stateMachine, newState, groupName);
                        }
                        else
                        {
                            ConnectAnyStateByTrigger(stateMachine, newState, groupName);
                        }
                        startState = newState;
                    }

                    //结束状态---连Idle
                    if (i == groupClips.Count - 1)
                    {
                        if (isBoolState)
                        {
                            ConnectIdleStateByBool(newState, entryState, groupName);
                        }
                        else
                        {
                            ConnectIdleState(newState, entryState);
                        }
                    }

                    //Debug.LogError($"ccc {i} {groupClips.Count - 1} {entryState}");
                }
            }

            //设置下循环
            if (groupClips.Count >= 3)
            {
                for (int i = 0; i < groupClips.Count; i++)
                {
                    AnimationClip clip = groupClips[i];
                    SerializedObject serializedClip = new SerializedObject(clip);
                    AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);

                    //默认倒数第二个循环
                    if (i == groupClips.Count - 2)
                    {
                        clipSettings.loopTime = true;
                    }
                    else
                    {
                        clipSettings.loopTime = false;
                    }
                    AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
                    serializedClip.ApplyModifiedProperties();
                    EditorUtility.SetDirty(clip);
                }
            }

            //非常之临时
            if (groupClips.Count == 1 && isLoop)
            {
                AnimationClip clip = groupClips[0];
                SerializedObject serializedClip = new SerializedObject(clip);
                AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);

                //默认倒数第二个循环
                clipSettings.loopTime = true;
                AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
                serializedClip.ApplyModifiedProperties();
                EditorUtility.SetDirty(clip);
            }

            return (startState, endState);
        }

        private static void ConnectAnyState(AnimatorStateMachine stateMachine, AnimatorState newState, string condName)
        {
            AnimatorStateTransition _animatorStateTransition = stateMachine.AddAnyStateTransition(newState);
            _animatorStateTransition.AddCondition(AnimatorConditionMode.If, 0, condName);
            _animatorStateTransition.hasExitTime = false;
            _animatorStateTransition.canTransitionToSelf = false;
            _animatorStateTransition.offset = 0;
            _animatorStateTransition.duration = 0;
        }

        private static AnimatorStateTransition ConnectAnyStateByTrigger(AnimatorStateMachine stateMachine, AnimatorState newState, string condName)
        {
            AnimatorStateTransition _animatorStateTransition = stateMachine.AddAnyStateTransition(newState);
            _animatorStateTransition.AddCondition(AnimatorConditionMode.If, 0, condName);
            _animatorStateTransition.hasExitTime = false;
            _animatorStateTransition.canTransitionToSelf = true;
            _animatorStateTransition.offset = 0;
            _animatorStateTransition.duration = 0;
            return _animatorStateTransition;
        }

        private static void ConnectOtherState(AnimatorState state, AnimatorState toConnectState)
        {
            if (state == null || toConnectState == null)
                return;
            AnimatorStateTransition _animatorStateTransition = state.AddTransition(toConnectState);
            _animatorStateTransition.hasExitTime = true;
            _animatorStateTransition.offset = 0;
            _animatorStateTransition.duration = 0;
        }

        private static void ConnectOtherStateByBool(AnimatorState state, AnimatorState toConnectState, string condName)
        {
            if (state == null || toConnectState == null)
                return;
            AnimatorStateTransition _animatorStateTransition = state.AddTransition(toConnectState);
            _animatorStateTransition.AddCondition(AnimatorConditionMode.IfNot, 0, condName);
            _animatorStateTransition.hasExitTime = true;
            _animatorStateTransition.offset = 0;
            _animatorStateTransition.duration = 0;
        }

        private static void ConnectIdleStateByBool(AnimatorState newState, AnimatorState entryState, string condName)
        {
            if (newState == null || entryState == null)
                return;
            AnimatorStateTransition _animatorStateTransition = newState.AddTransition(entryState);
            if (newState.name != EntryStateName)
            {
                _animatorStateTransition.AddCondition(AnimatorConditionMode.IfNot, 0, condName);
            }
            _animatorStateTransition.hasExitTime = true;
            _animatorStateTransition.offset = 0;
            _animatorStateTransition.duration = 0;
        }

        private static void ConnectIdleState(AnimatorState newState, AnimatorState entryState)
        {
            if (newState == null || entryState == null)
                return;
            AnimatorStateTransition _animatorStateTransition = newState.AddTransition(entryState);
            _animatorStateTransition.hasExitTime = true;
            _animatorStateTransition.offset = 0;
            _animatorStateTransition.duration = 0;
        }
    }
}