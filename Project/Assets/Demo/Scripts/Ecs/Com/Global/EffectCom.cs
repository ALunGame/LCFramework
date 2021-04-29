using Demo.Config;
using Demo.System;
using LCECS.Core.ECS;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Com
{
    /// <summary>
    /// 特效组件（全局唯一）
    /// </summary>
    [Com(ViewName = "特效组件", IsGlobal = true)]
    public class EffectCom : BaseCom
    {
        //参数
        public int EffectId = 0;
        public int EffectEntityId = 0;
        public Vector3 EffectPos = Vector3.zero;
        public bool EffectDir = false;
        public float EffectHideTime = 0;
        public float EffectGapTime = 0;

        public Dictionary<int, EffectData> CurrShowEffects = new Dictionary<int, EffectData>();
        public Dictionary<int, EffectData> CacheEffects = new Dictionary<int, EffectData>();
    }

    public class EffectData
    {
        public Demo.System.EffectInfo Info;
        public List<EffectGo> EffectGos = new List<EffectGo>();
    }

    public class EffectGo
    {
        public float Time;
        public float HideTime;
        public GameObject Go;
    }
}
