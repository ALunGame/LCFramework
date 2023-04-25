using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using LCLoad;
using LCJson;
using LCECS;

namespace Demo.System
{
    public class WayPointMoveSystem : BaseSystem
    {
        public static string CnfSavePath = "Assets/Demo/Asset/Config/TbMapRoadCnf.txt";
        public static string CnfFileName = "TbMapRoadCnf";

        public static TbMapRoadCnf RoadCnf;

        protected override List<Type> RegContainListenComs()
        {
            string jsonStr = LoadHelper.LoadString(CnfFileName);
            List<MapRoadCnf> roadCnfs = JsonMapper.ToObject<List<MapRoadCnf>>(jsonStr);

            RoadCnf = new TbMapRoadCnf();
            foreach (var item in roadCnfs)
            {
                Vector2Int pos = item.tileWorldPos;
                if (!RoadCnf.Exist(pos))
                {
                    if (!RoadCnf.ContainsKey(pos.x))
                    {
                        RoadCnf[pos.x] = new Dictionary<int, MapRoadCnf>();
                    }
                    RoadCnf[pos.x].Add(pos.y, item);
                }
            }

            return new List<Type>() { typeof(WayPointMoveCom), typeof(AnimCom), typeof(BasePropertyCom), typeof(TransCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            WayPointMoveCom wayPointMoveCom = GetCom<WayPointMoveCom>(comList[0]);
            AnimCom animCom = GetCom<AnimCom>(comList[1]);
            BasePropertyCom propertyCom = GetCom<BasePropertyCom>(comList[2]);
            TransCom transCom = GetCom<TransCom>(comList[3]);

            if (wayPointMoveCom.currRoadCnf == null)
                return;

            if (CheckMoveFinish(wayPointMoveCom, animCom))
            {

            }
            else
            {
                Vector2 selfPos = transCom.Pos;
                Vector2 targetPos = wayPointMoveCom.currRoadCnf.roadPos;

                DirType dir = DirType.Right;
                //方向
                if (selfPos.x - targetPos.x > 0)
                    dir = DirType.Left;
                else
                    dir = DirType.Right;

                int yDir;
                if (selfPos.y - targetPos.y > 0)
                {
                    yDir = -1;
                }
                else
                {
                    yDir = 1;
                }

                // //位移
                // Vector3 delta = new Vector3(dir == DirType.Right ? 1 : -1, yDir, 0);
                // transCom.MoveDir(delta, propertyCom.MoveSpeed.Curr);
            }
        }


        private bool CheckMoveFinish(WayPointMoveCom wayPointMoveCom, AnimCom animCom)
        {
            if (Vector2.Distance(wayPointMoveCom.trans.position, wayPointMoveCom.currRoadCnf.roadPos) <= 0.02f)
            {
                //完成
                if (wayPointMoveCom.currTargetPos.Equals(wayPointMoveCom.endTargetPos))
                {
                    Debug.Log("完成>>>>>");
                    return true;
                }

                //分叉返回中
                if (wayPointMoveCom.isBranchWayBack && wayPointMoveCom.branchWayBackPath.Count > 0)
                {
                    SetNextPos(wayPointMoveCom, animCom, RoadCnf, wayPointMoveCom.branchWayBackPath.Dequeue());
                    Debug.Log("分叉返回中>>>>>");
                    return false;
                }
                wayPointMoveCom.isBranchWayBack = false;

                if (wayPointMoveCom.movedWayPoints.Count <= 0 || !wayPointMoveCom.movedWayPoints.Peek().Equals(wayPointMoveCom.currTargetPos))
                {
                    wayPointMoveCom.movedWayPoints.Push(wayPointMoveCom.currTargetPos);
                }

                bool isBranch = false;
                if (CheckIsBranchWayPoint(wayPointMoveCom.currTargetPos, wayPointMoveCom, RoadCnf))
                {
                    isBranch = true;
                    wayPointMoveCom.branchWayPointStack.Push(wayPointMoveCom.currTargetPos);
                }

                if (GetNextPoint(wayPointMoveCom, RoadCnf, out var nextPos))
                {
                    if (isBranch)
                    {
                        if (!wayPointMoveCom.branchMovedPoints.Contains(nextPos))
                        {
                            wayPointMoveCom.branchMovedPoints.Add(nextPos);
                        }
                    }
                        
                    Debug.Log("下一个目标>>>>>" + nextPos);
                    SetNextPos(wayPointMoveCom, animCom, RoadCnf, nextPos);
                }
                else
                {
                    if (wayPointMoveCom.branchWayPointStack.Count > 0)
                    {
                        Debug.Log("没有下一个目标，返回分叉路>>>>>" + wayPointMoveCom.currTargetPos);


                        wayPointMoveCom.isBranchWayBack = true;
                        Vector2Int branchPoint = wayPointMoveCom.branchWayPointStack.Pop();
                        wayPointMoveCom.branchWayBackPath = GetBranchWayBackPath(branchPoint, wayPointMoveCom);
                        SetNextPos(wayPointMoveCom, animCom, RoadCnf, wayPointMoveCom.branchWayBackPath.Dequeue());
                    }
                    else
                    {
                        Debug.Log("没有路了>>>>>");
                    }
                }
            }
            return false;
        }

        private void SetNextPos(WayPointMoveCom wayPointMoveCom, AnimCom animCom, TbMapRoadCnf roadCnf, Vector2Int nextPos)
        {
            wayPointMoveCom.currTargetPos = nextPos;
            wayPointMoveCom.currRoadCnf   = roadCnf[nextPos.x][nextPos.y];
            if (wayPointMoveCom.currRoadCnf.roadAnim != "")
            {
                animCom.SetReqAnim("walk", AnimLayer.Side);
            }
        }

        private Queue<Vector2Int> GetBranchWayBackPath(Vector2Int branchPoint, WayPointMoveCom wayPointMoveCom)
        {
            Queue<Vector2Int> path = new Queue<Vector2Int>();
            for (int i = 0; i < wayPointMoveCom.movedWayPoints.Count; i++)
            {
                Vector2Int movedPoint = wayPointMoveCom.movedWayPoints.Pop();
                path.Enqueue(movedPoint);
                if (movedPoint.Equals(branchPoint))
                {
                    break;
                }
            }
            return path;
        }

        private bool GetNextPoint(WayPointMoveCom wayPointMoveCom, TbMapRoadCnf roadCnf, out Vector2Int nextPos,bool checkMoved = true)
        {
            nextPos = Vector2Int.zero;
            List<Vector2Int> nearlist = GetNearWayPoints(wayPointMoveCom, wayPointMoveCom.currTargetPos, roadCnf, checkMoved);
            if (nearlist.Count <= 0)
                return false;
            Vector2Int endPos  = wayPointMoveCom.endTargetPos;
            Vector2Int currPos = wayPointMoveCom.currTargetPos;

            int checkX = 0;
            int checkY = 0;
            if (endPos.x != currPos.x)
                checkX = endPos.x > currPos.x ? 1 : -1;
            if (endPos.y != currPos.y)
                checkY = endPos.y > currPos.y ? 1 : -1;

            Vector2Int checkPos = new Vector2Int(currPos.x + checkX, currPos.y + checkY);
            if (nearlist.Contains(checkPos))
            {
                nextPos = checkPos;
                return true;
            }

            int startX = 0;
            int endX = 0;
            int startY = 0;
            int endY = 0;
            if (checkX != 0)
            {
                startX = checkX > 0 ? 0 : checkX;
                endX = checkX > 0 ? checkX : 0;
            }
            if (checkY != 0)
            {
                startY = checkY > 0 ? 0 : checkY;
                endY = checkY > 0 ? checkY : 0;
            }

            for (int x = startX; x <= endX; x++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    checkPos = new Vector2Int(currPos.x + x, currPos.y + y);
                    if (nearlist.Contains(checkPos))
                    {
                        nextPos = checkPos;
                        return true;
                    }
                }
            }

            nextPos = nearlist[0];
            return true;
        }

        private List<Vector2Int> GetNearWayPoints(WayPointMoveCom wayPointMoveCom, Vector2Int checkPos, TbMapRoadCnf roadCnf, bool checkMoved = true)
        {
            List<Vector2Int> nearlist = new List<Vector2Int>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector2Int nearPos = new Vector2Int(x, y) + checkPos;
                    if (!nearPos.Equals(checkPos) && roadCnf.Exist(nearPos))
                    {
                        if (checkMoved)
                        {
                            if (!wayPointMoveCom.movedWayPoints.Contains(nearPos) && !wayPointMoveCom.branchMovedPoints.Contains(nearPos))
                            {
                                nearlist.Add(nearPos);
                            }
                        }
                        else
                        {
                            nearlist.Add(nearPos);
                        }
                    }
                }
            }
            return nearlist;    
        }

        private bool CheckIsBranchWayPoint(Vector2Int point, WayPointMoveCom wayPointMoveCom, TbMapRoadCnf roadCnf)
        {
            List<Vector2Int> nearlist = GetNearWayPoints(wayPointMoveCom, point, roadCnf);
            return nearlist.Count > 1;
        }
    }
}