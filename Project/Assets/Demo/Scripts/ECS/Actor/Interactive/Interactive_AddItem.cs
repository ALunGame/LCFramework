using System.Collections.Generic;
using Cnf;
using Demo.Com;
using DG.Tweening;
using LCMap;
using UnityEngine;

namespace Demo
{
    /// <summary>
    /// 采集物品交互
    /// </summary>
    public class Interactive_AddItem : ActorInteractive
    {
        private int _type = (int)InteractiveType.AddItem;
        public override int Type { get => _type;}

        protected override InteractiveState OnExecute(Actor executeActor, params object[] pParams)
        {
            ItemInfo itemInfo = (ItemInfo)pParams[0];
            
            actor.BagCom.AddItem(itemInfo.itemId, itemInfo.itemCnt);
            executeActor.BagCom.RemoveItem(itemInfo.itemId, itemInfo.itemCnt);
            
            return InteractiveState.Success;
        }
    }
}
