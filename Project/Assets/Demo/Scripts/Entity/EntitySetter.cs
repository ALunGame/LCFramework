using Demo.Com;
using LCECS.Core;
using UnityEngine;
using LCECS;
using LCECS.Data;

namespace Demo
{
    public static class EntitySetter
    {
        /// <summary>
        /// 实体移动指定偏移
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pos"></param>
        public static void MovePos(this Entity entity,Vector3 pos)
        {
            PlayerMoveCom playerMoveCom = entity.GetCom<PlayerMoveCom>();
            if (playerMoveCom != null)
            {
                playerMoveCom.ReqMove = pos;
            }
            else
            {
                TransformCom transformCom = entity.GetCom<TransformCom>();
                transformCom.ReqMove = pos;
            }
        }

        /// <summary>
        /// 暂停实体决策
        /// </summary>
        public static void PauseEntityDec(this Entity entity)
        {
            ECSLocate.DecCenter.RemoveEntityDecision(entity.DecTreeId, entity.Uid);
            ECSLayerLocate.Request.PushRequest(entity.Uid, RequestId.StopBev, new ParamData());
        }

        /// <summary>
        /// 恢复实体决策
        /// </summary>
        public static void ResumeEntityDec(this Entity entity)
        {
            EntityWorkData workData = ECSLayerLocate.Info.GetEntityWorkData(entity.Uid);
            workData.ChangeRequestId(RequestId.None);
            ECSLocate.DecCenter.AddEntityDecision(entity.DecGroup, entity.DecTreeId, entity.Uid);
        }
    }
}
