using System;
using UnityEngine;

namespace LCECS.Core
{
    public class BaseCom
    {
        [NonSerialized]
        private string entityUid = "";
        [NonSerialized]
        private int entityCnfId = 0;

        /// <summary>
        /// 为了序列化，运行时不可设置
        /// </summary>
        public bool isActive = true;
        public bool IsActive { get => isActive;}

        public int EntityCnfId { get => entityCnfId;}
        public string EntityUid { get => entityUid; }

        //初始化（首次添加调用）
        public void Init(Entity entity)
        {
            this.entityUid = entity.Uid;
            this.entityCnfId = entity.Id;
            OnEnable();
            OnInit(entity.Go);
        }

        //实体本身启用
        public void EntityEnable()
        {
            isActive = true;
            OnEnable();
        }

        //启用
        public void Enable()
        {
            isActive = true;
            OnEnable();
            ECSLocate.ECS.CheckEntityInSystem(EntityUid);
        }

        //实体本身禁用
        public void EntityDisable()
        {
            isActive = false;
            OnDisable();
        }

        //禁用
        public void Disable()
        {
            isActive = false;
            OnDisable();
            ECSLocate.ECS.CheckEntityInSystem(EntityUid);
        }

        //初始化（首次添加调用）
        protected virtual void OnInit(GameObject go)
        {

        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }

        public virtual void OnDrawGizmosSelected()
        {

        }

        public virtual void OnDrawGizmos()
        {

        }
        
    }
}
