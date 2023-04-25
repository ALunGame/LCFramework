using Demo.Com;
using LCECS;
using LCECS.Core;
using LCSkill.Timeline;
using LCToolkit;

namespace LCSkill.Common
{
    public class AnimTrackGroup : BaseTrackGroup
    {
        
    }

    public class AnimTrack : BaseTrack
    {
        
    }
    
    public class AnimClip : BaseClip
    {
        public UnityObjectAsset clip = new UnityObjectAsset(UnityObjectAsset.AssetType.AnimClip);

        public override void OnEnter(SkillTimelineSpec pSpec)
        {
            if (pSpec.Owner.IsNull())
                return;
            Entity entity = ECSLocate.ECS.GetEntity(pSpec.Owner.OwnerCom.EntityUid);
            if (entity == null)
            {
                return;
            }
            AnimCom animCom = entity.GetCom<AnimCom>();
            animCom.SetReqAnim(clip.ObjName);
        }

        public override void OnExit(SkillTimelineSpec pSpec)
        {
            if (pSpec.Owner.IsNull())
                return;
            Entity entity = ECSLocate.ECS.GetEntity(pSpec.Owner.OwnerCom.EntityUid);
            if (entity == null)
            {
                return;
            }
            AnimCom animCom = entity.GetCom<AnimCom>();
            animCom.SetDefaultAnim();
        }
    }
}