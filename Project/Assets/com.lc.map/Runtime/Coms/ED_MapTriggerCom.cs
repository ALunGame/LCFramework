using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// 地图触发组件
    /// </summary>
    public class ED_MapTriggerCom : ED_MapDataCom
    {

        //获得碰撞盒坐标点
        public static Vector2[] GetPolygonCollider2DPoints(Transform trans)
        {
            PolygonCollider2D collider2D = trans.GetComponent<PolygonCollider2D>();
            if (collider2D == null)
                return null;
            Vector2[] vectors = new Vector2[collider2D.GetTotalPointCount()];
            for (int i = 0; i < collider2D.GetTotalPointCount(); i++)
            {
                //本地坐标变为世界坐标
                vectors[i] = new Vector2(collider2D.gameObject.transform.position.x, collider2D.gameObject.transform.position.y) + collider2D.points[i];
            }
            return vectors;
        }

        public override object ExportData()
        {
            MapTriggerModel triggerData = new MapTriggerModel();
            Vector2[] points = GetPolygonCollider2DPoints(transform);
            if (points != null)
            {
                triggerData.points = new List<Vector2>();
                for (int i = 0; i < points.Length; i++)
                {
                    triggerData.points.Add(points[i]);
                }
            }
            return triggerData;
        }
    }
}