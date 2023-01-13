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
                    originator.ExecuteInteractive(executor,InteractiveType.GiveItem, (InteractiveState state) =>
                    {
                        if (state == InteractiveState.Success)
                        {
                            ExecuteFinish();
                        }   
                        else if (state == InteractiveState.Fail)
                        {
                            OnExecute();
                        }
                    },new ItemInfo(itemId,itemCnt));
                });
            }
            else
            {
                //1，产出演员身边
                MoveRequestCom.MoveToActorInteractiveRange(executor,outputActor, () =>
                {
                    outputActor.ExecuteInteractive(executor,InteractiveType.Collect, (InteractiveState state) =>
                    {
                        OnExecute();
                    }, itemId);
                });
            }
        }
    }
}
