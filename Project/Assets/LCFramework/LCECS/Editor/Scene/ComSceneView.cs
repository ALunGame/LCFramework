using LCECS.Core.ECS;
using System;

namespace LCECS.Scene
{
    /// <summary>
    /// 需要自己实现组件渲染的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComEditorViewAttribute : Attribute
    {
        public Type ComType { get; set; } = null;

        public ComEditorViewAttribute(Type type)
        {
            ComType = type;
        }
    }

    public class ComEditorView
    {


        public BaseCom TargetCom;

        //运行时场景显示
        public virtual void OnDrawScene(float width, float height)
        {

        }
    }
}
