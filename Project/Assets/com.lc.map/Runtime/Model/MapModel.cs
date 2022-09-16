#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// 导出的地图配置数据
    /// </summary>
    public class MapModel
    {
        public int mapId;
        public int currMaxActorUid;
        public ActorModel mainActor = null;
        public List<AreaModel> areas = new List<AreaModel>();
    }

    /// <summary>
    /// 区域配置数据
    /// </summary>
    public class AreaModel
    {
        public int areaId;
        public Vector3 pos;
        public Rect rect;

        public string areaPrefab;

        public List<ActorModel> actors = new List<ActorModel>();
        public Dictionary<int, MapTriggerModel> triggers = new Dictionary<int, MapTriggerModel>();
    }

    public class MapTriggerModel
    {
        public List<Vector2> points;
    }

    public class ActorModel
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
        public List<ActorPathModel> paths = new List<ActorPathModel>();
        //交互点
        public Vector2 interactivePoint = Vector2.zero;

        public ActorModel()
        {
        }

        public ActorModel(int id,ActorType actorType, Vector3 pos)
        {
            this.id = id;
            this.type = actorType;  
            this.pos = pos;
        }
    }

    public class ActorPathModel
    {
        public bool defaultPath = false;
        public bool closePath = false;
        public float pathSpeed = 0;
        public List<ActorPointModel> points = new List<ActorPointModel>();
    }

    public class ActorPointModel
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