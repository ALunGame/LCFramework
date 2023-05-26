using System;
using System.Collections.Generic;
using Demo.AStar.Com;
using UnityEngine;

namespace Demo.AStar
{
    public class PathGridConnectInfo
    {
        public int inGridId;
        public Vector2Int inPoint;

        public int outGridId;
        public Vector2Int outPoint;

        public PathGridConnectInfo(int inGridId, Vector2Int inPoint, int outGridId, Vector2Int outPoint)
        {
            this.inGridId = inGridId;
            this.inPoint = inPoint;
            this.outGridId = outGridId;
            this.outPoint = outPoint;
        }

        public PathGridConnectInfo()
        {

        }
    }
    
    public class PathMapServer
    {
        private List<PathGridConnectInfo> connectInfos = new List<PathGridConnectInfo>();
        private List<PathGrid> grids = new List<PathGrid>();
        private List<PathfinderCom> finderComs = new List<PathfinderCom>();


        public void Init()
        {
            
        }

        public void Clear()
        {
            for (int i = 0; i < finderComs.Count; i++)
            {
                finderComs[i].Clear();
            }
            finderComs.Clear();
        }

        private void PauseFind()
        {
            for (int i = 0; i < finderComs.Count; i++)
            {
                finderComs[i].Pause();
            }
        }
        
        private void ResumeFind()
        {
            for (int i = 0; i < finderComs.Count; i++)
            {
                finderComs[i].Resume();
            }
        }
        
        
        public void AddCom(PathfinderCom pCom)
        {
            finderComs.Add(pCom);
        }
        
        public void RemoveCom(PathfinderCom pCom)
        {
            finderComs.Remove(pCom);
        }

        #region Get

        private PathGrid GetGrid(Vector2Int pTilePos)
        {
            foreach (var item in grids)
            {
                if (item.CheckInGrid(pTilePos))
                {
                    return item;
                }
            }
            return null;
        }

        #endregion

        #region Set
        
        public void SetPointsObs(List<Vector2Int> pObsTilePos, List<Vector2Int> pWalkTilePos)
        {
            PauseFind();
            
            if (pObsTilePos.IsLegal())
            {
                for (int i = 0; i < pObsTilePos.Count; i++)
                {
                    PathGrid grid = GetGrid(pObsTilePos[i]);
                    if (grid == null)
                        return;
                    Vector2Int gridPos = grid.TileToGridPos(pObsTilePos[i]);
                    grid.SetObs(gridPos, true);
                }
            }
            
            if (pWalkTilePos.IsLegal())
            {
                for (int i = 0; i < pWalkTilePos.Count; i++)
                {
                    PathGrid grid = GetGrid(pWalkTilePos[i]);
                    if (grid == null)
                        return;
                    Vector2Int gridPos = grid.TileToGridPos(pWalkTilePos[i]);
                    grid.SetObs(gridPos, true);
                }
            }

            ResumeFind();
        }

        #endregion

        #region Cehck

        public bool CheckIsObs(int pX, int pY)
        {
            var tilePos = new Vector2Int(pX, pY);
            return CheckIsObs(tilePos);
        }
        
        public bool CheckIsObs(Vector2Int pTilePos)
        {
            PathGrid grid = GetGrid(pTilePos);
            if (grid == null)
                return true;
            Vector2Int gridPos = grid.TileToGridPos(pTilePos);
            return grid.CheckIsObs(gridPos);
        }

        #endregion
        
