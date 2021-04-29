using UnityEngine;

namespace LCECS.Core.ECS
{
    [Com(GroupName = "辅助组件", ViewName = "存储GameObject组件")]
    public class GameObjectCom : BaseCom
    {
        public GameObject Go;
        public Transform Tran;
        public SpriteRenderer Renderer;

        protected override void OnInit(GameObject go)
        {
            Go          = go;
            Tran        = go.transform;
            Renderer    = go.GetComponent<SpriteRenderer>();
        }
    }
}