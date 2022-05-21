using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using LCMap;
using LCToolkit;
using Cinemachine;

namespace Demo.System
{
    public class DragCameraSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() {typeof(DragCameraCom)};
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            DragCameraCom dragCameraCom = GetCom<DragCameraCom>(comList[0]);

        }

        private void UpdateMove()
        {

        }

        private void HandleEvent(DragCameraCom dragCameraCom)
        {
            if (dragCameraCom.EventInfo == null)
                return;
            switch (dragCameraCom.EventInfo.EventType)
            {
                case LCToolkit.UnityEventType.Up:
                    if (dragCameraCom.IsSpringDamping)
                        dragCameraCom.IsSpringMove = true;
                    break;
                case LCToolkit.UnityEventType.Down:
                    dragCameraCom.IsInertiaMove = false;
                    break;
                case LCToolkit.UnityEventType.Click:
                    break;
                case LCToolkit.UnityEventType.LongClick:
                    break;
                case LCToolkit.UnityEventType.BeginDrag:
                    OnBeginDrag(dragCameraCom, dragCameraCom.EventInfo.EventData);
                    break;
                case LCToolkit.UnityEventType.Drag:
                    OnDrag(dragCameraCom, dragCameraCom.EventInfo.EventData);
                    break;
                case LCToolkit.UnityEventType.EndDrag:
                    OnEndDrag(dragCameraCom, dragCameraCom.EventInfo.EventData);
                    break;
                default:
                    break;
            }
            dragCameraCom.EventInfo = null;
        }

        #region 拖拽事件处理

        private void OnBeginDrag(DragCameraCom dragCameraCom, PointerEventData eventData)
        {

        }

        private void OnDrag(DragCameraCom dragCameraCom, PointerEventData eventData)
        {

        }

        private void OnEndDrag(DragCameraCom dragCameraCom, PointerEventData eventData)
        {

        }

        #endregion

        #region 区域处理

        //计算虚拟相机区域
        private void CalcCMArea(DragCameraCom dragCameraCom)
        {
            PolygonCollider2D collider2D = MapLocate.Map.CurrArea.GetCameraCollider();
            Vector2[] points             = collider2D.GetPoints();

            Vector2 areaMin = points[0];
            Vector2 areaMax = points[0];
            for (int i = 1; i < points.Length; i++)
            {
                Vector2 tPoint = points[i];
                if (tPoint.x <= areaMin.x)
                    areaMin.x = tPoint.x;
                if (tPoint.x >= areaMax.x)
                    areaMax.x = tPoint.x;

                if (tPoint.y <= areaMin.y)
                    areaMin.y = tPoint.y;
                if (tPoint.y >= areaMax.y)
                    areaMax.y = tPoint.y;
            }

            dragCameraCom.CMArea.AABBMin = areaMin;
            dragCameraCom.CMArea.AABBMax = areaMax;
        }

        //更新区域
        private void UpdateAreas(DragCameraCom dragCameraCom)
        {
            CinemachineVirtualCamera cam = dragCameraCom.CMCamera;
            Vector2 offset = new Vector2(cam.m_Lens.OrthographicSize * cam.m_Lens.Aspect, cam.m_Lens.OrthographicSize);
            dragCameraCom.SpringArea = ZoomArea(dragCameraCom.SpringArea, offset);
            dragCameraCom.DragArea = ZoomArea(dragCameraCom.SpringArea, new Vector2(dragCameraCom.SpringOffset, dragCameraCom.SpringOffset));
        }

        private Shape ZoomArea(Shape shape,Vector2 offset)
        {
            shape.AABBMin.x += offset.x;
            shape.AABBMin.y += offset.y;

            shape.AABBMax.x -= offset.x;
            shape.AABBMax.y -= offset.y;

            return shape;
        }

        private Vector2 ClampAreaPos(Shape shape, Vector2 pos)
        {
            float x = Mathf.Clamp(pos.x, shape.AABBMin.x, shape.AABBMax.x);
            float y = Mathf.Clamp(pos.y, shape.AABBMin.y, shape.AABBMax.y);
            return new Vector2(x,y);
        }

        #endregion

        private void SetCameraPos(DragCameraCom dragCameraCom, Vector2 pos)
        {
            Vector3 dragPos = dragCameraCom.DragTarget.transform.position;
            Vector2 resPos = ClampAreaPos(dragCameraCom.SpringArea, new Vector2(dragPos.x, dragPos.y));
            dragCameraCom.DragTarget.transform.position = new Vector3(resPos.x, resPos.y, dragPos.z);
        }
    }
}
