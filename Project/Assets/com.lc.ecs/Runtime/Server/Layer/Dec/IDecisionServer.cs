using LCECS.Data;

namespace LCECS.Server.Layer
{
    /// <summary>
    /// 决策服务
    /// 1,负责决策树的创建
    /// 2,添加决策实体
    /// 3,执行决策逻辑
    /// </summary>
    public interface IDecisionServer
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 添加决策实体
        /// </summary>
        /// <param name="decId">决策树Id</param>
        /// <param name="workData">实体数据流</param>
        void AddDecisionEntity(int decId, EntityWorkData workData);

        /// <summary>
        /// 删除决策实体
        /// </summary>
        /// <param name="decId">决策树Id</param>
        /// <param name="entityId">实体Id</param>
        void RemoveDecisionEntity(int decId, int entityId);

        /// <summary>
        /// 执行决策树
        /// </summary>
        void Execute();
    }
}
