using System;
using UnityEditor;
using UnityEngine;

namespace Demo.AStar.Com
{
    [ExecuteAlways]
    public class PathGridConnectCom : MonoBehaviour
    {
        private PathGridCom gridCom;
        
        [Header("连接区域Id")]
        public int connectGridId;

        [Header("连接区域位置")]
        public Transform connectTrans;
        
        private void Awake()
        {
            gridCom = transform.parent.GetComponent<PathGridCom>();
            connectTrans = transform.Find("connect");
            if (connectTrans == null)
            {
                connectTrans = new GameObject("connect").transform;
                connectTrans.SetParent(transform);
            }
        }

        private void OnEnable()
        {
            gridCom = transform.parent.GetComponent<PathGridCom>();
            connectTrans = transform.Find("connect");
            if (connectTrans == null)
            {
                connectTrans = new GameObject("connect").transform;
                connectTrans.SetParent(transform);
            }
        }
        
        public PathGridConnectInfo CreateConnectInfo()
        {
            PathGridConnectInfo connectInfo = new PathGridConnectInfo();

            connectInfo.inGridId = gridCom.gridId;
            Vector3Int tilePos   = gridCom.gridTilemap.WorldToCell(transform.position);
            connectInfo.inPoint  = new Vector2Int(tilePos.x, tilePos.y);

            connectInfo.outGridId = connectGridId;
            tilePos = gridCom.gridTilemap.WorldToCell(connectTrans.position);
            connectInfo.outPoint = new Vector2Int(tilePos.x, tilePos.y);

            return connectInfo;
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            GUIStyle gUIStyle = new GUIStyle();
            gUIStyle.fontSize = 12;
            gUIStyle.fontStyle = FontStyle.Bold;
            
            Handles.DrawLine(transform.position,connectTrans.position);
            
            Handles.Label(transform.position, gridCom.gridTilemap.WorldToCell(transform.position).ToString(), gUIStyle);
            if (connectTrans != null)
            {
                Handles.Label(connectTrans.position, gridCom.gridTilemap.WorldToCell(connectTrans.position).ToString(), gUIStyle);
            }
#endif
        }
    }
}