using System;
using System.Collections.Generic;
using AStar;
using Demo.AStar;
using Demo.AStar.Com;
using LCMap;
using LCToolkit;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Demo.Logic
{
    /// <summary>
    /// 地图寻路逻辑
    /// </summary>
    public class MapSeekPathLogic : BaseServerLogicModule<MapServer>
    {
        private Dictionary<int, MapRoadCnf> mapRoadCnfDict = new Dictionary<int, MapRoadCnf>();

        private Dictionary<MapArea, Tilemap> areaGridTileMapDict = new Dictionary<MapArea, Tilemap>();
        private Dictionary<MapArea, PathGrid> areaGridDict = new Dictionary<MapArea, PathGrid>();
        private List<PathGridConnectInfo> connectInfos = new List<PathGridConnectInfo>();
        
        private List<PathfinderCom> finderComs = new List<PathfinderCom>();
        private Tilemap pathTileMap;
        
        public override void OnInit()
        {
            foreach (MapArea area in server.areaDict.Values)
            {
                PathGridCom pathGridCom = area.AreaEnvGo.GetComponentInChildren<PathGridCom>();
                if (pathGridCom != null)
                {
                    areaGridTileMapDict.Add(area,pathGridCom.gridTilemap);
                    PathGrid pathGrid = pathGridCom.CreateGrid();
                    areaGridDict.Add(area,pathGrid);
                    connectInfos.AddRange(pathGrid.ConnectInfos);
                    
                    pathTileMap = pathGridCom.gridTilemap;
                }
            }
        }

        public override void OnClear()
        {
            areaGridTileMapDict.Clear();
            areaGridDict.Clear();
            connectInfos.Clear();
            
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
            foreach (var item in areaGridDict.Values)
            {
                if (item.CheckInGrid(pTilePos))
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 世界坐标转Tile坐标
        /// </summary>
        /// <returns></returns>
        public Vector3Int WorldToTilePos(Vector3 pWorldPos)
        {
            return pathTileMap.WorldToCell(pWorldPos);
        }
        
        /// <summary>
        /// Tile坐标转世界坐标
        /// </summary>
        /// <returns></returns>
        public Vector3 TileToWorldPos(Vector3Int pTilePos)
        {
            return pathTileMap.CellToWorld(pTilePos);
        }

        #endregion

        #region Set
        
        public void SetPointsObs(List<Vector2Int> pObsTilePos, List<RoadCnf> pRoads = null)
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
            
            if (pRoads.IsLegal())
            {
                for (int i = 0; i < pRoads.Count; i++)
                {
                    PathGrid grid = GetGrid(pRoads[i].tilePos);
                    if (grid == null)
                        return;
                    grid.SetRoad(pRoads[i]);
                }
            }

            ResumeFind();
        }

        public void StopFind(GameObject pGo)
        {
            PathfinderCom pathfinderCom = pGo.GetComponent<PathfinderCom>();
            if (pathfinderCom == null)
                return;
            
            pathfinderCom.Clear();
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
        /// <param name="pStartPoint">瓦片起始点</param>
        /// <param name="pTargetPoint">瓦片目标点</param>
        /// <param name="finishCallBack">返回瓦片坐标</param>
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

        /// <summary>
        /// 随机一个可移动的点
        /// </summary>
        /// <param name="centerTilePos">中心点</param>
        /// <param name="range">范围</param>
        /// <param name="resultTilePos"></param>
        /// <returns></returns>
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