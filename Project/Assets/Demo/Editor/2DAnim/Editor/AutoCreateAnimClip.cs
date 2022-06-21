using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Demo.AutoCreate
{
    public static class AutoCreateAnimClip 
    {
        //关键帧间隔时间
        private const float KeyframeTime = 0.03f;
        private const float FrameRate = 30f;

        public static List<AnimationClip> CreateAnimClips(string spritePath)
        {
            List<AnimationClip> animClips = new List<AnimationClip>();  
            foreach (var animDir in Directory.GetDirectories(spritePath))
            {
                string animName = Path.GetFileName(animDir).Trim().ToLower();
                string[] childDir = Directory.GetDirectories(animDir);
                if (childDir == null || childDir.Length <= 0)
                {
                    string animClipName = $"{animName}";
                    AnimationClip clip = CreateAnimClip(animDir);
                    clip.name = animClipName;
                    animClips.Add(clip);
                }
                else
                {
                    foreach (var childAnimDir in childDir)
                    {
                        string childAnimName = Path.GetFileName(childAnimDir).Trim().ToLower();
                        string animClipName = $"{animName}_{childAnimName}";
                        AnimationClip clip = CreateAnimClip(childAnimDir);
                        clip.name = animClipName;
                        animClips.Add(clip);
                    }
                }
            }
            return animClips;
        }

        private static AnimationClip CreateAnimClip(string animSpritePath)
        {
            List<string> imagePath = new List<string>();
            foreach (var item in Directory.GetFiles(animSpritePath, "*.png"))
            {
                FileInfo fileInfo = new FileInfo(item);
                string assetPath  = fileInfo.FullName.Substring(fileInfo.FullName.IndexOf("Assets"));
                TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                importer.textureType = TextureImporterType.Sprite;
                importer.mipmapEnabled = false;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.SaveAndReimport();
                imagePath.Add(assetPath);
            }

            AnimationClip clip = new AnimationClip();

            EditorCurveBinding curveBinding = new EditorCurveBinding();
            curveBinding.type = typeof(SpriteRenderer);
            curveBinding.path = "";
            curveBinding.propertyName = "m_Sprite";

            ObjectReferenceKeyframe[] keyFrames = new ObjectReferenceKeyframe[imagePath.Count];
            for (int i = 0; i < imagePath.Count; i++)
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(imagePath[i]);
                keyFrames[i] = new ObjectReferenceKeyframe();
                keyFrames[i].time  = KeyframeTime * i;
                keyFrames[i].value = sprite;
            }
            //动画帧率，30比较合适
            clip.frameRate = FrameRate;

            AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyFrames);
            return clip;
        }
    }
}