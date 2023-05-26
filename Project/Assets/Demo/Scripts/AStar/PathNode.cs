using UnityEngine;

namespace Demo.AStar
{
    public class PathCost : IHeapItem<PathCost>
    {
        private int heapIndex;
        
        //路径点
        private int x;
        private int y;

        //寻路花费
        public float hCost;
        public float gCost;
        public float fCost;

        public PathNode parentNode;

        public int HeapIndex { get => heapIndex; set => heapIndex = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public PathCost(int posX,int posY)
        {
            x = posX;
            y = posY;
            gCost = 2147483647;
            hCost = 0;
        }

        public override string ToString()
        {
            return $"Pos:({x},{y}) H:{hCost} G:{gCost} F:{fCost}";
        }

        public int CompareTo(PathCost other)
        {
            int compare = fCost.CompareTo(other.fCost);
            if (compare == 0)
                compare = hCost.CompareTo(other.hCost);
            return compare;
        }
    }
    
    public class PathNode
    {
        //路径点位置
        private int x;
        private int y;

        /// <summary>
        /// 是否是阻挡
        /// </summary>
        public bool IsObs = true;
        
        public int X { get => x;}
        public int Y { get => y;}

        //锁定不可改变行走状态
        public bool isLocked = false;

        public PathNode(int posX,int posY)
        {
            x = posX;
            y = posY;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is PathNode)
            {
                PathNode node = (PathNode)obj;
                return node.x == x && node.y == y;
            }
            return false;
        }

        public Vector2Int ToVector()
        {
            return new Vector2Int(X, Y);
        }
    }
}