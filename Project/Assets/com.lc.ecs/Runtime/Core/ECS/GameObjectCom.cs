using System;
using UnityEngine;

namespace LCECS.Core
{
    [Serializable]
    public class GameObjectCom : BaseCom
    {
        [NonSerialized]
        public GameObject Go;
        [NonSerialized]
        public Transform Tran;
        [NonSerialized]
        public SpriteRenderer Renderer;

        protected override void OnInit(GameObject go)
        {
            Go          = go;
            Tran        = go.transform;
            Renderer    = go.GetComponent<SpriteRenderer>();
        }
    }
}