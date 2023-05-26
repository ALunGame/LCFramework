using UnityEngine;

namespace Demo.AStar
{
    public enum FinderType
    {
        Eight,
        Four,
    }
    
    /// <summary>
    /// 寻路网格
    /// </summary>
    public class PathGrid
    {
        public FinderType FinderType {get; private set;}
        
        /// <summary>
        /// 网格Id
        /// </summary>
        public int Id { get; private set; }
        
        /// <summary>
        /// 网格类型
        /// </summary>
        public int GridType { get; private set; }

        /// <summary>
        /// 网格左下位置
        /// </summary>
        public Vector2Int Pos { get; private set; }

        /// <summary>
        /// 网格大小
        /// </summary>
        public Vector2Int Size { get; private set; }
        
        /// <summary>
        /// 范围
        /// </summary>
        public RectInt Rect { get; private set; }
        
        /// <summary>
        /// 格子信息
        /// </summary>
        private PathNode[,] grid;

        public PathGrid()
        {
            FinderType = FinderType.Four;
        }

        public void Init(RectInt pRect)
        {
            Rect = pRect;
            
            Pos = Rect.position;
            
            Size = Rect.size;

            grid = new PathNode[Size.x + 1, Size.y + 1];
            
            for (int x = 0; x <= Size.x; x++)
            {
                for (int y = 0; y <= Size.y; y++)
                {
                    grid[x, x] = new PathNode(x, y);
                }
            }
        }
        
        /// <summary>
        /// 瓦片坐标转寻路路径
        /// </summary>
        /// <returns></returns>
        public Vector2Int TileToGridPos(Vector2Int pTilePos)
        {
            return pTilePos - Pos;
        }

        /// <summary>
        /// 寻路路径转瓦片坐标
        /// </summary>
        /// <returns></returns>
        public Vector2Int GridToTilePos(Vector2Int pGridPos)
        {
            return pGridPos + Pos;
        }
        
        /// <summary>
        /// 获得网格点
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PathNode GetNode(int x,int y)
        {
            return grid[x, y];
        }
        
        /// <summary>
        /// 检测是否在区域内
        /// </summary>
        /// <param name="pTilePos"></param>
        /// <returns></returns>
        public bool CheckInGrid(Vector2Int pTilePos)
        {
            return pTilePos.x >= Rect.xMin && pTilePos.y >= Rect.yMin && pTilePos.x <= Rect.xMax && pTilePos.y <= Rect.yMax;
        }

        public bool CheckPointIsLegal(int x, int y)
        {
            if (x < 0 || y < 0)
                return false;
            if (x > Size.x || y > Size.y)
                return false;
            return true;
        }
        
        public bool CheckIsObs(Vector2Int pGridPos)
        {
            if (!CheckPointIsLegal(pGridPos.x, pGridPos.y))
                return true;
            return grid[pGridPos.x, pGridPos.y].IsObs;
        }

        public void SetObs(Vector2Int pGridPos, bool pIsObs)
        {
            if (!CheckPointIsLegal(pGridPos.x, pGridPos.y))
                return;
            grid[pGridPos.x, pGridPos.y].IsObs = pIsObs;
        }
    }
}