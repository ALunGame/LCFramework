using LCMap;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCTask
{
    /// <summary>
    /// 任务阶段
    /// </summary>
    public enum TaskStage
    {
        /// <summary>
        /// 接受前，执行接受目标表现
        /// </summary>
        PreAccept,

        /// <summary>
        /// 接受，判断接受条件，执行接受行为
        /// </summary>
        Accept,

        /// <summary>
        /// 提交前，执行提交目标表现
        /// </summary>
        PreExecute,

        /// <summary>
        /// 提交，判断提交条件，执行提交行为
        /// </summary>
        Execute,
    }

    /// <summary>
    /// 任务目标
    /// </summary>
    public struct TaskTarget
    {
        public int mapId;
        public List<int> actorIds;
    }

    /// <summary>
    /// 任务配置数据
    /// </summary>
    public struct TaskModel
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public int id;

        /// <summary>
        /// 前置解锁任务
        /// </summary>
        public List<int> preUnlockTasks;

        /// <summary>
        /// 解锁任务
        /// </summary>
        public List<int> unlockTasks;

        /// <summary>
        /// 接受目标
        /// </summary>
        public TaskTarget acceptTarget;


    } 

    /// <summary>
    /// 运行时创建的任务对象
    /// </summary>
    public class TaskObj
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public int TaskId { get; private set; }

        /// <summary>
        /// 数据
        /// </summary>
        public TaskModel Model;

        /// <summary>
        /// 当前任务阶段目标
        /// </summary>
        public List<ActorObj> Targets { get; private set; }
    }
}
