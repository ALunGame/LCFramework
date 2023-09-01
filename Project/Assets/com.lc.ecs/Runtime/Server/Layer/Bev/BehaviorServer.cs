﻿using LCECS.Data;
using LCECS.Layer.Behavior;
using LCJson;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCECS.Server.Layer
{
    public class BehaviorServer : IBehaviorServer
    {
        private Dictionary<RequestId, List<BehaviorTree>> BevDict = new Dictionary<RequestId, List<BehaviorTree>>();
        
        public void Init()
        {
            foreach (var item in Enum.GetValues(typeof(RequestId)))
            {
                List<BehaviorTree> trees = LoadBehavior((RequestId)item);
                if (trees != null)
                {
                    BevDict.Add((RequestId)item, trees);
                }
            }
        }

        public void ReqBev(EntityWorkData workData, RequestId clearReqId)
        {
            //删除
            if (BevDict.TryGetValue(clearReqId, out List<BehaviorTree> lastBehaviors))
            {
                foreach (var bev in lastBehaviors)
                {
                    bev.RemoveWorkData(workData);
                }
            }
            
            //执行
            BehaviorTree currBehavior = GetBehavior(workData.CurrReqId,workData);
            if (currBehavior != null)
                currBehavior.AddWorkData(workData);
        }

        public void Execute()
        {
            foreach (List<BehaviorTree> items in BevDict.Values)
            {
                foreach (var tree in items)
                {
                    tree.Execute();
                }
            }
        }

        /// <summary>
        /// 获得行为树
        /// </summary>
        /// <returns></returns>
        private BehaviorTree GetBehavior(RequestId reqId,EntityWorkData workData)
        {
            if (!BevDict.ContainsKey(reqId))
            {
                return null;
            }
            
            List<BehaviorTree> items = BevDict[reqId];
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Evaluate(workData))
                {
                    return items[i];
                }   
            }
            return null;
        }

        /// <summary>
        /// 加载行为树
        /// </summary>
        /// <returns></returns>
        private List<BehaviorTree> LoadBehavior(RequestId treeId)
        {
            string jsonStr = IAFramework.GameContext.Asset.LoadString(ECSDefPath.GetBevTreeCnfName(treeId));
            try
            {
                List<BehaviorTree> behaviors = JsonMapper.ToObject<List<BehaviorTree>>(jsonStr);
                if (behaviors == null)
                    return null;
                return behaviors;
            }
            catch (Exception e)
            {
                ECSLocate.Log.LogError("行为树序列化失败",treeId);
                return null;
            }
        }
    }
}


