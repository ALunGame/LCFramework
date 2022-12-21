using LCECS.Core;
using LCECS.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Demo.Com
{
    /// <summary>
    /// 输入行为
    /// </summary>
    public enum InputAction
    {
        None,
        Move,
        Skill,
    }

    public class InputCom : BaseCom
    {
        /// <summary>
        /// 上一个行为
        /// </summary>
        public InputAction LastAction { get; private set; }

        /// <summary>
        /// 当前行为
        /// </summary>
        public InputAction CurrAction { get; private set; }

        /// <summary>
        /// 参数
        /// </summary>
        [NonSerialized]
        public RequestData Param;

        /// <summary>
        /// 输入行为
        /// </summary>
        /// <param name="action">行为</param>
        /// <param name="param">参数</param>
        public void PushAction(InputAction action, RequestData param)
        {
            LastAction = CurrAction;
            CurrAction = action;
            Param = param;
        }

        public void ClearAction()
        {
            LastAction = CurrAction;
            CurrAction = InputAction.None;
        }
    } 
}
