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

        void CreatePlayerEntity(int entityCnfId, ref GameObject gameObject);

        Entity GetPlayerEntity();

        EntityWorkData GetPlayerWorkData();

        ParamData GetReqParam(int reqId);

        void PushPlayerReq(int reqId);
    }
}
