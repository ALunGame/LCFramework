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
        /// 获得世界实体
        /// </summary>
        /// <returns></returns>
        Entity GetWorld();

        /// <summary>
        /// 创建演员实体
        /// </summary>
        /// <param name="actorObj"></param>
        /// <returns></returns>
        Entity CreateEntity(ActorObj actorObj);

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="id">配置Id</param>
        /// <param name="go"></param>
        /// <returns></returns>
        Entity CreateEntity(int uid, int id, GameObject go);

        /// <summary>
        /// 获得实体
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        Entity GetEntity(int uid);

        /// <summary>
        /// 获得所有实体
        /// </summary>
        /// <returns></returns>
        Dictionary<int, Entity> GetAllEntitys();

        /// <summary>
        /// 检测系统是否检测该实体
        /// </summary>
        /// <param name="entityId"></param>
        void CheckEntityInSystem(int uid);

        /// <summary>
        /// 注册在Update中更新的系统
        /// </summary>
        /// <param name="system"></param>
        void RegUpdateSystem(BaseSystem system);

        /// <summary>
        /// 注册在FixedUpdate中更新的系统
        /// </summary>
        /// <param name="system"></param>
        void RegFixedUpdateSystem(BaseSystem system);

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
