#if UNITY_EDITOR
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// ��ͼ�����������
    /// </summary>
    public abstract class ED_MapDataCom : MonoBehaviour
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public abstract object ExportData();
    }
} 
#endif
