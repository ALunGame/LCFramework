using System.Collections.Generic;
using UnityEngine;
using LCJson;
using System;

namespace LCECS.Core
{
    public sealed class Entity
    {
        private string uid;
        /// <summary>
        /// 唯一Id
        /// </summary>
        public string Uid { get => uid; }

        private int id;
        /// <summary>
        /// 配置Id
        /// </summary>
        public int Id { get => id; }

        private string name;
        /// <summary>
        /// 实体名
        /// </summary>
        public string Name { get => name; }

        private DecisionGroup decGroup = DecisionGroup.HighThread;
        /// <summary>
        /// 决策组
        /// </summary>
        public DecisionGroup DecGroup { get => decGroup; }

        private int decTreeId = 0;
        /// <summary>
        /// 决策树Id
        /// </summary>
        public int DecTreeId { get => decTreeId; }

        [NonSerialized]
        private GameObject go;
        /// <summary>
        /// 实体节点（当然可以没有）
        /// </summary>
        public GameObject Go { get => go; }

        /// <summary>
        /// 实体是否开启
        /// </summary>
        public bool IsEnable { get; private set; } = true;

        /// <summary>
        /// 组件
        /// </summary>
        private Dictionary<string, BaseCom> coms = new Dictionary<string, BaseCom>();

#if UNITY_EDITOR
        [JsonIgnore]
        public List<string> Systems = new List<string>();
        //获得所有组件名
        public HashSet<string> GetAllComStr()
        {
            return new HashSet<string>(coms.Keys);
        }
#endif

        public Entity()
        {

        }

        public Entity(int id, DecisionGroup decGroup,string name,int treeId,List<BaseCom> coms)
        {
            this.id = id;   
            this.decTreeId = treeId;
            this.name = name;
            this.decGroup = decGroup;
            for (int i = 0; i < coms.Count; i++)
            {
                this.coms.Add(coms[i].GetType().FullName, coms[i]);
            }
        }

        /// <summary>
        /// 设置实体节点
        /// </summary>
        /// <param name="go"></param>
        public void SetEntityGo(GameObject go)
        {
            this.go = go;
        }

        #region 生命周期

        /// <summary>
        /// 创建实体初始化
        /// </summary>
        /// <param name="uid">实体唯一Id</param>
        public void Init(string uid)
        {
            this.uid = uid;
        }

        /// <summary>
        /// 设置实体开启
        /// 1，System检测是否还需要关注这个实体
        /// 2，决策层尝试添加这个实体
        /// </summary>
        public void Enable()
        {
            IsEnable = true;
            foreach (BaseCom com in coms.Values)
            {
                com.EntityEnable();
            }
            ECSLocate.ECS.CheckEntityInSystem(Uid);
            ECSLocate.DecCenter.AddEntityDecision(DecGroup, DecTreeId, Uid);
        }

        /// <summary>
        /// 设置实体关闭
        /// 1，System检测是否还需要关注这个实体
        /// 2，决策层删除这个实体
        /// </summary>
        public void Disable()
        {
            IsEnable = false;
            foreach (BaseCom com in coms.Values)
            {
                com.EntityDisable();
            }
            ECSLocate.ECS.CheckEntityInSystem(Uid);
            ECSLocate.DecCenter.RemoveEntityDecision(DecTreeId, Uid);
        }

        #endregion

        #region Override

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj is Entity)
            {
                Entity other = (Entity)obj;
                return other.Uid == Uid;
            }
            return false;
        }

        #endregion

        #region 组件相关

        /// <summary>
        /// 获得所有组件
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BaseCom> GetComs()
        {
            foreach (var item in coms.Values)
            {
                yield return item;
            }
        }

        /// <summary>
        /// 通过类型名获得组件
        /// </summary>
        /// <param name="comName">FullName</param>
        /// <returns></returns>
        public BaseCom GetCom(string comName)
        {
            if (!coms.ContainsKey(comName))
                return null;
            return coms[comName];
        }

        /// <summary>
        /// 获得组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetCom<T>() where T : BaseCom
        {
            string typeName = typeof(T).FullName;
            if (!coms.ContainsKey(typeName))
                return null;
            return coms[typeName] as T;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="com"></param>
        public void AddCom<T>(T com) where T : BaseCom
        {
            string typeName = typeof(T).FullName;

            if (coms.ContainsKey(typeName))
                return;

            //调用函数
            if (!com.IsActive)
                com.Enable();

            //保存数据
            coms.Add(typeName, com);
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="com"></param>
        public void AddCom(BaseCom com)
        {
            string fullName = com.GetType().FullName;
            if (coms.ContainsKey(fullName))
                return;

            //调用函数
            if (!com.IsActive)
                com.Enable();

            //保存数据
            coms.Add(fullName, com);
        }

        /// <summary>
        /// 删除组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="com"></param>
        public void RemoveCom<T>() where T : BaseCom
        {
            string typeName = typeof(T).Name;
            if (!coms.ContainsKey(typeName))
                return;

            //调用函数
            BaseCom com = coms[typeName];
            if (com.IsActive)
                com.Disable();

            //清除数据
            coms.Remove(typeName);
        }

        /// <summary>
        /// 删除组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="com"></param>
        public void RemoveCom(string typeName)
        {
            if (!coms.ContainsKey(typeName))
                return;

            //调用函数
            BaseCom com = coms[typeName];
            if (com.IsActive)
                com.Disable();

            //清除数据
            coms.Remove(typeName);
        }

        /// <summary>
        /// 覆盖实体组件
        /// </summary>
        /// <param name="com"></param>
        public void CoverCom(BaseCom com)
        {
            BaseCom oldCom = GetCom(com.GetType().FullName);
            if (oldCom == null)
            {
                ECSLocate.Log.LogError("覆盖实体组件失败，没有对应组件>>>>>>>", com);
                return;
            }
            coms[com.GetType().FullName] = com;
        }

        #endregion

        /// <summary>
        /// 改变决策树
        /// </summary>
        public void ChangeDecTree(DecisionGroup decGroup,int treeId)
        {
            this.decTreeId = treeId;
            this.decGroup = decGroup;
            ECSLocate.DecCenter.AddEntityDecision(decGroup, decTreeId, uid);
        }
    }
}
