using DG.Tweening;
using LCMap;
using System.Collections.Generic;
using UnityEngine;

namespace Demo
{
    public class GiveItemInteractive : ActorInteractive
    {
        public List<BagItem> giveItems = new List<BagItem>();

        public override int Type => (int)InteractiveType.GiveItem;

        protected override bool OnExecute(Actor pInteractiveActor)
        {
            BagCom owerBagCom = actor.GetCom<BagCom>();
            BagCom interactiveBagCom = pInteractiveActor.GetCom<BagCom>();
            for (int i = 0; i < giveItems.Count; i++)
            {
                owerBagCom.AddItem(giveItems[i]);
                interactiveBagCom.RemoveItem(giveItems[i].id, giveItems[i].cnt);
            }

            actor.GetStateGo().transform.DOComplete(false);
            actor.GetStateGo().transform.DOPunchPosition(new Vector3(-0.2f * actor.GetDirValue(), 0, 0), 0.1f, 1, 0);

            return true;
        }
    }
}
