using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCDialog
{
    /// <summary>
    /// 对话表现服务
    /// </summary>
    public interface IDialogDisplayServer 
    {
        /// <summary>
        /// 当创建一个对话时
        /// </summary>
        /// <param name="dialog">对话对象</param>
        /// <param name="actorUids">对话参与的演员Uid</param>
        void OnCreateDialog(DialogObj dialog, List<string> actorUids);

        /// <summary>
        /// 当播放一个对话
        /// </summary>
        /// <param name="dialog">对话对象</param>
        /// <param name="stepModel">对话步骤配置</param>
        void OnPlayDialog(DialogObj dialog, DialogStepModel stepModel);

        /// <summary>
        /// 当关闭对话
        /// </summary>
        /// <param name="dialog">对话对象</param>
        void OnCloseDialog(DialogObj dialog);

        /// <summary>
        /// 当点击选项
        /// </summary>
        /// <param name="dialog">对话对象</param>
        /// <param name="disposeId">选项Id</param>
        void OnClickDispose(DialogObj dialog, int disposeId);
    }
}