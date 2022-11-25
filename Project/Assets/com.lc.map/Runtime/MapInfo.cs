#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// 导出的地图配置数据
    /// </summary>
    public class MapInfo
    {
        public int mapId;
        public int currMaxActorUid;
        public ActorInfo mainActor = null;
        public List<AreaInfo> areas = new List<AreaInfo>();
    }

    /// <summary>
    /// 区域配置数据
    /// </summary>
    public class AreaInfo
    {
        public int areaId;
        public Vector3 pos;
        public Rect rect;

        public string areaPrefab;

        public List<ActorInfo> actors = new List<ActorInfo>();
        public Dictionary<int, MapTriggerInfo> triggers = new Dictionary<int, MapTriggerInfo>();
    }

    public class MapTriggerInfo
    {
        public List<Vector2> points;
    }

    public class ActorInfo
    {
        //唯一Id
        public string uid;
        //配置Id
        public int id;
        //演员类型
        public ActorType type;
        //位置
        public Vector3 pos;
        //旋转
        public Vector3 roate;
        //缩放
        public Vector3 scale;
        //显示隐藏
        public bool isActive = true;
        //状态名
        public string stateName;
        //路径
        public List<ActorPathInfo> paths = new List<ActorPathInfo>();
        //交互点
        public Vector2 interactivePoint = Vector2.zero;

        public ActorInfo()
        {
        }

        public ActorInfo(int id,ActorType actorType, Vector3 pos)
        {
            this.id = id;
            this.type = actorType;  
            this.pos = pos;
        }
    }

    public class ActorPathInfo
    {
        public bool defaultPath = false;
        public bool closePath = false;
        public float pathSpeed = 0;
        public List<ActorPointInfo> points = new List<ActorPointInfo>();
    }

    public class ActorPointInfo
    {
        public Vector3 point;

        //移动到此点的动画
        public string runAnimName;

        //等待时间
        public float waitTime;
        //等待动画
        public string waitAnimName;
        //等待扩展参数
        public string waitExParam;
    }
}

#endif