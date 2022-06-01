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
    public enum AnimDefaultState
    {
        Idle,
        Run,
        Dead,
        JumpUp,
        JumpDown,
        Dash,
        Climb,
        DoTrigger,                //正在执行触发动画
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
        public List<string> AnimParamList = new List<string>();

        //[NonSerialized]
        //private string ReqAnimName = AnimSystem.IdleState;
        [NonSerialized]
        private BindableValue<string> ReqAnimName = new BindableValue<string>();

        protected override void OnInit(GameObject go)
        {
            ReqAnimName.Value = AnimSystem.IdleState;

            ActorObj actorObj = go.GetComponent<ActorObj>();
            actorObj.OnDisplayGoChange += OnDisplayGoChange;
            OnDisplayGoChange(actorObj);
        }

        private void OnDisplayGoChange(ActorObj actorObj)
        {
            Transform animTrans = actorObj.GetDisplayGo().transform.Find("Anim");
            if (animTrans == null)
            {
                Anim = null;
                AnimParamList = new List<string>();
            }
            else
            {
                Anim = animTrans.GetComponent<Animator>();
                AnimParamList = AnimHelp.GetAllParamNames(Anim);
            }
        }

        public void SetDefaultAnim()
        {
            ReqAnimName.Value = AnimSystem.IdleState;
        }

        public void SetReqAnim(string animName)
        {
            ReqAnimName.Value = animName;
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
    }
}
