using LCMap;
using LCToolkit;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    public struct AoeModel
    {
        /// <summary>
        /// AoeId
        /// </summary>
        public string id;

        /// <summary>
        /// Aoe预制体（可以是空）
        /// </summary>
        public UnityObjectAsset asset;

        /// <summary>
        /// Aoe区域
        /// </summary>
        public List<Vector3> area;

        /// <summary>
        /// Aoe的执行OnTick间隔
        /// </summary>
        public float tickTime;

        /// <summary>
        /// 移动调用
        /// </summary>
        public AoeMoveFunc moveFunc;

        #region 生命周期函数

        /// <summary>
        /// 创建调用
        /// </summary>
        public List<AoeLifeCycleFunc> onCreateFunc;

        /// <summary>
        /// onTick调用
        /// </summary>
        public List<AoeLifeCycleFunc> onTickFunc;

        /// <summary>
        /// 移除调用
        /// </summary>
        public List<AoeLifeCycleFunc> onRemovedFunc;

        #endregion

        #region 进入离开

        /// <summary>
        /// 演员进入调用
        /// </summary>
        public List<AoeActorEnter> onActorEnterFunc;

        /// <summary>
        /// 演员离开调用
        /// </summary>
        public List<AoeActorLeave> onActorLeaveFunc;

        /// <summary>
        /// 子弹进入调用
        /// </summary>
        public List<AoeBulletEnter> onBulletEnterFunc;

        /// <summary>
        /// 子弹离开调用
        /// </summary>
        public List<AoeBulletLeave> onBulletLeaveFunc;

        #endregion
    }

    /// <summary>
    /// Aoe移动信息
    /// </summary>
    public class AoeMoveInfo
    {
        /// <summary>
        /// 当前速度
        /// </summary>
        public Vector3 velocity;

        /// <summary>
        /// 当前角度
        /// </summary>
        public float rotate;
    }

    /// <summary>
    /// 运行中创建的一个Aoe
    /// </summary>
    public class AoeObj
    {
        /// <summary>
        /// 要释放的aoe
        /// </summary>
        public AoeModel model;

        /// <summary>
        /// 是否被视作刚创建会调用onCreate函数
        /// </summary>
        public bool justCreated = true;

        /// <summary>
        /// aoe区域尺寸（根据不同状态区域大小发生改变）
        /// </summary>
        public float size = 1;

        /// <summary>
        /// aoe拥有者，可以是空
        /// </summary>
        public SkillCom ower;

        /// <summary>
        /// aoe存在的时间，单位：秒
        /// </summary>
        public float duration;

        /// <summary>
        /// aoe已经存在过的时间，单位：秒
        /// </summary>
        public float timeElapsed = 0;

        /// <summary>
        /// aoe移动了多少时间了，单位：秒
        /// <summary>
        public float moveRunnedTime = 0;

        /// <summary>
        /// 现在aoe范围内的所有演员
        /// </summary>
        public List<ActorObj> actorInRange = new List<ActorObj>();

        /// <summary>
        /// 现在aoe范围内的所有子弹
        /// </summary>
        public List<BulletObj> bulletInRange = new List<BulletObj>();

        /// <summary>
        /// 当前移动信息
        /// </summary>
        public AoeMoveInfo CurrMoveInfo { get; private set; }
        public void SetMoveInfo(AoeMoveInfo moveInfo)
        {
            CurrMoveInfo = moveInfo;
        }
    }
}
