﻿using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace IANodeGraph.View.Utils
{
    /// <summary>
    /// 重新实现连接边缘显示
    /// </summary>
    public class EdgeControlA : EdgeControl
    {
        Edge edgeView;
        bool pointsChanged;
        CurveInfo curveInfo = new CurveInfo();

        public EdgeControlA(Edge connectionView)
        {
            this.edgeView = connectionView;
        }

        protected override void PointsChanged()
        {
            base.PointsChanged();
            ComputeControlPoints();
            if (controlPoints == null)
                return;
            if (edgeView.input?.node != edgeView.output?.node)
                return;
            pointsChanged = true;
            Vector2 vector0 = controlPoints[0];
            Vector2 vector1 = controlPoints[1];
            Vector2 vector2 = controlPoints[2];
            Vector2 vector3 = controlPoints[3];

            curveInfo.points.Clear();
            curveInfo.points.Add(vector0);
            curveInfo.points.Add(vector1);
            curveInfo.points.Add(vector2);
            curveInfo.points.Add(vector3);
            if (inputOrientation == outputOrientation && outputOrientation == Orientation.Horizontal)
            {
                var y = Mathf.Max(vector1.y, vector2.y);
                var c1 = new Vector2(vector1.x, y + 10);
                var c2 = new Vector2(vector2.x, c1.y);
                curveInfo.points.Insert(2, c1);
                curveInfo.points.Insert(3, c2);
            }
            else if (inputOrientation == outputOrientation && outputOrientation == Orientation.Vertical)
            {
                //var dis = vector0.x - vector3.x;
                //vector1.y += 7;
                //curveInfo.points[1] = vector1;
                //vector2.y -= 7;
                //curveInfo.points[2] = vector2;
                //if (Mathf.Abs(dis) < 5 || dis <= 0)
                //{
                //    var outputOffset = (edgeView.output.parent.layout.width - edgeView.output.layout.center.x) + 20;
                //    var c1 = new Vector2(vector1.x + outputOffset, vector1.y);
                //    var c2 = new Vector2(c1.x, vector2.y);
                //    curveInfo.points.Insert(2, c1);
                //    curveInfo.points.Insert(3, c2);
                //}
                //else if (dis > 0)
                //{
                //    var outputOffset = edgeView.output.layout.center.x + 20;
                //    var c1 = new Vector2(vector1.x - outputOffset, vector1.y);
                //    var c2 = new Vector2(c1.x, vector2.y);
                //    curveInfo.points.Insert(2, c1);
                //    curveInfo.points.Insert(3, c2);
                //}

                var x = Mathf.Max(vector1.x, vector2.x);
                var c1 = new Vector2(x + 10, vector1.y);
                var c2 = new Vector2(c1.x, vector2.y);
                curveInfo.points.Insert(2, c1);
                curveInfo.points.Insert(3, c2);
            }
            else if (inputOrientation != outputOrientation && outputOrientation == Orientation.Horizontal)
            {
                var c = new Vector2(vector1.x, vector2.y);
                curveInfo.points.Insert(2, c);
            }
            else if (inputOrientation != outputOrientation && outputOrientation == Orientation.Vertical)
            {
                var c = new Vector2(vector2.x, vector1.y);
                curveInfo.points.Insert(2, c);
            }
            curveInfo.SetDirty();
        }

        protected override void UpdateRenderPoints()
        {
            if (edgeView.input?.node != edgeView.output?.node)
            {
                base.UpdateRenderPoints();
                return;
            }

            if (!(bool)(RenderPointsDirtyField.GetValue(this)) && controlPoints != null)
                return;
            if (!pointsChanged)
                return;
            pointsChanged = false;

            var renderPoints = RenderPointsField.GetValue(this) as List<Vector2>;
            renderPoints.Clear();
            renderPoints.AddRange(curveInfo.points);

            for (int i = 0; i < renderPoints.Count; i++)
            {
                renderPoints[i] -= layout.position;
            }
        }

        public override void UpdateLayout()
        {
            if (edgeView.input?.node != edgeView.output?.node)
            {
                base.UpdateLayout();
                return;
            }

            Rect r = new Rect(curveInfo.minX, curveInfo.minY, curveInfo.maxX - curveInfo.minX, curveInfo.maxY - curveInfo.minY);
            r.xMin -= 5;
            r.xMax += 5;
            r.yMin -= 5;
            r.yMax += 5;

            LayoutProperty.SetValue(this, r);
        }

        #region Static
        static FieldInfo RenderPointsDirtyField;
        static FieldInfo RenderPointsField;
        static PropertyInfo LayoutProperty;

        static EdgeControlA()
        {
            RenderPointsDirtyField = typeof(EdgeControl).GetField("m_RenderPointsDirty", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            RenderPointsField = typeof(EdgeControl).GetField("m_RenderPoints", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            LayoutProperty = typeof(EdgeControl).GetProperty("layout", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
        #endregion

        #region Define
        public class CurveInfo
        {
            public float minX = float.MaxValue, minY = float.MaxValue;
            public float maxX = float.MinValue, maxY = float.MinValue;
            public List<Vector2> points = new List<Vector2>();

            public void SetDirty()
            {
                minX = float.MaxValue;
                minY = float.MaxValue;
                maxX = float.MinValue;
                maxY = float.MinValue;
                foreach (var point in points)
                {
                    minX = Mathf.Min(point.x, minX);
                    maxX = Mathf.Max(point.x, maxX);
                    minY = Mathf.Min(point.y, minY);
                    maxY = Mathf.Max(point.y, maxY);
                }
            }
        }
        #endregion
    }
}
