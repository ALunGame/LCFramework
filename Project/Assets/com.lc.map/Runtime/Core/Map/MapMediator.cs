using UnityEngine;

namespace LCMap
{
    public static class MapMediator
    {
        public static MapArea GetPosArea(Vector3 pPos)
        {
            return MapLocate.Map.GetPosArea(pPos);
        }

        public static MapArea GetAreaByActor(Actor pActor)
        {
            return MapLocate.Map.GetAreaByActor(pActor);
        }

        /// <summary>
        /// 在指定地图区域添加一个演员
        /// </summary>
        /// <param name="pActorInfo"></param>
        /// <param name="pArea"></param>
        /// <returns></returns>
        public static Actor AddActorInArea(ActorInfo pActorInfo, MapArea pArea)
        {
            return MapLocate.Map.CreateActor(pActorInfo,pArea);
        }
    }
}