using System.Collections.Generic;
using LCSkill.Timeline;
using LCToolkit;
using LCToolkit.Element;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Common
{
    [TimlineClipElement(typeof(ParticleSystemClip))]
    public class ParticleSystemClip_Element : Clip_Element<ParticleSystemClip>
    {
        private GameObject preViewEffecGo = null;
        
        protected override void OnInit()
        {
            Model.name = Model.effetGo.ObjName;
            RootElement.CreateGUIFunc(OnGui);
        }

        public override void OnClear()
        {
            if (preViewEffecGo != null)
            {
                GameObject.DestroyImmediate(preViewEffecGo);
            }
        }

        private void OnEffectChange()
        {
            Model.name = Model.effetGo.ObjName;
            if (preViewEffecGo != null)
            {
                GameObject.DestroyImmediate(preViewEffecGo);
            }
        }
        
        private void OnGui()
        {
        }
        
        protected override void OnPlaying(float pPlayTime)
        {
            if (preViewEffecGo == null && Model.effetGo.Obj != null)
            {
                preViewEffecGo = GameObject.Instantiate(Model.effetGo.Obj as GameObject);
            }
            
            if (preViewEffecGo != null)
            {
                EditorPlayEffectHelper.PlayParticle(preViewEffecGo,pPlayTime);
            }
        }

        protected override void OnExitPlaying()
        {
        }
        
        private void OnEndFrameChange(int pValue)
        {
        }
    }
}