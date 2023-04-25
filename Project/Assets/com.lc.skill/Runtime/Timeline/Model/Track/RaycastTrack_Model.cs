using System.Collections.Generic;
using Demo;
using LCGAS;
using LCMap;
using LCSkill.Timeline;
using LCToolkit;
using UnityEngine;

namespace LCSkill.Common
{
    public class RaycastTrackGroup : BaseTrackGroup
    {
        
    }

    public class RaycastTrack : BaseTrack
    {
        
    }

    public class RaycastClipEventData
    {
        /// <summary>
        /// 添加的GE
        /// </summary>
        public List<GameplayEffectName> addGE = new List<GameplayEffectName>();

        /// <summary>
        /// 删除的GE
        /// </summary>
        public List<GameplayEffectName> removeGE = new List<GameplayEffectName>();
    }
    
    public class RaycastClip : BaseClip
    {
        class RaycastClipContext : TimelineContext
        {
            //当前区域
            public Shape currShape;
            
            //在区域内的演员
            public List<Actor> actorInRange = new List<Actor>();
        }
        
        /// <summary>
        /// 射线检测的标签
        /// </summary>
        public GameplayTagContainer tag = new GameplayTagContainer();
        
        /// <summary>
        /// 射线的形状
        /// </summary>
        public Shape areaShape = new Shape();
        
        /// <summary>
        /// 射线的位置，相对位置
        /// </summary>
        public Vector2 areaPos = Vector2.zero;

        /// <summary>
        /// 是否跟随
        /// </summary>
        public bool follow = true;
        
        /// <summary>
        /// 检测自身
        /// </summary>
        public bool checkSelf = false;

        /// <summary>
        /// 演员进入
        /// </summary>
        public RaycastClipEventData onActorEnter = new RaycastClipEventData();

        /// <summary>
        /// 演员离开
        /// </summary>
        public RaycastClipEventData onActorExit = new RaycastClipEventData();

        private void UpdateShape(RaycastClipContext pContext, SkillTimelineSpec pSpec)
        {
            if (pSpec.OwnerActor.Roate.y != 0)
                pContext.currShape.FlipX();
            pContext.currShape.Translate(pSpec.OwnerActor.Pos.ToVector2() + areaPos);
        }

        private List<Actor> RaycastActors(Shape pShape,SkillTimelineSpec pSpec)
        {
            List<Actor> actors = new List<Actor>();

            //先检测玩家
            Actor playerActor = MapLocate.Map.PlayerActor;
            if (playerActor.GetTag().HasAny(tag))
            {
                Shape playerBody = EntityGetter.GetEntityColliderShape(playerActor);
                if (pShape.Intersects(playerBody))
                {
                    if (playerActor.Equals(pSpec.OwnerActor))
                    {
                        if (checkSelf)
                        {
                            actors.Add(playerActor);
                        }
                    }
                    else
                    {
                        actors.Add(playerActor);
                    }
                }
            }
            
            
            foreach (Actor actor in pSpec.OwnerActor.CurrArea.InAreaAtors.Values)
            {
                if (actor.GetTag().HasAny(tag))
                {
                    Shape tBody = EntityGetter.GetEntityColliderShape(actor);
                    if (pShape.Intersects(tBody))
                    {
                        if (actor.Equals(pSpec.OwnerActor))
                        {
                            if (checkSelf)
                            {
                                actors.Add(actor);
                            }
                        }
                        else
                        {
                            actors.Add(actor);
                        }
                    }
                }
            }
            
            return actors;
        }

        private void ExecuteActorEventData(Actor pActor, RaycastClipEventData pEventData,SkillTimelineSpec pSpec)
        {
            foreach (GameplayEffectName addGe in pEventData.addGE)
            {
                pActor.Ability.AddGameplayEffect(addGe, pSpec.Owner.OwnerCom);
            }
            
            foreach (GameplayEffectName removeGe in pEventData.removeGE)
            {
                pActor.Ability.RemoveGameplayEffect(removeGe);
            }
        }

        public override void OnEnter(SkillTimelineSpec pSpec)
        {
            RaycastClipContext tContext = GetContext<RaycastClipContext>(pSpec);
            tContext.currShape = areaShape;
            UpdateShape(tContext, pSpec);
        }

        public override void OnStay(SkillTimelineSpec pSpec)
        {
            RaycastClipContext tContext = GetContext<RaycastClipContext>(pSpec);

            //跟随就刷新
            if (follow)
                UpdateShape(tContext, pSpec);

            List<Actor> newInActors = RaycastActors(tContext.currShape, pSpec);
            
            //检测进入
            foreach (Actor newInActor in newInActors)
            {
                //不在就进入
                if (!tContext.actorInRange.Contains(newInActor))
                {
                    ExecuteActorEventData(newInActor, onActorEnter,pSpec);
                }
            }
            
            //检测离开
            foreach (Actor oldInActor in tContext.actorInRange)
            {
                //不在就离开
                if (!newInActors.Contains(oldInActor))
                {
                    ExecuteActorEventData(oldInActor, onActorExit,pSpec);
                }
            }

            tContext.actorInRange = newInActors;
        }
    }
}