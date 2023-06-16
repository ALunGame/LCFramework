using System;
using System.Collections.Generic;
using Demo.GAS.Attribute;
using Demo.Logic;
using LCECS.Core;
using LCGAS;
using LCMap;
using LCToolkit;
using UnityEngine;

namespace Demo.Com.Move
{
    public class MovePointInfo
    {
        public Vector2 targetPos;

        public string moveAnimName;

        public string arriveAnimName;
    }
    
    public class ActorMoveCom : BaseCom
    {
        [NonSerialized] public const float FinishDis = 0.05f;
        
        [NonSerialized] private TransCom transCom;
        [NonSerialized] private AbilitySystemCom abilitySystemCom;
        [NonSerialized] private BindGoCom bindGoCom;
        
        [NonSerialized] public Vector3 currTargetPos;
        [NonSerialized] public Vector3Int currTargetTilePos;
        
        [NonSerialized] public int currTargetIndex;
        [NonSerialized] private List<Vector3> movePoints = new List<Vector3>();
        [NonSerialized] private List<Vector3Int> moveTilePoints = new List<Vector3Int>();
        
        [NonSerialized] private Action finishCallBack;
        [NonSerialized] private Action failCallBack;
        
        [NonSerialized] private Action<Vector3> preframeCallBack;
        [NonSerialized] private Action<Vector3, Vector3, Vector3Int> prePointCallBack;
        
        [NonSerialized] private MapSeekPathLogic seekPathLogic;
        
        protected override void OnAwake(Entity pEntity)
        {
            seekPathLogic = MapLocate.Map.GetLogicModule<MapSeekPathLogic>();
            transCom = pEntity.GetCom<TransCom>();
            abilitySystemCom = pEntity.GetCom<AbilitySystemCom>();
            bindGoCom = pEntity.GetCom<BindGoCom>();
        }

        protected override void OnDisable()
        {
            Stop();
        }

        protected override void OnDestroy()
        {
            Stop();
        }

        private void HandleMoveToTargetPos()
        {
            float speed = abilitySystemCom.GetAttrValue(AttributeDef.Speed);
            Vector3 pos = Vector3.MoveTowards(transCom.Pos, currTargetPos, Time.deltaTime * speed);
            transCom.SetPos(pos);
            
            if (Vector3.Distance(transCom.Pos, currTargetPos) <= FinishDis)
            {
                currTargetIndex++;
                StartMoveToPoint();
            }
            else
            {
                preframeCallBack?.Invoke(transCom.Pos);
            }
        }
        
        private void StartMoveToPoint()
        {
            if (currTargetIndex >= movePoints.Count)
            {
                OnMoveFinish();
                return;
            }
            currTargetPos = movePoints[currTargetIndex];
            currTargetTilePos = moveTilePoints[currTargetIndex];
            OnStartMoveToPoint();
        }
        
        private void OnMoveFinish()
        {
            currTargetIndex = 0;
            movePoints.Clear();
            moveTilePoints.Clear();

            Action func = finishCallBack;
            ClearCallBack();
            func?.Invoke();
        }
        
        private void OnStartMoveToPoint()
        {
            prePointCallBack?.Invoke(transCom.Pos, currTargetPos, currTargetTilePos);
        }

        public void StopFind()
        {
            seekPathLogic.StopFind(bindGoCom.Go);
        }

        #region 接口

        public void SetCallBack(Action finishCallBack, Action<Vector3, Vector3, Vector3Int> prePointCallBack, Action<Vector3> preframeCallBack, Action failCallBack = null)
        {
            this.finishCallBack = finishCallBack;
            this.prePointCallBack = prePointCallBack;   
            this.preframeCallBack = preframeCallBack;   
            this.failCallBack = failCallBack;
        }
        
        private void ClearCallBack()
        {
            this.finishCallBack = null;
            this.prePointCallBack = null;   
            this.preframeCallBack = null;   
            this.failCallBack = null;
        }
        
        /// <summary>
        /// 停止移动
        /// </summary>
        public void Stop()
        {
            ClearCallBack();
            
            currTargetIndex = 0;
            movePoints.Clear();
            moveTilePoints.Clear();

            StopFind();
        }
        
        /// <summary>
        /// 移动到指定位置（寻路）
        /// </summary>
        /// <param name="pTargetPos"></param>
        public void MoveByAStar(Vector3 pTargetPos)
        {
            StopFind();
            
            Vector3Int startTilePos = seekPathLogic.WorldToTilePos(transCom.Pos);
            Vector3Int targetTilePos = seekPathLogic.WorldToTilePos(pTargetPos);
            
            if (startTilePos == targetTilePos)
            {
                OnMoveFinish();
                return;
            }
            
            seekPathLogic.ReqSearchPath(bindGoCom.Go,startTilePos.ToVectorInt2(),targetTilePos.ToVectorInt2(), (resPath) =>
            {
                if (!resPath.IsLegal())
                {
                    failCallBack?.Invoke();
                    return;
                }
                
                List<Vector3> movePath = new List<Vector3>();
                List<Vector3Int> moveTilePoints = new List<Vector3Int>();
                for (int i = 0; i < resPath.Count; i++)
                {
                    Vector3Int tilePos = resPath[i].ToVectorInt3();
                    movePath.Add(seekPathLogic.TileToWorldPos(tilePos));
                    moveTilePoints.Add(tilePos);
                }

                MoveByPath(movePath,moveTilePoints);
            });
        }
        
        /// <summary>
        /// 路径移动
        /// </summary>
        /// <param name="pMovePoints"></param>
        /// <param name="pMoveTilePoints"></param>
        public void MoveByPath(List<Vector3> pMovePoints,List<Vector3Int> pMoveTilePoints)
        {
            StopFind();
            
            this.movePoints = movePoints;
            this.moveTilePoints = pMoveTilePoints;
            this.currTargetIndex = 0;
            
            seekPathLogic.StopFind(bindGoCom.Go);
            
            StartMoveToPoint();
        }
        
        /// <summary>
        /// 路径移动
        /// </summary>
        /// <param name="pMovePoints"></param>
        /// <param name="pMoveTilePoints"></param>
        public void MoveByPath(List<Vector3> pMovePoints)
        {
            StopFind();
            
            this.movePoints = movePoints;
            this.moveTilePoints.Clear();
            for (int i = 0; i < movePoints.Count; i++)
            {
                moveTilePoints.Add(seekPathLogic.WorldToTilePos(movePoints[i]));
            }
            this.currTargetIndex = 0;
            
            seekPathLogic.StopFind(bindGoCom.Go);
            
            StartMoveToPoint();
        }
        
        /// <summary>
        /// 移动到指定位置（直接过去）
        /// </summary>
        /// <param name="pTargetPoint"></param>
        public void MoveToPoint(Vector3 pTargetPoint)
        {
            StopFind();
            
            this.movePoints = new List<Vector3>() { pTargetPoint };
            this.moveTilePoints = new List<Vector3Int>() { seekPathLogic.WorldToTilePos(pTargetPoint) };
            this.currTargetIndex = 0;
            
            seekPathLogic.StopFind(bindGoCom.Go);
            
            StartMoveToPoint();
        }

        #endregion
    }
}