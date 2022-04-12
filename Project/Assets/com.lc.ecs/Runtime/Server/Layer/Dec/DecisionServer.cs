﻿using LCECS.Core.Tree.Base;
using LCECS.Data;
using LCECS.Layer.Decision;
using LCJson;
using System.Collections.Generic;
using UnityEngine;

namespace LCECS.Server.Layer
{
    public class DecisionServer : IDecisionServer
    {
        private Dictionary<int, BaseDecision> DesDict = new Dictionary<int, BaseDecision>();
        
        //设置决策树
        private void SetDecTrees(Dictionary<string, Node> decTrees)
        {
            if (decTrees == null)
                return;
            CreateDec(decTrees);
        }

        //创建决策
        private void CreateDec(Dictionary<string, Node> decTrees)
        {
            foreach (var item in decTrees)
            {
                int decId = int.Parse(item.Value.Uid);
                BaseDecision decision = new BaseDecision(item.Value);
                DesDict.Add(decId, decision);
            }
        }
        
        public void Init()
        {
            TextAsset jsonData                  = ECSLocate.Factory.GetProduct<TextAsset>(FactoryType.Asset, null, ECSDefPath.DecTreePath);
            Dictionary<string, Node> decTrees   = JsonMapper.ToObject<Dictionary<string, Node>>(jsonData.text);
            SetDecTrees(decTrees);
        }
        
        /// <summary>
        /// 添加决策实体
        /// </summary>
        public void AddDecisionEntity(int decId, EntityWorkData workData)
        {
            if (!DesDict.ContainsKey(decId))
            {
                ECSLocate.Log.LogR("添加决策实体错误，没有对应决策树>>>>>>>", decId);
                return;
            }
            BaseDecision decision = DesDict[decId];
            decision.AddEntity(workData);
        }

        /// <summary>
        /// 删除决策实体
        /// </summary>
        public void RemoveDecisionEntity(int decId, int entityId)
        {
            if (!DesDict.ContainsKey(decId))
            {
                return;
            }
            BaseDecision decision = DesDict[decId];
            decision.RemoveEntity(entityId);
        }

        /// <summary>
        /// 执行决策
        /// </summary>
        public void Execute()
        {
            foreach (BaseDecision item in DesDict.Values)
            {
                if (item != null)
                {
                    item.Execute();
                }
            }
        }
    }
}
