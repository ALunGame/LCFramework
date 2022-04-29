using Demo.Com;
using Demo.Info;
using LCECS.Core;
using LCMap;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo.System
{
    public class Collider2DSystem : BaseSystem
    {
        private float RepairOffset = Collider2DCom.CollisionOffset + 0.1f;

        private MapSensor mapSensor = null;

        protected override List<Type> RegListenComs()
        {
            mapSensor = LCECS.ECSLayerLocate.Info.GetSensor<MapSensor>(LCECS.SensorType.Map);
            return new List<Type>() { typeof(Collider2DCom), typeof(MoveCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            Collider2DCom collider2DCom = GetCom<Collider2DCom>(comList[0]);
            MoveCom moveCom = GetCom<MoveCom>(comList[1]);
            HandleCollider(collider2DCom, moveCom);
            HandleRepairPos(collider2DCom, moveCom);
        }

        private void HandleCollider(Collider2DCom collider2DCom,MoveCom moveCom)
        {
            MapArea mapArea = mapSensor.GetMapArea(moveCom.Trans.position);
            Collider2D tilemapCollider2D = mapArea.AreaEnvGo.transform.Find("Tilemaps/Ground/GroundTile").GetComponent<Collider2D>();

            collider2DCom.Collider.Up = tilemapCollider2D.OverlapPoint(collider2DCom.UpCheckPoint);
            collider2DCom.Collider.Down = tilemapCollider2D.OverlapPoint(collider2DCom.DownCheckPoint);
            collider2DCom.Collider.Left = tilemapCollider2D.OverlapPoint(collider2DCom.LeftCheckPoint);
            collider2DCom.Collider.Right = tilemapCollider2D.OverlapPoint(collider2DCom.RightCheckPoint);

            Debug.Log("HandleCollider:"+collider2DCom.Collider.ToString());
        }

        private void HandleRepairPos(Collider2DCom collider2DCom,MoveCom moveCom)
        {
            if (collider2DCom.Collider.IsNull())
                return;

            float yValue = 0;
            float xValue = 0;
            if (collider2DCom.Collider.Up)
                yValue = -RepairOffset;
            if (collider2DCom.Collider.Down)
                yValue = RepairOffset;
            if (collider2DCom.Collider.Left)
                xValue = RepairOffset;
            if (collider2DCom.Collider.Right)
                xValue = -RepairOffset;

            moveCom.Trans.position = moveCom.Trans.position;
        }
    }
}