using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class MapRoadCnf
    {
        public Vector2Int tileWorldPos;
        public Vector2 roadPos;
        public string roadAnim;
    }

    public class TbMapRoadCnf : Dictionary<int, Dictionary<int, MapRoadCnf>>
    {
        public bool Exist(int posX,int posY)
        {
            if (!ContainsKey(posX))
                return false;
            if (!this[posX].ContainsKey(posY))
                return false;
            return true;
        }

        public bool Exist(Vector2Int pos)
        {
            return Exist(pos.x,pos.y);
        }

        public bool CalcRoadPos(Vector3 pos,out Vector2Int roadPos)
        {
            int posX = (int)pos.x;
            int posY = (int)(pos.y - 0.3);
            roadPos = new Vector2Int(posX, posY);
            if (Exist(roadPos))
            {
                return true;
            }
            for (int x = -1; x < 1; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    Vector2Int nearPos = new Vector2Int(x, y) + roadPos;
                    if (Exist(nearPos))
                    {
                        roadPos = nearPos;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
