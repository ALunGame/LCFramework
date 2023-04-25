#if UNITY_EDITOR
using System;
using LCToolkit;
using UnityEngine;

namespace LCSkill.Help
{
    [ExecuteAlways]
    public class SkillShapeDrawCom : MonoBehaviour
    {
        public Shape DrawShape = new Shape();
        private Vector2[] renderCache;
        
        private void OnDrawGizmos()
        {
            renderCache = new Vector2[DrawShape.VertexCnt()];
            Shape.RenderShape(DrawShape, renderCache);
        }
    }
}

#endif