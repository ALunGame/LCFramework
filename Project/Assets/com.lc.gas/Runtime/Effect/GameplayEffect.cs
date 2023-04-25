using System.Collections.Generic;
using LCToolkit;

namespace LCGAS
{
    /// <summary>
    /// 目标类型
    /// </summary>
    public enum TargetType
    {
        /// <summary>
        /// 拥有者
        /// </summary>
        Owner,
        
        /// <summary>
        /// 目标
        /// </summary>
        Target,
        
        /// <summary>
        /// 来源
        /// </summary>
        Source,
    }
    

    #region 添加和激活
    
    /// <summary>
    /// GE标签
    /// </summary>
    public class GameplayEffectTag
    {
        /// <summary>
        /// 该GE的标签。
        /// </summary>
        public GameplayTagContainer tag = new GameplayTagContainer();

        /// <summary>
        /// GE添加时会赋予目标ASC的Tag
        /// </summary>
        public CommonAddAndRemoveTag grantedTag = new CommonAddAndRemoveTag();

        /// <summary>
        /// GE添加的条件标签，检测目标的Tag是否满足
        /// </summary>
        public CommonConditionTag addConTag = new CommonConditionTag();
        
        /// <summary>
        /// GE被移除的条件标签
        /// </summary>
        public CommonConditionTag removeTags = new CommonConditionTag();
    }
    
    /// <summary>
    /// 激活概率和条件，激活就是在周期作用时时候生效
    /// </summary>
    public class GameplayEffectActiveRate
    {
        /// <summary>
        /// 激活条件标签
        /// </summary>
        public CommonConditionTag conditionTag = new CommonConditionTag();

        /// <summary>
        /// 额外检测函数
        /// </summary>
        public List<string> checkFuns = new List<string>();

        /// <summary>
        /// 概率（0-100）
        /// </summary>
        public int rateValue = 100;
    }
    
    #endregion

    #region 类型和周期

    /// <summary>
    /// Gameplay Effect 类型
    /// </summary>
    public enum GameplayEffectType
    {
        /// <summary>
        /// 立即改变Base Value（扣血）。
        /// </summary>
        Instand,
        
        /// <summary>
        /// 永久改变Current Value（按下疾跑修改速度）只能通过GA或ASC取消。
        /// </summary>
        Infinite,
        
        /// <summary>
        /// 临时修改Current Value（临时Buff）。
        /// </summary>
        HasDuration,
    }

    /// <summary>
    /// 周期
    /// </summary>
    public class GameplayEffectPeriod
    {
        /// <summary>
        /// 周期秒
        /// </summary>
        public float period = 0;

        /// <summary>
        /// 激活立刻执行，或者等待周期在执行
        /// </summary>
        public bool executeOnActive = true;
    }
    
    #endregion

    #region 堆叠效果

    /// <summary>
    /// 效果栈 到期策略
    /// </summary>
    public enum StackExpirationPolicy
    {
        /// <summary>
        /// 什么都不做
        /// </summary>
        None,
        
        /// <summary>
        /// 清空所有层数
        /// </summary>
        ClearEntireStack,
        
        /// <summary>
        /// 清空一层
        /// </summary>
        RefreshDuration,
    }
    
    /// <summary>
    /// 效果栈
    /// 每层Effect如果是Modifiers来计算，则为直接叠加的效果，比如用Modifiers来增加3攻击力，则第一层为增加3攻击力，则第二层为增加6攻击力，则第三层为增加9攻击力，而如果需要根据层数不同而改变增加的值，则需要使用Executions。
    /// </summary>
    public class GameplayEffectStack
    {
        /// <summary>
        /// 最大堆叠数量
        /// </summary>
        public int limitCnt;

        /// <summary>
        /// 堆叠目标身上or施法者身上
        /// 举个例子，假设层数为3，如果是by Target模式，那么3个敌人对我释放的Debuff只能叠三层。
        /// 如果是by Source模式，那么3个敌人可以对我叠加9层Debuff。
        /// </summary>
        public TargetType type = TargetType.Target;

        /// <summary>
        /// 新的GE添加时刷新持续时间
        /// </summary>
        public bool addRefreshDuration;

        /// <summary>
        /// 当一层GE的Duration到期后的处理方式。
        /// </summary>
        public StackExpirationPolicy expirationPolicy = StackExpirationPolicy.None;
    }

    /// <summary>
    /// 效果栈溢出时候效果 配合 效果栈 使用
    /// </summary>
    public class GameplayEffectOverflow
    {
        /// <summary>
        /// 效果栈溢出时 添加的GE名
        /// </summary>
        public List<string> addEffectNames = new List<string>();

        /// <summary>
        /// True 溢出的Apply不会刷新Duration
        /// </summary>
        public bool denyOverflowActive = false;
        
        /// <summary>
        /// 溢出时清空栈 denyOverflowActive 必须为True
        /// </summary>
        public bool clearStackOnOverflow;
    }

    #endregion

    #region 结束

    /// <summary>
    /// 效果持续时间被打断或者正常结束时候
    /// </summary>
    public class GameplayEffectExpiration
    {
        /// <summary>
        /// 打断时Apply的GE。
        /// </summary>
        public GameplayTagContainer prematureEffectTags = new GameplayTagContainer();
        
        /// <summary>
        /// 正常结束时Apply的GE
        /// </summary>
        public GameplayTagContainer routineEffectTags = new GameplayTagContainer();
    }

    #endregion
    
    #region 激活的额外表现

    /// <summary>
    /// GE表现，调用GC
    /// </summary>
    public class GameplayEffectDisplay
    {
        /// <summary>
        /// 激活 GC的标签。
        /// </summary>
        public GameplayTagContainer tags = new GameplayTagContainer();
    }

    #endregion

    /// <summary>
    /// 效果
    /// 流程：先判断能否添加，然后判断能否激活(执行后续操作,不可激活这个就只是一个标记位),根据类型来处理属性改变->效果展示->检测堆叠，效果被打断或结束执行后续的行为
    /// 0，配置模板类，没有逻辑
    /// 1，唯一改变属性的地方
    /// 2，Ability对自己或他人产生影响的途径
    /// 3，可以理解为Buff
    /// 4，释放技能时候的伤害结算，施加特殊效果的控制、霸体效果 （修改GameplayTag）都是通过GE来实现的
    /// </summary>
    public class GameplayEffect
    {
        /// <summary>
        /// 效果名
        /// </summary>
        public string name = "";

        /// <summary>
        /// 标签
        /// </summary>
        public GameplayEffectTag tag = new GameplayEffectTag();

        /// <summary>
        /// 类型
        /// </summary>
        public GameplayEffectType type = GameplayEffectType.Instand;

        /// <summary>
        /// 应用的概率，可以添加，但是应用是另一回事
        /// </summary>
        public GameplayEffectActiveRate rate;

        /// <summary>
        /// 周期
        /// </summary>
        public GameplayEffectPeriod period;

        /// <summary>
        /// 持续时间
        /// </summary>
        public GameplayEffectModifierMagnitude duration;

        /// <summary>
        /// 属性改变流程
        /// </summary>
        public List<GameplayEffectModifier> modifiers;

        /// <summary>
        /// 栈
        /// </summary>
        public GameplayEffectStack stack;

        /// <summary>
        /// 栈溢出策略
        /// </summary>
        public GameplayEffectOverflow overflow;

        /// <summary>
        /// 被打断或者正常结束策略
        /// </summary>
        public GameplayEffectExpiration expiration;

        /// <summary>
        /// 应用的表现
        /// </summary>
        public GameplayEffectDisplay display;
    }
}