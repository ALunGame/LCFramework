using System.Collections.Generic;
using LCECS.Core.Tree.Base;
using LCECS.Data;
using LCECS.Layer.Behavior;
using UnityEngine;
using XPToolchains.Json;
#if UNITY_EDITOR
using LCECS.Scene;
#endif

namespace LCECS.Server.Layer
{
    public class BehaviorServer : IBehaviorServer
    {
        private Dictionary<int, BaseEntityBehavior> BevDict = new Dictionary<int, BaseEntityBehavior>();

        //设置行为树
        private void SetBevTrees(Dictionary<string, Node> bevTrees)
        {
            if (bevTrees == null)
                return;

            CreateBev(bevTrees);
        }

        //创建行为树
        private void CreateBev(Dictionary<string, Node> bevTrees)
        {
            foreach (var item in bevTrees)
            {
                int bevId = int.Parse(item.Value.Uid);
                BaseEntityBehavior behavior = new BaseEntityBehavior(item.Value, bevId);
                BevDict.Add(bevId, behavior);
            }
        }
        
        /// <summary>
        /// 获得行为
        /// </summary>
        private BaseEntityBehavior GetBev(int bevId)
        {
            if (BevDict.ContainsKey(bevId))
            {
                return BevDict[bevId];
            }
            return null;
        }
        
        public void Init()
        {
            TextAsset jsonData = ECSLocate.Factory.GetProduct<TextAsset>(FactoryType.Asset, null, ECSDefPath.BevTreePath);
            Dictionary<string, Node> bevTrees  = JsonMapper.ToObject<Dictionary<string, Node>>(jsonData.text);
            SetBevTrees(bevTrees);
        }

        /// <summary>
        /// 获得行为树
        /// </summary>
        public Node GetBevNode(int bevId)
        {
            BaseEntityBehavior behavior = GetBev(bevId);
            if (behavior != null)
            {
                return behavior.Tree;
            }
            return null;
        }

        /// <summary>
        /// 放入实体行为
        /// </summary>
        public void PushBev(EntityWorkData workData)
        {
            //删除
            BaseEntityBehavior lastBehavior = GetBev(workData.ClearReqId);
            if (lastBehavior != null)
            {
                lastBehavior.RemoveWorkData(workData);
            }

            //添加
            BaseEntityBehavior currBehavior = GetBev(workData.CurrReqId);
            if (currBehavior != null)
            {
                currBehavior.AddWorkData(workData);
            }
        }

        /// <summary>
        /// 执行实体行为
        /// </summary>
        public void Execute()
        {
            foreach (BaseEntityBehavior item in BevDict.Values)
            {
                item.Execute();
            }
        }
    }
}


