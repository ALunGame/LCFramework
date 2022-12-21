using Demo.Help;
using Demo.System;
using LCECS.Core;
using LCMap;
using System;
using System.Collections.Generic;
using UnityEngine;
using LCToolkit;
using static UnityEngine.EventSystems.EventTrigger;

namespace Demo.Com
{
    public enum ActorAnimLayer
    {
        Front,          //前
        Back,           //后
        Side,           //左右
    }

    public class ActorAnimCom : BaseCom
    {
        [NonSerialized]
        public const string StateExName = "_state";

        public string DefaultIdleAnim = "";
        public string DefaultRunAnim  = "";
        public string DefaultWalkAnim = "";

        #region NonSerialized

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
        private BindableValue<ActorAnimLayer> ReqAnimLayer = new BindableValue<ActorAnimLayer>();

        [NonSerialized]
        public ActorDisplayCom _actorDisplayCom; 

        #endregion

        protected override void OnAwake(Entity pEntity)
        {
            ActorDisplayCom actorDisplayCom = pEntity.GetCom<ActorDisplayCom>();
            if (actorDisplayCom != null)
            {
                this._actorDisplayCom = actorDisplayCom;
                actorDisplayCom.RegStateChange(OnStateChange);
            }
        }

        private void OnStateChange(string pStateName)
        {
            if (_actorDisplayCom.DisplayGo == null)
            {
                Anim = null;
                AnimOverride = null;
                AnimParamList = new List<string>();
            }
            else
            {
                Anim = _actorDisplayCom.DisplayGo.GetComponent<Animator>();
                AnimOverride = _actorDisplayCom.DisplayGo.GetComponent<AnimatorOverride>();
                AnimParamList = AnimHelp.GetAllParamNames(Anim);
            }
        }

        public void SetReqAnim(string animName, ActorAnimLayer layer = ActorAnimLayer.Side)
        {
            ReqAnimName.Value = animName;
            ReqAnimLayer.Value = layer;
        }

        public void RegReqAnimChange(Action<string> callBack)
        {
            ReqAnimName.RegisterValueChangedEvent(callBack);
        }

        public void ClearReqAnimChangeCallBack()
        {
            ReqAnimName.ClearChangedEvent();
        }

        public void RegReqAnimLayerChange(Action<ActorAnimLayer> callBack)
        {
            ReqAnimLayer.RegisterValueChangedEvent(callBack);
        }

        public void ClearReqAnimLayerChangeCallBack()
        {
            ReqAnimLayer.ClearChangedEvent();
        }
    }
}