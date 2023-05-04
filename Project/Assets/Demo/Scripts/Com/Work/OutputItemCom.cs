﻿using LCECS.Core;
using System.Collections.Generic;
using System;
using Config;
using LCMap;

namespace Demo.Com
{
    /// <summary>
    /// 产出物品
    /// </summary>
    public class OutputItemCom : BaseCom
    {
        public List<ItemInfo> outputInfos = new List<ItemInfo>();

        protected override void OnAwake(Entity pEntity)
        {
            if (pEntity is Actor)
            {
                Actor actor = pEntity as Actor;
                actor.AddInteractive(new CollectInteractive());
            }
        }

        /// <summary>
        /// 检测是否输出该物品
        /// </summary>
        /// <param name="pItemId"></param>
        /// <returns></returns>
        public bool CheckIsOutputItem(int pItemId)
        {
            for (int i = 0; i < outputInfos.Count; i++)
            {
                if (outputInfos[i].itemId == pItemId)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得产出信息
        /// </summary>
        /// <param name="pItemId"></param>
        /// <returns></returns>
        public ItemInfo GetOutputInfo(int pItemId)
        {
            for (int i = 0; i < outputInfos.Count; i++)
            {
                if (outputInfos[i].itemId == pItemId)
                {
                    return outputInfos[i];
                }
            }
            return null;
        }
    }
}