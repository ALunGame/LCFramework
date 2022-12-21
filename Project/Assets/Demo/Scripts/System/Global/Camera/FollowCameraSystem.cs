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
        private FollowCameraCom followCameraCom;

        protected override List<Type> RegContainListenComs()
        {
            globalSensor = LCECS.ECSLayerLocate.Info.GetSensor<GlobalSensor>(LCECS.SensorType.Global);
            globalSensor.CurrArea.RegisterValueChangedEvent(HandleCurrAreaChange);
            globalSensor.FollowActor.RegisterValueChangedEvent(HandleFollowActorChange);
            return new List<Type>() { typeof(FollowCameraCom) };
        }

        protected override void OnAddCheckComs(List<BaseCom> comList)
        {
            followCameraCom = GetCom<FollowCameraCom>(comList[0]);
            HandleCurrAreaChange(globalSensor.CurrArea.Value);
            HandleFollowActorChange(globalSensor.FollowActor.Value);
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
        }

        private void HandleCurrAreaChange(MapArea area)
        {
            if (followCameraCom == null || area == null)
                return;
            followCameraCom.CMCamera = area.FollowCamera;
        }

        private void HandleFollowActorChange(Actor obj)
        {
            if (followCameraCom == null || obj == null)
                return;
            followCameraCom.CMCamera.Follow = obj.DisplayCom.CameraFollowGo.transform;
        }
    }
}