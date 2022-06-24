using Demo.Help;
using Demo.System;
using LCECS.Core;
using LCMap;
using System;
using System.Collections.Generic;
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

    [Serializable]
    public class AnimCom : BaseCom
    {
        [NonSerialized]
        public const string StateExName = "_state";

        [NonSerialized]
        public string CurrAnimName;

        [NonSerialized]
        public Animator Anim;

        [NonSerialized]
        public AnimatorOverride AnimOverride;

        [NonSerialized]
        public List<string> AnimParamList = new List<string>();

        [NonSerialized]
        private BindableValue<string> ReqAnimName = new BindableValue<string>();

        [NonSerialized]
        private BindableValue<AnimLayer> ReqAnimLayer = new BindableValue<AnimLayer>();

        protected override void OnInit(GameObject go)
        {
            ReqAnimName.Value = AnimSystem.IdleState;
            ReqAnimLayer.Value = AnimLayer.Side;

            ActorObj actorObj = go.GetComponent<ActorObj>();
            actorObj.OnDisplayGoChange += OnDisplayGoChange;
            OnDisplayGoChange(actorObj);
        }

        private void OnDisplayGoChange(ActorObj actorObj)
        {
            Transform animTrans = actorObj.GetDisplayGo().transform;
            if (animTrans == null)
            {
                Anim = null;
                AnimOverride = null;
                AnimParamList = new List<string>();
            }
            else
            {
                Anim = animTrans.GetComponent<Animator>();
                AnimOverride = animTrans.GetComponent<AnimatorOverride>();
                AnimParamList = AnimHelp.GetAllParamNames(Anim);
            }
        }

        public void SetDefaultAnim()
        {
            ReqAnimName.Value = AnimSystem.IdleState;
        }

        public void SetReqAnim(string animName, AnimLayer layer = AnimLayer.Side)
        {
            ReqAnimName.Value = animName;
            ReqAnimLayer.Value = layer;
        }

        public string GetReqAnim()
        {
            return ReqAnimName.Value;
        }

        public void RegReqAnimChange(Action<string> callBack)
        {
            ReqAnimName.RegisterValueChangedEvent(callBack);
        }

        public void ClearReqAnimChangeCallBack()
        {
            ReqAnimName.ClearChangedEvent();
        }

        public void RegReqAnimLayerChange(Action<AnimLayer> callBack)
        {
            ReqAnimLayer.RegisterValueChangedEvent(callBack);
        }

        public void ClearReqAnimLayerChangeCallBack()
        {
            ReqAnimLayer.ClearChangedEvent();
        }
    }
}
