using LCECS.Core;
using LCECS.Core.Tree.Base;
using System.Collections.Generic;
using UnityEngine;

namespace LCECS.Data
{
    /// <summary>
    /// 实体数据流
    /// </summary>
    public class EntityWorkData : NodeData
    {
        public Entity MEntity { get; }

        //请求参数
        private Dictionary<int, ParamData> ReqParamData = new Dictionary<int, ParamData>();
        //需要清除Id
        public int ClearReqId;
        //当前请求Id
        public int CurrReqId;
        //下一个请求Id
        public int NextReqId;

        public EntityWorkData(int id, Entity entity) : base(id)
        {
            MEntity = entity;
        }

        //获取请求参数
        public ParamData GetReqParam(int reqId)
        {
            if (!ReqParamData.ContainsKey(reqId))
            {
                ReqParamData.Add(reqId, new ParamData());
            }
            return ReqParamData[reqId];
        }
    }

    /// <summary>
    /// 参数数据
    /// </summary>
    public class ParamData
    {
        private bool boolData = false;
        private int intData = 0;
        private float floatData = 0;
        private double doubleData = 0;
        private string stringData = "";

        private Vector2 vect2Data = Vector2.zero;
        private Vector2Int vect2IntData = Vector2Int.zero;
        
        private object lockObject = new object();
        //现在不需要（应该也不需要）
        //private GameObject goData;

        #region Set

        public void SetBool(bool value)
        {
            lock (lockObject)
            {
                boolData = value;
            }
        }

        public void SetInt(int value)
        {
            lock (lockObject)
            {
                intData = value;
            }
        }

        public void SetFloat(float value)
        {
            lock (lockObject)
            {
                floatData = value;
            }
        }

        public void SetDouble(double value)
        {
            lock (lockObject)
            {
                doubleData = value;
            }
        }

        public void SetString(string value)
        {
            lock (lockObject)
            {
                stringData = value;
            }
        }

        public void SetVect2(Vector2 value)
        {
            lock (lockObject)
            {
                vect2Data = value;
            }
        }

        public void SetVect2Int(Vector2Int value)
        {
            lock (lockObject)
            {
                vect2IntData = value;
            }
        }

        #endregion

        #region Get   取一次值，就清空

        public bool GetBool()
        {
            bool tmp = boolData;
            lock (lockObject)
            {
                boolData = false;
            }
            return tmp;
        }

        public int GetInt()
        {
            int tmp = intData;
            lock (lockObject)
            {
                intData = 0;
            }
            return tmp;
        }

        public float GetFloat()
        {
            float tmp = floatData;
            lock (lockObject)
            {
                floatData = 0;
            }
            return tmp;
        }

        public double GetDouble()
        {
            double tmp = doubleData;
            lock (lockObject)
            {
                doubleData = 0;
            }
            return tmp;
        }

        public string GetString()
        {
            string tmp = stringData;
            lock (lockObject)
            {
                stringData = "";
            }
            return tmp;
        }

        public Vector2 GetVect2()
        {
            Vector2 tmp = vect2Data;
            lock (lockObject)
            {
                vect2Data = Vector2.zero;
            }
            return tmp;
        }

        public Vector2Int GetVect2Int()
        {
            Vector2Int tmp = vect2IntData;
            lock (lockObject)
            {
                vect2IntData = Vector2Int.zero;
            }
            return tmp;
        }

        #endregion
    }
}
