using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Demo.AutoCreate
{
    public static class AutoCreateAnimController
    {
        private static List<string> LoopStateNames = new List<string>() { "idle", "run", "walk" };

        public static List<string> selPaths = new List<string>();

        [MenuItem("Assets/创建动画片段和状态机")]
        public static void CreateAnimControllers()
        {
            foreach (var item in selPaths)
            {
                CreateAnimController(item);
            }
        }

        [MenuItem("Assets/创建动画片段和状态机",true)]
        public static bool CreateAnimControllers_True()
        {
            Object[] guidArray = Selection.objects;
            selPaths.Clear();
            foreach (var uid in guidArray)
            {
                string foldPath = AssetDatabase.GetAssetPath(uid);
                if (Directory.Exists(Path.Combine(foldPath, "Sprite")))
                {
                    selPaths.Add(foldPath);
                }
            }
            return selPaths.Count > 0;
        }

        public static void CreateAnimController(string selPath)
        {
            string spritePath = Path.Combine(selPath, "Sprite");
            List<AnimationClip> animClips = AutoCreateAnimClip.CreateAnimClips(spritePath);
            if (animClips.Count == 0)
                return;
            
            if (!Directory.Exists(selPath + "/Anim"))
            {
                Directory.CreateDirectory(selPath + "/Anim");
            }

            string controllerName = Path.GetFileName(selPath).Trim().ToLower();
            string controllerPath = selPath + "/Anim/" + controllerName + "_controller" + ".controller";

            AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);

            string animClipTmpPath = selPath + "/Temp";
            Directory.CreateDirectory(animClipTmpPath);

            foreach (var clip in animClips)
            {
                if (LoopStateNames.Contains(clip.name))
                {
                    SerializedObject serializedClip = new SerializedObject(clip);
                    AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
                    clipSettings.loopTime = true;
                    AnimationUtility.SetAnimationClipSettings(clip, clipSettings);

                    serializedClip.ApplyModifiedProperties();
                }
                AssetDatabase.AddObjectToAsset(clip, controllerPath);
                EditorUtility.SetDirty(clip);
            }

            EditorUtility.SetDirty(animatorController);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}