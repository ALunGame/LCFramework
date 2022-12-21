using System;
using LCECS;
using LCECS.Data;
using LCMap;
using UnityEngine;

namespace Demo
{
    public class RequestInputMove : RequestData
    {
        public override RequestId ReqId { get => RequestId.InputMove; }

        public Vector2 inputMove;
    }
    
    public class RequestMoveToActor : RequestData
    {
        public override RequestId ReqId { get => RequestId.MoveToActor; }

        public Actor targetActor;

        public Action finishCallBack;
    }
}