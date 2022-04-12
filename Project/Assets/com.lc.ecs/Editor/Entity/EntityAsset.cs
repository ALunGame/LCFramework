using LCECS.Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCECS.Model
{
    public class EntityAsset
    {
        [Header("实体Id")]
        [SerializeField]
        public int id;

        [Header("实体Id")]
        [SerializeField]
        public string name;

        [Header("决策树Id")]
        [SerializeField]
        public int decTreeId;

        [HideInInspector]
        public string prefabPath;

        [HideInInspector]
        [SerializeField]
        string serializedComs = String.Empty;

        public void Save(List<BaseCom> coms)
        {
            serializedComs = LCJson.JsonMapper.ToJson(coms);
        }

        public List<BaseCom> Deserialize()
        {
            List<BaseCom> coms = LCJson.JsonMapper.ToObject<List<BaseCom>>(serializedComs);
            if (coms == null)
                coms = new List<BaseCom>();
            return coms;
        }
    }
}
