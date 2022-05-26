using Demo.Com;
using LCECS.Core;
using UnityEngine;

namespace Demo
{
    public static class EntitySetter
    {
        /// <summary>
        /// 实体移动指定偏移
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pos"></param>
        public static void MovePos(this Entity entity,Vector3 pos)
        {
            PlayerMoveCom playerMoveCom = entity.GetCom<PlayerMoveCom>();
            if (playerMoveCom != null)
            {
                playerMoveCom.ReqMove = pos;
            }
            else
            {
                TransformCom transformCom = entity.GetCom<TransformCom>();
                transformCom.ReqMove = pos;
            }
        }
    }
}
