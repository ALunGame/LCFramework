using LCECS.Core;
using LCECS.Data;
using UnityEngine;

namespace LCECS.Server.Player
{
    /// <summary>
    /// 玩家服务
    /// </summary>
    public interface IPlayerServer
    {
        GameObject GetPalyerGo();

        void SetPlayerEntity(Entity entity);

        Entity GetPlayerEntity();

        EntityWorkData GetPlayerWorkData();

        ParamData GetReqParam(RequestId reqId);

        void PushPlayerReq(RequestId reqId);
    }
}
