using Demo.Com;
using LCMap;
using System.Collections.Generic;
using Config;

namespace Demo
{
    /// <summary>
    /// 采集物品工作命令
    /// </summary>
    public class CollectItemWorkCmd : WorkCommand
    {
        public int itemId;
        public int itemCnt;

        public Actor outputActor;
        public int collectCnt;

        public CollectItemWorkCmd(int pItemId, int pItemCnt)
        {
            itemId = pItemId;
            itemCnt = pItemCnt;
        }

        public override bool CanTakeCommand(Actor pWorkActor)
        {
            if (pWorkActor.Equals(originator))
                return true;
            return false;
        }

        protected override void OnCommandTook()
        {
            
        }
        
        private void FindOutputActor()
        {
            List<Actor> actors = GameLocate.FuncModule.Work.GetOutPutItemActor(itemId, originator);
            outputActor = originator.GetNearestActor(actors);
        }

        public override bool CanExecute()
        {
            executor.GetCom(out BagCom exbagCom);
            if (exbagCom.GetItemCnt(itemId) >= itemCnt)
                return true;
            
            if (outputActor == null)
                FindOutputActor();
            return outputActor != null;
        }

        protected override void OnExecute()
        {
            executor.GetCom(out BagCom exbagCom);
            //0，采集完成
            if (exbagCom.GetItemCnt(itemId) >= itemCnt)
            {
                //1，移动到命令发起者演员身边
                MoveRequestCom.MoveToActorInteractiveRange(executor,originator, () =>
                {
                    //1，扣除采集道具
                    executor.GetCom(out BagCom exbagCom);
                    exbagCom.RemoveItem(itemId, itemCnt);
                    //2，添加对方道具
                    originator.GetCom(out BagCom orbagCom);
                    orbagCom.AddItem(itemId, itemCnt);
                
                    ExecuteFinish();
                });
            }
            else
            {
                //1，产出演员身边
                MoveRequestCom.MoveToActorInteractiveRange(executor,outputActor, () =>
                {
                    //1，产出物品
                    outputActor.GetCom(out OutputItemCom outputItemCom);
                    ItemInfo outputItem = outputItemCom.GetOutputInfo(itemId);

                    //2，采集物品
                    executor.GetCom(out BagCom exbagCom);
                    exbagCom.AddItem(outputItem.itemId, outputItem.itemCnt);
                    
                    //3，记录采集数量
                    collectCnt += outputItem.itemCnt;
                    
                    //4，继续执行
                    OnExecute();
                });
            }
        }
    }
}
