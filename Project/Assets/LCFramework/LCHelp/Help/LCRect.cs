using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LCHelp
{
    public class LCRect
    {
        /// <summary>
        /// 将目标点设置为Rect的中心点
        /// </summary>
        /// <param name="point">中心点</param>
        /// <param name="area">大小</param>
        /// <returns></returns>
        public static Rect GetMidRcet(Vector2Int point, Vector2Int area)
        {
            Vector2 leftDownPoint = new Vector2(point.x - (float)area.x / 2, point.y - (float)area.y / 2);
            Rect rect = new Rect(leftDownPoint, area);
            return rect;
        }

        /// <summary>
        /// 检测一个点是否在中心Rect
        /// </summary>
        public static bool CheckPointInMidRect(Vector2Int point, Vector2Int area, Vector2Int chekPoint)
        {
            Rect areaRect = GetMidRcet(point, area);
            return areaRect.Contains(chekPoint);
        }

        /// <summary>
        /// 将一个Rect的设置为中心点
        /// </summary>
        /// <param name="point">中心点</param>
        /// <param name="area">大小</param>
        /// <returns></returns>
        public static Rect SetMidRcet(Rect rect)
        {
            rect.position= new Vector2(rect.position.x - rect.position.x / 2, rect.position.y - (float)rect.position.y / 2);
            return rect;
        }
    }
}
