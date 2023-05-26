using System.Collections.Generic;
using LCToolkit;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo.AStar.Com
{
    [ExecuteAlways]
    public class PathGridCom : MonoBehaviour
    {
        public int gridId;
        [Header("区域类型:0:默认 1:水域")]
        public int gridType;
        public RectInt gridRect;

        public Tilemap gridTilemap;
        public Tilemap envTilemap;
        public bool showGizmos = false; 
        
        private void Awake()
        {
            gridTilemap = GetComponent<Tilemap>();
            
            if (transform.parent.Find("Env_TileMap",out Transform outEnvTrans))
                envTilemap = outEnvTrans.GetComponent<Tilemap>();
        }

        private void OnEnable()
        {
            gridTilemap = GetComponent<Tilemap>();
            
            if (transform.parent.Find("Env_TileMap",out Transform outEnvTrans))
                envTilemap = outEnvTrans.GetComponent<Tilemap>();
        }
        
        private void OnDrawGizmos()
        {
            if (!showGizmos)
                return;
#if UNITY_EDITOR
            Rect rect = new Rect(gridRect.position, gridRect.size);
            GizmosHelper.DrawRect(rect, Color.red);
#endif
        }
        
        public PathGrid CreateGrid()
        {
            gridRect = CalcGridRect(this);
            PathGrid grid = new PathGrid();
            grid.Init(gridRect);
            return grid;
        }

        public List<PathGridConnectInfo> CreateConnectInfos()
        {
            List<PathGridConnectInfo> infos = new List<PathGridConnectInfo>();
            PathGridConnectCom[] connectComs = GetComponentsInChildren<PathGridConnectCom>();
            for (int i = 0; i < connectComs.Length; i++)
            {
                infos.Add(connectComs[i].CreateConnectInfo());
            }
            return infos;
        }
        
        
        /// <summary>
        /// 计算网格尺寸
        /// </summary>
        /// <param name="gridCom"></param>
        /// <returns></returns>
        public RectInt CalcGridRect(PathGridCom gridCom)
        {
            Tilemap gridTilemap = gridCom.gridTilemap;

            int minX = gridTilemap.cellBounds.xMin;
            int maxX = gridTilemap.cellBounds.xMax;

            int minY = gridTilemap.cellBounds.yMin;
            int maxY = gridTilemap.cellBounds.yMax;


            int minGridX = int.MinValue;
            int maxGridX = int.MinValue;

            int minGridY = int.MinValue;
            int maxGridY = int.MinValue;

            Vector3Int tempTilePos = Vector3Int.zero;
            TileBase tileBase = null;
            Vector3 worldPos;
            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    tempTilePos.x = i;
                    tempTilePos.y = j;

                    //tileBase = gridTilemap.GetTile(tempTilePos);
                    //worldPos = gridTilemap.CellToWorld(tempTilePos);
                    if (gridTilemap.HasTile(tempTilePos))
                    {
                        if (minGridX == int.MinValue)
                            minGridX = tempTilePos.x;
                        
                        if (maxGridX == int.MinValue)
                            maxGridX = tempTilePos.x;

                        if (minGridY == int.MinValue)
                            minGridY = tempTilePos.y;

                        if (maxGridY == int.MinValue)
                            maxGridY = tempTilePos.y;

                        if (tempTilePos.x < minGridX)
                        {
                            minGridX = tempTilePos.x;
                        }

                        if (tempTilePos.x > maxGridX)
                        {
                            maxGridX = tempTilePos.x;
                        }

                        if (tempTilePos.y < minGridY)
                        {
                            minGridY = tempTilePos.y;
                        }

                        if (tempTilePos.y > maxGridY)
                        {
                            maxGridY = tempTilePos.y;
                        }
                    }
                }
            }

            return new RectInt(new Vector2Int(minGridX, minGridY), new Vector2Int(maxGridX - minGridX, maxGridY - minGridY));
        }
    }
}