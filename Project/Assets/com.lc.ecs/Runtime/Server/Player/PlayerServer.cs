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

        public void CreatePlayerEntity(int entityCnfId, ref GameObject gameObject)
        {
            Entity entity = ECSLocate.ECS.CreateEntity(entityCnfId, ref gameObject);
            playerEntity = entity;
            playerGo = gameObject;

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

        //请求参数
        public ParamData GetReqParam(int reqId)
        {
            return playerWorkData.GetReqParam(reqId);
        }

        //请求
        public void PushPlayerReq(int reqId)
        {
            ECSLayerLocate.Request.PushRequest(playerEntity.GetHashCode(), reqId);
        }
    }
}
