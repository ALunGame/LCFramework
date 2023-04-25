using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCMap
{
    /// <summary>
    /// 演员服务
    /// 1，存储当前所有演员
    /// 2，演员创建和初始化
    /// </summary>
    public class ActorServer
    {
        //所有演员
        private Dictionary<string, Actor> actors = new Dictionary<string, Actor>();
        //Id和演员
        private Dictionary<int, List<Actor>> actorMap = new Dictionary<int, List<Actor>>();
        //主角
        private Actor mainActor;

        /// <summary>
        /// 添加一个演员
        /// </summary>
        /// <param name="pActorInfo"></param>
        /// <returns></returns>
        public Actor AddActor(ActorInfo pActorInfo)
        {
            if (actors.ContainsKey(pActorInfo.uid))
            {
                ActorLocate.Log.Log("添加演员失败，重复的Uid",pActorInfo.uid);
                return null;
            }
            
            Actor tActor = ActorCreator.CreateActor(pActorInfo);
            
            //保存
            actors.Add(tActor.Uid,tActor);
            if (!actorMap.ContainsKey(tActor.Id))
                actorMap.Add(tActor.Id,new List<Actor>());
            actorMap[tActor.Id].Add(tActor);

            return tActor;
        }

        /// <summary>
        /// 删除演员
        /// </summary>
        public void RemoveActor(Actor pActor)
        {
            if (!actors.ContainsKey(pActor.Uid))
            {
                ActorLocate.Log.Log("删除演员失败，没有该演员",pActor.Uid);
                return;
            }
            
            //删除
            actorMap[pActor.Id].Remove(pActor);
            actors.Remove(pActor.Uid);
        }

        #region Set

        /// <summary>
        /// 设置主角
        /// </summary>
        public void SetMainActor(Actor pActor)
        {
            mainActor = pActor;
        }

        #endregion

        #region Get

        public Actor GetMainActor()
        {
            return mainActor;
        }
        
        public Actor GetActor(string pActorUid)
        {
            if (actors.ContainsKey(pActorUid))
            {
                return actors[pActorUid];
            }

            return null;
        }

        public List<Actor> GetActors(int pActorId)
        {
            if (actorMap.ContainsKey(pActorId))
            {
                return actorMap[pActorId];
            }

            return null;
        }
        
        public IEnumerable<Actor> GetActors(string pComTypeFullName)
        {
            foreach (var actor in actors.Values)
            {
                if (actor.HasCom(pComTypeFullName))
                    yield return actor;
            }
        }

        #endregion

        #region Check
        
        /// <summary>
        /// 检测是不是有该演员
        /// </summary>
        /// <param name="pActor"></param>
        /// <returns></returns>
        public bool CheckHasActor(Actor pActor)
        {
            return actors.ContainsKey(pActor.Uid);
        }

        #endregion
        
    }
}