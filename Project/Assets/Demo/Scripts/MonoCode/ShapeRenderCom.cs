using LCToolkit;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public enum ShapeRenderType
    {
        警戒范围,
        攻击范围,
    }

    /// <summary>
    /// 形状绘制
    /// </summary>
    public class ShapeRenderCom : MonoBehaviour
    {
        [Header("选择显示的绘制")]
        public ShapeRenderType selType = ShapeRenderType.警戒范围;
        public Dictionary<ShapeRenderType,Dictionary<string,Shape>> shapeDict = new Dictionary<ShapeRenderType,Dictionary<string, Shape>>();
        private Vector2[] renderCache;

        private void Awake()
        {
            GameLocate.SetShapeRenderCom(this);
        }

        public void AddShape(ShapeRenderType renderType,string entityUid,Shape shape)
        {
#if !UNITY_EDITOR
            return;
#endif
            if (!shapeDict.ContainsKey(renderType))
            {
                shapeDict.Add(renderType, new Dictionary<string, Shape>());
            }
            if (!shapeDict[renderType].ContainsKey(entityUid))
            {
                shapeDict[renderType].Add(entityUid, shape);
            }
            else
            {
                shapeDict[renderType][entityUid] = shape;
            }
        }

        public void RemoveShape(ShapeRenderType renderType, string entityUid)
        {
#if !UNITY_EDITOR
            return;
#endif
            if (shapeDict.ContainsKey(renderType) && shapeDict[renderType].ContainsKey(entityUid))
            {
                shapeDict[renderType].Remove(entityUid);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!shapeDict.ContainsKey(selType))
                return;
            int cnt = 0;
            foreach (var item in shapeDict[selType])
            {
                cnt += item.Value.VertexCnt();
            }
            renderCache = new Vector2[cnt];
            foreach (var item in shapeDict[selType])
            {
                Shape.RenderShape(item.Value, renderCache);
            }
        }
    }
}
