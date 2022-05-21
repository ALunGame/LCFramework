using Demo.Com;
using DG.Tweening;
using LCECS;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.System
{
    public class EffectInfo
    {
        public int Id;
        public string Des = "";
        public bool FollowObj = false;
        public bool FixDir = false;
        public string Prefab = "";
    }

    //特效系统
    public class EffectSystem : BaseSystem
    {
        private Dictionary<int, EffectInfo> effectDict = new Dictionary<int, EffectInfo>();
        private EffectCom effectCom;
        private GameObjectCom goCom;

        protected override List<Type> RegListenComs()
        {
            OnInit();
            return new List<Type>() { typeof(EffectCom), typeof(GameObjectCom) };
        }

        private void OnInit()
        {
            //LCConfig.Config effectConfig = LCConfigLocate.GetConfig("Effect");

            //for (int i = 0; i < effectConfig.Items[0].DataList.Count; i++)
            //{
            //    int dataIndex = i;
            //    EffectInfo effectInfo = new EffectInfo();
            //    for (int j = 0; j < effectConfig.Items.Count; j++)
            //    {
            //        ConfigItem configItem = effectConfig.Items[j];
            //        if (configItem.Name=="Id")
            //        {
            //            effectInfo.Id = (int)LCToolkit.LCConvert.StrChangeToObject(configItem.DataList[dataIndex],typeof(int).FullName);
            //        }
            //        if (configItem.Name == "Des")
            //        {
            //            effectInfo.Des = (string)LCToolkit.LCConvert.StrChangeToObject(configItem.DataList[dataIndex], typeof(string).FullName);
            //        }
            //        if (configItem.Name == "FollowObj")
            //        {
            //            effectInfo.FollowObj = (bool)LCToolkit.LCConvert.StrChangeToObject(configItem.DataList[dataIndex], typeof(bool).FullName);
            //        }
            //        if (configItem.Name == "FixDir")
            //        {
            //            effectInfo.FixDir = (bool)LCToolkit.LCConvert.StrChangeToObject(configItem.DataList[dataIndex], typeof(bool).FullName);
            //        }
            //        if (configItem.Name == "Prefab")
            //        {
            //            effectInfo.Prefab = (string)LCToolkit.LCConvert.StrChangeToObject(configItem.DataList[dataIndex], typeof(string).FullName);
            //        }
            //    }
            //    effectDict.Add(effectInfo.Id, effectInfo);
            //}
            
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            if (effectCom == null)
                effectCom = GetCom<EffectCom>(comList[0]);
            if (goCom == null)
                goCom = GetCom<GameObjectCom>(comList[1]);
            HandleEffect(effectCom);
            RecycleEffect(effectCom);
        }

        private void HandleEffect(EffectCom effectCom)
        {
            if (effectCom.EffectId <= 0)
            {
                return;
            }
            //冲刺拖尾
            if (effectCom.EffectId == 5004)
            {
                HandleDashEffect(effectCom);
            }
            else
            {
                HandleNormalEffect(effectCom);
            }

            effectCom.EffectId = 0;
        }

        //回收特效
        private void RecycleEffect(EffectCom effectCom)
        {
            foreach (var item in effectCom.CurrShowEffects.Keys)
            {
                EffectData effectData = effectCom.CurrShowEffects[item];
                for (int i = 0; i < effectData.EffectGos.Count; i++)
                {
                    //超时
                    if (Time.time > effectData.EffectGos[i].Time + effectData.EffectGos[i].HideTime)
                    {
                        PushEffectGoInCache(effectCom, effectData.Info.Id, effectData.EffectGos[i]);
                        effectData.EffectGos.RemoveAt(i);
                    }
                }
            }
        }

        #region 公共方法
        //将特效加入缓存
        public void PushEffectGoInCache(EffectCom effectCom, int effectId, EffectGo effectGo)
        {
            effectGo.Go.SetActive(false);
            if (effectCom.CacheEffects.ContainsKey(effectId))
            {
                effectCom.CacheEffects[effectId].EffectGos.Add(effectGo);
            }
            else
            {
                EffectData effectData = new EffectData();
                effectData.Info = GetEffectInfo(effectId);
                effectData.EffectGos.Add(effectGo);
                effectCom.CacheEffects.Add(effectId, effectData);
            }
        }

        //获得创建的特效节点
        private EffectGo GetEffectGo(EffectCom effectCom, int effectId, ref EffectInfo info)
        {
            //创建或者从缓存中拿到预制体
            EffectGo effectGo;
            if (effectCom.CacheEffects.ContainsKey(effectId))
            {
                EffectData effect = effectCom.CacheEffects[effectId];
                effectGo = effect.EffectGos[0];
                effect.EffectGos.RemoveAt(0);
                if (effect.EffectGos.Count <= 0)
                {
                    effectCom.CacheEffects.Remove(effectId);
                }
            }
            else
            {
                effectGo = CreateEffectGo(effectId);
            }
            

            //保存在当前显示的字典中
            if (effectCom.CurrShowEffects.ContainsKey(effectCom.EffectId))
            {
                info = effectCom.CurrShowEffects[effectCom.EffectId].Info;
                effectCom.CurrShowEffects[effectCom.EffectId].EffectGos.Add(effectGo);
            }
            else
            {
                EffectData effectData = new EffectData();
                effectData.Info = GetEffectInfo(effectCom.EffectId);
                effectData.EffectGos.Add(effectGo);
                effectCom.CurrShowEffects.Add(effectCom.EffectId, effectData);
                info = effectData.Info;
            }

            effectGo.Time = Time.time;
            effectGo.HideTime = effectCom.EffectHideTime;
            effectGo.Go.SetActive(true);
            return effectGo;
        }

        private EffectGo CreateEffectGo(int effectId)
        {
            EffectInfo effectInfo = GetEffectInfo(effectId);
            if (effectInfo == null)
            {
                return null;
            }

            GameObject assetGo = null;

            EffectGo effectGo = new EffectGo
            {
                Time = Time.time,
                Go   = GameObject.Instantiate(assetGo), 
            };
            effectGo.Go.SetActive(false);
            effectGo.Go.transform.SetParent(goCom.Tran);
            effectGo.Go.transform.position = Vector3.zero;
            return effectGo;
        }

        private EffectInfo GetEffectInfo(int effectId)
        {
            if (effectDict.ContainsKey(effectId))
            {
                return effectDict[effectId];
            }
            return null;
        }
        #endregion

        #region 处理冲刺拖尾特效
        private void HandleDashEffect(EffectCom effectCom)
        {
            Entity entity = LCECS.ECSLocate.ECS.GetEntity(effectCom.EffectEntityId);
            GameObjectCom goCom = entity.GetCom<GameObjectCom>();
            Transform entityTran = goCom.Tran;

            //获得特效节点
            EffectInfo info = null;
            EffectGo effectGo = GetEffectGo(effectCom, effectCom.EffectId, ref info);

            //动画队列
            Sequence s = DOTween.Sequence();
            for (int i = 0; i < effectGo.Go.transform.childCount; i++)
            {
                Transform dashGo = effectGo.Go.transform.GetChild(i);
                dashGo.gameObject.SetActive(false);
                s.AppendCallback(() => InitDashGo(entity, dashGo, i));
                //间隔后，才赋值位置
                s.AppendInterval(effectCom.EffectGapTime);
            }
        }

        //初始化冲刺的节点
        private void InitDashGo(Entity entity, Transform dashGo, int index)
        {
            //SpriteRenderer sp = entity.GetCom<AnimCom>().SpriteRender;
            //SpriteRenderer dashSp = dashGo.GetComponent<SpriteRenderer>();
            ////大小
            //dashGo.transform.localScale = entity.GetCom<GameObjectCom>().Tran.localScale;
            ////位置
            //dashGo.transform.position = entity.GetCom<GameObjectCom>().Tran.position;
            ////方向
            //dashSp.flipX = sp.flipX;
            ////图片
            //dashSp.sprite = sp.sprite;
            //dashGo.gameObject.SetActive(true);
        }

        //渐隐冲刺节点
        private void DashFadeSprite(Transform current, float hideTime)
        {
            current.GetComponent<SpriteRenderer>().DOColor(new Color(255, 255, 255, 0), hideTime);
        }
        #endregion

        #region 处理一般的特效

        private void HandleNormalEffect(EffectCom effectCom)
        {
            Entity entity = LCECS.ECSLocate.ECS.GetEntity(effectCom.EffectEntityId);
            GameObjectCom goCom = entity.GetCom<GameObjectCom>();
            Transform entityTran = goCom.Tran;

            //获得特效节点
            EffectInfo info = null;
            EffectGo effectGo = GetEffectGo(effectCom, effectCom.EffectId, ref info);

            if (info.FixDir)
            {
                SpriteRenderer renderer = effectGo.Go.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.flipX = effectCom.EffectDir;
                }
            }

            //设置位置
            effectGo.Go.transform.position = effectCom.EffectPos;
            effectGo.Go.SetActive(true);
        }
        #endregion
    }
}
