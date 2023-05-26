using System;
using System.Collections.Generic;
using Demo.GAS.Attribute;
using LCECS.Core;
using LCGAS;
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
        public const float FinishDis = 0.05f;
        
        private TransCom transCom;
        private AbilitySystemCom abilitySystemCom;
        
        public Vector3 currTargetPos;
        public Vector3Int currTargetTilePos;
        public int currTargetIndex;
        private List<Vector3> movePoints = new List<Vector3>();
        private List<Vector3Int> moveTilePoints = new List<Vector3Int>();
        
        private Action finishCallBack;
        private Action failCallBack;
        
        private Action<Vector3> preframeCallBack;
        private Action<Vector3, Vector3, Vector3Int> prePointCallBack;

        
        protected override void OnAwake(Entity pEntity)
        {
            pEntity.GetCom<TransCom>();
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

            Action func = finishCallBack;
            ClearCallBack();
            func?.Invoke();
        }
        
        private void OnStartMoveToPoint()
        {
            prePointCallBack?.Invoke(transCom.Pos, currTargetPos, currTargetTilePos);
        }

        
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
    }
}