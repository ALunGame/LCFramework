using LCToolkit;
using System;
using UnityEngine;

namespace LCECS.Core
{
    /// <summary>
    /// 实体绑定节点
    /// </summary>
    public class BindGoCom : BaseCom
    {
        [NonSerialized]
        private BindableValue<GameObject> go = new BindableValue<GameObject>();

        public GameObject Go { get { return go.Value; } }

        protected override void OnInit(Entity entity)
        {
            base.OnInit(entity);
        }

        public void SetBindGo(GameObject pGo)
        {
            go.Value = pGo;
        }

        public void RegGoChange(Action<GameObject> callBack)
        {
            go.RegisterValueChangedEvent(callBack);
        }

        public void ClearGoChange()
        {
            go.ClearChangedEvent();
        }
    }
}
