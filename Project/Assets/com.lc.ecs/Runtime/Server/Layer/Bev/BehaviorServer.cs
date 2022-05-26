using LCECS.Data;
using LCECS.Layer.Behavior;
using LCJson;
using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace LCECS.Server.Layer
{
    public class BehaviorServer : IBehaviorServer
    {
        private Dictionary<RequestId, BehaviorTree> BevDict = new Dictionary<RequestId, BehaviorTree>();
        
        public void Init()
        {
            foreach (var item in Enum.GetValues(typeof(RequestId)))
            {
                BehaviorTree tree = LoadBehavior((RequestId)item);
                if (tree != null)
                {
                    BevDict.Add((RequestId)item, tree);
                }
            }
        }

        public void ReqBev(EntityWorkData workData, RequestId clearReqId)
        {
            //删除
            if (BevDict.TryGetValue(clearReqId, out BehaviorTree lastBehavior))
                lastBehavior.RemoveWorkData(workData);
            //执行
            BehaviorTree currBehavior = GetBehavior(workData.CurrReqId);
            if (currBehavior != null)
                currBehavior.AddWorkData(workData);
        }

        public void Execute()
        {
            foreach (BehaviorTree item in BevDict.Values)
            {
                item.Execute();
            }
        }

        /// <summary>
        /// 获得行为树
        /// </summary>
        /// <returns></returns>
        private BehaviorTree GetBehavior(RequestId reqId)
        {
            if (!BevDict.ContainsKey(reqId))
            {
                return null;
            }
            return BevDict[reqId];
        }

        /// <summary>
        /// 加载行为树
        /// </summary>
        /// <returns></returns>
        private BehaviorTree LoadBehavior(RequestId treeId)
        {
            string jsonStr = LCLoad.LoadHelper.LoadString(ECSDefPath.GetBevTreeCnfName(treeId));
            BehaviorTree behavior = JsonMapper.ToObject<BehaviorTree>(jsonStr);
            if (behavior == null)
                return null;
            return behavior;
        }
    }
}


