// using System;
// using System.Collections.Generic;
// using Demo.Com;
// using LCMap;
// using LCTask;
//
// namespace Demo.Task
// {
//     /// <summary>
//     /// 发送采集命令
//     /// </summary>
//     public class TaskCollectItemCmdFunc : TaskActionFunc
//     {
//         public int itemId;
//         public int itemCnt;
//         public bool needWait;
//
//         protected override TaskActionState OnStart(TaskObj taskObj)
//         {
//             CollectItemWorkCmd collectItemWorkCmd = new CollectItemWorkCmd(itemId, itemCnt);
//             if (needWait)
//             {
//                 collectItemWorkCmd.workFinishCallBack += WaitCmdFinish;
//                 GameLocate.FuncModule.Work.SendWorkCommand(collectItemWorkCmd);
//                 return TaskActionState.Running;
//             }
//             else
//             {
//                 GameLocate.FuncModule.Work.SendWorkCommand(collectItemWorkCmd);
//                 return TaskActionState.Finished;
//             }
//         }
//
//         protected override void OnClear(TaskObj taskObj)
//         {
//         }
//
//         private void WaitCmdFinish(WorkCommand pCmd)
//         {
//             Finish();
//         }
//     }
//     
//     /// <summary>
//     /// 发送生产命令
//     /// </summary>
//     public class TaskProduceItemCmdFunc : TaskActionFunc
//     {
//         public int itemId;
//         public int itemCnt;
//         public bool needWait;
//
//         protected override TaskActionState OnStart(TaskObj taskObj)
//         {
//             ProduceItemWorkCmd produceItemWorkCmd = new ProduceItemWorkCmd(itemId, itemCnt);
//             if (needWait)
//             {
//                 produceItemWorkCmd.workFinishCallBack += WaitCmdFinish;
//                 GameLocate.FuncModule.Work.SendWorkCommand(produceItemWorkCmd);
//                 return TaskActionState.Running;
//             }
//             else
//             {
//                 GameLocate.FuncModule.Work.SendWorkCommand(produceItemWorkCmd);
//                 return TaskActionState.Finished;
//             }
//         }
//
//         protected override void OnClear(TaskObj taskObj)
//         {
//         }
//
//         private void WaitCmdFinish(WorkCommand pCmd)
//         {
//             Finish();
//         }
//     }
//
//     /// <summary>
//     /// 发送修复演员命令
//     /// </summary>
//     public class TaskRepairActorCmdFunc : TaskActionFunc
//     {
//         public int actorId;
//         public bool needWait;
//         
//         protected override TaskActionState OnStart(TaskObj taskObj)
//         {
//             List<Actor> actors = LCMap.ActorMediator.GetActors(actorId);
//             if (actors == null || actors.Count <= 0)
//             {
//                 return TaskActionState.Fail;
//             }
//
//             Actor resActor = null;
//             for (int i = 0; i < actors.Count; i++)
//             {
//                 Actor tActor = actors[i];
//                 if (tActor.GetCom(out BasePropertyCom propertyCom))
//                 {
//                     if (!propertyCom.Hp.CheckOutTotal())
//                     {
//                         resActor = tActor;
//                         break;
//                     }
//                 }
//             }
//
//             if (resActor == null)
//             {
//                 return TaskActionState.Fail;
//             }
//             else
//             {
//                 RepairActorWorkCmd cmd = new RepairActorWorkCmd(resActor);
//                 if (needWait)
//                 {
//                     cmd.workFinishCallBack += WaitCmdFinish;
//                     GameLocate.FuncModule.Work.SendWorkCommand(cmd);
//                     return TaskActionState.Running;
//                 }
//                 else
//                 {
//                     GameLocate.FuncModule.Work.SendWorkCommand(cmd);
//                     return TaskActionState.Finished;
//                 }
//             }
//         }
//
//         private void WaitCmdFinish(WorkCommand obj)
//         {
//             Finish();
//         }
//
//         protected override void OnClear(TaskObj taskObj)
//         {
//             
//         }
//     }
// }