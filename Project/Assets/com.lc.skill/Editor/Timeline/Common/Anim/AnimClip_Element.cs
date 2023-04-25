using System.Collections.Generic;
using LCSkill.Timeline;
using LCToolkit;
using LCToolkit.Element;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Common
{
    [TimlineClipElement(typeof(AnimClip))]
    public class AnimClip_Element : Clip_Element<AnimClip>
    {
        protected override void OnInit()
        {
            Model.name = Model.clip.ObjName;

            RootElement.CreateGUIFunc(OnGui);
        }

        public override void OnClear()
        {
        }

        private void OnGui()
        {
            AnimationClip animationClip = Model.clip.Obj as AnimationClip;
            if (animationClip == null)
            {
                return;
            }
            
            Vector3 clipPos = RootElement.transform.position;
            int clipFrame = window.CalcFrameByTime(animationClip.length);
            int clipCnt = Model.DurationFrame / clipFrame;
            for (int i = 0; i < clipCnt; i++)
            {
                float posX = (i+1) * clipFrame * Frame_Element.FrameWidth;
                
                Rect boxRect = new Rect(new Vector2(posX, clipPos.y),
                    new Vector2(InternalTrack_Element.TrackHeight, Frame_Element.FrameWidth));
                EditorGUI.DrawRect(boxRect, Color.red);
            }
        }
        
        protected override void OnPlaying(float pPlayTime)
        {
            AnimationMode.StartAnimationMode();
            GameObject preGo = (GameObject)window.PreviewGo;
            AnimationClip preClip = (AnimationClip)Model.clip.Obj;
            if (preGo == null || preClip == null)
            {
                return;
            }
            EditorPlayEffectHelper.PlayAnim(preGo,preClip,pPlayTime);
        }

        protected override void OnExitPlaying()
        {
            AnimationMode.StopAnimationMode();
        }

        public override void OnFocuseWindow()
        {
            Model.name = Model.clip.ObjName;
        }
    }
}