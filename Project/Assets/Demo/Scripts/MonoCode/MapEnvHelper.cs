using LCToolkit;
using UnityEngine;

namespace Demo
{
    [ExecuteAlways]
    public class MapEnvHelper : MonoBehaviour
    {
        [Header("地图尺寸")]
        public Vector2Int mapSize;

        [Header("显示地图标尺")]
        public bool showRuler = true;

        private Rect areaRect;

        private void Awake()
        {
            UpdateRect();
        }

        private void Update()
        {
            UpdateRect();
        }

        private void OnDrawGizmos()
        {
            if (!showRuler)
                return;
            GizmosHelper.DrawRect(areaRect, Color.red);
            Vector3 center = areaRect.center;

            //X轴
            GizmosHelper.DrawLine(new Vector3(areaRect.xMin, center.y, 0), new Vector3(areaRect.xMax, center.y, 0),Color.blue);
            GizmosHelper.DrawLine(new Vector3(center.x, areaRect.yMin, 0), new Vector3(center.x, areaRect.yMax, 0),Color.blue);
        }

        private void UpdateRect()
        {
            Vector2Int areaSize = mapSize;
            Vector3 centerPos   = transform.position - new Vector3(areaSize.x, areaSize.y) / 2;
            areaRect = new Rect(centerPos, areaSize);
        }
    } 
}
