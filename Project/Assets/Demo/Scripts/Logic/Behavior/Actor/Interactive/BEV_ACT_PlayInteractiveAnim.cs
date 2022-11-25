using DG.Tweening;
using LCECS.Core.Tree;
using LCECS.Core.Tree.Base;
using LCECS.Core.Tree.Nodes.Action;
using LCECS.Data;
using LCMap;
using UnityEngine;

namespace Demo.Behavior
{
    /// <summary>
    /// 播放交互动画
    /// </summary>
    public class BEV_ACT_PlayInteractiveAnim : NodeAction
    {
        //动画时长
        public float animTime = 0.3f;
        //动画次数
        public int animCnt = 1;

        class InteractiveAnimData
        {
            public bool isPlaying;
        }

        protected override void OnEnter(NodeData wData)
        {
            EntityWorkData workData = wData as EntityWorkData;

            //获取环境数据
            NodeActionContext context = GetContext<NodeActionContext>(wData);
            InteractiveAnimData userData = context.GetUserData<InteractiveAnimData>();
            userData.isPlaying = true;

            //当前演员
            Actor actorObj = LCMap.MapLocate.Map.GetActor(workData.MEntity.Uid);
            actorObj.GetStateGo().transform.DOPunchPosition(new Vector3(-0.2f * actorObj.GetDirValue(), 0, 0), animTime, 1, 0).OnComplete(() =>
            {
                userData.isPlaying = false;
            }).SetLoops(animCnt);

            //当前演员
            Actor targetActor = LCMap.MapLocate.Map.GetActor(wData.Blackboard[BEV_BlackboardKey.InteractiveActorUid].ToString());
            targetActor.GetStateGo().transform.DOPunchPosition(new Vector3(-0.2f * targetActor.GetDirValue(), 0, 0), animTime, 1, 0).SetLoops(animCnt);
        }

        protected override int OnRunning(NodeData wData)
        {
            //获取环境数据
            NodeActionContext context = GetContext<NodeActionContext>(wData);
            InteractiveAnimData userData = context.GetUserData<InteractiveAnimData>();

            if (userData.isPlaying)
                return NodeState.EXECUTING;
            return NodeState.FINISHED;
        }

        protected override void OnExit(NodeData wData, int runningStatus)
        {
        }
    }
}