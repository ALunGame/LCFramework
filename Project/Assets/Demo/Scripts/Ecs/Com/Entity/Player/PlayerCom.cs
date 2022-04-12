using LCECS.Core;
using UnityEngine;

namespace Demo.Com
{
    /// <summary>
    /// 玩家组件
    /// 1,这边会保存一些临时的状态（玩家本来就很特殊）
    /// </summary>
    [Com(ViewName = "玩家组件", GroupName = "Player")]
    public class PlayerCom : BaseCom
    {
        public Transform Trans;
        public SpriteRenderer SpriteRender;
        public Transform WaveTrans;

        //能量
        [ComValue]
        public float CurrEnergy         = 0;
        [ComValue]
        public float ReplyEnergy        = 10;
        [ComValue(ViewEditor = true)]
        public float MaxEnergy          = 200;

        //冲刺
        [ComValue]
        public bool DoDash              = false;
        [ComValue]
        public bool IsDash              = false;
        [ComValue(ViewEditor = true)]
        public float DashDragTime       = 0.5f;

        //几段跳
        [ComValue]
        public int CurrJumpIndex        = 0;
        [ComValue(ViewEditor = true)]
        public int MaxJumpIndex         = 1;

        //各阶段降落
        [ComValue(ViewEditor = true)]
        public float FallMultiplier     = 1.5f;
        [ComValue(ViewEditor = true)]
        public float LowJumpMultiplier  = 1f;

        protected override void OnInit(GameObject go)
        {
            SpriteRender = go.transform.Find("Anim").GetComponent<SpriteRenderer>();
            Trans = go.transform;
            WaveTrans = go.transform.Find("Wave");
        }
    }
}