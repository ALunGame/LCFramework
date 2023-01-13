using DG.Tweening;
using LCMap;
using System.Collections.Generic;
using Config;
using UnityEngine;

namespace Demo
{
    /// <summary>
    /// 给与物品交互
    /// </summary>
    public class GiveItemInteractive : ActorInteractive
    {
        public override int Type => (int)InteractiveType.GiveItem;

        protected override InteractiveState OnExecute(Actor pInteractiveActor, params object[] pParams)
        {
            ItemInfo itemInfo = (ItemInfo)pParams[0];
            
            //1，扣除道具
            pInteractiveActor.GetCom(out BagCom exbagCom);
            exbagCom.RemoveItem(itemInfo);
            
            //2，添加对方道具
            actor.GetCom(out BagCom orbagCom);
            orbagCom.AddItem(itemInfo);

            actor.GetStateGo().transform.DOComplete(false);
            actor.GetStateGo().transform.DOPunchPosition(new Vector3(-0.2f * actor.GetDirValue(), 0, 0), 0.1f, 1, 0);

            return InteractiveState.Success;
        }
    }
}
