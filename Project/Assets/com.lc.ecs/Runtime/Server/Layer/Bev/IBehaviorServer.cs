using LCECS.Core.Tree.Base;
using LCECS.Data;

namespace LCECS.Server.Layer
{
    /// <summary>
    /// 行为服务
    /// 1,初始化行为
    /// 2,提供请求执行行为的接口
    /// </summary>
    public interface IBehaviorServer
    {
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();

        /// <summary>
        /// 请求执行行为
        /// </summary>
        /// <param name="workData"></param>
        void ReqBev(EntityWorkData workData);

        /// <summary>BevDict
        /// 运行行为树
        /// </summary>
        void Execute();
    }
}
