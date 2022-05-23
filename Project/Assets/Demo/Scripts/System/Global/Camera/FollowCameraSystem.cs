using Demo.Com;
using LCECS.Core;
using LCMap;
using System;
using System.Collections.Generic;

namespace Demo.System
{
    public class FollowCameraSystem : BaseSystem
    {
        private GlobalSensor globalSensor;
        private bool _InitEvent = false;
        private FollowCameraCom followCameraCom;

        protected override List<Type> RegListenComs()
        {
            globalSensor = LCECS.ECSLayerLocate.Info.GetSensor<GlobalSensor>(LCECS.SensorType.Global);
            globalSensor.CurrArea.RegisterValueChangedEvent(HandleCurrAreaChange);
            globalSensor.FollowActor.RegisterValueChangedEvent(HandleFollowActorChange);
            return new List<Type>() { typeof(FollowCameraCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            if (!_InitEvent)
            {
                followCameraCom = GetCom<FollowCameraCom>(comList[0]);
                HandleCurrAreaChange(globalSensor.CurrArea.Value);
                HandleFollowActorChange(globalSensor.FollowActor.Value);
                _InitEvent = true;
            }
        }

        private void HandleCurrAreaChange(MapArea area)
        {
            if (followCameraCom == null)
                return;
            followCameraCom.CMCamera = area.FollowCamera;
        }

        private void HandleFollowActorChange(ActorObj obj)
        {
            if (followCameraCom == null)
                return;
            followCameraCom.CMCamera.Follow = obj.transform;
        }
    }
}