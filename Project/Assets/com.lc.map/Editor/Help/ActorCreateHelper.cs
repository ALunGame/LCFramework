using LCToolkit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LCMap
{
    public static class ActorCreateHelper
    {
        private static List<Sprite> actorSprites = new List<Sprite>();

        [MenuItem("Assets/Actor/通过Sprite创建演员")]
        public static void Create2DAnim()
        {
            foreach (var item in actorSprites)
            {
                CreateActorPrefab(item);
            }
        }

        [MenuItem("Assets/Actor/通过Sprite创建演员", true)]
        public static bool Create2DAnimValidate()
        {
            Object[] guidArray = Selection.objects;
            if (guidArray == null || guidArray.Length <= 0)
            {
                return false;
            }
            actorSprites.Clear();
            for (int i = 0; i < guidArray.Length; i++)
            {
                if (guidArray[i] is Sprite)
                {
                    actorSprites.Add(guidArray[i] as Sprite);
                }
            }
            return actorSprites.Count > 0;
        }


        public static GameObject CreateActorPrefab(Sprite sprite)
        {
            GameObject actorTmp = MapSetting.Setting.ActorTemplate;
            GameObject newActor = GameObject.Instantiate(actorTmp);
            newActor.name = "Actor_" + sprite.name;

            Transform displayRoot = newActor.transform.Find("State/Default/Display");
            Transform imgTrans    = displayRoot;

            SpriteRenderer rendererCom = imgTrans.GetOrAddCom<SpriteRenderer>();
            rendererCom.sprite = sprite;

            GameObject clickBox = AutoCreatePolygonCollider2D.CreatePolygonCollider2DBySprite(imgTrans.gameObject, "ClickBox");
            clickBox.AddComponent<LockTransCom>();
            clickBox.layer = LayerMask.NameToLayer("ActorClick");

            GameObject bodyBox = AutoCreateBoxCollider2D.CreateBoxCollider2DBySprite(imgTrans.gameObject, "BodyCollider");
            bodyBox.AddComponent<LockTransCom>();
            bodyBox.layer = LayerMask.NameToLayer("ActorBody");

            imgTrans.GetComponent<ActorDisplay>().RefreshDisplayOffset();
            return newActor;
        }
    }
}
