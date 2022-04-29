using LCECS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
        public Animator Animtor;
        [NonSerialized]
        public SpriteRenderer SpriteRender;
        [ComValue]
        public AnimDefaultState CurState;
        [ComValue]
        public bool DoTrigger;

        protected override void OnInit(GameObject go)
        {
            Animtor = go.transform.Find("Anim").GetComponent<Animator>();
            SpriteRender = Animtor.GetComponent<SpriteRenderer>();
        }
    }
}
