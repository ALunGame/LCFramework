using LCECS.Core;
using System.Collections.Generic;
using LCMap;
using LCToolkit;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 技能组件
    /// 当前拥有的技能，当前拥有的Buff
    /// </summary>
    public class SkillCom : BaseCom
    {
        private List<SkillObj> skills = new List<SkillObj>();
        /// <summary>
        /// 拥有的技能
        /// </summary>
        public IReadOnlyList<SkillObj> Skills { get => skills;}

        private List<BuffObj> buffs = new List<BuffObj>();
        /// <summary>
        /// 拥有的Buff
        /// </summary>
        public IReadOnlyList<BuffObj> Buffs { get => buffs; }

        private TimelineObj timeline;

        public TimelineObj Timeline
        {
            get
            {
                return timeline;
            }
        }
        
        
        protected override void OnAwake(Entity pEntity)
        {
            if (pEntity is Actor)
            {
                Actor actor = pEntity as Actor;
                ActorCnf actorCnf = LCConfig.Config.ActorCnf[actor.Id];
                if (actorCnf.defaultSkills.IsLegal())
                {
                    for (int i = 0; i < actorCnf.defaultSkills.Count; i++)
                    {
                        int skillId = actorCnf.defaultSkills[i];
                        SkillLocate.Skill.LearnSkill(this, skillId.ToString());
                    }
                }
                if (actorCnf.defaultBuffs.IsLegal())
                {
                    for (int i = 0; i < actorCnf.defaultBuffs.Count; i++)
                    {
                        int buffId = actorCnf.defaultBuffs[i];
                        AddBuffModel addBuffModel = new AddBuffModel();
                        addBuffModel.id = buffId.ToString();
                        addBuffModel.addStack = 1;
                        addBuffModel.durationSetType = true;
                        addBuffModel.duration = 1;
                        addBuffModel.isPermanent = true;
                        SkillLocate.Skill.CreateBuff(this, this, addBuffModel);
                    }
                }
            }
        }

        #region Skill

        /// <summary>
        /// 学习技能
        /// </summary>
        /// <param name="skillModel">技能配置模板</param>
        /// <param name="level">等级</param>
        /// <returns></returns>
        internal bool LearnSkill(SkillModel skillModel, int level = 1)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].model.id == skillModel.id)
                {
                    SkillLocate.Log.LogError("学习技能失败，重复技能", skillModel.name, skillModel.id);
                    return false;
                }
            }

            SkillObj skillObj = new SkillObj(skillModel, level);
            skills.Add(skillObj);
            if (skillModel.addBuffs != null)
            {
                for (int i = 0; i < skillModel.addBuffs.Count; i++)
                {
                    SkillLocate.Skill.CreateBuff(this,this,skillModel.addBuffs[i]);
                }
            }
            return true;
        }

        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="skillId">技能Id</param>
        /// <returns>是否释放成功</returns>
        internal bool ReleaseSkill(string skillId)
        {
            if (timeline != null)
            {
                if (timeline.isFinish)
                {
                    timeline = null;
                }
                else
                {
                    LCSkill.SkillLocate.Log.LogError("释放技能失败，正在释放技能",skillId);
                    return false;
                }
            }
            
            SkillObj skillObj = GetSkill(skillId);
            if (skillObj == null || skillObj.coldDown > 0)
            {
                return false;
            }
            //检测条件
            if (skillObj.model.condition != null && !skillObj.model.condition.IsTrue())
                return false;
            if (SkillLocate.Model.GetTimelineModel(skillObj.model.timeline, out TimelineModel model))
            {
                timeline = new TimelineObj(skillId, model, this);
                //通知Buff技能即将释放
                for (int i = 0; i < buffs.Count; i++)
                {
                    timeline = ExecuteFreedFunc(buffs[i], skillObj, timeline);
                }
                if (timeline == null)
                {
                    return false;
                }
                timeline.skillId = skillId;
                skillObj.coldDown = 0.1f;
                return true;
            }
            else
                return false;
        }

        private TimelineObj ExecuteFreedFunc(BuffObj buffObj, SkillObj skillObj, TimelineObj timeline)
        {
            if (buffObj.model.onFreedFunc == null)
                return timeline;
            return buffObj.model.onFreedFunc.Execute(buffObj, skillObj, timeline);
        }

        /// <summary>
        /// 获得技能
        /// </summary>
        /// <param name="id">技能Id</param>
        /// <returns></returns>
        public SkillObj GetSkill(string id)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i].model.id == id)
                {
                    return skills[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 检测技能完成
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public bool CheckSkillIsFinish(string skillId)
        {
            if (timeline == null)
            {
                return true;
            }
            else
            {
                return timeline.isFinish;
            }
        }

        /// <summary>
        /// 停止当前技能
        /// </summary>
        public void StopSkill()
        {
            Debug.LogError("StopSkill>>>>>>");
            if (timeline == null)
            {
                return;
            }
            
            timeline.timeElapsed = timeline.model.duration;
            timeline.isFinish = true;
            for (int j = 0; j < timeline.model.nodes.Count; j++)
            {
                TimelineFunc timelineFunc = timeline.model.nodes[j];
                timelineFunc.Exit(timeline);
            }
        }

        #endregion

        #region Buff

        /// <summary>
        /// 添加删除Buff
        /// </summary>
        /// <returns></returns>
        internal void AddBuff(AddBuffInfo addBuffInfo)
        {
            List<SkillCom> owers = new List<SkillCom>();
            if (addBuffInfo.ower != null)
                owers.Add(addBuffInfo.ower);
            //检测有没有相同Buff
            List<BuffObj> hasBuffs = GetBuffs(addBuffInfo.buffModel.id, owers);
            //更新已经存在的
            if (hasBuffs.Count > 0)
                UpdateBuff(addBuffInfo, hasBuffs);
            else
            {
                int addStack = Mathf.Min(addBuffInfo.addStack, addBuffInfo.buffModel.maxStack);
                if (addStack <= 0)
                    return;
                BuffObj newBuff = new BuffObj(
                    addBuffInfo.ower,
                    addBuffInfo.buffModel,
                    this,
                    addBuffInfo.duration,
                    addStack,
                    addBuffInfo.isPermanent
                );
                buffs.Add(newBuff);
                buffs.Sort((a, b) =>
                {
                    return a.model.priority.CompareTo(b.model.priority);
                });
                ExecuteBuffOccurFunc(newBuff, addStack);
            }
        }

        /// <summary>
        /// 更新Buff数据
        /// </summary>
        private void UpdateBuff(AddBuffInfo addBuffInfo, List<BuffObj> buffs)
        {
            if (buffs == null || buffs.Count <= 0)
                return;
            int addStack = Mathf.Min(addBuffInfo.addStack, addBuffInfo.buffModel.maxStack);
            for (int i = 0; i < buffs.Count; i++)
            {
                BuffObj buffObj = buffs[i];
                //持续时间
                buffObj.duration = (addBuffInfo.durationSetType == true) ? addBuffInfo.duration : (addBuffInfo.duration + buffObj.duration);
                //层数
                int tmpStack = buffObj.stack + addStack;
                if (tmpStack >= buffObj.model.maxStack)
                {
                    addStack = buffObj.model.maxStack - buffObj.stack;
                }
                else
                {
                    //累加后小于0直接删除这个Buff
                    if (tmpStack <= 0)
                        addStack = 0 - buffObj.stack;
                }
                buffObj.stack += addStack;
                //永久Buff
                buffObj.isPermanent = addBuffInfo.isPermanent;
                //层数大于0
                if (buffObj.stack > 0)
                {
                    ExecuteBuffOccurFunc(buffObj, addStack);
                }
                else
                {
                    //设置持续为0，下一帧移除
                    buffObj.duration = 0;
                }
            }
        }

        private void ExecuteBuffOccurFunc(BuffObj buff, int modifyStack)
        {
            if (buff.model.onOccurFunc == null)
                return;
            //执行攻击者的Buff,OnHit函数
            for (int i = 0; i < buff.model.onOccurFunc.Count; i++)
            {
                BuffLifeCycleFunc func = buff.model.onOccurFunc[i];
                func.Execute(buff, modifyStack);
            }
        }

        /// <summary>
        /// 获得Buff
        /// </summary>
        /// <param name="id">BuffId</param>
        /// <param name="checkOwers">需要检测拥有者</param>
        /// <returns></returns>
        public List<BuffObj> GetBuffs(string id, List<SkillCom> checkOwers)
        {
            List<BuffObj> res = new List<BuffObj>();
            for (int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i].model.id == id && (checkOwers == null || checkOwers.Count <= 0 || checkOwers.Contains(buffs[i].ower) == true))
                {
                    res.Add(buffs[i]);
                }
            }
            return res;
        }

        /// <summary>
        /// 移除Buff
        /// </summary>
        /// <param name="buffObj"></param>
        public void RemoveBuff(BuffObj buffObj)
        {
            for (int i = 0; i < buffs.Count; i++)
            {
                if (buffs[i].Equals(buffObj))
                {
                    buffs.RemoveAt(i);
                }
            }
        }

        #endregion

        #region Timeline

        public void RemoveTimeline(TimelineObj timelineObj)
        {
            if (timeline == null)
            {
                return;
            }
            if (timeline.Equals(timelineObj))
            {
                timeline = null;
            }
        }

        #endregion
    }
}