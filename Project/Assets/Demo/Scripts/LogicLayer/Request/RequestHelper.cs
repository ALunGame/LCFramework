using System;
using LCMap;
using LCECS;
using Layer = LCECS.ECSLayerLocate;

namespace Demo
{
    public static class RequestHelper
    {
        public static void MoveToActor(string pEntityUid, Actor pTargetActor, Action pFinishCallBack)
        {
            RequestMoveToActor data = new RequestMoveToActor();
            data.targetActor = pTargetActor;
            data.finishCallBack = pFinishCallBack;
            Layer.Request.PushRequest(pEntityUid,data);
        }
    }
}