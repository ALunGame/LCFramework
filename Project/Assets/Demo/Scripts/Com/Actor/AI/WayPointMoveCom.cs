using System.Collections;
using UnityEngine;
using LCECS.Core;
using System.Collections.Generic;
using Demo.System;
using System;

namespace Demo.Com
{
    /// <summary>
    /// 道路移动
    /// </summary>
    public class WayPointMoveCom : BaseCom
    {

        //最终目标
        [NonSerialized]
        public Vector2Int endTargetPos;

        //当前移动的目标
        [NonSerialized]
        public Vector2Int currTargetPos;
        //当前道路信息
        [NonSerialized]
        public MapRoadCnf currRoadCnf;

        //已经移动的路径点
        [NonSerialized]
        public Stack<Vector2Int> movedWayPoints = new Stack<Vector2Int>();

        //分叉路栈
        [NonSerialized]
        public Stack<Vector2Int> branchWayPointStack = new Stack<Vector2Int>();

        //正在返回分叉路
        [NonSerialized]
        public bool isBranchWayBack = false;
        [NonSerialized]
        public Queue<Vector2Int> branchWayBackPath = new Queue<Vector2Int>();
        //已经移动的路径点
        [NonSerialized]
        public List<Vector2Int> branchMovedPoints = new List<Vector2Int>();
        [NonSerialized]
        public Transform trans;

        protected override void OnInit(GameObject go)
        {
            trans = go.transform;
        }

        public bool CheckIsFinish()
        {
            if (currRoadCnf == null)
            {
                return true;
            }
            if (Vector2.Distance(trans.position, currRoadCnf.roadPos) <= 0.1f)
            {
                //完成
                if (currTargetPos.Equals(endTargetPos))
                {
                    Debug.Log("完成>>>>>");
                    return true;
                }
            }
            return false;
        }

        public void SetWayPointTarget(Vector2Int endPos)
        {
            if (WayPointMoveSystem.RoadCnf.CalcRoadPos(trans.position, out var pos))
            {
                endTargetPos = endPos;
                currTargetPos = pos;
                currRoadCnf = WayPointMoveSystem.RoadCnf[pos.x][pos.y];
                movedWayPoints.Clear();
                branchWayPointStack.Clear();
                isBranchWayBack = false;
                branchWayBackPath.Clear();
                branchMovedPoints.Clear();

                Debug.Log($"设置路径移动信息>>>>>Pos:{trans.position}Curr:{currTargetPos} End:{endTargetPos}");
            }
        }

        public void Clear()
        {
            currRoadCnf = null;
            movedWayPoints.Clear();
            branchWayPointStack.Clear();
            isBranchWayBack = false;
            branchWayBackPath.Clear();
            branchMovedPoints.Clear();

        }
    }
}