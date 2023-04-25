using LCECS.Core;
using LCMap;
using LCToolkit;
using System.Collections.Generic;
using UnityEngine;

namespace LCECS.Server.ECS
{
    /// <summary>
    /// ECS服务类
    /// </summary>
    public interface IECSServer : IServer
    {
        /// <summary>
        /// 获得实体组件配置模板
        /// </summary>
        /// <param name="pCnfId"></param>
        /// <returns></returns>
        List<BaseCom> GetEntityComsModel(int pCnfId);

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="pUid">实体Uid</param>
        /// <param name="pEntityId">实体配置Id</param>
        Entity CreateEntity(string pUid, int pEntityId);

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="entity"></param>
        void AddEntity(Entity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        void RemoveEntity(Entity entity);

        /// <summary>
        /// 设置世界实体
        /// </summary>
        /// <param name="pWorld"></param>
        void SetWorld(Entity pWorld);

        /// <summary>
        /// 获得世界实体
        /// </summary>
        /// <returns></returns>
        Entity GetWorld();

        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Entity GetEntity(string uid);

        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="pEntityId"></param>
        /// <returns></returns>
        List<Entity> GetEntitys(int pEntityId);

        /// <summary>
        /// 获得所有实体
        /// </summary>
        /// <returns></returns>
        Dictionary<string, Entity> GetAllEntitys();

        /// <summary>
        /// 检测系统是否检测该实体
        /// </summary>
        /// <param name="entityId"></param>
        void CheckEntityInSystem(string uid);

        /// <summary>
        /// 注册系统
        /// </summary>
        /// <param name="system"></param>
        void RegSystem(BaseSystem system);

        /// <summary>
        /// 执行UpdateSystem
        /// </summary>
        void ExcuteUpdateSystem();

        /// <summary>
        /// 执行FixedUpdateSystem
        /// </summary>
        void ExcuteFixedUpdateSystem();

    }
}
