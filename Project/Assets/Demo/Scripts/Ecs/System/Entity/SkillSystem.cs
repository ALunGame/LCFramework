using Demo.Com;
using Demo.Info;
using LCECS;
using LCECS.Core.ECS;
using LCHelp;
using LCSkill;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Demo.System
{
    //技能系统
    public class SkillSystem : BaseSystem
    {
        private SkillSensor SkillHelp;

        protected override List<Type> RegListenComs()
        {
            SkillHelp = ECSLayerLocate.Info.GetSensor<SkillSensor>(SensorType.Skill);
            return new List<Type>() { typeof(SkillCom), typeof(GameObjectCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            HandleSkill(GetCom<SkillCom>(comList[0]), GetCom<GameObjectCom>(comList[1]));
        }

        protected override void OnDrawEntityGizmos(int entityId, object data)
        {
            EDGizmos.DrawRect((Rect)data, Color.cyan);
        }

        //判断是否可以释放技能
        private bool CheckCanUseSkill(SkillCom skillCom)
        {
            if (skillCom.ReqSkillId <= 0)
            {
                return false;
            }

            //只要有不可打断的表现
            foreach (var item in skillCom.CurrSkillDict.Values)
            {
                if (item.CurImpact != null)
                {
                    if (item.CurImpact.CanBreak==false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private SkillInfo CreateSkillInfo(SkillCom skillCom)
        {
            //释放相同的技能
            if (skillCom.CurrSkillDict.ContainsKey(skillCom.ReqSkillId))
            {
                SkillInfo tmpInfo = skillCom.CurrSkillDict[skillCom.ReqSkillId];
                tmpInfo.SkillLine.ReSet();
            }

            //找到所有可以退出的表现
            List<SkillInfo> canBreakSkill = new List<SkillInfo>();
            foreach (var item in skillCom.CurrSkillDict.Values)
            {
                if (item.CurImpact != null)
                {
                    if (item.CurImpact.CanBreak && item.CurImpact.BreakExit)
                    {
                        canBreakSkill.Add(item);
                    }
                }
            }

            //退出一切可退出的
            for (int i = 0; i < canBreakSkill.Count; i++)
            {
                SkillImpact impact = canBreakSkill[i].CurImpact;
                if (impact.BreakExit)
                {
                    //重置时间线
                    canBreakSkill[i].SkillLine.ReSet();
                }
            }

            //创建一个技能
            SkillInfo skillInfo = new SkillInfo();
            skillInfo.SkillId = skillCom.ReqSkillId;
            skillInfo.SkillLine = TimeLineHelp.CreateTimeLine();
            TimeLineHelp.AddTimeLine(skillInfo.SkillLine);

            return skillInfo;
        }

        private void HandleSkill(SkillCom skillCom, GameObjectCom goCom)
        {
            //检测是否可以释放技能
            if (!CheckCanUseSkill(skillCom))
            {
                skillCom.ReqSkillId = 0;
                return;
            }

            //配置检查
            SkillJson json = LCSkillLocate.GetSkillInfo(skillCom.ReqSkillId);
            if (json.ImpactList.Count<=0)
            {
                skillCom.ReqSkillId = 0;
                return;
            }
            
            //创建技能信息
            SkillInfo skillInfo = CreateSkillInfo(skillCom);
            if (skillInfo==null)
            {
                skillCom.ReqSkillId = 0;
                return;
            }

            //添加进技能池
            TimeLine timeLine = skillInfo.SkillLine;
            skillCom.CurrSkillDict.Add(skillInfo.SkillId,skillInfo);

            //播放技能表现
            for (int i = 0; i < json.ImpactList.Count; i++)
            {
                SkillImpact impact = json.ImpactList[i];
                timeLine.AddTrack((float)impact.Time, 0).OnStart(()=> {
                    skillInfo.CurImpact = impact;
                });
                PlaySkillImpact(goCom,skillCom, timeLine, impact);
            }
            
            //技能结束后的清理
            timeLine.AddTrack((float)json.ContinueTime, 0).OnCompleted(() =>
            {
                skillCom.CurShowSkillId = 0;
                skillCom.LastShowSkillId = skillInfo.SkillId;
                skillCom.CurrSkillDict.Remove(skillInfo.SkillId);
            }).OnClear(() =>
            {
                //可能正常停止
                if (skillInfo!=null)
                {
                    skillCom.CurShowSkillId  = 0;
                    skillCom.LastShowSkillId = skillInfo.SkillId;
                    skillCom.CurrSkillDict.Remove(skillInfo.SkillId);
                }
            });

            //开始播放技能
            timeLine.Start();
            skillCom.CurShowSkillId = skillCom.ReqSkillId;
            skillCom.ReqSkillId = 0;
        }

        private void PlaySkillImpact(GameObjectCom gameobjectCom,SkillCom skillCom,TimeLine skillLine, SkillImpact impact)
        {
            if (impact.SelfInfo!=null)
            {
                HandleSelfSkillImpactInfo(gameobjectCom, skillCom, skillLine, impact, impact.SelfInfo);
            }
            if (impact.OtherInfo != null)
            {
                HandleOtherSkillImpactInfo(gameobjectCom, skillCom, skillLine, impact, impact.OtherInfo);
            }
        }

        #region 自身的效果
        //自身的效果肯定作用自身
        private void HandleSelfSkillImpactInfo(GameObjectCom gameobjectCom, SkillCom skillCom, TimeLine skillLine, SkillImpact impact, SkillSelfImpactInfo info)
        {
            Entity selfEntity = ECSLocate.ECS.GetEntity(skillCom.EntityId);
            PlaySelfImpact(gameobjectCom, skillCom, skillLine, impact, selfEntity);
        }

        private void PlaySelfImpact(GameObjectCom gameobjectCom, SkillCom skillCom, TimeLine skillLine, SkillImpact impact, Entity selfEntity)
        {
            float stopTime = (float)impact.SelfInfo.StopTime;
            SkillAnim animInfo = impact.SelfInfo.Anim;
            if (animInfo != null)
            {
                AnimCom animCom = selfEntity.GetCom<AnimCom>();
                float animClipTime = LCUtil.GetAnimClipTime(animCom.Animtor, animInfo.AnimName);
                skillLine.AddTrack((float)impact.Time + (float)animInfo.Time, animClipTime).OnStart(() =>
                {
                    PlayAnim(skillCom, animInfo.AnimName, selfEntity);
                }).OnCompleted(() =>
                {
                    selfEntity.GetCom<AnimCom>().DoTrigger = false;
                }).OnClear(() =>
                {
                    selfEntity.GetCom<AnimCom>().DoTrigger = false;
                });
            }

            SkillEffect effectInfo = impact.SelfInfo.Effect;
            if (effectInfo != null)
            {
                skillLine.AddTrack((float)impact.Time + (float)effectInfo.Time, 0).OnStart(() =>
                {
                    PlayEffect(skillCom, effectInfo, selfEntity);
                });
            }

            SkillAudio audioInfo = impact.SelfInfo.Audio;
            if (audioInfo != null)
            {
                skillLine.AddTrack((float)impact.Time + (float)audioInfo.Time, 0).OnStart(() =>
                {
                    PlayAudio(skillCom, audioInfo.AudioId);
                });
            }

            if (impact.SelfInfo.Data!=null)
            {
                PlaySelfData(gameobjectCom, skillCom, skillLine, impact, impact.SelfInfo.Data, selfEntity);
            }
        } 
        #endregion

        #region 他人的效果
        //他人的效果肯定作用的对象是实时的
        //需要通过作用数据去驱动
        private void HandleOtherSkillImpactInfo(GameObjectCom goCom, SkillCom skillCom, TimeLine skillLine, SkillImpact impact, SkillOtherImpactInfo info)
        {
            List<Entity> skillUseEntityList = null;
            switch (info.DataType)
            {
                case SkillDataImpactType.Once:
                    skillLine.AddTrack((float)impact.Time + (float)info.Time, 0).OnStart(() =>
                    {
                        skillUseEntityList = SkillHelp.CollectSkillUseEntitys(skillCom, impact, goCom, this);
                        OnPlayOtherImpactStart(skillCom, info, skillUseEntityList);
                    }).OnCompleted(() =>
                    {
                        OnPlayOtherImpactCompleted(skillCom, info, skillUseEntityList, false);
                    }).OnClear(() =>
                    {
                        OnPlayOtherImpactClear(skillCom, info, skillUseEntityList, false);
                    });
                    break;
                case SkillDataImpactType.Gap:
                    int gapCnt = (int)(info.ContinueTime / info.GapTime);
                    for (int i = 0; i < gapCnt; i++)
                    {
                        float delayTime = (float)(impact.Time + info.Time + i * info.GapTime);

                        skillLine.AddTrack(delayTime, 0).OnStart(() =>
                        {
                            skillUseEntityList = SkillHelp.CollectSkillUseEntitys(skillCom, impact, goCom,this);
                            OnPlayOtherImpactStart(skillCom, info, skillUseEntityList);
                        }).OnCompleted(() =>
                        {
                            OnPlayOtherImpactCompleted(skillCom, info, skillUseEntityList, false);
                        }).OnClear(() =>
                        {
                            OnPlayOtherImpactClear(skillCom, info, skillUseEntityList, false);
                        });
                    }
                    break;
                case SkillDataImpactType.Buff:
                    skillLine.AddTrack((float)impact.Time + (float)info.Time, 0).OnStart(() =>
                    {
                        skillUseEntityList = SkillHelp.CollectSkillUseEntitys(skillCom, impact, goCom, this);
                        OnPlayOtherImpactStart(skillCom, info, skillUseEntityList);
                    }).OnCompleted(() =>
                    {
                        OnPlayOtherImpactCompleted(skillCom, info, skillUseEntityList, true);
                    }).OnClear(() =>
                    {
                        OnPlayOtherImpactClear(skillCom, info, skillUseEntityList, true);
                    });
                    break;
                default:
                    break;
            }
        }

        private void OnPlayOtherImpactStart(SkillCom skillCom, SkillOtherImpactInfo info, List<Entity> skillUseEntitys)
        {
            SkillAnim animInfo = info.Anim;
            SkillEffect effectInfo = info.Effect;
            SkillAudio audioInfo = info.Audio;
            SkillOtherData dataInfo = info.Data;
            for (int i = 0; i < skillUseEntitys.Count; i++)
            {
                if (animInfo != null)
                    PlayAnim(skillCom, animInfo.AnimName, skillUseEntitys[i]);
                if (effectInfo != null)
                    PlayEffect(skillCom, effectInfo, skillUseEntitys[i]);
                if (audioInfo != null)
                    PlayAudio(skillCom, audioInfo.AudioId);
                if (dataInfo != null)
                    PlayOtherData(dataInfo, skillUseEntitys[i], false);

                //僵直时间
                AnimCom animCom = skillUseEntitys[i].GetCom<AnimCom>();
                StateCom stateCom = skillUseEntitys[i].GetCom<StateCom>();
                animCom.DoTrigger = true;
                stateCom.CurState = EntityState.Stop;
                DOVirtual.Float(1, 0, (float)info.StopTime,(x)=> { }).OnComplete(()=> {
                    animCom.DoTrigger = false;
                    stateCom.CurState = EntityState.Normal;
                });
            }

        }

        private void OnPlayOtherImpactCompleted(SkillCom skillCom, SkillOtherImpactInfo info, List<Entity> skillUseEntitys, bool reSetData)
        {
            SkillAnim animInfo = info.Anim;
            SkillEffect effectInfo = info.Effect;
            SkillAudio audioInfo = info.Audio;
            SkillOtherData dataInfo = info.Data;
            for (int i = 0; i < skillUseEntitys.Count; i++)
            {
                if (dataInfo != null && reSetData)
                    PlayOtherData(dataInfo, skillUseEntitys[i], reSetData);
            }
        }

        private void OnPlayOtherImpactClear(SkillCom skillCom, SkillOtherImpactInfo info, List<Entity> skillUseEntitys, bool reSetData)
        {
            SkillAnim animInfo = info.Anim;
            SkillEffect effectInfo = info.Effect;
            SkillAudio audioInfo = info.Audio;
            SkillOtherData dataInfo = info.Data;
            for (int i = 0; i < skillUseEntitys.Count; i++)
            {
                if (dataInfo != null && reSetData)
                    PlayOtherData(dataInfo, skillUseEntitys[i], reSetData);
            }
        } 

        //播放技能僵直时间
        private void PlayStopTime(SkillImpact impact,TimeLine skillLine, SkillOtherImpactInfo info, List<Entity> skillUseEntitys)
        {
            for (int i = 0; i < skillUseEntitys.Count; i++)
            {
                //僵直时间
                skillLine.AddTrack((float)impact.Time + (float)info.Time, 0).OnStart(() =>
                {
                    AnimCom animCom = skillUseEntitys[i].GetCom<AnimCom>();
                    animCom.DoTrigger = true;
                }).OnCompleted(() =>
                {
                    AnimCom animCom = skillUseEntitys[i].GetCom<AnimCom>();
                    animCom.DoTrigger = false;
                }).OnClear(() =>
                {
                    AnimCom animCom = skillUseEntitys[i].GetCom<AnimCom>();
                    animCom.DoTrigger = false;
                });
            }
        }

        #endregion

        //动画
        private void PlayAnim(SkillCom skillCom, string animName, Entity entity)
        {
            if (animName == "")
            {
                ECSLocate.ECSLog.LogR("动画 为空>>>>>>> ", skillCom.ReqSkillId, animName);
                return;
            }
            AnimCom animCom = entity.GetCom<AnimCom>();
            animCom.DoTrigger = true;
            animCom.Animtor.Play(animName);
        }

        //特效
        private void PlayEffect(SkillCom skillCom, SkillEffect effect, Entity entity)
        {
            if (effect.EffectId <= 0)
            {
                ECSLocate.ECSLog.LogR("特效Id 为空>>>>>>> ", skillCom.ReqSkillId, effect.EffectId);
                return;
            }
            ECSLocate.ECSLog.LogR("播放特效>>>>>>>>>", effect.EffectId);

            //作用位置
            Vector3 usesPos = (Vector3)LCConvert.StrChangeToObject(effect.Pos, typeof(Vector3).FullName);
            GameObjectCom goCom = entity.GetCom<GameObjectCom>();
            SpriteRenderer sprite = goCom.Go.GetComponent<SpriteRenderer>();

            bool tempDir = false;
            if (sprite != null)
            {
                int dir = sprite.flipX ? -1 : 1;
                usesPos = new Vector3(usesPos.x * dir, usesPos.y, usesPos.z);
                tempDir = sprite.flipX;
            }
            usesPos = new Vector3(usesPos.x + goCom.Tran.position.x, usesPos.y + goCom.Tran.position.y, usesPos.z + goCom.Tran.position.z);

            ECSLocate.ECS.SetGlobalSingleComData((EffectCom com) =>
            {
                com.EffectId = effect.EffectId;
                com.EffectEntityId = entity.GetHashCode();
                com.EffectPos = usesPos;
                com.EffectDir = tempDir;
                com.EffectHideTime = (float)effect.HideTime;
            });
        }

        //音效
        private void PlayAudio(SkillCom skillCom, int audioId)
        {
            if (audioId <= 0)
            {
                ECSLocate.ECSLog.LogR("音效Id 为空>>>>>>> ", skillCom.ReqSkillId, audioId);
                return;
            }

            ECSLocate.ECSLog.Log("播放音效Id ", skillCom.ReqSkillId, audioId);
        }

        //数据
        private void PlaySelfData(GameObjectCom gameobjectCom,SkillCom skillCom,TimeLine skillLine, SkillImpact impact, SkillSelfData info,Entity selfEntity)
        {
            List<Entity> skillUseEntityList = new List<Entity>() { selfEntity };
            switch (info.DataType)
            {
                case SkillDataImpactType.Once:
                    skillLine.AddTrack((float)impact.Time + (float)info.Time, 0).OnStart(() =>
                    {
                        SkillHelp.HandleSkillUseAttrData(skillUseEntityList, info.DataOperates);
                    });
                    break;
                case SkillDataImpactType.Gap:
                    int gapCnt = (int)(info.ContinueTime / info.GapTime);
                    for (int i = 0; i < gapCnt; i++)
                    {
                        float delayTime = (float)(impact.Time + info.Time + i * info.GapTime);
                        skillLine.AddTrack(delayTime, 0).OnStart(() =>
                        {
                            skillUseEntityList = SkillHelp.CollectSkillUseEntitys(skillCom, impact, gameobjectCom, this);
                            SkillHelp.HandleSkillUseAttrData(skillUseEntityList, info.DataOperates);
                        });
                    }
                    break;
                case SkillDataImpactType.Buff:
                    skillLine.AddTrack((float)impact.Time + (float)info.Time, 0).OnStart(() =>
                    {
                        SkillHelp.HandleSkillUseAttrData(skillUseEntityList, info.DataOperates);
                    }).OnCompleted(() =>
                    {
                        SkillHelp.HandleSkillUseAttrData(skillUseEntityList, info.DataOperates,true);
                    }).OnClear(() => {
                        SkillHelp.HandleSkillUseAttrData(skillUseEntityList, info.DataOperates,true);
                    });
                    break;
                default:
                    break;
            }
        }

        private void PlayOtherData(SkillOtherData info, Entity entity,bool reSetData)
        {
            List<Entity> skillUseEntityList = new List<Entity>() { entity };
            SkillHelp.HandleSkillUseAttrData(skillUseEntityList, info.DataOperates, reSetData);
        }
    }
}
