using Demo.Com;
using LCNode;
using LCTask;
using LCTask.TaskGraph;

namespace Demo.Task
{
    #region 工作

    [NodeMenuItem("工作/发送采集物品命令")]
    public class Task_ACT_SendCollectItemCmd : Task_CommonActionFuncNode
    {
        public override string Title { get => "发送采集物品命令"; set => base.Title = value; }
        public override string Tooltip { get => "发送采集物品命令"; set => base.Tooltip = value; }

        [NodeValue("物品Id")]
        public int itemId;
        [NodeValue("物品数量")]
        public int itemCnt;
        [NodeValue("是否需要等待")]
        public bool needWait;

        public override TaskActionFunc CreateFunc()
        {
            TaskCollectItemCmdFunc func = new TaskCollectItemCmdFunc();
            func.itemId = itemId;
            func.itemCnt = itemCnt;
            func.needWait = needWait;
            return func;
        }
    }
    
    [NodeMenuItem("工作/发送生产物品命令")]
    public class Task_ACT_SendCollectItem : Task_CommonActionFuncNode
    {
        public override string Title { get => "发送生产物品命令"; set => base.Title = value; }
        public override string Tooltip { get => "发送生产物品命令"; set => base.Tooltip = value; }

        [NodeValue("物品Id")]
        public int itemId;
        [NodeValue("物品数量")]
        public int itemCnt;
        [NodeValue("是否需要等待")]
        public bool needWait;

        public override TaskActionFunc CreateFunc()
        {
            TaskProduceItemCmdFunc func = new TaskProduceItemCmdFunc();
            func.itemId = itemId;
            func.itemCnt = itemCnt;
            func.needWait = needWait;
            return func;
        }
    }
    
    [NodeMenuItem("工作/发送修复演员命令")]
    public class Task_ACT_SendRepairActorCmd : Task_CommonActionFuncNode
    {
        public override string Title { get => "发送修复演员命令"; set => base.Title = value; }
        public override string Tooltip { get => "发送修复演员命令"; set => base.Tooltip = value; }

        [NodeValue("演员Id")]
        public int actorId;
        [NodeValue("是否需要等待")]
        public bool needWait;

        public override TaskActionFunc CreateFunc()
        {
            TaskRepairActorCmdFunc func = new TaskRepairActorCmdFunc();
            func.actorId = actorId;
            func.needWait = needWait;
            return func;
        }
    }

    #endregion

    #region 演员
    
    [NodeMenuItem("演员/设置演员基础属性")]
    public class Task_ACT_SetActorBaseProperty : Task_CommonActionFuncNode
    {
        public override string Title { get => "设置演员基础属性"; set => base.Title = value; }
        public override string Tooltip { get => "设置演员基础属性"; set => base.Tooltip = value; }

        [NodeValue("演员Id")]
        public int actorId;
        [NodeValue("属性类型")]
        public BasePropertyType propertyType;
        [NodeValue("属性值")]
        public string value;

        public override TaskActionFunc CreateFunc()
        {
            TaskSetActorBasePropertyFunc func = new TaskSetActorBasePropertyFunc();
            func.actorId = actorId;
            func.propertyType = propertyType;
            func.value = value;
            return func;
        }
    }
    

    #endregion
}