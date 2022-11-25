using LCToolkit;
using System;
using UnityEngine;

namespace LCECS.Core
{
    /// <summary>
    /// 决策组件
    /// </summary>
    public class DecisionCom : BaseCom
    {
        public int DecisionId { get; private set; }
        public DecisionThread DecisionThread { get; private set; }

        public void SetDecisionId(int pDecisionId)
        {
            ECSLocate.DecCenter.RemoveEntityDecision(DecisionId, EntityUid);
            DecisionId = pDecisionId;
            ECSLocate.DecCenter.AddEntityDecision(DecisionThread, DecisionId, EntityUid);
        }

        public void SetDecisionThread(DecisionThread pDecisionThread)
        {
            ECSLocate.DecCenter.RemoveEntityDecision(DecisionId, EntityUid);
            DecisionThread = pDecisionThread;
            ECSLocate.DecCenter.AddEntityDecision(DecisionThread, DecisionId, EntityUid);
        }
    }
}
