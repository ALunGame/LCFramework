using LCECS.Core;
using LCMap;
using System;
using UnityEngine;

namespace Demo.Com
{
    public enum DirType
    {
        None,
        Left,
        Right,
    }

    public class TransformCom : BaseCom
    {
        //正朝向
        public DirType ForwardDir = DirType.Right;

        [NonSerialized]
        public DirType CurrDir = DirType.None;

        [NonSerialized]
        public DirType ReqDir;

        /// <summary>
        /// 创建时的坐标
        /// </summary>
        public Vector3 CreatePos { get;private set; }

        [NonSerialized]
        public Transform Trans;

        [NonSerialized]
        public Transform DisplayRootTrans;

        [NonSerialized]
        public Transform DisplayTrans;

        protected override void OnInit(GameObject go)
        {
            Trans = go.transform;
            CreatePos = go.transform.position;
            ActorObj actorObj = go.GetComponent<ActorObj>();
            actorObj.OnDisplayGoChange += OnDisplayGoChange;
            OnDisplayGoChange(actorObj);
        }

        private void OnDisplayGoChange(ActorObj actorObj)
        {
            DisplayRootTrans = actorObj.GetDisplayRootGo().transform;
            DisplayTrans = actorObj.GetDisplayGo().transform;
        }
    }
}
