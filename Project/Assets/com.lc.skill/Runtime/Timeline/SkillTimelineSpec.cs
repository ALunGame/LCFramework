using System;
using System.Collections.Generic;
using Demo;
using Demo.Com;
using LCECS;
using LCECS.Core.Tree.Base;
using LCMap;
using UnityEngine;

namespace LCSkill.Timeline
{
    public class SkillTimelineSpec
    {
        /// <summary>
        /// 配置数据
        /// </summary>
        public BaseTimeline Model { get; private set; }
        
        /// <summary>
        /// 释放者
        /// </summary>
        public SkillAbilitySpec Owner { get; private set; }
        
        /// <summary>
        /// 释放演员
        /// </summary>
        public Actor OwnerActor { get; private set; }
        
        /// <summary>
        /// 运行时长(帧)
        /// </summary>
        public int TimeElapsed { get; private set; }
        
        private float _timeScale = 1.00f;
        /// <summary>
        /// 倍速
        /// </summary>
        public float TimeScale
        {
            get
            {
                return _timeScale;
            }
            set
            {
                _timeScale = Mathf.Max(0.100f, value);
            }
        }

        /// <summary>
        /// 已经完成
        /// </summary>
        public int IsFinish { get; private set; }
        
        /// <summary>
        /// 是否可以打断
        /// </summary>
        public bool CanBreak { get; internal set; }
        
        /// <summary>
        /// 当前帧
        /// </summary>
        public int CurrFrame { get; internal set; }

        private TimerInfo executeTimer;
        private Action executeFinishCallBack;
        
        /// <summary>
        /// 运行上下文
        /// </summary>
        public Dictionary<string, TimelineContext> Context;

        public SkillTimelineSpec(BaseTimeline pModel, SkillAbilitySpec pAbilitySpec)
        {
            Model = pModel;
            Owner = pAbilitySpec;
            OwnerActor = ECSLocate.ECS.GetEntity(Owner.OwnerCom.EntityUid) as Actor;
            Context = new Dictionary<string, TimelineContext>();
        }

        public void Start(Action pExecuteFinishCallBack)
        {
            executeFinishCallBack = pExecuteFinishCallBack;
            
            Model.Start(this);
            CurrFrame = 0;
            
            //立即执行
            if (Model.totalFrame <= 0)
            {
                UpdateFrame();
                End();
            }
            else
            {
                executeTimer = GameLocate.TimerServer.LoopFrame(1, -1,() =>
                {
                    if (CurrFrame > Model.totalFrame)
                    {
                        End();
                        return;
                    }
                    
                    UpdateFrame();
                    CurrFrame++;
                },true);
            }
        }

        public void End()
        {
            GameLocate.TimerServer.StopTimer(executeTimer);   
            Model.End(this);
            Context.Clear();

            Action func = executeFinishCallBack;
            executeFinishCallBack = null;
            func?.Invoke();
        }

        private void UpdateFrame()
        {
            Model.UpdateFrame(this, CurrFrame);
        }
    }
}