using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCToolkit
{
    /// <summary>
    /// 自动创建碰撞区域
    /// </summary>
    public static class AutoCreatePolygonCollider2D
    {
        //边
        struct Edge2D
		{

			public Vector2 a;
			public Vector2 b;

			public override bool Equals(object obj)
			{
				if (obj is Edge2D)
				{
					var edge = (Edge2D)obj;
					return (edge.a == a && edge.b == b) || (edge.b == a && edge.a == b);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return a.GetHashCode() ^ b.GetHashCode();
			}

			public override string ToString()
			{
				return string.Format("[" + a.x + "," + a.y + "->" + b.x + "," + b.y + "]");
			}
		}

		private const string defaultClickGoName = "ClickBox";

		public static GameObject CreatePolygonCollider2DByMesh(GameObject go = null,string colliderName = null)
        {
            string clickGoName = colliderName ?? defaultClickGoName;

            GameObject selGo = go??Selection.activeGameObject;
			MeshFilter filter = selGo.GetComponent<MeshFilter>();

			PolygonCollider2D polyCollider = null;
			if (selGo.transform.parent == null)
            {
				GameObject colliderGo = new GameObject(clickGoName);
				polyCollider = colliderGo.AddComponent<PolygonCollider2D>();
			}
            else
            {
				polyCollider = selGo.transform.parent.GetComponentInChildren<PolygonCollider2D>();
                if (polyCollider==null)
                {
					GameObject colliderGo = new GameObject(clickGoName);
					colliderGo.transform.SetParent(selGo.transform.parent);
					colliderGo.transform.localPosition = Vector3.zero;
					colliderGo.transform.localRotation = selGo.transform.localRotation;
					colliderGo.transform.localScale = Vector3.one;
					polyCollider = colliderGo.AddComponent<PolygonCollider2D>();
				}
			}

			var edges = BuildEdgesFromMesh(filter);
			var paths = BuildColliderPaths(edges);
			ApplyPathsToPolygonCollider(paths, polyCollider);
            polyCollider.transform.SetParent(selGo.transform);
            polyCollider.isTrigger = true;

            return polyCollider.gameObject;
        }

        public static GameObject CreatePolygonCollider2DBySprite(GameObject go = null, string colliderName = null)
        {
            string clickGoName = colliderName ?? defaultClickGoName;

            GameObject selGo = go??Selection.activeGameObject;
            SpriteRenderer spriteRenderer = selGo.GetComponent<SpriteRenderer>();

            PolygonCollider2D polyCollider = null;
            if (selGo.transform.parent == null)
            {
                GameObject colliderGo = new GameObject(clickGoName);
                polyCollider = colliderGo.AddComponent<PolygonCollider2D>();
            }
            else
            {
               /* polyCollider = selGo.transform.parent.GetComponentInChildren<PolygonCollider2D>();
                if (polyCollider == null) { }*/
                GameObject colliderGo = new GameObject(clickGoName);
                colliderGo.transform.SetParent(selGo.transform.parent);
                colliderGo.transform.localPosition = selGo.transform.localPosition;
                colliderGo.transform.localRotation = Quaternion.identity;
                colliderGo.transform.localScale = Vector3.one;
                polyCollider = colliderGo.AddComponent<PolygonCollider2D>();
            }

            var edges = BuildEdgesFromSprite(spriteRenderer);
            var paths = BuildColliderPaths(edges);
            ApplyPathsToPolygonCollider(paths, polyCollider);
            polyCollider.transform.SetParent(selGo.transform);
            polyCollider.isTrigger = true;
            return polyCollider.gameObject;
        }

        public static GameObject CreatePolygonCollider2DBySkeletonAnim(GameObject go = null, string colliderName = null)
        {
            string clickGoName = colliderName ?? defaultClickGoName;
            GameObject selGo = go??Selection.activeGameObject;
            MeshFilter filter = selGo.GetComponent<MeshFilter>();

            PolygonCollider2D polyCollider = null;
            if (selGo.transform.parent == null)
            {
                GameObject colliderGo = new GameObject(clickGoName);
                polyCollider = colliderGo.AddComponent<PolygonCollider2D>();
            }
            else
            {
                polyCollider = selGo.transform.parent.GetComponentInChildren<PolygonCollider2D>();
                if (polyCollider == null)
                {
                    GameObject colliderGo = new GameObject(clickGoName);
                    colliderGo.transform.SetParent(selGo.transform.parent);
                    colliderGo.transform.localPosition = Vector3.zero;
                    colliderGo.transform.localRotation = Quaternion.identity;
                    colliderGo.transform.localScale = Vector3.one;
                    polyCollider = colliderGo.AddComponent<PolygonCollider2D>();
                }
            }

            var edges = BuildEdgesFromSkeletonAnim(filter);
            var paths = BuildColliderPaths(edges);
            ApplyPathsToPolygonCollider(paths, polyCollider);
            polyCollider.transform.SetParent(selGo.transform);
            polyCollider.isTrigger = true;

            return polyCollider.gameObject;
        }

        /// <summary>
        /// 通过mesh去找所有的边，存到一个字典里
        /// 如果 int = 1 则证明是边缘的边，如果 int = 2 则证明则是公用的（不是边缘）
        /// </summary>
        /// <returns>返回存Edge2D的字典</returns>
        private static Dictionary<Edge2D, int> BuildEdgesFromMesh(MeshFilter filter)
		{
			var mesh = filter.sharedMesh;

			if (mesh == null)
				return null;

			var verts = mesh.vertices;
			var tris = mesh.triangles;
			var edges = new Dictionary<Edge2D, int>();

			for (int i = 0; i < tris.Length - 2; i += 3)
			{

				var faceVert1 = verts[tris[i]];
				var faceVert2 = verts[tris[i + 1]];
				var faceVert3 = verts[tris[i + 2]];

				Edge2D[] faceEdges;
				faceEdges = new Edge2D[] {
				new Edge2D{ a = faceVert1, b = faceVert2 },
				new Edge2D{ a = faceVert2, b = faceVert3 },
				new Edge2D{ a = faceVert3, b = faceVert1 },
			};

				foreach (var edge in faceEdges)
				{
					if (edges.ContainsKey(edge))
						edges[edge]++;
					else
						edges[edge] = 1;
				}
			}

			return edges;
		}

        /// <summary>
		/// 通过SpriteRenderer去找所有的边，存到一个字典里
		/// 如果 int = 1 则证明是边缘的边，如果 int = 2 则证明则是公用的（不是边缘）
		/// </summary>
		/// <returns>返回存Edge2D的字典</returns>
        private static Dictionary<Edge2D, int> BuildEdgesFromSprite(SpriteRenderer spriteRenderer)
        {
            if (spriteRenderer == null)
                return null;

            var verts = spriteRenderer.sprite.vertices;
            var tris  = spriteRenderer.sprite.triangles;
            var edges = new Dictionary<Edge2D, int>();

            for (int i = 0; i < tris.Length - 2; i += 3)
            {

                var faceVert1 = verts[tris[i]];
                var faceVert2 = verts[tris[i + 1]];
                var faceVert3 = verts[tris[i + 2]];

                Edge2D[] faceEdges;
                faceEdges = new Edge2D[] {
                new Edge2D{ a = faceVert1, b = faceVert2 },
                new Edge2D{ a = faceVert2, b = faceVert3 },
                new Edge2D{ a = faceVert3, b = faceVert1 },
            };

                foreach (var edge in faceEdges)
                {
                    if (edges.ContainsKey(edge))
                        edges[edge]++;
                    else
                        edges[edge] = 1;
                }
            }

            return edges;
        }

        /// <summary>
        /// 骨骼动画直接是找最大的区域
        /// </summary>
        /// <returns>返回存Edge2D的字典</returns>
        private static Dictionary<Edge2D, int> BuildEdgesFromSkeletonAnim(MeshFilter filter)
        {
            var mesh = filter.sharedMesh;

            if (mesh == null)
                return null;

            
            var verts = mesh.vertices;
            var tris = mesh.triangles;
            var edges = new Dictionary<Edge2D, int>();

            Vector3 vert = verts[0];
            float minXPos = vert.x;
            float maxXPos = vert.x;
            float minYPos = vert.y;
            float maxYPos = vert.y;
            for (int i = 1; i < verts.Length; i++)
            {
                Vector3 checkVert = verts[i];
                if (checkVert.x < minXPos)
                    minXPos = checkVert.x;
                if (checkVert.x > maxXPos)
                    maxXPos = checkVert.x;

                if (checkVert.y < minYPos)
                    minYPos = checkVert.y;
                if (checkVert.y > maxYPos)
                    maxYPos = checkVert.y;
            }

            //修复偏移
            minYPos = 0;

            Edge2D[] faceEdges;
            faceEdges = new Edge2D[] {
                new Edge2D{ a = new Vector2(minXPos,minYPos), b = new Vector2(minXPos,maxYPos) },
                new Edge2D{ a = new Vector2(minXPos,maxYPos), b = new Vector2(maxXPos,maxYPos) },
                new Edge2D{ a = new Vector2(maxXPos,maxYPos), b = new Vector2(maxXPos,minYPos) },
                new Edge2D{ a = new Vector2(maxXPos,minYPos), b = new Vector2(minXPos,minYPos) },
            };

            foreach (var edge in faceEdges)
            {
                if (edges.ContainsKey(edge))
                    edges[edge]++;
                else
                    edges[edge] = 1;
            }
            return edges;
        }

        #region 优化碰撞线条

        private static List<Vector2[]> BuildColliderPaths(Dictionary<Edge2D, int> allEdges)
        {

            if (allEdges == null)
                return null;

            var outerEdges = GetOuterEdges(allEdges);

            var paths = new List<List<Edge2D>>();
            List<Edge2D> path = null;

            while (outerEdges.Count > 0)
            {

                if (path == null)
                {
                    path = new List<Edge2D>();
                    path.Add(outerEdges[0]);
                    paths.Add(path);

                    outerEdges.RemoveAt(0);
                }

                bool foundAtLeastOneEdge = false;

                int i = 0;
                while (i < outerEdges.Count)
                {
                    var edge = outerEdges[i];
                    bool removeEdgeFromOuter = false;

                    if (edge.b == path[0].a)
                    {
                        path.Insert(0, edge);
                        removeEdgeFromOuter = true;
                    }
                    else if (edge.a == path[path.Count - 1].b)
                    {
                        path.Add(edge);
                        removeEdgeFromOuter = true;
                    }

                    if (removeEdgeFromOuter)
                    {
                        foundAtLeastOneEdge = true;
                        outerEdges.RemoveAt(i);
                    }
                    else
                        i++;
                }

                if (!foundAtLeastOneEdge)
                    path = null;

            }

            var cleanedPaths = new List<Vector2[]>();

            foreach (var builtPath in paths)
            {
                var coords = new List<Vector2>();

                foreach (var edge in builtPath)
                    coords.Add(edge.a);

                cleanedPaths.Add(CoordinatesCleaned(coords));
            }

            return cleanedPaths;
        }

        /// <summary>
        /// 取到边缘的边（即字典中int = 1的值）
        /// </summary>
        /// <param name="allEdges"></param>
        /// <returns>边缘的边</returns>
        private static List<Edge2D> GetOuterEdges(Dictionary<Edge2D, int> allEdges)
        {
            var outerEdges = new List<Edge2D>();

            foreach (var edge in allEdges.Keys)
            {
                var numSharedFaces = allEdges[edge];
                if (numSharedFaces == 1)
                    outerEdges.Add(edge);
            }

            return outerEdges;
        }

        private static bool CoordinatesFormLine(Vector2 a, Vector2 b, Vector2 c)
        {
            float area = a.x * (b.y - c.y) +
                b.x * (c.y - a.y) +
                    c.x * (a.y - b.y);

            return Mathf.Approximately(area, 0f);

        }

        private static Vector2[] CoordinatesCleaned(List<Vector2> coordinates)
        {
            List<Vector2> coordinatesCleaned = new List<Vector2>();
            coordinatesCleaned.Add(coordinates[0]);

            var lastAddedIndex = 0;

            for (int i = 1; i < coordinates.Count; i++)
            {

                var coordinate = coordinates[i];

                Vector2 lastAddedCoordinate = coordinates[lastAddedIndex];
                Vector2 nextCoordinate = (i + 1 >= coordinates.Count) ? coordinates[0] : coordinates[i + 1];

                if (!CoordinatesFormLine(lastAddedCoordinate, coordinate, nextCoordinate))
                {

                    coordinatesCleaned.Add(coordinate);
                    lastAddedIndex = i;
                }

            }

            return coordinatesCleaned.ToArray();
        }

        #endregion

        /// <summary>
        /// 对多边形碰撞体设置路径
        /// </summary>
        /// <param name="paths"></param>
        private static void ApplyPathsToPolygonCollider(List<Vector2[]> paths, PolygonCollider2D polyCollider)
        {
            if (paths == null)
                return;

            polyCollider.pathCount = paths.Count;
            for (int i = 0; i < paths.Count; i++)
            {
                var path = paths[i];
                polyCollider.SetPath(i, path);
            }
        }
    } 
}
