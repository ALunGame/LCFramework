#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// 演员路径点组件
    /// </summary>
    [ExecuteAlways]
    public class ED_ActorPathPointCom : ED_MapDataCom
    {
        [SerializeField]
        [Header("移动到此点的动画")]
        public string runAnimName;

        [SerializeField]
        [Header("等待时间")]
        public float waitTime;

        [SerializeField]
        [Header("等待动画")]
        public string waitAnimName;

        [SerializeField]
        [Header("等待参数")]
        public string waitExParam;

        public override object ExportData()
        {
            ActorPointInfo pointData = new ActorPointInfo();
            pointData.runAnimName = runAnimName;
            pointData.waitTime = waitTime;
            pointData.waitAnimName = waitAnimName;
            pointData.waitExParam = waitExParam;

            pointData.point = ED_ActorCom.HandlePos(transform.position);
            return pointData;
        }
    }
} 
#endif
