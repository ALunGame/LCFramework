using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace LCToolkit.Server
{
    public class GameDataServer
    {
        private string gameDataUid = "00";
        private string gameDataRootName = "/GameData/";
        private Dictionary<Type, string> gameDataFileDict = new Dictionary<Type, string>();
        private Dictionary<Type, GameData> gameDataDict = new Dictionary<Type, GameData>();

        public void Init(string gameDataUid)
        {
            Clear();
            this.gameDataUid = gameDataUid;
        }

        public void Save()
        {
            foreach (var item in gameDataDict)
            {
                string filePath = GetFilePath(item.Key);
                string jsonStr = LCJson.JsonMapper.ToJson(item.Value);
                File.WriteAllText(filePath, jsonStr);
            }
        }

        public void Clear()
        {
            gameDataDict.Clear();
        }

        public T GetGameData<T>() where T : GameData, new()
        {
            Type type = typeof(T);
            if (gameDataDict.ContainsKey(type))
                return (T)gameDataDict[type];
            string filePath = GetFilePath(type);
            if (string.IsNullOrEmpty(filePath))
                return null;

            T gameData = new T();
            if (File.Exists(filePath))
                gameData = LCJson.JsonMapper.ToObject<T>(filePath);
            gameDataDict.Add(type, gameData);
            return gameData;
        }

        public void AddGameData(Type type,string fileName)
        {
            if (!gameDataFileDict.ContainsKey(type))
                return;
            gameDataFileDict.Add(type, fileName);
        }

        private string GetFilePath(Type type)
        {
            if (!gameDataFileDict.ContainsKey(type))
            {
                ToolkitLocate.Log.LogError("游戏数据没有注册到服务器中>>>>", type);
                return "";
            }
            string fileName = gameDataFileDict[type];
            string filePath = Application.persistentDataPath + gameDataRootName + gameDataUid + "/";
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            filePath = Path.Combine(filePath, fileName);
            return filePath;
        }
    }
}
