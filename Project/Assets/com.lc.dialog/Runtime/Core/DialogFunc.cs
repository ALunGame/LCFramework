using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCDialog
{
    /// <summary>
    /// 播放下一步对话调用函数
    /// </summary>
    public abstract class DialogStepFunc
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        public abstract void Execute(DialogObj dialog,int step);
    }

    /// <summary>
    /// 点击选项调用函数
    /// </summary>
    public abstract class DialogDisposeFunc
    {
        /// <summary>
        /// 执行函数
        /// </summary>
        public abstract void Execute(DialogObj dialog, int disposeId);
    }
}
