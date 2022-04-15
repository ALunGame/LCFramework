using LCECS.Data;
using LCECS.Layer.Behavior;
using LCJson;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace LCECS.Server.Layer
{
    public class BehaviorServer : IBehaviorServer
    {
        private Dictionary<int, BehaviorTree> BevDict = new Dictionary<int, BehaviorTree>();
        
        public void Init()
        {
        }

        public void ReqBev(EntityWorkData workData)
        {
            if (workData.ClearReqId == workData.CurrReqId)
            {
                BehaviorTree currBehavior = GetBehavior(workData.CurrReqId);
                if (currBehavior != null)
                    currBehavior.AddWorkData(workData);
            }
            else
            {
                //删除
                if (BevDict.TryGetValue(workData.ClearReqId,out BehaviorTree lastBehavior))
                    lastBehavior.RemoveWorkData(workData);

                BehaviorTree currBehavior = GetBehavior(workData.CurrReqId);
                if (currBehavior != null)
                    currBehavior.AddWorkData(workData);
            }

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
        private BehaviorTree GetBehavior(int treeId)
        {
            if (!BevDict.ContainsKey(treeId))
            {
                BehaviorTree tree = LoadBehavior(treeId);
                if (tree != null)
                {
                    BevDict.Add(treeId, tree);
                }
            }
            return BevDict[treeId];
        }

        /// <summary>
        /// 加载行为树
        /// </summary>
        /// <returns></returns>
        private BehaviorTree LoadBehavior(int treeId)
        {
            TextAsset jsonData = ECSLocate.Factory.GetProduct<TextAsset>(FactoryType.Asset, null, ECSDefPath.GetDecTreePath(treeId.ToString()));
            if (jsonData == null)
                return null;
            BehaviorTree behavior = JsonMapper.ToObject<BehaviorTree>(jsonData.text);
            if (behavior == null)
                return null;
            return behavior;
        }
    }
}


