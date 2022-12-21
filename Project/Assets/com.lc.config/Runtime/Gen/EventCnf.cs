using System;
using Demo;
using System.Collections.Generic;
using Config;

namespace Demo.Config
{
    
    /// <summary>
    /// 事件配置
    /// </summary>
    public class EventCnf
    {
        
        /// <summary>
        /// 任务事件Id
        /// </summary>
        public int id;

        /// <summary>
        /// 事件名
        /// </summary>
        public string name;

        /// <summary>
        /// 描述
        /// </summary>
        public string des;

        /// <summary>
        /// 事件解锁条件
        /// </summary>
        public ConditionType cond;

        /// <summary>
        /// 事件解锁参数
        /// </summary>
        public string condParam;

        /// <summary>
        /// 开始的任务Id
        /// </summary>
        public int startTaskId;

        /// <summary>
        /// 成功下一个事件
        /// </summary>
        public int nextSuccess;

        /// <summary>
        /// 成功奖励
        /// </summary>
        public List<ItemInfo> successReward;

        /// <summary>
        /// 失败下一个事件
        /// </summary>
        public int nextFail;

        /// <summary>
        /// 失败奖励
        /// </summary>
        public List<ItemInfo> failReward;

    }

}

