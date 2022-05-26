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

        /// <summary>
        /// 创建时的坐标
        /// </summary>
        public Vector3 CreatePos { get;private set; }

        public Vector3 CurrPos;

        [NonSerialized]
        private Transform Trans;

        [NonSerialized]
        public Transform DisplayRootTrans;

        [NonSerialized]
        public Transform DisplayTrans;

        [NonSerialized]
        public DirType ReqDir;
        [NonSerialized]
        public Vector3 ReqMove;

        protected override void OnInit(GameObject go)
        {
            Trans = go.transform;
            CreatePos = go.transform.position;
            CurrPos = CreatePos;

            ActorObj actorObj = go.GetComponent<ActorObj>();
            actorObj.OnDisplayGoChange += OnDisplayGoChange;
            OnDisplayGoChange(actorObj);
        }

        private void OnDisplayGoChange(ActorObj actorObj)
        {
            DisplayRootTrans = actorObj.GetDisplayRootGo().transform;
            DisplayTrans = actorObj.GetDisplayGo().transform;
        }

        public Vector2 GetPos()
        {
            return CurrPos;
        }

        public void Translate(Vector3 delta)
        {
            Trans.Translate(delta, Space.World);
            CurrPos = Trans.position;
        }

        public void UpdatePos()
        {
            CurrPos = Trans.position;
        }
    }
}
