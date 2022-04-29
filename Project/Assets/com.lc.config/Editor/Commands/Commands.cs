using LCToolkit.Command;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LCConfig
{
    /// <summary>
    /// 添加配置命令
    /// </summary>
    public class AddConfigCommand : ICommand
    {
        List<IConfig> configs;
        IConfig config;

        public AddConfigCommand(List<IConfig> configs, IConfig config)
        {
            this.configs = configs;
            this.config = config;
        }

        public void Do()
        {
            configs.Add(config);
        }

        public void Undo()
        {
            configs.Remove(config);
        }
    }

    /// <summary>
    /// 删除配置命令
    /// </summary>
    public class RemoveConfigCommand : ICommand
    {
        List<IConfig> configs;
        IConfig config;

        public RemoveConfigCommand(List<IConfig> configs, IConfig config)
        {
            this.configs = configs;
            this.config = config;
        }

        public void Do()
        {
            configs.Remove(config);
        }

        public void Undo()
        {
            configs.Add(config);
        }
    }

    /// <summary>
    /// 改变配置值命令
    /// </summary>
    public class ChangeValueCommand : ICommand
    {
        object target;
        FieldInfo field;
        object oldValue, newValue;

        Action OnDoFunc;
        Action OnUndoFunc;

        public ChangeValueCommand(object target, FieldInfo field, object newValue, Action OnDoFunc = null, Action OnUndoFunc = null)
        {
            this.target = target;
            this.field = field;
            this.newValue = newValue;
            this.OnDoFunc = OnDoFunc;
            this.OnUndoFunc = OnUndoFunc;
        }

        public void Do()
        {
            oldValue = field.GetValue(target);
            field.SetValue(target, newValue);
            OnDoFunc?.Invoke();
        }

        public void Undo()
        {
            field.SetValue(target, oldValue);
            OnUndoFunc?.Invoke();
        }
    }
}