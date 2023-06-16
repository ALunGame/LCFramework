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
        /// 暂停实体决策
        /// </summary>
        public static void PauseEntityDec(this Entity entity)
        {
            // if (entity.GetCom(out DecisionCom outCom))
            // {
            //     ECSLocate.DecCenter.RemoveEntityDecision(outCom.DecisionId, entity.Uid);
            // }
            // ECSLayerLocate.Request.PushRequest(entity.Uid, RequestId.StopBev, new ParamData());
        }

        /// <summary>
        /// 恢复实体决策
        /// </summary>
        public static void ResumeEntityDec(this Entity entity)
        {
            // EntityWorkData workData = ECSLayerLocate.Info.GetEntityWorkData(entity.Uid);
            // workData.ChangeRequestId(RequestId.None);
            //
            // if (entity.GetCom(out DecisionCom outCom))
            // {
            //     ECSLocate.DecCenter.AddEntityDecision(outCom.DecisionThread, outCom.DecisionId, entity.Uid);
            // }
        }
    }
}
