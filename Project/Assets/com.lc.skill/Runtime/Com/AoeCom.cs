using LCECS.Core;
using LCToolkit;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCSkill
{
    /// <summary>
    /// Aoe组件
    /// 存储游戏中所有的Aoe
    /// </summary>
    public class AoeCom : BaseCom
    {
        [NonSerialized]
        private List<AoeObj> aoes = new List<AoeObj>();
        [NonSerialized]
        private Vector2[] _cachedVectors;

        public IReadOnlyList<AoeObj> Aoes { get => aoes; }

        public void AddAoe(AoeObj aoeObj)
        {
            aoes.Add(aoeObj);
        }

        public void RemoveAoe(AoeObj aoeObj)
        {
            for (int i = 0; i < aoes.Count; i++)
            {
                if (aoes[i].Equals(aoeObj))
                {
                    aoes.RemoveAt(i);
                }
            }
        }

        public override void OnDrawGizmos()
        {
            int vertexCnt = 0;
            for (int i = 0; i < Aoes.Count; i++)
                vertexCnt += Aoes[i].model.areaShape.VertexCnt();
            _cachedVectors = new Vector2[vertexCnt];

            for (int i = 0; i < Aoes.Count; i++)
            {
                var startColor = Gizmos.color;
                Shape newShape = Aoes[i].CalcArea();

                var doesIntersect = false;
                for (var j = 0; j < Aoes.Count; j++)
                {
                    if (j == i)
                        continue;

                    Shape testShape = Aoes[j].CalcArea();
                    doesIntersect |= testShape.Intersects(newShape);
                }

                Gizmos.color = doesIntersect ? Color.green : Color.white;

                Shape.RenderShape(newShape, _cachedVectors);

                Gizmos.color = startColor;
            }
        }
    }
}
