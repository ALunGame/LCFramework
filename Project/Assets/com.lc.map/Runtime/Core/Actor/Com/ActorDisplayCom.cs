using LCECS.Core;
using LCToolkit;
using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace LCMap
{
    public class ActorDisplayCom : BaseCom
    {
        #region Const
        [NonSerialized] public const string DefaultName = "Default";
        [NonSerialized] public const string StateRootName = "State";
        [NonSerialized] public const string DisplayGoName = "Display";
        [NonSerialized] public const string CameraFollowGoName = "Camera_Follow";
        #endregion

        [NonSerialized] private GameObject bindGo;
        [NonSerialized] private BindableValue<string> stateName = new BindableValue<string>();
        public string StateName { get { return stateName.Value; } }

        //状态节点
        public GameObject StateGo { get; private set; }
        //表现节点
        public GameObject DisplayGo { get; private set; }
        //相机跟随节点
        public GameObject CameraFollowGo { get; private set; }

        //点击区域
        public PolygonCollider2D ClickCollider { get; private set; }
        //检测区域
        public BoxCollider2D BodyCollider { get; private set; }

        protected override void OnAwake(Entity pEntity)
        {
            BindGoCom bindGoCom = pEntity.GetCom<BindGoCom>();
            if (bindGoCom != null)
            {
                bindGoCom.RegGoChange(OnBindGoChange);
            }
        }

        private void OnBindGoChange(GameObject pGo)
        {
            bindGo = pGo;
            ChangeState(stateName.Value);
            stateName.ValueChanged();
        }

        private void ChangeState(string pStateName)
        {
            if (bindGo == null)
            {
                StateGo = null;
                DisplayGo = null;
                CameraFollowGo = null;
                stateName.SetValueWithoutNotify(pStateName);
                return;
            }

            //没有状态节点，直接默认
            Transform stateRoot = bindGo.transform.Find(StateRootName);
            if (stateRoot == null)
            {
                StateGo = bindGo.gameObject;
                DisplayGo = bindGo.gameObject;
                CameraFollowGo = bindGo.gameObject;
                UpdateCollider();
                stateName.Value = bindGo.name;
                return;
            }

            //隐藏旧的
            stateRoot.SetActive(stateName.Value,false);
            
            //没有这个状态走第一个默认
            if (!stateRoot.Find(pStateName,out Transform checkTrans))
            {
                pStateName = stateRoot.GetChild(0).name;
            }
            
            //赋值新的
            if (stateRoot.Find(pStateName,out Transform newStateTrans))
            {
                StateGo         = newStateTrans.gameObject;

                //表现节点
                if (StateGo.transform.Find(DisplayGoName, out Transform newDisplayTrans))
                    DisplayGo = newDisplayTrans.gameObject;
                else
                    DisplayGo = StateGo;

                //跟随节点
                if (StateGo.transform.Find(CameraFollowGoName, out Transform newCMFollowTrans))
                    CameraFollowGo = newCMFollowTrans.gameObject;
                else
                    CameraFollowGo = StateGo;

                StateGo.SetActive(true);
            }
            else
            {
                MapLocate.Log.LogError("设置状态节点出错>>>", bindGo.name, pStateName);
            }

            UpdateCollider();
            //数据更新
            stateName.Value = pStateName;
        }

        private void UpdateCollider()
        {
            if (DisplayGo.transform.Find("ClickBox") != null)
            {
                ClickCollider = DisplayGo.transform.Find("ClickBox").GetComponent<PolygonCollider2D>();
            }
            else
            {
                ClickCollider = null;
            }
            if (DisplayGo.transform.Find("BodyCollider") != null)
            {
                BodyCollider = DisplayGo.transform.Find("BodyCollider").GetComponent<BoxCollider2D>();
            }
            else
            {
                BodyCollider = null;
            }
        }

        public void SetState(string pStateName)
        {
            if (stateName.Value == pStateName)
                return;
            ChangeState(pStateName);
        }

        public bool HasState(string pStateName)
        {
            if (bindGo.Find(StateRootName, out Transform stateRoot))
            {
                if (stateRoot.Find(pStateName, out Transform newStateTrans))
                {
                    return true;
                }
            }

            return false;
        }

        public void RegStateChange(Action<string> callBack)
        {
            stateName.RegisterValueChangedEvent(callBack);
        }

        public void ClearStateChange()
        {
            stateName.ClearChangedEvent();
        }
    }
}
