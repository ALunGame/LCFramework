using LCECS.Core;
using LCNode;
using UnityEditor;
using UnityEngine;

namespace LCECS.EntityGraph
{
    [NodeMenuItem("基础/GameObject组件")]
    public class Entity_Node_GoCom : Entity_ComNode
    {
        public override string Title { get => "存储GameObject组件"; set => base.Title = value; }
        public override string Tooltip { get => "存储GameObject组件"; set => base.Tooltip = value; }

        public override BaseCom CreateRuntimeNode()
        {
            GameObjectCom gameObjectCom = new GameObjectCom();
            return gameObjectCom;
        }
    }
}