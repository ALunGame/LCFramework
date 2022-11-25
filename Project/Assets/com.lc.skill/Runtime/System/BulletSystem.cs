using LCECS.Core;
using LCMap;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 管理Bullet的更新和删除
    /// </summary>
    public class BulletSystem : BaseSystem
    {
        private BaseBulletSensor Sensor;

        protected override List<Type> RegListenComs()
        {
            Sensor = LCECS.ECSLayerLocate.Info.GetSensor<BaseBulletSensor>(LCECS.SensorType.Skill_Bullet);
            return new List<Type>() { typeof(BulletCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            BulletCom bulletCom = GetCom<BulletCom>(comList[0]);
            DealBullet(bulletCom);
        }

        private void DealBullet(BulletCom bulletCom)
        {
            float timePassed = SkillLocate.DeltaTime;
            if (bulletCom.Bullets == null || bulletCom.Bullets.Count <= 0)
                return;
            for (int i = 0; i < bulletCom.Bullets.Count; i++)
            {
                BulletObj bulletObj = bulletCom.Bullets[i];

                //创建函数
                if (bulletObj.timeElapsed <= 0)
                    ExecuteCreateFunc(bulletObj);

                //处理命中纪录
                UpdateHitRecords(bulletObj, timePassed);

                //移动
                if (bulletObj.duration > 0 && bulletObj.model.moveFunc != null)
                {
                    BulletMoveInfo bulletMoveInfo = bulletObj.model.moveFunc.Execute(bulletObj, bulletObj.timeElapsed, bulletObj.followActor);
                    bulletObj.SetMoveInfo(bulletMoveInfo);
                }

                //碰撞
                if (bulletObj.canHitAfterCreated > 0)
                {
                    bulletObj.canHitAfterCreated -= timePassed;
                }
                else
                {
                    List<Actor> hitActors = Sensor.GetHitActors(bulletObj);
                    for (int a = 0; a < hitActors.Count; a++)
                    {
                        if (CheckInHitRecord(bulletObj,hitActors[a]))
                            continue;
                        //次数减一
                        bulletObj.model.hitTimes -= 1;

                        //执行击中函数
                        Actor hitActor = hitActors[a];
                        ExecuteHitActorFunc(bulletObj,hitActor);

                        //记录击中记录
                        bulletObj.hitRecords.Add(new BulletHitRecord(hitActor, bulletObj.model.sameTargetDelay));
                    }
                }

                //生命周期计算
                bulletObj.duration -= timePassed;
                bulletObj.timeElapsed += timePassed;
                if (bulletObj.duration <= 0 || bulletObj.model.hitTimes <= 0)
                {
                    ExecuteRemoveFunc(bulletObj);
                    bulletCom.RemoveBullet(bulletObj);
                    DestroyBullet(bulletObj);
                }
            }
        }

        private void UpdateHitRecords(BulletObj bulletObj, float timePassed)
        {
            for (int i = 0; i < bulletObj.hitRecords.Count; i++)
            {
                BulletHitRecord record = bulletObj.hitRecords[i];
                record.timeToCanHit -= timePassed;
                if (record.timeToCanHit <= 0 || record.target == null)
                {
                    bulletObj.hitRecords.RemoveAt(i);
                }
            }
        }

        private bool CheckInHitRecord(BulletObj bulletObj, Actor actor)
        {
            for (int i = 0; i < bulletObj.hitRecords.Count; i++)
            {
                BulletHitRecord record = bulletObj.hitRecords[i];
                if (record.target != null && record.target.Equals(actor))
                {
                    return true;
                }
            }
            return false;
        }

        private void ExecuteCreateFunc(BulletObj bulletObj)
        {
            if (bulletObj.model.onCreateFunc == null || bulletObj.model.onCreateFunc.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < bulletObj.model.onCreateFunc.Count; i++)
            {
                BulletLifeCycleFunc func = bulletObj.model.onCreateFunc[i];
                func.Execute(bulletObj);
            }
        }

        private void ExecuteRemoveFunc(BulletObj bulletObj)
        {
            if (bulletObj.model.onRemovedFunc == null || bulletObj.model.onRemovedFunc.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < bulletObj.model.onRemovedFunc.Count; i++)
            {
                BulletLifeCycleFunc func = bulletObj.model.onRemovedFunc[i];
                func.Execute(bulletObj);
            }
        }

        private void ExecuteHitActorFunc(BulletObj bulletObj, Actor actor)
        {
            if (bulletObj.model.onHitFunc == null || bulletObj.model.onHitFunc.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < bulletObj.model.onHitFunc.Count; i++)
            {
                BulletHitFunc func = bulletObj.model.onHitFunc[i];
                func.Execute(bulletObj,actor);
            }
        }

        private void DestroyBullet(BulletObj bulletObj)
        {
            Debug.LogWarning("目前bulletObj直接销毁》》》》！！！！");
        }

    }
}