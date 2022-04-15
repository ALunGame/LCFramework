#if UNITY_EDITOR
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// 地图导出数据组件
    /// </summary>
    public abstract class ED_MapDataCom : MonoBehaviour
    {
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        public abstract object ExportData();
    }
} 
#endif
