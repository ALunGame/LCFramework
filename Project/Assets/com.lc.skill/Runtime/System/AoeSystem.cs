using LCECS.Core;
using LCMap;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// 管理Aoe的更新和删除
    /// </summary>
    public class AoeSystem : BaseSystem
    {
        private AoeSensor Sensor;

        protected override List<Type> RegListenComs()
        {
            Sensor = LCECS.ECSLayerLocate.Info.GetSensor<AoeSensor>(LCECS.SensorType.Skill);
            return new List<Type>() { typeof(AoeCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            AoeCom aoeCom = GetCom<AoeCom>(comList[0]);
            DealAoe(aoeCom);
        }

        private void DealAoe(AoeCom aoeCom)
        {
            float timePassed = SkillLocate.DeltaTime;
            if (aoeCom.Aoes == null || aoeCom.Aoes.Count <= 0)
                return;

            for (int i = 0; i < aoeCom.Aoes.Count; i++)
            {
                AoeObj aoeObj = aoeCom.Aoes[i];

                //移动
                if (aoeObj.duration > 0 && aoeObj.model.moveFunc != null)
                {
                    AoeMoveInfo aoeMoveInfo = aoeObj.model.moveFunc.Execute(aoeObj, aoeObj.moveRunnedTime);
                    aoeObj.moveRunnedTime += timePassed;
                    aoeObj.SetMoveInfo(aoeMoveInfo);
                }

                if (aoeObj.justCreated)
                {
                    aoeObj.justCreated = false;
                    //检测进入的演员
                    var enterActors = Sensor.GetActorsInRange(aoeObj);
                    ExecuteActorEnterFunc(aoeObj, enterActors);
                    aoeObj.actorInRange.AddRange(enterActors);

                    //检测进入的子弹
                    var enterBullets = Sensor.GetBulletsInRange(aoeObj);
                    ExecuteBulletEnterFunc(aoeObj, enterBullets);
                    aoeObj.bulletInRange.AddRange(enterBullets);

                    //执行OnCreate
                    ExecuteCreateFunc(aoeObj);
                }
                else
                {
                    UpdateActors(aoeObj);
                    UpdateBullets(aoeObj);
                }

                //Aoe生命周期
                aoeObj.duration     -= timePassed;
                aoeObj.timeElapsed  += timePassed;
                if (aoeObj.duration <= 0)
                {
                    ExecuteRemoveFunc(aoeObj);
                    aoeCom.RemoveAoe(aoeObj);
                    DestroyAoe(aoeObj);
                }
                else
                {
                    ExecuteOnTickFunc(aoeObj);
                }
            }
        }

        private void ExecuteCreateFunc(AoeObj aoeObj)
        {
            if (aoeObj.model.onCreateFunc == null || aoeObj.model.onCreateFunc.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < aoeObj.model.onCreateFunc.Count; i++)
            {
                AoeLifeCycleFunc func = aoeObj.model.onCreateFunc[i];
                func.Execute(aoeObj);
            }
        }

        private void ExecuteRemoveFunc(AoeObj aoeObj)
        {
            if (aoeObj.model.onRemovedFunc == null || aoeObj.model.onRemovedFunc.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < aoeObj.model.onRemovedFunc.Count; i++)
            {
                AoeLifeCycleFunc func = aoeObj.model.onRemovedFunc[i];
                func.Execute(aoeObj);
            }
        }

        private void ExecuteOnTickFunc(AoeObj aoeObj)
        {
            if (aoeObj.model.tickTime <= 0)
                return;
            if (aoeObj.model.onTickFunc == null || aoeObj.model.onTickFunc.Count <= 0)
                return;
            if (Mathf.RoundToInt(aoeObj.duration * 1000) % Mathf.RoundToInt(aoeObj.model.tickTime * 1000) != 0)
                return;
            for (int i = 0; i < aoeObj.model.onTickFunc.Count; i++)
            {
                AoeLifeCycleFunc func = aoeObj.model.onTickFunc[i];
                func.Execute(aoeObj);
            }
        }

        private void DestroyAoe(AoeObj aoeObj)
        {
            Debug.LogWarning("目前Aoe直接销毁》》》》！！！！");
        }

        #region 演员进入离开

        private void UpdateActors(AoeObj aoeObj)
        {
            //离开的演员
            List<ActorObj> leaveActors = new List<ActorObj>();
            for (int i = 0; i < aoeObj.actorInRange.Count; i++)
            {
                ActorObj actor = aoeObj.actorInRange[i];
                if (!Sensor.CheckActorInRange(aoeObj, actor))
                    leaveActors.Add(actor);
            }
            for (int a = 0; a < leaveActors.Count; a++)
                aoeObj.actorInRange.Remove(leaveActors[a]);
            ExecuteActorLeaveFunc(aoeObj, leaveActors);

            //新进入的演员
            List<ActorObj> enterActors = new List<ActorObj>();
            foreach (var item in Sensor.GetActorsInRange(aoeObj))
            {
                if (aoeObj.actorInRange.IndexOf(item) < 0)
                    enterActors.Add(item);
            }
            ExecuteActorEnterFunc(aoeObj, enterActors);
            for (int a = 0; a < enterActors.Count; a++)
                aoeObj.actorInRange.Add(enterActors[a]);
        }

        private void ExecuteActorLeaveFunc(AoeObj aoeObj, List<ActorObj> leaveActors)
        {
            if (aoeObj.model.onActorLeaveFunc == null || aoeObj.model.onActorLeaveFunc.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < aoeObj.model.onActorLeaveFunc.Count; i++)
            {
                AoeActorLeave func = aoeObj.model.onActorLeaveFunc[i];
                func.Execute(aoeObj, leaveActors);
            }
        }

        private void ExecuteActorEnterFunc(AoeObj aoeObj, List<ActorObj> enterActors)
        {
            if (aoeObj.model.onActorEnterFunc == null || aoeObj.model.onActorEnterFunc.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < aoeObj.model.onActorEnterFunc.Count; i++)
            {
                AoeActorEnter func = aoeObj.model.onActorEnterFunc[i];
                func.Execute(aoeObj, enterActors);
            }
        }

        #endregion

        #region 子弹进入离开

        private void UpdateBullets(AoeObj aoeObj)
        {
            //离开的演员
            List<BulletObj> leaveBullets = new List<BulletObj>();
            for (int i = 0; i < aoeObj.bulletInRange.Count; i++)
            {
                BulletObj bullet = aoeObj.bulletInRange[i];
                if (!Sensor.CheckBulletInRange(aoeObj, bullet))
                    leaveBullets.Add(bullet);
            }
            for (int a = 0; a < leaveBullets.Count; a++)
                aoeObj.bulletInRange.Remove(leaveBullets[a]);
            ExecuteBulletLeaveFunc(aoeObj, leaveBullets);

            //新进入的演员
            List<BulletObj> enterBullets = new List<BulletObj>();
            foreach (var item in Sensor.GetBulletsInRange(aoeObj))
            {
                if (aoeObj.bulletInRange.IndexOf(item) < 0)
                    enterBullets.Add(item);
            }
            ExecuteBulletEnterFunc(aoeObj, enterBullets);
            for (int a = 0; a < enterBullets.Count; a++)
                aoeObj.bulletInRange.Add(enterBullets[a]);
        }

        private void ExecuteBulletLeaveFunc(AoeObj aoeObj, List<BulletObj> leaveBullets)
        {
            if (aoeObj.model.onBulletLeaveFunc == null || aoeObj.model.onBulletLeaveFunc.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < aoeObj.model.onBulletLeaveFunc.Count; i++)
            {
                AoeBulletLeave func = aoeObj.model.onBulletLeaveFunc[i];
                func.Execute(aoeObj, leaveBullets);
            }
        }

        private void ExecuteBulletEnterFunc(AoeObj aoeObj, List<BulletObj> enterBullets)
        {
            if (aoeObj.model.onBulletEnterFunc == null || aoeObj.model.onBulletEnterFunc.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < aoeObj.model.onBulletEnterFunc.Count; i++)
            {
                AoeBulletEnter func = aoeObj.model.onBulletEnterFunc[i];
                func.Execute(aoeObj, enterBullets);
            }
        }

        #endregion
    }
}
