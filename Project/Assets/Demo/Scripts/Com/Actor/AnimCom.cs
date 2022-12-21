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
            }
            else
            {
                Anim = animTrans.GetComponent<Animator>();
                AnimOverride = animTrans.GetComponent<AnimatorOverride>();
                AnimParamList = AnimHelp.GetAllParamNames(Anim);
            }
        }

        protected override void OnDestroy()
        {
            ReqAnimName.ClearChangedEvent();
            ReqAnimLayer.ClearChangedEvent();
        }

        protected override void OnDisable()
        {
            ReqAnimName.ClearChangedEvent();
            ReqAnimLayer.ClearChangedEvent();
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

        public void RegReqAnimLayerChange(Action<AnimLayer> callBack)
        {
            ReqAnimLayer.RegisterValueChangedEvent(callBack);
        }
    }
}
