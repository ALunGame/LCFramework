using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace LC2DAnim
{
    public class LCCreate2DAnimWizard : ScriptableWizard
    {
        public static string SelGUID          = "";

        //默认的状态
        private string IdleStateName           = "Idle";
        private string DamageStateName         = "Damage";
        
        private List<string> DefaultStateNames = new List<string>(){"Run","Damage","Dead","JumpUp","JumpDown","Dash","Climb"};
        private List<string> LoopStateNames    = new List<string>(){"Idle","Run","Climb"};
        
        private Sprite DefaultSprite        = null;
        [Header("关键帧间隔时间 (秒)")]
        public float KeyframeTime           = 0.15f;
        [Header("预制体尺寸")]
        public float PrefabScale            = 2.5f;
        public static void Open(string selGUID)
        {
            SelGUID = selGUID;
            LCCreate2DAnimWizard wizard = ScriptableWizard.DisplayWizard<LCCreate2DAnimWizard>("创建2DAnim");
            wizard.minSize = new Vector2(300, 250);        
        }

        void OnWizardCreate()
        {
            CreateAnim();
        }

        private void CreateAnim()
        {
            DefaultSprite = null;
            //创建Anim目录
            string selectFloder = AssetDatabase.GUIDToAssetPath(SelGUID);
            DirectoryInfo selectDirInfo = new DirectoryInfo(selectFloder);
            if (!Directory.Exists(selectFloder + "/Anim"))
                Directory.CreateDirectory(selectFloder + "/Anim");

            //查找Sprite目录
            string spriteFloder = "";
            string[] childFloder = AssetDatabase.GetSubFolders(selectFloder);
            for (int i = 0; i < childFloder.Length; i++)
            {
                DirectoryInfo temDirInfo = new DirectoryInfo(childFloder[i]);
                if (temDirInfo.Name=="Sprite")
                {
                    spriteFloder = childFloder[i];
                }
            }
            if (string.IsNullOrEmpty(spriteFloder))
                return;

            //一个目录就是一个动画
            List<AnimationClip> clips = new List<AnimationClip>();
            string[] checkAnimFloder = AssetDatabase.GetSubFolders(spriteFloder);
            for (int i = 0; i < checkAnimFloder.Length; i++)
            {
                DirectoryInfo childDirInfo = new DirectoryInfo(checkAnimFloder[i]);
                string[] textureIds = AssetDatabase.FindAssets("t:Texture", new[] { checkAnimFloder[i] });
                if (textureIds.Length<=0)
                {
                    continue;
                }

                List<string> imagePath = new List<string>();
                //1，设置格式
                for (int j = 0; j < textureIds.Length; j++)
                {
                    string assetPath = AssetDatabase.GUIDToAssetPath(textureIds[j]);
                    imagePath.Add(assetPath);
                    TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                    importer.textureType = TextureImporterType.Sprite;
                    importer.mipmapEnabled = false;
                    importer.filterMode = FilterMode.Point;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    importer.SaveAndReimport();
                }

                //2，生成Clip
                clips.Add(CreateAnimClip(childDirInfo.Name, imagePath, selectFloder + "/Anim"));
            }
            if (clips.Count<=0)
                return;
            
            //创建动画控制器
            AnimatorController animatorController = CreateAnimatorController(selectDirInfo.Name, clips, selectFloder + "/Anim");
            
            //创建预制体
            if (!Directory.Exists(selectFloder + "/Prefab"))
                Directory.CreateDirectory(selectFloder + "/Prefab");
            CreatePrefab(selectDirInfo.Name,animatorController,selectFloder + "/Prefab");
        }

        //创建动画片段
        private AnimationClip CreateAnimClip(string animName,List<string> images,string createPath)
        {
            AnimationClip clip = new AnimationClip();
            //AnimationUtility.SetAnimationType(clip, ModelImporterAnimationType.Generic);

            EditorCurveBinding curveBinding = new EditorCurveBinding();
            curveBinding.type = typeof(SpriteRenderer);
            curveBinding.path = "";
            curveBinding.propertyName = "m_Sprite";

            ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[images.Count];
            for (int i = 0; i < images.Count; i++)
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(images[i]);
                if (animName.IndexOf("Idle")>=0 && DefaultSprite==null)
                {
                    DefaultSprite = sprite;
                }
                keyFrames[i] = new ObjectReferenceKeyframe();
                keyFrames[i].time = KeyframeTime * i;
                keyFrames[i].value = sprite;
            }
            //动画帧率，30比较合适
            clip.frameRate = 30;

            //循环动画
            if (LoopStateNames.Contains(animName))
            {
                SerializedObject serializedClip = new SerializedObject(clip);
                AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
                clipSettings.loopTime = true;
                AnimationUtility.SetAnimationClipSettings(clip, clipSettings);

                serializedClip.ApplyModifiedProperties();
            }

            AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
            AssetDatabase.CreateAsset(clip, createPath + "/" + animName + ".anim");
            AssetDatabase.SaveAssets();
            return clip;
        }

        //创建动画控制器
        private AnimatorController CreateAnimatorController(string animControlName, List<AnimationClip> clips,string createPath)
        {
            AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(createPath + "/" + animControlName + ".controller");

            //默认参数
            CreateAnimatorControllerParameter(animatorController,clips);

            AnimatorControllerLayer layer = animatorController.layers[0];
            AnimatorStateMachine sm = layer.stateMachine;
            sm.anyStatePosition   = new Vector2(sm.entryPosition.x + 600, sm.entryPosition.y - 200);

            CreateAllState(sm, clips);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return animatorController;
        }

        private void CreateAnimatorControllerParameter(AnimatorController animatorController,List<AnimationClip> clips)
        {
            //默认参数
            animatorController.AddParameter(IdleStateName, AnimatorControllerParameterType.Bool);
            for (int i = 0; i < DefaultStateNames.Count; i++)
            {
                if (DefaultStateNames[i].Contains(DamageStateName))
                {
                    animatorController.AddParameter(DefaultStateNames[i], AnimatorControllerParameterType.Trigger);
                }
                else
                {
                    animatorController.AddParameter(DefaultStateNames[i], AnimatorControllerParameterType.Bool);
                }
            }
        }

        private void CreateAllState(AnimatorStateMachine stateMachine, List<AnimationClip> clips)
        {
            //首先创建Idle
            AnimatorState idleState = CreateDefaultState(stateMachine, clips, IdleStateName);
            //创建其余的默认状态
            for (int i = 0; i < DefaultStateNames.Count; i++)
            {
                int stateIndex = i+1;
                if (stateIndex >= DefaultStateNames.Count/2)
                {
                    stateIndex = DefaultStateNames.Count/2 - stateIndex-1;
                }
                
                CreateDefaultState(stateMachine, clips, DefaultStateNames[i], idleState, stateIndex);
            }
            //创建其他状态
            int index = 1;
            for (int i = 0; i < clips.Count; i++)
            {
                AnimationClip clip = clips[i];
                if (!DefaultStateNames.Contains(clip.name) && clip.name != IdleStateName)
                {
                    CreateOtherState(stateMachine, clips, clip.name, idleState, index);
                    index++;
                }
            }
        }

        //创建默认状态
        private AnimatorState CreateDefaultState(AnimatorStateMachine stateMachine, List<AnimationClip> clips, string stateName,AnimatorState idleState=null,int stateIndex=0)
        {
            float posX = stateIndex == 0 ? stateMachine.entryPosition.x       : stateMachine.entryPosition.x + 300;
            float posY = stateIndex == 0 ? stateMachine.entryPosition.y - 200 : stateMachine.entryPosition.y - 200 + 50*stateIndex;
            Vector2 statePos = new Vector2(posX,posY);
            AnimatorState defaultState = stateMachine.AddState(stateName,statePos);
            foreach (var item in clips)
            {
                if (item.name.Contains(stateName))
                {
                    defaultState.motion = item;
                    break;
                }
            }
            
            AnimatorStateTransition _animatorStateTransition = stateMachine.AddAnyStateTransition(defaultState);
            _animatorStateTransition.AddCondition(AnimatorConditionMode.If, 0, stateName);
            _animatorStateTransition.hasExitTime = false;
            _animatorStateTransition.canTransitionToSelf = false;
            _animatorStateTransition.offset = 0;
            _animatorStateTransition.duration = 0;
            
            //都要连Idle
            if (idleState==null)
                return defaultState;
            _animatorStateTransition = defaultState.AddTransition(idleState);
            if (!stateName.Contains(DamageStateName))
            {
                _animatorStateTransition.AddCondition(AnimatorConditionMode.IfNot, 0, stateName);
            }
            _animatorStateTransition.hasExitTime = true;
            return defaultState;
        }

        private AnimatorState CreateOtherState(AnimatorStateMachine stateMachine, List<AnimationClip> clips,
            string stateName, AnimatorState idleState, int stateIndex = 0)
        {
            float posX = stateMachine.entryPosition.x + 300;
            float posY = stateIndex == 0 ? stateMachine.entryPosition.y : stateMachine.entryPosition.y + 50 * stateIndex;
            Vector2 statePos = new Vector2(posX,posY);
            AnimatorState otherState = stateMachine.AddState(stateName,statePos);
            foreach (var item in clips)
            {
                if (item.name.Contains(stateName))
                {
                    otherState.motion = item;
                    break;
                }
            }
            
            //都要连Idle
            AnimatorStateTransition transition = otherState.AddTransition(idleState);
            transition.hasExitTime = true;
            return otherState;
        }

        //创建预制体
        private void CreatePrefab(string name, AnimatorController animControl, string createPath)
        {
            string prefabPath = createPath + "/" + name + ".prefab";
            GameObject actor = null;
            if (File.Exists(prefabPath))
            {
                actor = PrefabUtility.LoadPrefabContents(prefabPath);
                if (actor != null)
                {
                    actor.transform.Find("Anim").GetComponent<Animator>().runtimeAnimatorController = animControl;
                    PrefabUtility.SaveAsPrefabAsset(actor, prefabPath);
                    return;    
                }
            }
            
            
            
            actor = new GameObject(name);
            actor.transform.position = Vector3.zero;
            actor.transform.localScale = new Vector3(PrefabScale,PrefabScale,0);
            
            GameObject anim = new GameObject("Anim");
            SpriteRenderer spriteRenderer = anim.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = DefaultSprite;
            Animator actorAnimator = anim.AddComponent<Animator>();
            actorAnimator.runtimeAnimatorController = animControl;
            anim.transform.SetParent(actor.transform);
            anim.transform.position = Vector3.zero;
            anim.transform.localScale = Vector3.one;

            GameObject prefabGo = PrefabUtility.SaveAsPrefabAsset(actor, prefabPath);
            GameObject.DestroyImmediate(actor);
            Selection.activeObject = prefabGo;
        }

    }
}
