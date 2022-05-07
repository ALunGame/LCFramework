using LCECS.Core;
using LCMap;
using System;
using UnityEngine;

namespace Demo.Com
{
    public enum DirType
    {
        Left,
        Right,
    }

    public class TransformCom : BaseCom
    {
        [NonSerialized]
        public DirType CurrDir;

        //正朝向
        public DirType ForwardDir = DirType.Right;

        [NonSerialized]
        public DirType ReqDir;

        [NonSerialized]
        public Transform DisplayRootTrans;

        [NonSerialized]
        public Transform DisplayTrans;

        protected override void OnInit(GameObject go)
        {
            ActorObj actorObj = go.GetComponent<ActorObj>();
            DisplayRootTrans = actorObj.DisplayRootGo.transform;
            actorObj.OnDisplayGoChange += OnDisplayGoChange;
            OnDisplayGoChange(actorObj.DisplayGo);
        }

        private void OnDisplayGoChange(GameObject displayGo)
        {
            DisplayTrans = displayGo.transform;
        }
    }
}
