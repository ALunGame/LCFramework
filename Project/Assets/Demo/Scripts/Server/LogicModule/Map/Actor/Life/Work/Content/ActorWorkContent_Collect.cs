using System.Collections.Generic;
using Cnf;
using LCMap;

namespace Demo.Life.State.Content
{
    public class ActorWorkContent_Collect : ActorWorkContent
    {
        /// <summary>
        /// 生产物品Id
        /// </summary>
        public int ItemId { get; private set; }
        
        /// <summary>
        /// 生产物品数量
        /// </summary>
        public int ItemCnt { get; private set; }

        private List<ActorLifeCom> findActors;
        private Actor targetActor;

        public override bool CanDoWork()
        {
            MapActorLifeLogic logic = MapLocate.Map.GetLogicModule<MapActorLifeLogic>();
            findActors = logic.GetProducersByItemId(ItemId);
            return findActors.IsLegal();
        }

        public override void OnDoWork()
        {
            targetActor = null;
            for (int i = 0; i < findActors.Count; i++)
            {
                ActorLifeCom lifeCom = findActors[i];
                if (lifeCom.Owner.BagCom.GetItemCnt(ItemId) > 0)
                {
                    targetActor = lifeCom.Owner;
                    break;
                }
            }

            if (targetActor == null)
            {
                WorkWait();
                return;
            }
            
            ActorHelper.SetMoveCallBack(Executor, () =>
            {
                Originator.ExecuteInteractive(Executor,InteractiveType.RemoveItem, (InteractiveState state) =>
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
    }
}