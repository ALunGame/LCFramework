using Cnf;
using LCMap;

namespace Demo.Life.State.Content
{
    public class ActorWorkContent_Produce : ActorWorkContent
    {
        /// <summary>
        /// 生产物品Id
        /// </summary>
        public int ItemId { get; private set; }
        
        /// <summary>
        /// 生产物品数量
        /// </summary>
        public int ItemCnt { get; private set; }
        
        public override bool CanDoWork()
        {
            ItemRecipeCnf recipeCnf = LCConfig.Config.ItemRecipeCnf[ItemId];

            if (recipeCnf.recipes.IsLegal())
            {
                for (int i = 0; i < recipeCnf.recipes.Count; i++)
                {
                    ItemInfo itemInfo = recipeCnf.recipes[i];
                    if (Executor.BagCom.GetItemCnt(itemInfo.itemId) < itemInfo.itemCnt)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return true;
            }
        }

        public override void OnDoWork()
        {
            ActorProduceCnf cnf = LCConfig.Config.ActorProduceCnf[ExecutorLifeCom.CnfId];
            ActorHelper.PlayAnimCnt(Executor,cnf.anim.animName,cnf.anim.animCnt,null, () =>
            {
                if (TryProduceItem())
                {
                    if (Executor.BagCom.GetItemCnt(ItemId) >= ItemCnt)
                    {
                        ActorHelper.SetMoveCallBack(Executor, () =>
                        {
                            Originator.ExecuteInteractive(Executor,InteractiveType.AddItem, (InteractiveState state) =>
                            {
                                if (state == InteractiveState.Success)
                                {
                                    WorkSuccess();
                                }   
                                else if (state == InteractiveState.Fail)
                                {
                                    WorkWait();
                                }
                            },new ItemInfo(ItemId,ItemCnt));
                        });
                        ActorHelper.MoveToPoint(Executor,Originator.Pos);
                    }
                    else
                    {
                        WorkWait();
                    }
                }
                else
                {
                    WorkFail();
                }
            });
        }
        
        private bool TryProduceItem()
        {
            BagCom bagCom = Executor.BagCom;
            if (LCConfig.Config.ItemRecipeCnf.TryGetValue(ItemId,out ItemRecipeCnf itemRecipeCnf))
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

                    bagCom.AddItem(ItemId, 1);
                    return true;
                }

                return false;
            }
            else
            {
                bagCom.AddItem(ItemId, 1);
                return true;
            }
        }
    }
}