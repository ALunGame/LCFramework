#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace LCMap
{
    public static class MapEditorHelper
    {
        public static GameObject CreateObj(GameObject obj, GameObject parent)
        {
            GameObject newGo = GameObject.Instantiate(obj);
            newGo.transform.SetParent(parent.transform);
            newGo.transform.localPosition = Vector3.zero;
            newGo.transform.localRotation = Quaternion.identity;
            newGo.transform.localScale = Vector3.one;
            newGo.name = obj.name;
            return newGo;
        }

        public static GameObject SetParent(GameObject obj, GameObject parent)
        {
            obj.transform.SetParent(parent.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            return obj;
        }

        public static List<T> GetComInScene<T>() where T : Component
        {
            Scene scene = EditorSceneManager.GetActiveScene();
            GameObject[] objects = scene.GetRootGameObjects();
            if (objects == null || objects.Length <= 0)
                return null;
            List<T> coms = new List<T>();
            for (int i = 0; i < objects.Length; i++)
            {
                GameObject go = objects[i];
                if (go.GetComponent<T>() != null)
                {
                    coms.Add(go.GetComponent<T>());
                }
            }
            return coms;
        }

        public static GameObject GetGoInScene(string goName)
        {
            Scene scene = EditorSceneManager.GetActiveScene();
            GameObject[] objects = scene.GetRootGameObjects();
            if (objects == null || objects.Length <= 0)
                return null;
            for (int i = 0; i < objects.Length; i++)
            {
                GameObject go = objects[i];
                if (go.name == goName)
                {
                    return go;
                }
            }
            return null;
        }
    }
} 
#endif
