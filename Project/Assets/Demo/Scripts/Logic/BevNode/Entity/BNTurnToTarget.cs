using Demo.Com;
using Demo.Info;
using LCECS;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using UnityEngine;

namespace Demo
{
    public class BNTurnToPlayer : NodeAction
    {
        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;
            AnimCom animCom         = workData.MEntity.GetCom<AnimCom>();
            SeekPathCom seekPathCom = workData.MEntity.GetCom<SeekPathCom>();

            MapSensor mapSensor     = ECSLayerLocate.Info.GetSensor<MapSensor>(SensorType.Map);
            Vector2Int playerPos    = mapSensor.GetPlayerMapPos();

            if (playerPos.x >= seekPathCom.CurrPos.x)
            {
                animCom.SpriteRender.flipX = false;
            }
            else
            {
                animCom.SpriteRender.flipX = true;
            }
        }
    }
}
