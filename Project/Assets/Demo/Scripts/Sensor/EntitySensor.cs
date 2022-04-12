using LCECS;
using LCECS.Core;
using LCECS.Layer.Info;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Info
{
    public enum EntityReqInfoType
    {
        GetEntitysByArea,           //指定区域的实体
    }

    [WorldSensor(SensorType.Entity)]
    public class EntitySensor : ISensor
    {
        private List<Entity> cllectEntitys = new List<Entity>();
        private Rect skillCheckRect = new Rect();
    }
}
