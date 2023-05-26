using System;
using System.Collections.Generic;
using Cnf;
using Demo.Config;
using LCECS.Core;
using LCLoad;
using LCMap;
using LCToolkit;
using UnityEngine;

namespace Demo.Com
{
    /// <summary>
    /// 演员武器组件
    /// </summary>
    public class WeaponCom : BaseCom
    {
        [SerializeField] private Transform weaponRootTrans;

        [SerializeField] private BindableValue<int> weaponId = new BindableValue<int>();
        [SerializeField] private Transform weaponTrans;
        [SerializeField] private TimerInfo weaponUseAnimTimer;
        
        protected override void OnAwake(Entity pEntity)
        {
            if (pEntity is Actor)
            {
                Actor actor = pEntity as Actor;
                actor.DisplayCom.RegStateChange((string state) =>
                {
                    if (!actor.DisplayCom.DisplayGo.Find("WeaponRoot",out weaponRootTrans))
                        weaponRootTrans = actor.DisplayCom.DisplayGo.transform;
                });
                if (actor.DisplayCom.DisplayGo != null)
                {
                    if (!actor.DisplayCom.DisplayGo.Find("WeaponRoot",out weaponRootTrans))
                        weaponRootTrans = actor.DisplayCom.DisplayGo.transform;
                }
            }
        }

        protected override void OnDestroy()
        {
            weaponId.ClearChangedEvent();
            GameLocate.TimerServer.StopTimer(weaponUseAnimTimer);
        }

        private void RefreshWeapon()
        {
            if (LCConfig.Config.WeaponCnf.ContainsKey(weaponId.Value))
            {
                WeaponCnf weaponCnf = LCConfig.Config.WeaponCnf[weaponId.Value];
                weaponRootTrans.SetActive(null,true);
                weaponRootTrans.HideAllChild();
                
                if (!weaponRootTrans.Find(weaponCnf.prefab, out weaponTrans))
                {
                    GameObject assetGo = LoadHelper.LoadPrefab(weaponCnf.prefab);
                    GameObject weaponGo = GameObject.Instantiate(assetGo);
                    weaponGo.name = weaponCnf.prefab;
                    weaponGo.transform.SetParent(weaponRootTrans);
                    weaponTrans = weaponGo.transform;
                }
            }
            else
            {
                weaponRootTrans.SetActive(null,false);
                weaponTrans = null;
            }
        }

        private AnimationClip GetClip(string pClipName)
        {
            AnimationClip clip = LCLoad.LoadHelper.Load<AnimationClip>(pClipName);
            return clip;
        }

        public void SetWeapon(int pWeaponId)
        {
            if (pWeaponId == weaponId.Value)
            {
                return;
            }
            if (LCConfig.Config.WeaponCnf.ContainsKey(pWeaponId))
            {
                WeaponCnf weaponCnf = LCConfig.Config.WeaponCnf[pWeaponId];
                weaponRootTrans.SetActive(null,true);
                weaponRootTrans.HideAllChild();
                
                if (!weaponRootTrans.Find(weaponCnf.prefab, out weaponTrans))
                {
                    GameObject assetGo = LoadHelper.LoadPrefab(weaponCnf.prefab);
                    GameObject weaponGo = GameObject.Instantiate(assetGo);
                    weaponGo.name = weaponCnf.prefab;
                    weaponGo.transform.SetParent(weaponRootTrans);
                    weaponGo.transform.ResetNoScale();
                    weaponTrans = weaponGo.transform;
                }
                
                weaponTrans.SetActive(null,true);
                weaponId.Value = pWeaponId;
            }
            else
            {
                GameLocate.Log.LogError("设置武器失败，没有对应武器配置",pWeaponId);
            }
        }

        public void HideWeapon()
        {
            weaponRootTrans.SetActive(null,false);
            weaponTrans = null;
            weaponId.Value = 0;
        }

        public void UseWeapon(int pWeaponId = 0,Action pUseAnimFinishCallBack = null)
        {
            GameLocate.TimerServer.StopTimer(weaponUseAnimTimer);
            if (pWeaponId != 0)
            {
                SetWeapon(pWeaponId);
            }
            if (LCConfig.Config.WeaponCnf.ContainsKey(weaponId.Value))
            {
                WeaponCnf weaponCnf = LCConfig.Config.WeaponCnf[weaponId.Value];
                if (weaponCnf.useSkillId > 0)
                {
                    if (ActorMediator.ReleaseSkill(ActorMediator.GetActor(EntityUid),weaponCnf.useSkillId))
                    {
                        PlayUseAnim(weaponCnf,pUseAnimFinishCallBack);
                    }
                }
                else
                {
                    PlayUseAnim(weaponCnf,pUseAnimFinishCallBack);
                }
            }
            else
            {
                GameLocate.Log.LogError("使用武器失败，当前没有武器",weaponId.Value);
            }
        }

        private void PlayUseAnim(WeaponCnf pWeaponCnf,Action pUseAnimFinishCallBack = null)
        {
            Animation anim = weaponTrans.GetComponent<Animation>();
            AnimationClip clip = anim.GetClip(pWeaponCnf.useAnim);
            if (clip == null)
            {
                clip = LCLoad.LoadHelper.Load<AnimationClip>(pWeaponCnf.useAnim);
                anim.AddClip(clip,pWeaponCnf.useAnim);
            }
            
            anim.Stop();
            anim.Play(pWeaponCnf.useAnim);
            weaponUseAnimTimer = GameLocate.TimerServer.WaitForSeconds(clip.length, pUseAnimFinishCallBack);
        }

        public void RegWeaponChange(Action<int> pWeaponChange)
        {
            weaponId.RegisterValueChangedEvent(pWeaponChange);
        }
    }
}