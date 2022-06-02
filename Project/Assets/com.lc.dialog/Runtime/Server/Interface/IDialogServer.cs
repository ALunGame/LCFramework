using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCDialog
{
    public interface IDialogServer
    {
        /// <summary>
        /// 创建一个对话
        /// </summary>
        /// <param name="addDialogInfo"></param>
        void CreateDialog(AddDialogInfo addDialogInfo);

        /// <summary>
        /// 播放对话
        /// <param name="uid">对话对象Uid</param>
        /// </summary>
        void Play(string uid);

        /// <summary>
        /// 播放下一步对话
        /// <param name="uid">对话对象Uid</param>
        /// </summary>
        void PlayNext(string uid);

        /// <summary>
        /// 关闭对话
        /// <param name="uid">对话对象Uid</param>
        /// </summary>
        void Close(string uid);

        /// <summary>
        /// 点击选项
        /// </summary>
        /// <param name="uid">对话对象Uid</param>
        void ClickDispose(string uid,int disposeId);
    }
}
