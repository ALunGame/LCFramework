#if UNITY_EDITOR
using LCToolkit;
using UnityEditor;
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// 演员路径组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [ExecuteAlways]
    public class ED_ActorPathCom : ED_MapDataCom
    {
        [SerializeField]
        [Header("默认路径")]
        public bool defaultPath = false;

        [SerializeField]
        [Header("闭合路径")]
        public bool closePath = false;

        [Header("移动速度,0为默认速度")]
        public float pathSpeed = 0;

        [HideInInspector]
        public GameObject pointGo;

        public ED_ActorPathPointCom CreatePoint()
        {
            GameObject newGo = MapEditorHelper.CreateObj(pointGo, gameObject);
            return newGo.GetComponent<ED_ActorPathPointCom>();
        }

        private void OnDrawGizmosSelected()
        {
            GUIStyle uIStyle = new GUIStyle();
            uIStyle.fontSize = 30;
            uIStyle.fontStyle = FontStyle.Bold;

            ED_ActorPathPointCom[] pointComs = gameObject.GetComponentsInChildren<ED_ActorPathPointCom>(true);
            if (pointComs != null)
            {
                for (int i = 0; i < pointComs.Length; i++)
                {
                    ED_ActorPathPointCom tPointCom = pointComs[i];
                    int nextIndex = i + 1;
                    if (nextIndex > 0 && nextIndex < pointComs.Length)
                    {
                        ED_ActorPathPointCom tNextPointCom = pointComs[nextIndex];
                        GizmosHelper.DrawLine(tPointCom.transform.position, tNextPointCom.transform.position, Color.white);
                    }

                    GUI.color = Color.black;
                    Handles.Label(tPointCom.transform.position, (i + 1).ToString(), uIStyle);
                    GUI.color = Color.white;
                }
            }
        }

        public override object ExportData()
        {
            ActorPathInfo pathData = new ActorPathInfo();
            pathData.defaultPath = defaultPath;
            pathData.closePath = closePath;
            pathData.pathSpeed = pathSpeed;

            ED_ActorPathPointCom[] pointComs = gameObject.GetComponentsInChildren<ED_ActorPathPointCom>(true);
            if (pointComs != null)
            {
                for (int i = 0; i < pointComs.Length; i++)
                {
                    ED_ActorPathPointCom tPointCom = pointComs[i];
                    pathData.points.Add((ActorPointInfo)tPointCom.ExportData());
                }
            }

            return pathData;
        }

    }
} 
#endif
