using LCECS.Core;
using LCECS.Data;
using UnityEngine;

namespace LCECS.Server.Player
{
    public class PlayerServer : IPlayerServer
    {
        private Entity playerEntity;
        private GameObject playerGo;
        private EntityWorkData playerWorkData;

        public void SetPlayerEntity(Entity entity)
        {
            playerEntity = entity;
            playerGo = entity.Go;
            playerWorkData = ECSLayerLocate.Info.GetEntityWorkData(entity.GetHashCode());
        }

        public Entity GetPlayerEntity()
        {
            return playerEntity;
        }

        public EntityWorkData GetPlayerWorkData()
        {
            return playerWorkData;
        }

        public GameObject GetPalyerGo()
        {
            return playerGo;
        }

        //请求
        public void PushPlayerReq(RequestId reqId, ParamData param)
        {
            ECSLayerLocate.Request.PushRequest(playerEntity.GetHashCode(), reqId, param);
        }
    }
}
