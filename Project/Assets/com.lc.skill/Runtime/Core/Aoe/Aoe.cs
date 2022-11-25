using LCMap;
using LCToolkit;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 配置添加一个Aoe
    /// </summary>
    public class AddAoeModel
    {
        /// <summary>
        /// AoeId
        /// </summary>
        [Header("AoeId")]
        public string id = "";

        /// <summary>
        /// 大小
        /// </summary>
        [Header("大小")]
        public float size = 1;

        /// <summary>
        /// Aoe持续时间
        /// </summary>
        [Header("持续时间")]
        public float duration;

        /// <summary>
        /// 是否跟随拥有者
        /// </summary>
        [Header("是否跟随拥有者")]
        public bool follow;
    }

    /// <summary>
    /// Aoe配置
    /// </summary>
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
        public Shape areaShape;

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
        /// aoe创建的节点可以是空
        /// </summary>
        public GameObject go;

        /// <summary>
        /// aoe拥有者，可以是空
        /// </summary>
        public SkillCom ower;

        /// <summary>
        /// 是否跟随拥有者
        /// </summary>
        public bool follow;

        /// <summary>
        /// 是否被视作刚创建会调用onCreate函数
        /// </summary>
        public bool justCreated = true;

        /// <summary>
        /// aoe区域尺寸（根据不同状态区域大小发生改变）
        /// </summary>
        public float size = 1;

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
        public List<Actor> actorInRange = new List<Actor>();

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

        /// <summary>
        /// 计算Aoe区域
        /// </summary>
        /// <returns></returns>
        public Shape CalcArea()
        {
            if (ower == null)
            {
                if (size != 1)
                {
                    Shape newShape = model.areaShape;
                    newShape.Scale(size);
                    return newShape;
                }
                return model.areaShape;
            }
            else
            {
                if (!follow && size == 1)
                    return model.areaShape;

                Shape newShape = model.areaShape;
                if (size != 1)
                    newShape.Scale(size);
                if (follow)
                {
                    Actor actorObj = MapLocate.Map.GetActor(ower.EntityUid);
                    if (actorObj == null)
                    {
                        SkillLocate.Log.LogError("计算区域出错,没有跟随对象>>", model.id, ower.EntityUid);
                    }
                    if (actorObj.Roate.y != 0)
                        newShape.FlipX();
                    newShape.Translate(actorObj.Pos);
                }
                return newShape;
            }
            
        }
    }
}
