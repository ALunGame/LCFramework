using System.Collections;
using UnityEngine;

namespace LCToolkit
{
    /// <summary>
    /// 锁定位置组件
    /// </summary>
    [ExecuteAlways]
    public class LockTransCom : MonoBehaviour
    {
        [Header("锁定位置")]
        public bool lockPos = true;
        [Header("锁定位置值")]
        public Vector3 lockPosValue = Vector3.zero;

        [Header("锁定旋转")]
        public bool lockRoate;
        [Header("锁定旋转值")]
        public Vector3 lockRoateValue = Vector3.zero;

        [Header("锁定缩放")]
        public bool lockScale;
        [Header("锁定缩放值")]
        public Vector3 lockScaleValue = Vector3.one;


        private void Update()
        {
            UpdateLockValue();
        }

        private void UpdateLockValue()
        {
            if (lockPos)
                transform.localPosition = lockPosValue;
            if (lockRoate)
                transform.localEulerAngles = lockRoateValue;
            if (lockScale)
                transform.localScale = lockScaleValue;
        }
    }
}