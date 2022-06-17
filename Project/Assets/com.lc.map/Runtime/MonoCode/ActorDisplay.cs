using LCToolkit;
using UnityEngine;

namespace LCMap
{
    public class ActorDisplay : MonoBehaviour
    {
        private Vector2 GetColliderOffset(BoxCollider2D collider2D)
        {
            return new Vector2(collider2D.bounds.center.x, collider2D.bounds.center.y) - transform.position.ToVector2();
        }

        public void RefreshDisplayOffset()
        {
            BoxCollider2D boxCollider2D = transform.Find("BodyCollider").GetComponent<BoxCollider2D>();

            Vector2 offset = GetColliderOffset(boxCollider2D);

            float xOffset = offset.x;
            xOffset = xOffset < 0 ? -xOffset : xOffset;

            float yOffset = offset.y - boxCollider2D.bounds.extents.y;
            yOffset = yOffset < 0 ? -yOffset : yOffset;

            //float yOffset = boxCollider2D.bounds.center.y - boxCollider2D.bounds.extents.y;
            //yOffset = yOffset < 0 ? - yOffset : yOffset;
            transform.GetComponent<LockTransCom>().lockPosValue = new Vector3(xOffset, yOffset, 0);
        }
    } 
}
