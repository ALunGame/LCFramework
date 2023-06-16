using Demo.Com;
using LCECS.Core;
using LCToolkit;

namespace Demo
{
    public static class EntityGetter
    {
        /// <summary>
        /// 获得实体身体区域
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Shape GetEntityColliderShape(this Entity entity)
        {
            Collider2DCom collider2DCom = entity.GetCom<Collider2DCom>();
            TransCom transCom = entity.GetCom<TransCom>();
            Shape shape = collider2DCom.colliderShape;
            shape.Translate(transCom.Pos);
            return shape;
        }
    }
}