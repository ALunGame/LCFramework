using Demo.Com;
using Demo.Config;
using Demo.Help;
using LCECS;
using LCECS.Core.ECS;
using LCHelp;
using LCTileMap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Demo.System
{
    /// <summary>
    /// 寻路系统
    /// </summary>
    public class SeekPathSystem:BaseSystem
    {
        private Stopwatch stopwatch = new Stopwatch();

        protected override List<Type> RegListenComs()
        {
            return new List<Type>(){typeof(SeekPathCom),typeof(SpeedCom),typeof(ColliderCom), typeof(StateCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            stopwatch.Restart();

            SeekPathCom seekPathCom = GetCom<SeekPathCom>(comList[0]);
            SpeedCom speedCom       = GetCom<SpeedCom>(comList[1]);

            HandleState(comList);
            HandleSeek(seekPathCom,speedCom);
            HandleSeekColliderDir(comList);
            HandleShowVelocity(comList);
            
            stopwatch.Stop();
            //Debug.Log("寻路系统一次轮询花费>>>>>>" + stopwatch.Elapsed.TotalMilliseconds);
        }

        private void HandleState(List<BaseCom> comList)
        {
            SeekPathCom seekPathCom = GetCom<SeekPathCom>(comList[0]);
            SpeedCom speedCom       = GetCom<SpeedCom>(comList[1]);
            StateCom stateCom       = GetCom<StateCom>(comList[3]);
            if (stateCom.CurState == EntityState.Stop || stateCom.CurState == EntityState.Dead)
            {
                seekPathCom.ReqSeek = false;
                seekPathCom.TargetPos = seekPathCom.CurrPos;
                seekPathCom.MovePath = null;
                seekPathCom.IsSeeking = false;

                speedCom.ReqDash = false;
                speedCom.ReqJumpSpeed = 0;
                speedCom.ReqMoveSpeed = 0;
            }
        }

        //处理寻路
        private void HandleSeek(SeekPathCom seekPathCom,SpeedCom speedCom)
        {
            //请求寻路
            if (seekPathCom.ReqSeek)
            {
                Vector2Int curPos     = seekPathCom.CurrPos;
                Vector2Int tarPos     = seekPathCom.TargetPos;
                MapData mapData       = TempConfig.GetMapData(seekPathCom.MapPos);
                TaskHelp.AddTaskTwoParam(seekPathCom, mapData, ExcuteFindPath, FinishFindPath);
                seekPathCom.ReqSeek   = false;
            }

            if (seekPathCom.MovePath==null|| seekPathCom.MovePath.Count<=0)
            {
                return;
            }

            MoveFollowPath(seekPathCom.Trans, seekPathCom.MovePath, ref seekPathCom.CurrPathIndex, speedCom.MaxMoveSpeed, seekPathCom);
        }

        //处理寻路时的碰撞方向
        private void HandleSeekColliderDir(List<BaseCom> comList)
        {
            SeekPathCom seekPathCom = GetCom<SeekPathCom>(comList[0]);
            SpeedCom speedCom       = GetCom<SpeedCom>(comList[1]);
            ColliderCom colliderCom = GetCom<ColliderCom>(comList[2]);
            
            MapData mapData         = TempConfig.GetMapData(seekPathCom.MapPos);
            Vector2Int currPos      = seekPathCom.CurrPos;
            
            //地面
            Vector2Int groundCheckPos    = new Vector2Int(currPos.x,currPos.y-1);
            bool isGround    = mapData.ObstaclePos.Contains(groundCheckPos);
            
            //右边
            Vector2Int rightCheckPos    = new Vector2Int(currPos.x+1,currPos.y);
            bool isRightWall = mapData.ObstaclePos.Contains(rightCheckPos);
            
            //左边
            Vector2Int leftCheckPos    = new Vector2Int(currPos.x-1,currPos.y-1);
            bool isLeftWall = mapData.ObstaclePos.Contains(leftCheckPos);
            
            //赋值
            if (isGround)
            {
                colliderCom.CollideDir = ColliderDir.Down;
                //赋值子碰撞
                if (isRightWall)
                    colliderCom.SubCollideDir = ColliderDir.Right;
                else if (isLeftWall)
                    colliderCom.SubCollideDir = ColliderDir.Left;
                else
                    colliderCom.SubCollideDir = ColliderDir.None;
            }
            else
            {
                if (isRightWall)
                {
                    colliderCom.SubCollideDir = ColliderDir.Right;
                    colliderCom.CollideDir = ColliderDir.Right;
                }
                else if (isLeftWall)
                {
                    colliderCom.SubCollideDir = ColliderDir.Left;
                    colliderCom.CollideDir = ColliderDir.Left;
                }
                else
                {
                    colliderCom.SubCollideDir = ColliderDir.None;
                    colliderCom.CollideDir = ColliderDir.None;
                }
            }
            
        }

        //处理显示的速度
        private void HandleShowVelocity(List<BaseCom> comList)
        {
            SeekPathCom seekPathCom = GetCom<SeekPathCom>(comList[0]);
            SpeedCom speedCom       = GetCom<SpeedCom>(comList[1]);
            ColliderCom colliderCom = GetCom<ColliderCom>(comList[2]);
            
            
            if (seekPathCom.IsSeeking==false)
            {
                speedCom.CurVelocity = Vector2.zero;
                return;
            }
            
            Vector2Int currPos      = seekPathCom.CurrPos;
            Vector2Int targetPos    = seekPathCom.TargetPos;

            if (targetPos.x==currPos.x)
            {
                speedCom.CurVelocity.x = 0;
            }
            else
            {
                if (targetPos.x>currPos.x)
                {
                    speedCom.CurVelocity.x = speedCom.MaxMoveSpeed;
                }
                else
                {
                    speedCom.CurVelocity.x = -speedCom.MaxMoveSpeed;
                }
            }
            
            if (targetPos.y==currPos.y)
            {
                speedCom.CurVelocity.y = 0;
            }
            else
            {
                if (targetPos.y>currPos.y)
                {
                    speedCom.CurVelocity.y = speedCom.MaxJumpSpeed;
                }
                else
                {
                    speedCom.CurVelocity.y = -speedCom.MaxJumpSpeed;
                }
            }
        }

        //路径移动
        //返回方向
        public void MoveFollowPath(Transform obj, List<Vector2Int> path, ref int index, float speed, SeekPathCom seekPathCom)
        {
            //走完了
            if (index >= path.Count)
            {
                seekPathCom.IsSeeking = false;
                path = null;
                return;
            }

            //偏移
            Vector2 targetPos   = new Vector2(path[index].x + 0.5f, path[index].y + 0.5f);
            obj.localPosition   = Vector2.MoveTowards(obj.localPosition, targetPos, speed * Time.deltaTime);

            float distance      = Vector2.Distance(targetPos, obj.localPosition);
            if (distance < 0.001f)
            {
                seekPathCom.CurrPos = path[index];
                index++;
                if (index == path.Count)
                {
                    Vector2 endPos        = new Vector2(path[index - 1].x + 0.5f, path[index - 1].y + 0.5f);
                    obj.localPosition     = endPos;
                    
                    seekPathCom.CurrPos   = path[index-1];
                    seekPathCom.IsSeeking = false;
                    path                  = null;
                }
            }
        }

        //执行寻路
        private (SeekPathCom SeekPathCom, List<Vector2Int> Path) ExcuteFindPath(SeekPathCom seekPathCom, MapData mapData)
        {
            List<Vector2Int> path = null;
            if (seekPathCom.CanFly)
            {
                path = AstarHelp.FindPath(seekPathCom.CurrPos, seekPathCom.TargetPos, mapData.ObstaclePos);
            }
            else
            {
                path = AstarHelp.FindPath(seekPathCom.CurrPos, seekPathCom.TargetPos, mapData.ObstaclePos, mapData.RoadPos);
            }
            return (seekPathCom, path);
        }

        //完成寻路
        private void FinishFindPath((SeekPathCom SeekPathCom, List<Vector2Int> Path) item)
        {
            //没有路径
            if (item.Path == null || item.Path.Count <= 0)
            {
                item.SeekPathCom.MovePath  = null;
                item.SeekPathCom.CurrPathIndex = 0;
                item.SeekPathCom.IsSeekHasNoWay = true;
            }
            else
            {
                //重复路径删除
                int newPathIndex = 0;
                Vector2Int currPos = item.SeekPathCom.CurrPos;
                if (currPos == null)
                    newPathIndex = 0;
                else
                {
                    for (int i = 0; i < item.Path.Count; i++)
                    {
                        if (item.Path[i].Equals(currPos))
                        {
                            newPathIndex = i;
                           
                            break;
                        }
                    }
                }
                
                //组件赋值
                item.SeekPathCom.MovePath = item.Path;
                item.SeekPathCom.MovePath.Insert(item.SeekPathCom.MovePath.Count, item.SeekPathCom.TargetPos);
                item.SeekPathCom.CurrPathIndex = newPathIndex;
                item.SeekPathCom.IsSeeking = true;
            }
        }
    }
}