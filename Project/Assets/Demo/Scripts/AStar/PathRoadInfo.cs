using UnityEngine;

namespace Demo.AStar
{
    /// <summary>
    /// 路径道路类型
    /// </summary>
    public enum PathRoadType
    {
        Normal,
    }
    
    /// <summary>
    /// 路径道路信息
    /// </summary>
    public class PathRoadInfo
    {
        public Vector2Int tilePos;
        
        public Vector2 roadPos;

        public PathRoadType type;
    }
}