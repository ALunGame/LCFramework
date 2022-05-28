using LCECS.Core;
using LCECS.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Demo.Com
{
    /// <summary>
    /// ������Ϊ
    /// </summary>
    public enum InputAction
    {
        Move,
        Skill,
    }

    public class InputCom : BaseCom
    {
        /// <summary>
        /// ��һ����Ϊ
        /// </summary>
        public InputAction LastAction { get; private set; }

        /// <summary>
        /// ��ǰ��Ϊ
        /// </summary>
        public InputAction CurrAction { get; private set; }

        /// <summary>
        /// ����
        /// </summary>
        [NonSerialized]
        public ParamData Param = new ParamData();

        /// <summary>
        /// ������Ϊ
        /// </summary>
        /// <param name="action">��Ϊ</param>
        /// <param name="param">����</param>
        public void PushAction(InputAction action, ParamData param)
        {
            LastAction = CurrAction;
            CurrAction = action;
            Param = param;
        }
    } 
}
