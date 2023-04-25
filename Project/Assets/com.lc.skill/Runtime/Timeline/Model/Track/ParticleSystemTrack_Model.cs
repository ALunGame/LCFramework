using LCSkill.Timeline;
using LCToolkit;
using UnityEngine;

namespace LCSkill.Common
{
    public class ParticleSystemTrackGroup : BaseTrackGroup
    {
        
    }

    public class ParticleSystemTrack : BaseTrack
    {
        
    }
    
    public class ParticleSystemClip : BaseClip
    {
        class ParticleSystemClipContext : TimelineContext
        {
            public GameObject effectGo;
        }
        
        
        public UnityObjectAsset effetGo = new UnityObjectAsset(UnityObjectAsset.AssetType.GameObj);
        public string followPath = "";
        public Vector3 offsetPos = Vector3.zero;

        public override void OnEnter(SkillTimelineSpec pSpec)
        {
            ParticleSystemClipContext tContext = GetContext<ParticleSystemClipContext>(pSpec);
            tContext.effectGo = ToolkitLocate.GoPool.Take(effetGo.ObjName);
            
            if (!string.IsNullOrEmpty(followPath))
            {
                tContext.effectGo.transform.SetParent(pSpec.OwnerActor.Go.transform.Find(followPath));   
            }

            tContext.effectGo.transform.localPosition = offsetPos;
        }

        public override void OnExit(SkillTimelineSpec pSpec)
        {
            ParticleSystemClipContext tContext = GetContext<ParticleSystemClipContext>(pSpec);
            ToolkitLocate.GoPool.Recycle(tContext.effectGo);
            tContext.effectGo = null;
        }

        public override void OnEnd(SkillTimelineSpec pSpec)
        {
            ParticleSystemClipContext tContext = GetContext<ParticleSystemClipContext>(pSpec);
            ToolkitLocate.GoPool.Recycle(tContext.effectGo);
        }
    }
}