﻿using Cinemachine;
using LCECS.Core;
using UnityEngine;

namespace Demo.Com
{
    [Com(ViewName = "相机组件", IsGlobal = true)]
    public class CameraCom : BaseCom
    {
        public CinemachineVirtualCamera VCCamera;

        public string TestStr;

        protected override void OnInit(GameObject go)
        {
            Transform comPoint = LCECS.ECSLocate.Player.GetPalyerGo().transform;

            Transform tran = GameObject.Find("Main Camera/VCam-FollowPalyer").transform;
            VCCamera = tran.GetComponent<CinemachineVirtualCamera>();
            VCCamera.Follow = comPoint;
        }
    }
}
