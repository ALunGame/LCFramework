using LCMap;
using LCToolkit;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 配置创建一个子弹
    /// </summary>
    public class AddBulletModel
    {
        /// <summary>
        /// AoeId
        /// </summary>
        [Header("子弹Id")]
        public int id;

        /// <summary>
        /// 发射的位置
        /// </summary>
        [Header("发射的位置")]
        public Vector3 firePos;

        /// <summary>
        /// 发射方向
        /// </summary>
        [Header("发射方向")]
        public float fireDir;

        /// <summary>
        /// 子弹的初速度，单位：米/秒
        /// </summary>
        [Header("子弹的初速度")]
        public float speed;

        /// <summary>
        /// 子弹的持续时间，单位：秒
        ///</summary>
        [Header("子弹的持续时间")]
        public float duration;

        /// <summary>
        /// 移动遵循发射角度
        /// </summary>
        [Header("移动遵循发射角度")]
        public bool useFireDegreeForever;

        /// <summary>
        /// 子弹创建后多久是没有碰撞的，这样比如子母弹之类的，不会在创建后立即命中目标，但绝大多子弹还应该是0的
        /// 单位：秒
        /// </summary>
        [Header("多久后才可以击中")]
        public float canHitAfterCreated = 0;

    }

    /// <summary>
    /// 子弹配置
    /// </summary>
    public struct BulletModel
    {
        /// <summary>
        /// BulletId
        /// </summary>
        public string id;

        /// <summary>
        /// 子弹预制体（可以是空）
        /// </summary>
        public UnityObjectAsset asset;

        ///<summary>
        ///子弹的半径
        ///</summary>
        public float radius;

        ///<summary>
        ///子弹可以碰触的次数，每次碰到目标-1，到0的时候子弹就结束了。
        ///</summary>
        public int hitTimes;

        ///<summary>
        ///子弹碰触同一个目标的延迟，单位：秒，最小值是Time.fixedDeltaTime（每帧发生一次）
        ///</summary>
        public float sameTargetDelay;

        ///<summary>
        ///子弹的是否碰到障碍物移除
        ///</summary>
        public bool removeOnObstacle;

        ///<summary>
        ///子弹是否会命中敌人
        ///</summary>
        public bool hitEnemy;

        ///<summary>
        ///子弹是否会命中盟军
        ///</summary>
        public bool hitFriend;

        /// <summary>
        /// 移动函数
        /// </summary>
        public BulletMoveFunc moveFunc;

        /// <summary>
        /// 释放时寻找目标
        /// </summary>
        public BulletCatchActorFunc catchFunc;

        /// <summary>
        /// 创建时调用
        /// </summary>
        public List<BulletLifeCycleFunc> onCreateFunc;

        /// <summary>
        /// 移除时调用
        /// </summary>
        public List<BulletLifeCycleFunc> onRemovedFunc;

        /// <summary>
        /// 命中目标调用
        /// </summary>
        public List<BulletHitFunc> onHitFunc;
    }

    /// <summary>
    /// 子弹命中纪录
    /// </summary>
    public class BulletHitRecord
    {
        public ActorObj target;

        /// <summary>
        /// 多久之后还能再次命中，单位秒
        /// </summary>
        public float timeToCanHit;

        public BulletHitRecord(ActorObj actor, float timeToCanHit)
        {
            this.target = actor;
            this.timeToCanHit = timeToCanHit;
        }
    }

    /// <summary>
    /// 子弹移动信息
    /// </summary>
    public class BulletMoveInfo
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
    /// 运行中创建的一个Bullet
    /// </summary>
    public class BulletObj
    {
        /// <summary>
        /// 数据
        /// </summary>
        public BulletModel model;

        /// <summary>
        /// Bullet拥有者，可以是空
        /// </summary>
        public SkillCom ower;

        /// <summary>
        /// 发射时方向
        /// </summary>
        public float fireDir;

        /// <summary>
        /// 子弹的初速度，单位：米/秒
        /// </summary>
        public float speed;

        /// <summary>
        /// 子弹的剩余时间
        /// </summary>
        public float duration;

        /// <summary>
        /// 子弹已经存在了多久了，单位：秒
        /// 毕竟duration是可以被重设的，比如经过一个aoe，生命周期减半了
        /// </summary>
        public float timeElapsed = 0;

        /// <summary>
        /// 子弹的移动轨迹是否严格遵循发射出来的角度
        /// </summary>
        public bool useFireDirForever = false;

        /// <summary>
        /// 当前移动信息
        /// </summary>
        public BulletMoveInfo CurrMoveInfo { get; private set; }
        public void SetMoveInfo(BulletMoveInfo moveInfo)
        {
            CurrMoveInfo = moveInfo;
        }

        /// <summary>
        /// 子弹命中纪录
        /// </summary>
        public List<BulletHitRecord> hitRecords = new List<BulletHitRecord>();

        /// <summary>
        /// 子弹创建后多久是没有碰撞的，这样比如子母弹之类的，不会在创建后立即命中目标，但绝大多子弹还应该是0的
        /// 单位：秒
        ///</summary>
        public float canHitAfterCreated = 0;

        /// <summary>
        /// 子弹正在追踪的目标
        /// </summary>
        public ActorObj followActor = null;

        /// <summary>
        /// 子弹传入的参数，逻辑用的到的临时记录
        /// </summary>
        public Dictionary<string, object> param = new Dictionary<string, object>();
    }
}
