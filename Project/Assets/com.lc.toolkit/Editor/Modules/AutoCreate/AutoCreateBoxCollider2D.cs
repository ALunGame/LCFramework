using UnityEditor;
using UnityEngine;

namespace LCToolkit
{
    public static class AutoCreateBoxCollider2D
    {
        private const string defaultClickGoName = "ClickBox";

        public static GameObject CreateBoxCollider2DBySprite(GameObject go = null, string colliderName = null)
        {
            string clickGoName = colliderName ?? defaultClickGoName;

            GameObject selGo = go ?? Selection.activeGameObject;
            SpriteRenderer spriteRenderer = selGo.GetComponent<SpriteRenderer>();
            BoxCollider2D copyCollider2D = spriteRenderer.gameObject.AddComponent<BoxCollider2D>();


            BoxCollider2D boxCollider = null;
            if (selGo.transform.parent == null)
            {
                GameObject colliderGo = new GameObject(clickGoName);
                boxCollider = colliderGo.AddComponent<BoxCollider2D>();
            }
            else
            {
                /* polyCollider = selGo.transform.parent.GetComponentInChildren<PolygonCollider2D>();
                 if (polyCollider == null) { }*/
                GameObject colliderGo = new GameObject(clickGoName);
                colliderGo.transform.SetParent(selGo.transform.parent);
                colliderGo.transform.localPosition = selGo.transform.localPosition;
                colliderGo.transform.localRotation = Quaternion.identity;
                colliderGo.transform.localScale = Vector3.one;
                boxCollider = colliderGo.AddComponent<BoxCollider2D>();
            }

            boxCollider.size = copyCollider2D.size;
            boxCollider.offset = copyCollider2D.offset;

            boxCollider.transform.SetParent(selGo.transform);
            boxCollider.isTrigger = true;

            GameObject.DestroyImmediate(copyCollider2D);

            return boxCollider.gameObject;
        }
    }
}