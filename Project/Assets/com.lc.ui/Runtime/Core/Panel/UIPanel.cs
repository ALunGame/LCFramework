namespace LCUI
{
    public class UIPanel<T> : InternalUIPanel where T : UIModel, new()
    {
        /// <summary>
        /// 绑定数据
        /// </summary>
        public T BindModel
        { 
            get 
            {
                if (_Model == null)
                {
                    _Model = new T();
                }
                return (T)_Model; 
            } 
        }

        /// <summary>
        /// 创建时初始化
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            for (int i = 0; i < Glues.Count; i++)
            {
                Glues[i].OnAwake(this);
            }
            OnAwake();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public override void Show()
        {
            base.Show();
            for (int i = 0; i < Glues.Count; i++)
            {
                Glues[i].OnBeforeShow(this);
            }
            OnShow();
            for (int i = 0; i < Glues.Count; i++)
            {
                Glues[i].OnAfterShow(this);
            }
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public override void Hide()
        {
            base.Hide();
            BindModel.ClearEvent();
            for (int i = 0; i < Glues.Count; i++)
            {
                Glues[i].OnHide(this);
            }
            OnHide();
        }

        public override void Destroy()
        {
            base.Destroy();
            BindModel.ClearEvent();
            for (int i = 0; i < Glues.Count; i++)
            {
                Glues[i].OnDestroy(this);
            }
            OnDestroy();
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

        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void OnDestroy()
        {

        }

        #endregion
    }
}
