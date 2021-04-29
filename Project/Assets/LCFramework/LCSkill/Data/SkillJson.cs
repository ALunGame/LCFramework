using System.Collections.Generic;

namespace LCSkill
{
    public class SkillList
    {
        public List<SkillJson> List = new List<SkillJson>();
    }

    public class SkillJson
    {
        public int Id = 0;
        public string DecStr = "";
        public double ContinueTime = 0;

        //作用列表
        public List<SkillImpact> ImpactList = new List<SkillImpact>();
    }

    /// <summary>
    /// 技能作用类型
    /// </summary>
    public enum SkillImpactType
    {
        Once,                   //作用一次
        Gap,                    //间断作用
        Buff,                   //作用一次,时间结束还原
    }

    /// <summary>
    /// 技能效果
    /// 1，一个效果会带有特效，音效，动画，属性改变
    /// 2，他们分为自身效果，对其他实体效果
    /// </summary>
    public class SkillImpact
    {
        public string Name = "";
        //作用时间
        public double Time = 0;
        //持续时间
        public double ContinueTime = 0;
        //该效果是否可被打断
        public bool CanBreak = false;
        //打断退出（意味着剩余的效果结束）
        public bool BreakExit = false;

        //自身数据
        public SkillSelfImpactInfo SelfInfo = null;
        //他人数据
        public SkillOtherImpactInfo OtherInfo = null;
    }

    //自身效果数据
    public class SkillSelfImpactInfo
    {
        //效果使实体僵直时间
        public double StopTime = 0.1f;

        public SkillSelfData Data = null;
        public SkillAnim Anim = null;
        public SkillEffect Effect = null;
        public SkillAudio Audio = null;
    }

    //他人效果数据
    public class SkillOtherImpactInfo
    {
        //作用时间
        public double Time = 0;
        //持续时间
        public double ContinueTime = 0;
        //间隔时间
        public double GapTime = 0;

        //效果位置
        public string Pos = "";
        //效果范围
        public string Area = "";

        //效果使实体僵直时间
        public double StopTime = 0.1f;

        //作用类型
        public SkillDataImpactType DataType = SkillDataImpactType.Once;

        //筛选
        public SkillFilter Filter = new SkillFilter();
        
        public SkillOtherData Data = null;

        public SkillAnim Anim = null;
        public SkillEffect Effect = null;
        public SkillAudio Audio = null;
    }

    #region Data

    /// <summary>
    /// 技能自身作用数据
    /// </summary>
    public class SkillSelfData
    {
        //作用时间
        public double Time = 0;
        //持续时间
        public double ContinueTime = 0;
        //间隔时间
        public double GapTime = 0;
        public SkillDataImpactType DataType = SkillDataImpactType.Once;
        public List<SkillDataOperate> DataOperates = new List<SkillDataOperate>();
    }

    /// <summary>
    /// 技能他人作用数据
    /// </summary>
    public class SkillOtherData
    {
        public List<SkillDataOperate> DataOperates = new List<SkillDataOperate>();
    }

    //技能数据作用公式
    public enum SkillDataMathType
    {
        Add,                    //加
        Remove,                 //减
        Ride,                   //乘法
        Cover,                  //覆盖
    }

    //技能数据作用类型
    public enum SkillDataImpactType
    {
        Once,                   //作用一次
        Gap,                    //间断作用
        Buff,                   //作用一次,时间结束还原
    }

    //技能作用数据操作
    public class SkillDataOperate
    {
        public SkillDataMathType MathType = SkillDataMathType.Add;
        //作用的数据名
        public string Name = "";
        //数据
        public double Data = 0;
    }

    #endregion

    #region 筛选

    /// <summary>
    /// 技能目标
    /// </summary>
    public enum SkillTargetType
    {
        Friend,                 //友方
        FriendAndSelf,          //友方以及自己
        Enemy,                  //敌方
    }

    /// <summary>
    /// 技能目标筛选规则
    /// </summary>
    public enum SkillFilterRule
    {
        None,                   //没有规则
        Min,                    //实体中最小
        Max,                    //实体中最大
    }

    /// <summary>
    /// 技能筛选配置
    /// </summary>
    public class SkillFilter
    {
        public SkillTargetType TargetType = SkillTargetType.Enemy;
        public SkillFilterRule Filter = SkillFilterRule.None;
        public int TargetCnt = -1;
        public string DataName = "";
    } 

    #endregion

    public class SkillEffect
    {
        public double Time = 0;
        public double HideTime = 0;
        public string Pos = "";
        public int EffectId = 0;
    }

    public class SkillAudio
    {
        public double Time = 0;
        public int AudioId = 0;
    }

    public class SkillAnim
    {
        public double Time = 0;
        public string AnimName = "";
    }
}
