using System;

namespace LCECS.Core
{
    public class BaseCom
    {
        [NonSerialized]
        private string entityUid = "";

        //为了序列化，运行时不可设置
        public bool isActive = true;
        public bool IsActive { get => isActive;}

        public string EntityUid { get => entityUid; }

        public string GetComName()
        {
            return this.GetType().Name;
        }

        //初始化（首次添加调用）
        public void Init(Entity pEntity)
        {
            this.entityUid = pEntity.Uid;
            OnInit(pEntity);

            if (isActive)
                OnEnable();
            else
                OnDisable();
        }

        protected virtual void OnInit(Entity pEntity)
        {

        }

        public void Enable(bool pSystemCheck = true)
        {
            isActive = true;
            OnEnable();
            if (pSystemCheck)
                ECSLocate.ECS.CheckEntityInSystem(EntityUid);
        }

        protected virtual void OnEnable()
        {

        }

        public void Disable(bool pSystemCheck = true)
        {
            isActive = false;
            OnDisable();
            if (pSystemCheck)
                ECSLocate.ECS.CheckEntityInSystem(EntityUid);
        }

        protected virtual void OnDisable()
        {

        }

        public void Destroy(bool pSystemCheck = true)
        {
            Disable(pSystemCheck);
            OnDestroy();
        }

        protected virtual void OnDestroy()
        {

        }

        #region Editor

        public virtual void OnDrawGizmosSelected()
        {

        }

        public virtual void OnDrawGizmos()
        {

        } 

        #endregion
    }
}
