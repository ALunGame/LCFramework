﻿using System;
using Demo.UI;
using UnityEngine;

namespace LCUI
{
    public class UIBtnGlue : UIGlue
    {
        private string _ComPath;
        private Transform _ComTrans;
        private Action _BtnCallBack;
        
        
        public UIBtnGlue(string pComPath, Action pBtnCallBack)
        {
            _BtnCallBack = pBtnCallBack;
            _ComPath = pComPath; 
        }

        public override void OnAwake(InternalUIPanel panel)
        {
            base.OnAwake(panel);
            RefreshBind();
        }

        public override void OnBeforeShow(InternalUIPanel panel)
        {
            base.OnBeforeShow(panel);
            BtnUtil.SetClick(_ComTrans,null,_BtnCallBack);
        }

        public override void OnHide(InternalUIPanel panel)
        {
            base.OnHide(panel);
            BtnUtil.ClearClick(_ComTrans,null);
        }

        private void RefreshBind()
        {
            if (_Panel == null || _Panel.transform == null)
            {
                UILocate.Log.LogError("组件绑定失败，界面被销毁！！");
                return;
            }
            if (string.IsNullOrEmpty(_ComPath))
            {
                _ComTrans = _Panel.transform;
                return;
            }
            Transform trans = _Panel.transform.Find(_ComPath);
            if (trans == null)
            {
                UILocate.Log.LogError("组件绑定失败，路径非法！！", _ComPath);
                return;
            }
            _ComTrans = trans;
        }
    }
}