        /// <summary>
        /// 请求寻路
        /// </summary>
        /// <param name="pGo"></param>
        /// <param name="pStartPoint"></param>
        /// <param name="pTargetPoint"></param>
        /// <param name="finishCallBack"></param>
        public void ReqSearchPath(GameObject pGo,Vector2Int pStartPoint, Vector2Int pTargetPoint, Action<List<Vector2Int>> finishCallBack)
        {
            if (pStartPoint.Equals(pTargetPoint))
            {
                finishCallBack?.Invoke(null);
                return;
            }

            if (CheckIsObs(pStartPoint) || CheckIsObs(pTargetPoint))
            {
                finishCallBack?.Invoke(null);
                return;
            }

            PathGrid startGrid  = GetGrid(pStartPoint);
            if (startGrid == null)
            {
                finishCallBack?.Invoke(null);
                return;
            }
            PathGrid targetGrid = GetGrid(pTargetPoint);
            if (targetGrid == null)
            {
                finishCallBack?.Invoke(null);
                return;
            }

            PathfinderCom pathfinderCom = pGo.GetComponent<PathfinderCom>();
            if (pathfinderCom == null)
                pathfinderCom = pGo.AddComponent<PathfinderCom>();

            List<PathfinderGridInfo> gridInfos = CalcGridInfos(pStartPoint, pTargetPoint, startGrid, targetGrid);
            pathfinderCom.ReqFindPath(pStartPoint, pTargetPoint, gridInfos, finishCallBack);
        }
        
        private List<PathfinderGridInfo> CalcGridInfos(Vector2Int pStartPoint, Vector2Int pTargetPoint, PathGrid startGrid, PathGrid targetGrid)
        {
            List<PathfinderGridInfo> gridInfos = new List<PathfinderGridInfo>();
            if (startGrid.Equals(targetGrid))
            {
                PathfinderGridInfo gridInfo = new PathfinderGridInfo(pStartPoint, pTargetPoint, startGrid);
                gridInfos.Add(gridInfo);
                return gridInfos;
            }

            float distance = int.MaxValue;
            PathGridConnectInfo resConnect = null;
            List<PathGridConnectInfo> startConnectInfos = GetOutConnectInfos(startGrid, targetGrid.Id);
            for (int i = 0; i < startConnectInfos.Count; i++)
            {
                float tDistance = Vector2Int.Distance(startConnectInfos[i].inPoint, pTargetPoint);
                if (tDistance < distance)
                {
                    resConnect = startConnectInfos[i];
                    distance = tDistance;
                }
            }

            if (resConnect == null)
            {
                return gridInfos;
            }

            if (CheckIsObs(resConnect.inPoint) || CheckIsObs(resConnect.outPoint))
            {
                Debug.Log($"寻路无效，网格连接点:{resConnect.inGridId}->{resConnect.outGridId}是阻挡{resConnect.inPoint}:{CheckIsObs(resConnect.inPoint)}--{resConnect.outPoint}:{CheckIsObs(resConnect.outPoint)}");
                return gridInfos;
            }

            PathfinderGridInfo startGridInfo  = new PathfinderGridInfo(pStartPoint, resConnect.inPoint, startGrid);
            PathfinderGridInfo targetGridInfo = new PathfinderGridInfo(resConnect.outPoint, pTargetPoint, targetGrid);
            gridInfos.Add(startGridInfo);
            gridInfos.Add(targetGridInfo);
            return gridInfos;
        }

        private List<PathGridConnectInfo> GetOutConnectInfos(PathGrid grid,int checkGridId)
        {
            List<PathGridConnectInfo> resInfos = new List<PathGridConnectInfo>();
            foreach (var item in connectInfos)
            {
                if (item.inGridId == grid.Id && item.outGridId == checkGridId)
                {
                    resInfos.Add(item);
                }
            }
            return resInfos;
        }

        //随机一个可移动的点
        public bool RandomPoint(Vector3Int centerTilePos, int range, out Vector3Int resultTilePos)
        {
            int maxPoolCnt = 10;
            List<Vector3Int> posPool = new List<Vector3Int>();
            Vector2Int checkTilePos = Vector2Int.zero;
            for (int x = -range; x <= range; x++)
            {
                for (int y = -range; y < range; y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    checkTilePos.x = centerTilePos.x + x;
                    checkTilePos.y = centerTilePos.y + y;
                    if (!CheckIsObs(checkTilePos))
                    {
                        posPool.Add(new Vector3Int(checkTilePos.x, checkTilePos.y, 0));
                        if (posPool.Count >= maxPoolCnt)
                            break;
                    }
                }
            }

            if (posPool.Count == 0)
            {
                resultTilePos = Vector3Int.zero;
                return false;
            }
            int randomIndex = UnityEngine.Random.Range(0, posPool.Count);
            resultTilePos = posPool[randomIndex];
            return true;
        }
    }
}