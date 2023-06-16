using Cnf;
using LCMap;

namespace Demo
{
    public class Interactive_RemoveItem : ActorInteractive
    {
        private int _type = (int)InteractiveType.RemoveItem;
        public override int Type { get => _type; }

        protected override InteractiveState OnExecute(Actor pInteractiveActor, params object[] pParams)
        {
            ItemInfo itemInfo = (ItemInfo)pParams[0];

            int currCnt = actor.BagCom.GetItemCnt(itemInfo.itemId);
            int addCnt = currCnt >= itemInfo.itemCnt ? itemInfo.itemCnt : currCnt;

            actor.BagCom.RemoveItem(itemInfo.itemId, currCnt);
            pInteractiveActor.BagCom.AddItem(itemInfo.itemId, addCnt);

            if (addCnt != itemInfo.itemCnt)
            {
                return InteractiveState.Fail;
            }
            
            return InteractiveState.Success;
        }
    }
}