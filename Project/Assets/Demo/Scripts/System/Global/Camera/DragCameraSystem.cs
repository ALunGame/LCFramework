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
        private GlobalSensor globalSensor;
        private bool _InitEvent = false;
        private DragCameraCom dragCameraCom;
        private FollowCameraCom followCameraCom;

        protected override List<Type> RegContainListenComs()
        {
            globalSensor = LCECS.ECSLayerLocate.Info.GetSensor<GlobalSensor>(LCECS.SensorType.Global);
            globalSensor.CurrArea.RegisterValueChangedEvent(HandleCurrAreaChange);
            globalSensor.FollowActor.RegisterValueChangedEvent(HandleFollowActorChange);
            return new List<Type>() {typeof(DragCameraCom), typeof(FollowCameraCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            if (!_InitEvent)
            {
                dragCameraCom = GetCom<DragCameraCom>(comList[0]);
                followCameraCom = GetCom<FollowCameraCom>(comList[1]);
                HandleCurrAreaChange(globalSensor.CurrArea.Value);
                HandleFollowActorChange(globalSensor.FollowActor.Value);
                _InitEvent = true;
            }
            if (globalSensor.FollowActor.Value == null)
                return;
            UpdateMove(dragCameraCom);
            HandleEvent(dragCameraCom);
        }

        private void UpdateMove(DragCameraCom dragCameraCom)
        {
            SpringBackMove(dragCameraCom);
            InertiaMove(dragCameraCom);
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

        private void HandleCurrAreaChange(MapArea area)
        {
            if (dragCameraCom == null)
                return;
            dragCameraCom.CMCamera = area.DragCamera;
            dragCameraCom.DragTarget = area.DragTarget;
            CalcCMArea(dragCameraCom, area);
            UpdateAreas(dragCameraCom);
        }

        private void HandleFollowActorChange(Actor actor)
        {
        }

        #region 弹簧回弹

        private void SpringBackMove(DragCameraCom dragCameraCom)
        {
            if (!dragCameraCom.IsSpringMove)
                return;
            Vector3 dragPos = GetDragPos(dragCameraCom);

            if (!dragCameraCom.DragArea.ContainPoint(dragPos))
            {
                dragCameraCom.IsSpringMove = true;
                Vector2 newPos = ClampAreaPos(dragCameraCom.DragArea,dragPos);
                Vector2 centerPos = dragCameraCom.DragArea.GetCenter();
                newPos = Vector3.MoveTowards(newPos, centerPos, 0.01f);

                Vector3 tmpVector = Vector3.zero;
                Vector3 resPos = Vector3.SmoothDamp(dragPos, newPos, ref tmpVector, dragCameraCom.SpringSmoothTime);
                SetCameraPos(dragCameraCom, resPos);
            }
            else
            {
                dragCameraCom.IsSpringMove = false;
                dragCameraCom.IsSpringDamping = false;
            }
        }

        #endregion

        #region 惯性移动

        private void InertiaMove(DragCameraCom dragCameraCom)
        {
            if (dragCameraCom.IsSpringMove && !dragCameraCom.IsInertiaMove)
                return;
            if (dragCameraCom.InertiaSpeed <= 0)
                return;

            Vector3 currPos = GetDragPos(dragCameraCom);
            Vector3 nextPos = currPos - (dragCameraCom.InertiaSpeed * dragCameraCom.InertiaDir * Time.deltaTime);
            if (dragCameraCom.DragArea.ContainPoint(nextPos))
            {
                SetCameraPos(dragCameraCom, nextPos);
                dragCameraCom.InertiaSpeed = dragCameraCom.InertiaSpeed - dragCameraCom.InertiaDamp * Time.deltaTime;
            }
            else
            {
                dragCameraCom.IsInertiaMove = false;
            }
        }

        #endregion

        #region 拖拽事件处理

        private void OnBeginDrag(DragCameraCom dragCameraCom, PointerEventData eventData)
        {
            Actor followActor = globalSensor.FollowActor.Value;
            if (followActor == null)
                return;
            Vector3 followPos = followActor.DisplayCom.CameraFollowGo.transform.position;
            SetCameraPos(dragCameraCom, new Vector2(followPos.x, followPos.y));
        }

        private Vector3 GetDampingPos(DragCameraCom dragCameraCom, Vector3 pos, Vector3 delta)
        {
            dragCameraCom.IsSpringDamping = true;
            float width = dragCameraCom.DragArea.AABBMax.x - dragCameraCom.DragArea.AABBMin.x;
            float height = dragCameraCom.DragArea.AABBMax.y - dragCameraCom.DragArea.AABBMin.y;
            
            float GetRubberDelta(float pOverStretching, float pViewSize)
            {
                return (1 - (1 / ((MathF.Abs(pOverStretching) * dragCameraCom.SpringIntensity / pViewSize) + 1))) 
                    * pViewSize * Mathf.Sign(pOverStretching);
            }

            float newX = pos.x - GetRubberDelta(delta.x, width);
            float newY = pos.y - GetRubberDelta(delta.y, height);

            return new Vector3(newX, newY);
        }

        private void OnDrag(DragCameraCom dragCameraCom, PointerEventData eventData)
        {
            Camera mainCamera = Camera.main;
            Vector2 delta = eventData.delta;
            Vector3 worldDelta = mainCamera.ScreenToWorldPoint(delta) - mainCamera.ScreenToWorldPoint(Vector2.zero);
            Vector3 dragPos = GetDragPos(dragCameraCom);

            Vector3 nextPos = dragPos + worldDelta * dragCameraCom.DragSpeed;
            if (!dragCameraCom.DragArea.ContainPoint(nextPos))
                nextPos = GetDampingPos(dragCameraCom, nextPos, worldDelta);
            SetCameraPos(dragCameraCom,nextPos);
        }

        private void OnEndDrag(DragCameraCom dragCameraCom, PointerEventData eventData)
        {
            Vector2 delta = eventData.delta;
            dragCameraCom.InertiaSpeed = delta.magnitude * dragCameraCom.InertiaRate;
            dragCameraCom.InertiaDir = delta.normalized;
            dragCameraCom.InertiaDamp = dragCameraCom.InertiaSpeed / dragCameraCom.InertiaDampDuration;
            dragCameraCom.IsInertiaMove = true;
        }

        #endregion

        #region 区域处理

        //计算虚拟相机区域
        private void CalcCMArea(DragCameraCom dragCameraCom, MapArea mapArea)
        {
            PolygonCollider2D collider2D = mapArea.GetCameraCollider();
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

        #region Set

        private void SetCameraPos(DragCameraCom dragCameraCom, Vector2 pos)
        {
            Vector3 dragPos = dragCameraCom.DragTarget.transform.position;
            Vector2 resPos = ClampAreaPos(dragCameraCom.SpringArea, new Vector2(pos.x, pos.y));
            dragCameraCom.DragTarget.transform.position = new Vector3(resPos.x, resPos.y, dragPos.z);
        }

        #endregion

        #region Get

        private Vector3 GetDragPos(DragCameraCom dragCameraCom)
        {
            return dragCameraCom.DragTarget.transform.position;
        }

        #endregion
    }
}
