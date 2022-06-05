using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;

namespace Demo.UI
{
    public static class TweenUtil
    { 
        public static void Clear(Tween tween)
        {
            if (tween == null)
                return;
            tween.onComplete = null;
            tween.onUpdate = null;
            tween.Pause();
            tween.Kill();
        }

        public static Tween DoDelayFunc(Action callBack, float delayTime = 0.1f)
        {
            float timeCount = delayTime;
            return DOTween.To(() => timeCount, a => timeCount = a, 0.1f, delayTime).OnComplete(new TweenCallback(delegate
            {
                callBack?.Invoke();
            }));
        }
    }
}