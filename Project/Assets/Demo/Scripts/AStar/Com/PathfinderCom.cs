using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Demo.Logic;
using LCMap;
using UnityEngine;

namespace Demo.AStar.Com
{
    public class PathfinderRequest
    {
        private const int timeOut = 5;

        private PathfinderGridInfo finderGridInfo;
        private Pathfinder currFinder;
        private TimeoutController timeoutController;
        private CancellationTokenSource cancelSource;
        
        private Action finishCallBack;
        private List<Vector2Int> resultPath;

        public PathfinderRequest(PathfinderGridInfo finderGridInfo, Action findCallBack)
        {
            this.finderGridInfo = finderGridInfo;
            
            PathNode startPathNode      = finderGridInfo.grid.GetNode(finderGridInfo.startGridPos.x, finderGridInfo.startGridPos.y);
            PathNode targetPathNode     = finderGridInfo.grid.GetNode(finderGridInfo.targetGridPos.x, finderGridInfo.targetGridPos.y);
            
            this.currFinder = new Pathfinder(finderGridInfo.grid, startPathNode, targetPathNode);
            this.finishCallBack = findCallBack;
        }

        public void StartFind()
        {
            StopFind();
            FindPathAsync().Forget();
        }
        
        private async UniTaskVoid FindPathAsync()
        {
            var (canceled,paths) = await UniTask.RunOnThreadPool(() =>
            {
                return currFinder.FindPath();
            },true,timeoutController.Timeout(TimeSpan.FromSeconds(timeOut))).SuppressCancellationThrow();
            timeoutController.Reset();

            await UniTask.Yield(PlayerLoopTiming.Update);
            
            if (canceled)
            {
                resultPath = null;
            }

            resultPath = paths;

            OnFinishFindPath();
        }
        
        private void OnFinishFindPath()
        {
            Action func = finishCallBack;
            finishCallBack = null;
            func?.Invoke();
        }
        
        public void StopFind()
        {
            timeoutController?.Dispose();
            cancelSource = new CancellationTokenSource();
            timeoutController = new TimeoutController(cancelSource);
        }

        public void Clear()
        {
            timeoutController?.Dispose();
            finishCallBack = null;
        }

        public List<Vector2Int> GetResultPath()
        {
            if (!resultPath.IsLegal())
            {
                return null;
            }
            else
            {
                List<Vector2Int> resPath = new List<Vector2Int>();
                for (int i = 0; i < resultPath.Count; i++)
                {
                    Vector2Int point = resultPath[i];
                    Vector2Int gridPos = finderGridInfo.grid.GridToTilePos(point);
                    resPath.Add(gridPos);
                }
                return resPath;
            }
        }
    }
    
    
    public class PathfinderGridInfo
    {
        public Vector2Int startTilePos;
        public Vector2Int startGridPos;

        public Vector2Int targetTilePos;
        public Vector2Int targetGridPos;
        
        public PathGrid grid;
        
        private PathfinderRequest request;
        private Action<PathfinderGridInfo> finishCallBack;

        public List<Vector3Int> resPath = new List<Vector3Int>();
        
        public PathfinderGridInfo(Vector2Int startTilePos, Vector2Int targetTilePos, PathGrid grid)
        {
            this.startTilePos  = startTilePos;
            this.startGridPos  = grid.TileToGridPos(startTilePos);

            this.targetTilePos = targetTilePos;
            this.targetGridPos = grid.TileToGridPos(targetTilePos);

            this.grid          = grid;
        }
    }
    
    public class PathfinderCom : MonoBehaviour
    {
        private Vector2Int startPoint;
        private Vector2Int targetPoint;

        private List<PathfinderGridInfo> findGridInfos = new List<PathfinderGridInfo>();
        private List<PathfinderRequest> finderRequests = new List<PathfinderRequest>();
        
        private int finishIndex = 0;
        private List<Vector2Int> findPaths = new List<Vector2Int>();
        private Action<List<Vector2Int>> finishCallBack;
        private bool waitFinish = false;

        private void Awake()
        {
            MapLocate.Map.GetLogicModule<MapSeekPathLogic>().AddCom(this);
        }

        private void OnDisable()
        {
            Clear();
        }

        private void OnDestroy()
        {
            Clear();
            MapLocate.Map.GetLogicModule<MapSeekPathLogic>().RemoveCom(this);
        }

        public void ReqFindPath(Vector2Int pStartPoint, Vector2Int pTargetPoint, List<PathfinderGridInfo> gridInfos, Action<List<Vector2Int>> finishCallBack)
        {
            this.finishCallBack = finishCallBack;

            //相同请求
            if (startPoint.Equals(pStartPoint) && targetPoint.Equals(pTargetPoint))
            {
                if (findPaths.Count > 0)
                {
                    OnFinish();
                    return;
                }
            }

            this.waitFinish = true;
            this.startPoint = pStartPoint;
            this.targetPoint = pTargetPoint;
            this.findPaths.Clear();

            this.finishIndex   = 0;
            this.findGridInfos = gridInfos;

            for (int i = 0; i < finderRequests.Count; i++)
            {
                finderRequests[i].Clear();
            }
            finderRequests.Clear();
            
            for (int i = 0; i < findGridInfos.Count; i++)
            {
                PathfinderRequest request = new PathfinderRequest(findGridInfos[i], () =>
                {
                    if (this.finishIndex >= findGridInfos.Count-1)
                    {
                        CalcPathInfo();
                        OnFinish();
                        return;
                    }
                    this.finishIndex++;
                });
                finderRequests.Add(request);
            }
        }
        
        public void Clear()
        {
            this.finishCallBack = null;
            
            this.findPaths.Clear();

            this.finishIndex   = 0;

            this.findGridInfos.Clear();
            
            for (int i = 0; i < finderRequests.Count; i++)
            {
                finderRequests[i].Clear();
            }
            finderRequests.Clear();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            for (int i = 0; i < finderRequests.Count; i++)
            {
                finderRequests[i].StopFind();
            }
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Resume()
        {
            for (int i = 0; i < finderRequests.Count; i++)
            {
                finderRequests[i].StartFind();
            }
        }

        private void CalcPathInfo()
        {
            this.findPaths.Clear();
            for (int i = 0; i < finderRequests.Count; i++)
            {
                PathfinderRequest request = finderRequests[i];
                List<Vector2Int> path = request.GetResultPath();
                if (!path.IsLegal())
                {
                    this.findPaths.Clear();
                    return;
                }
                
                this.findPaths.AddRange(path);
            }
        }

        private void OnFinish()
        {
            Action<List<Vector2Int>> func = finishCallBack;
            this.finishCallBack = null;
            Clear();
            
            func(findPaths);
        }
    }
}