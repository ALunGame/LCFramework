using Demo.Com;
using LCECS.Core;
using System;
using System.Collections.Generic;

namespace Demo.System
{
    public class FollowCameraSystem : BaseSystem
    {
        protected override List<Type> RegListenComs()
        {
            return new List<Type>() { typeof(FollowCameraCom) };
        }

        protected override void HandleComs(List<BaseCom> comList)
        {
            throw new NotImplementedException();
        }
    }
}