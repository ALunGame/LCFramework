using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCSkill;
using DG.Tweening;
using LCECS.Core;
using LCMap;
using UnityEngine;

namespace Demo.Skill.Buff
{
    /// <summary>
    /// 受伤抖动
    /// </summary>
    public class SkillBuffBeHurtShake : BuffBeHurtFunc
    {
        public override void Execute(BuffObj buff, ref AddDamageInfo damageInfo, SkillCom attacker)
        {
            SkillCom skillCom = damageInfo.target;
            ActorObj actorObj = LCMap.MapLocate.Map.GetActor(skillCom.EntityId);
            GameObject displayGo = actorObj.GetDisplayGo();
            displayGo.transform.DOShakePosition(0.3f);
        }
    }

    /// <summary>
    /// 受伤暂停决策
    /// </summary>
    public class SkillBuffBeHurtPauseDec : BuffBeHurtFunc
    {
        public float pauseTime;

        public override void Execute(BuffObj buff, ref AddDamageInfo damageInfo, SkillCom attacker)
        {
            SkillCom skillCom = damageInfo.target;
            Entity entity = LCECS.ECSLocate.ECS.GetEntity(skillCom.EntityId);
            entity.PauseEntityDec();
            float timeCount = pauseTime;
            DOTween.To(() => timeCount, a => timeCount = a, 0.1f, pauseTime).OnComplete(new TweenCallback(delegate
            {
                entity.ResumeEntityDec();
            }));
        }
    }
}
