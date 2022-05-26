using LCJson;
using System;
using UnityEngine;

namespace LCECS.Core
{
    /// <summary>
    /// 组件特性 类可用
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComAttribute : Attribute
    {
        /// <summary>
        /// 编辑器显示名
        /// </summary>
        public string ViewName { get; set; } = "";

        /// <summary>
        /// 组件分组名
        /// </summary>
        public string GroupName { get; set; } = "";

        /// <summary>
        /// 全局唯一的组件
        /// </summary>
        public bool IsGlobal { get; set; } = false;
    }

    /// <summary>
    /// 组件字段特性 字段可用
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ComValueAttribute : Attribute
    {
        /// <summary>
        /// 可以在编辑器模式下编辑
        /// </summary>
        public bool ViewEditor { get; set; } = false;
        /// <summary>
        /// 可以在编辑器模式下显示
        /// </summary>
        public bool ShowView { get; set; } = true;
    }

    public class BaseCom
    {
        [NonSerialized]
        private int entityId = 0;
        [NonSerialized]
        private int entityCnfId = 0;

        /// <summary>
        /// 为了序列化，运行时不可设置
        /// </summary>
        public bool isActive = true;
        public bool IsActive { get => isActive;}

        public int EntityCnfId { get => entityCnfId;}
        public int EntityId { get => entityId;}

        //初始化（首次添加调用）
        public void Init(Entity entity)
        {
            this.entityId = entity.Uid;
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
            ECSLocate.ECS.CheckEntityInSystem(entityId);
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
            ECSLocate.ECS.CheckEntityInSystem(entityId);
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
