using LCToolkit;
using UnityEditor;
using UnityEngine;

namespace LCMap
{
    public static class ActorCreateHelper
    {
        private static Sprite actorSprite;

        [MenuItem("Assets/Actor/通过Sprite创建演员")]
        public static void Create2DAnim()
        {
            CreateActorPrefab(actorSprite);
        }

        [MenuItem("Assets/Actor/通过Sprite创建演员", true)]
        public static bool Create2DAnimValidate()
        {
            Object[] guidArray = Selection.objects;
            if (guidArray == null || guidArray.Length != 1)
            {
                return false;
            }
            Object selGuid = guidArray[0];
            if (selGuid is Sprite)
            {
                actorSprite = selGuid as Sprite;
                return true;
            }
            actorSprite = null;
            return false;
        }


        public static GameObject CreateActorPrefab(Sprite sprite)
        {
            GameObject actorTmp = MapSetting.Setting.ActorTemplate;
            GameObject newActor = GameObject.Instantiate(actorTmp);
            newActor.name = "Actor_" + sprite.name;

            Transform displayRoot = newActor.transform.Find("State/Default/Display");
            Transform imgTrans = displayRoot.Find("Img");
            if (imgTrans == null)
            {
                imgTrans = new GameObject("Img").transform;
                imgTrans.SetParent(displayRoot);
                imgTrans.gameObject.AddComponent<LockTransCom>();
            }

            SpriteRenderer rendererCom = imgTrans.GetOrAddCom<SpriteRenderer>();
            rendererCom.sprite = sprite;

            GameObject clickBox = AutoCreatePolygonCollider2D.CreatePolygonCollider2DBySprite(imgTrans.gameObject, "ClickBox");
            clickBox.AddComponent<LockTransCom>();

            GameObject bodyBox = AutoCreateBoxCollider2D.CreateBoxCollider2DBySprite(imgTrans.gameObject, "BodyCollider");
            bodyBox.AddComponent<LockTransCom>();

            return newActor;
        }
    }
}
