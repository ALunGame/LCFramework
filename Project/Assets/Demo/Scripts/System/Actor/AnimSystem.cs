using Demo.Com;
using Demo.Help;
using LCECS.Core;
using System;
using System.Collections.Generic;

namespace Demo.System
{
    public class AnimSystem : BaseSystem
    {
        public const string IdleState = "idle";

        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(AnimCom)};
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
        }

        protected override void OnAddCheckComs(List<BaseCom> comList)
        {
            AnimCom animCom = GetCom<AnimCom>(comList[0]);
            animCom.RegReqAnimChange((string x) =>
            {
                string reqAnim = animCom.GetReqAnim();
                if (string.IsNullOrEmpty(reqAnim))
                    return;

                string currAnim = animCom.CurrAnimName;
                if (CheckIsLoopAnim(animCom, reqAnim))
                {
                    if (reqAnim == currAnim)
                    {
                        return;
                    }
                    if (!string.IsNullOrEmpty(currAnim))
                    {
                        PlayLoopAnim(animCom, currAnim, false);
                    }
                    animCom.CurrAnimName = reqAnim;
                    PlayLoopAnim(animCom, reqAnim, true);
                }
                else
                {
                    if (CheckIsLoopAnim(animCom, currAnim))
                    {
                        PlayLoopAnim(animCom, currAnim, false);
                    }
                    animCom.CurrAnimName = reqAnim;
                    PlayTriggerAnim(animCom, reqAnim);
                }
            });
        }

        protected override void OnRemoveCheckComs(List<BaseCom> comList)
        {
            AnimCom animCom = GetCom<AnimCom>(comList[0]);
            animCom.ClearReqAnimChangeCallBack();
        }

        private bool CheckIsLoopAnim(AnimCom animCom,string animName)
        {
            if (IdleState == animName)
            {
                return true;
            }
            string stateName = animName + AnimCom.StateExName;
            return animCom.AnimParamList.Contains(stateName);
        }

        private void PlayLoopAnim(AnimCom animCom, string animName,bool animState)
        {
            if (AnimHelp.CheckIsInState(animCom.Anim,animName) && animState)
            {
                return;
            }
            if (animName == IdleState)
            {
                animCom.Anim.SetBool(IdleState, animState);
                return;
            }

            string stateName = animName + AnimCom.StateExName;
            if (animCom.AnimParamList.Contains(stateName))
            {
                animCom.Anim.SetBool(stateName, animState);
                animCom.Anim.SetTrigger(animName);
            }
        }

        private void PlayTriggerAnim(AnimCom animCom, string animName)
        {
            animCom.Anim.SetTrigger(animName);
        }
    }
}
