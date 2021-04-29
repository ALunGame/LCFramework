using LCECS;
using LCECS.Core.ECS;
using LCECS.Layer.Info;
using UnityEngine;

namespace Demo.Info
{
    [WorldSensor(SensorType.Player)]
    public class PlayerSensor : ISensor
    {
        private Entity playerEntity = null;
        private GameObjectCom playerGoCom = null;

        private void GollectPlayer()
        {
            if (playerEntity == null)
            {
                playerEntity = ECSLocate.Player.GetPlayerEntity();
                playerGoCom  = playerEntity.GetCom<GameObjectCom>();
            }
        }

        public Vector2 GetPlayerPos()
        {
            GollectPlayer();
            return playerGoCom.Tran.position;
        }
    }
}