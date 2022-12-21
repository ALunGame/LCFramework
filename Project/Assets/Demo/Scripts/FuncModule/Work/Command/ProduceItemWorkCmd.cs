using Demo.Com;
using Demo.Config;
using LCMap;

namespace Demo
{
    /// <summary>
    /// 生产物品工作命令
    /// </summary>
    public class ProduceItemWorkCmd : WorkCommand
    {
        public int itemId;
        public int itemCnt;

        public ProduceItemWorkCmd(int pItemId,int pItemCnt)
        {
            itemId = pItemId;
            itemCnt = pItemCnt;
        }

        public override bool CanTakeCommand(Actor pWorkActor)
        {
            if (pWorkActor.GetCom(out ProduceItemCom produceItemCom))
            {
                if (produceItemCom.produceItems.Contains(itemId))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        protected override void OnCommandTook()
        {
            executor.GetCom(out BagCom bagCom);
            if (bagCom.GetItemCnt(itemId) >= itemCnt)
                return;

            //判断是否物品是否需要合成
            if (LCConfig.Config.ItemRecipeCnf.TryGetValue(itemId,out ItemRecipeCnf itemRecipeCnf))
            {
                foreach (var recipe in itemRecipeCnf.recipes)
                {
                    ProduceItemWorkCmd cmd = new ProduceItemWorkCmd(recipe.itemId,recipe.itemCnt);
                    cmd.SetOriginator(executor);
                    cmd.workFinishCallBack += TryRecipeItem;
                    GameLocate.FuncModule.Work.SendWorkCommand(cmd);
                }
            }
            else
            {
                CollectItemWorkCmd collectCmd = new CollectItemWorkCmd(itemId, itemCnt);
                collectCmd.SetOriginator(executor);
                GameLocate.FuncModule.Work.SendWorkCommand(collectCmd);
            }
        }

        public override bool CanExecute()
        {
            executor.GetCom(out BagCom bagCom);
            if (bagCom.GetItemCnt(itemId) >= itemCnt)
                return true;
            return false;
        }

        /// <summary>
        /// 尝试合成物品
        /// </summary>
        /// <param name="pCmd"></param>
        private void TryRecipeItem(WorkCommand pCmd)
        {
            executor.GetCom(out BagCom bagCom);
            if (LCConfig.Config.ItemRecipeCnf.TryGetValue(itemId,out ItemRecipeCnf itemRecipeCnf))
            {
                bool canRecipe = true;
                foreach (var recipe in itemRecipeCnf.recipes)
                {
                    if (bagCom.GetItemCnt(recipe.itemId)<recipe.itemCnt)
                    {
                        canRecipe = false;
                        break;
                    }
                }

                if (canRecipe)
                {
                    foreach (var recipe in itemRecipeCnf.recipes)
                    {
                        bagCom.RemoveItem(recipe.itemId, recipe.itemCnt);
                    }

                    bagCom.AddItem(itemId, itemCnt);
                }
            }
        }

        protected override void OnExecute()
        {
            //0，移动到命令发起者演员身边
            MoveRequestCom.MoveToActorInteractiveRange(executor,originator, () =>
            {
                //1，扣除生产道具
                executor.GetCom(out BagCom exbagCom);
                exbagCom.RemoveItem(itemId, itemCnt);
                //2，添加对方道具
                originator.GetCom(out BagCom orbagCom);
                orbagCom.AddItem(itemId, itemCnt);
                ExecuteFinish();
            });
        }
    }
}
