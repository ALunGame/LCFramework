using DG.Tweening;
using LCECS.Core;
using LCMap;
using LCSkill;
using UnityEngine;

namespace Demo.Skill.Buff
{
    /// <summary>
    /// 受伤抖动
    /// </summary>
    public class SkillBuffBeHurtShake : BuffBeHurtFunc
    {
        public float shakeTime;

        public override void Execute(BuffObj buff, ref AddDamageInfo damageInfo, SkillCom attacker)
        {
            SkillCom skillCom = damageInfo.target;
            ActorObj actorObj = LCMap.MapLocate.Map.GetActor(skillCom.EntityUid);

            GameObject displayGo = actorObj.GetDisplayGo();
            displayGo.transform.DOComplete(false);
            displayGo.transform.DOPunchPosition(new Vector3(-0.2f * actorObj.GetDirValue(), 0,0), shakeTime, 1, 0);
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
            Debug.LogWarning("受伤暂停决策>>>>>");

            SkillCom skillCom = damageInfo.target;
            Entity entity = LCECS.ECSLocate.ECS.GetEntity(skillCom.EntityUid);
            entity.PauseEntityDec();
            float timeCount = pauseTime;
            DOTween.To(() => timeCount, a => timeCount = a, 0.1f, pauseTime).OnComplete(new TweenCallback(delegate
            {
                Debug.LogWarning("受伤开启决策>>>>>");
                entity.ResumeEntityDec();
            }));
        }
    }
}
