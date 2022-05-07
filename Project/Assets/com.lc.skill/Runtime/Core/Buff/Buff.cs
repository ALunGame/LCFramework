using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 配置的Buff数据结构
    /// </summary>
    public struct BuffModel 
    {
        /// <summary>
        /// BuffId
        /// </summary>
        public int id;

        /// <summary>
        /// Buff名字
        /// </summary>
        public string name;

        /// <summary>
        /// Buff的标签
        /// </summary>
        public string[] tags;

        /// <summary>
        /// Buff优先级（优先级越低的buff越后面执行）
        /// </summary>
        public int priority;

        /// <summary>
        /// 最大堆叠层数
        /// </summary>
        public int maxStack;

        /// <summary>
        /// Buff的执行OnTick间隔
        /// </summary>
        public float tickTime;

        /// <summary>
        /// Buff添加的属性
        /// 1,这边用数组的意义在于有时候属性的计算是复杂的，需要几个属性共同计算才有目标结果
        /// </summary>
        //public Property[] props;

        /// <summary>
        /// Buff对于状态的影响
        /// 1,状态就是一种Bool运算所有不应该有多个
        /// </summary>
        //public State state;

        /// <summary>
        /// 当释放一个技能时执行的函数
        /// 为了处理当技能释放时，更改技能表现，比如没有魔法还要释放，会执行没有魔法的动画
        /// </summary>
        public BuffOnFreedFunc onFreedFunc;


        #region 生命周期函数

        /// <summary>
        /// 当Buff被添加、改变层数时执行的函数
        /// </summary>
        public List<BuffLifeCycleFunc> onOccurFunc;

        /// <summary>
        /// Buff在tickTime间隔执行的函数
        /// </summary>
        public List<BuffLifeCycleFunc> onTickFunc;

        /// <summary>
        /// Buff将要被移除时执行的函数
        /// </summary>
        public List<BuffLifeCycleFunc> onRemovedFunc;

        #endregion

        #region 伤害流程

        /// <summary>
        /// 在执行伤害流程时，拥有这个Buff作为攻击者执行的函数
        /// </summary>
        public List<BuffHurtFunc> onHurtFunc;

        /// <summary>
        /// 在执行伤害流程时，拥有这个Buff作为挨打者执行的函数
        /// </summary>
        public List<BuffBeHurtFunc> onBeHurtFunc;

        /// <summary>
        /// 在执行伤害流程时，如果击杀目标执行的函数
        /// </summary>
        public List<BuffKilledFunc> onKilledFunc;

        /// <summary>
        /// 在执行伤害流程时，拥有这个Buff被杀死执行的函数
        /// </summary>
        public List<BuffBeKilledFunc> onBeKilledFunc; 

        #endregion
    }

    /// <summary>
    /// 配置添加一个Buff
    /// </summary>
    public struct AddBuffModel
    {
        /// <summary>
        /// BuffId
        /// </summary>
        public int id;

        /// <summary>
        /// 添加的层数，负数就是减少
        /// </summary>
        public int addStack;

        /// <summary>
        /// 持续时间设置模式(true:覆盖 false:累加)
        /// </summary>
        public bool durationSetType;

        /// <summary>
        /// 添加的持续时间
        /// </summary>
        public float duration;

        /// <summary>
        /// 是否是一个永久的buff,如果持续使劲按减少到0，也会被删除
        /// </summary>
        public bool isPermanent;
    }

    /// <summary>
    /// 用于添加一条Buff的信息
    /// </summary>
    public struct AddBuffInfo
    {
        /// <summary>
        /// 添加的发起者
        /// </summary>
        public SkillCom ower;

        /// <summary>
        /// 添加的目标
        /// </summary>
        public SkillCom target;

        /// <summary>
        /// 添加的BuffId
        /// </summary>
        public BuffModel buffModel;

        /// <summary>
        /// 添加的层数，负数就是减少
        /// </summary>
        public int addStack;

        /// <summary>
        /// 持续时间设置模式(true:覆盖 false:累加)
        /// </summary>
        public bool durationSetType;

        /// <summary>
        /// 添加的持续时间
        /// </summary>
        public float duration;

        /// <summary>
        /// 是否是一个永久的buff,如果持续使劲按减少到0，也会被删除
        /// </summary>
        public bool isPermanent;

        /// <summary>
        /// 添加的参数
        /// </summary>
        public Dictionary<string, object> buffParam;

        public AddBuffInfo(
            BuffModel buffModel, SkillCom ower, SkillCom target,
            int addStack, float duration, bool durationSetType = true,
            bool isPermanent = false,
            Dictionary<string, object> buffParam = null
        )
        {
            this.buffModel = buffModel;
            this.ower = ower;
            this.target = target;
            this.addStack = addStack;
            this.duration = duration;
            this.durationSetType = durationSetType;
            this.isPermanent = isPermanent;
            this.buffParam = buffParam;
        }
    }

    /// <summary>
    /// 运行中挂在身上的Buff
    /// </summary>
    public class BuffObj
    {
        /// <summary>
        /// 数据
        /// </summary>
        public BuffModel model;

        /// <summary>
        /// 剩余时间
        /// </summary>
        public float duration;

        /// <summary>
        /// 是否是一个永久Buff
        /// </summary>
        public bool isPermanent;

        /// <summary>
        /// 当前层数
        /// </summary>
        public int stack;

        /// <summary>
        /// 这个Buff是通过谁添加的（可以是空）
        /// </summary>
        public SkillCom originer;

        /// <summary>
        /// 这个Buff的拥有者
        /// </summary>
        public SkillCom ower;

        /// <summary>
        /// 这个Buff存在时间
        /// </summary>
        public float timeElapsed = 0;

        /// <summary>
        /// 这个Buff执行多少次onTick
        /// </summary>
        public int tickCnt = 0;

        ///<summary>
        ///buff的一些参数，这些参数是逻辑使用的，比如wow中牧师的盾还能吸收多少伤害，就可以记录在buffParam里面
        ///</summary>
        public Dictionary<string, object> buffParam = new Dictionary<string, object>();

        /// <summary>
        /// 创建Buff对象
        /// </summary>
        /// <param name="originer">Buff创建者(可以为空)</param>
        /// <param name="model">Buff配置数据</param>
        /// <param name="ower">Buff携带者</param>
        /// <param name="duration">持续时间</param>
        /// <param name="stack">Buff层数</param>
        /// <param name="permanent">是不是永久Buff</param>
        /// <param name="buffParam">Buff参数</param>
        public BuffObj(SkillCom originer,BuffModel model, SkillCom ower, float duration, int stack, bool permanent = false, 
            Dictionary<string, object> buffParam = null)
        {
            this.originer = originer;
            this.model = model;
            this.ower = ower;
            this.duration = duration;
            this.stack = stack; 
            this.isPermanent = permanent;
            this.buffParam = buffParam; 
        }
    }
}