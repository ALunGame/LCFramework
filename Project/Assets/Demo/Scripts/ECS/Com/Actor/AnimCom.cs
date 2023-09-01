using Demo.Help;
using Demo.System;
using LCECS.Core;
using LCMap;
using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using LCToolkit;

namespace Demo.Com
{
    public enum AnimLayer
    {
        Front,
        Back,
        Side,
    }
    
    public class AnimCom : BaseCom
    {
        public const string StateExName = "_state";
        
        private BindableValue<string> ReqAnimName = new BindableValue<string>();
        private BindableValue<AnimLayer> ReqAnimLayer = new BindableValue<AnimLayer>();
        
        public Animator Anim;
        
        public AnimatorOverride AnimOverride;
        
        public List<string> AnimParamList = new List<string>();
        public Dictionary<AnimLayer,Dictionary<string,float>>  AnimTimeDict = new Dictionary<AnimLayer,Dictionary<string,float>>();
        
        private CancellationTokenSource cancelSource;

        /// <summary>
        /// 当前动画状态
        /// </summary>
        public string CurrAnimName { get; set; }

        protected override void OnAwake(Entity pEntity)
        {
            ReqAnimName.Value = AnimSystem.IdleState;
            ReqAnimLayer.Value = AnimLayer.Side;

            ActorDisplayCom displayCom = pEntity.GetCom<ActorDisplayCom>();
            if (displayCom != null)
            {
                displayCom.RegStateChange((stateName) =>
                {
                    OnDisplayGoChange(displayCom.DisplayGo);
                });
            }
        }

        private void OnDisplayGoChange(GameObject displayGo)
        {
            Transform animTrans = displayGo.transform;
            if (animTrans == null)
            {
                Anim = null;
                AnimOverride = null;
                AnimParamList = new List<string>();
                AnimTimeDict = new Dictionary<AnimLayer,Dictionary<string,float>>();
            }
            else
            {
                Anim = animTrans.GetComponent<Animator>();
                AnimOverride = animTrans.GetComponent<AnimatorOverride>();
                AnimParamList = AnimHelp.GetAllParamNames(Anim);
                AnimTimeDict = new Dictionary<AnimLayer,Dictionary<string,float>>();
                
                if (AnimOverride == null)
                {
                    AnimTimeDict.Add(AnimLayer.Side,new Dictionary<string, float>());
                    foreach (string param in AnimParamList)
                    {
                        if (param.Contains(StateExName))
                        {
                            continue;
                        }
                        AnimTimeDict[AnimLayer.Side].Add(param,AnimHelp.GetClipTime(Anim,param));
                    }
                }
                else
                {
                    
                }
            }
        }

        protected override void OnDestroy()
        {
            ReqAnimName.ClearChangedEvent();
            ReqAnimLayer.ClearChangedEvent();
            ClearTask();
        }

        protected override void OnDisable()
        {
            ReqAnimName.ClearChangedEvent();
            ReqAnimLayer.ClearChangedEvent();
            ClearTask();
        }

        public void SetDefaultAnim()
        {
            ReqAnimName.Value = AnimSystem.IdleState;
        }

        public void PlayAnim(string animName, AnimLayer layer = AnimLayer.Side)
        {
            ClearTask();
            
            ReqAnimName.Value = animName;
            ReqAnimLayer.Value = layer;
        }

        public async UniTaskVoid PlayAnimCnt(string animName, AnimLayer layer, int pCnt, Action pPreCallBack, Action pFinishCallBack)
        {
            ReqAnimLayer.Value = layer;

            ClearTask();

            cancelSource = new CancellationTokenSource();
            
            float clipTime = GetAnimClipTime(animName, layer);
            for (int i = 0; i < pCnt; i++)
            {
                ReqAnimName.Value = animName;
                pPreCallBack?.Invoke();
                
                bool isCanceled = await UniTask.Delay(TimeSpan.FromSeconds(clipTime),false,PlayerLoopTiming.Update,cancelSource.Token).SuppressCancellationThrow();
                if (isCanceled)
                {
                    return;
                }
            }
            
            pFinishCallBack?.Invoke();
        }

        private void ClearTask()
        {
            if (cancelSource == null)
            {
                return;
            }
            cancelSource.Cancel();
            cancelSource.Dispose();
        }

        public string GetReqAnim()
        {
            return ReqAnimName.Value;
        }

        public float GetAnimClipTime(string animName, AnimLayer layer = AnimLayer.Side)
        {
            if (Anim == null)
            {
                return 0;
            }

            if (AnimTimeDict.ContainsKey(layer) && AnimTimeDict[layer].ContainsKey(animName))
            {
                return AnimTimeDict[layer][animName];
            }
            
            if (AnimOverride == null)
            {
                if (!AnimTimeDict.ContainsKey(AnimLayer.Side))
                    AnimTimeDict.Add(AnimLayer.Side,new Dictionary<string, float>());

                float time = AnimHelp.GetClipTime(Anim,animName);
                AnimTimeDict[AnimLayer.Side].Add(animName,time);
                return time;
            }
            else
            {
                if (!AnimTimeDict.ContainsKey(layer))
                    AnimTimeDict.Add(layer,new Dictionary<string, float>());
                float time = AnimOverride.GetClipTime(animName,layer);
                AnimTimeDict[layer].Add(animName,time);
                return time;
            }
        }

        public void RegReqAnimChange(Action<string> callBack)
        {
            ReqAnimName.RegisterValueChangedEvent(callBack);
        }

        public void RegReqAnimLayerChange(Action<AnimLayer> callBack)
        {
            ReqAnimLayer.RegisterValueChangedEvent(callBack);
        }
    }
}
