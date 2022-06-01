using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace LCUI
{
    public class UIPanel<T> : InternalUIPanel where T : UIModel, new()
    {
        private T bindmodel;

        /// <summary>
        /// 绑定数据
        /// </summary>
        public T BindModel
        { 
            get 
            {
                if (bindmodel == null)
                {
                    bindmodel = new T();
                }
                return bindmodel; 
            } 
        }

        /// <summary>
        /// 创建时初始化
        /// </summary>
        public void Awake()
        {
            OnAwake();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show()
        {
            OnShow();
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            BindModel.ClearEvent();
            OnHide();
        }

        #region Virtual

        /// <summary>
        /// 初始化时
        /// </summary>
        public virtual void OnAwake()
        {

        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void OnShow()
        {

        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void OnHide()
        {

        }

        #endregion
    }
}
