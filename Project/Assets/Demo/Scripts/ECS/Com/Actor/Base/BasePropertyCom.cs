using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cnf;
using Demo.Config;
using LCECS.Core;
using LCMap;
using LCToolkit;
using UnityEngine;

namespace Demo.Com
{
    public abstract class Property<T> where T:struct
    {
        /// <summary>
        /// 总计
        /// </summary>
        public T Total { get; protected set; }

        protected BindableValue<T> curr = new BindableValue<T>();
        /// <summary>
        /// 当前
        /// </summary>
        public T Curr
        {
            get => curr.Value;
            set
            {
                if (CheckSetValueIsLegal(value))
                {
                    curr.Value = value;
                }
            }
        }
        
        /// <summary>
        /// 最小
        /// </summary>
        public T Min { get; protected set; }

        public void RegChange(Action<T> callBack)
        {
            curr.RegisterValueChangedEvent(callBack);
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            curr.ClearChangedEvent();
            OnClear();
        }

        /// <summary>
        /// 清理
        /// </summary>
        protected virtual void OnClear()
        {
            
        }

        /// <summary>
        /// 检测设置值是否合法
        /// </summary>
        /// <param name="pNewValue"></param>
        /// <returns></returns>
        protected abstract bool CheckSetValueIsLegal(T pNewValue);

        /// <summary>
        /// 检测超过总值
        /// </summary>
        /// <returns></returns>
        public abstract bool CheckOutTotal();

        /// <summary>
        /// 空
        /// </summary>
        /// <returns></returns>
        public abstract Property<T> Zero();
    }

    public class PropertyInt : Property<int>
    {
        public PropertyInt()
        {
            
        }
        
        public PropertyInt(int pCurr,int pTotal,int pMin)
        {
            Total = pTotal;
            Min = pMin;
            curr.SetValueWithoutNotify(pCurr);
        }

        protected override bool CheckSetValueIsLegal(int pNewValue)
        {
            if (Min <= pNewValue && Total >= pNewValue)
            {
                return true;
            }

            return false;
        }

        public override bool CheckOutTotal()
        {
            return Curr >= Total;
        }

        public override Property<int> Zero()
        {
            return new PropertyInt(0, 0, 0);
        }
    }

    public class PropertyFloat : Property<float>
    {
        public PropertyFloat()
        {
            
        }
        public PropertyFloat(float pCurr,float pTotal,float pMin)
        {
            Total = pTotal;
            Min = pMin;
            curr.SetValueWithoutNotify(pCurr);
        }
        
        protected override bool CheckSetValueIsLegal(float pNewValue)
        {
            if (Min <= pNewValue && Total >= pNewValue)
            {
                return true;
            }

            return false;
        }

        public override bool CheckOutTotal()
        {
            return Total >= Curr;
        }

        public override Property<float> Zero()
        {
            return new PropertyFloat(0, 0, 0);
        }
    }

    public enum BasePropertyType
    {
        HP,
        MoveSpeed,
        JumpSpeed,
        Attack,
        Defense,
        ActionSpeed,
    }
    
    /// <summary>
    /// 基础属性
    /// </summary>
    public class BasePropertyCom : BaseCom
    {
        /// <summary>
        /// 生命
        /// </summary>
        [NonSerialized] public PropertyInt Hp = new PropertyInt();
        /// <summary>
        /// 移动速度
        /// </summary>
        [NonSerialized] public PropertyFloat MoveSpeed = new PropertyFloat();
        /// <summary>
        /// 跳跃速度
        /// </summary>
        [NonSerialized] public PropertyFloat JumpSpeed = new PropertyFloat();
        /// <summary>
        /// 攻击
        /// </summary>
        [NonSerialized] public PropertyFloat Attack = new PropertyFloat();
        /// <summary>
        /// 防御
        /// </summary>
        [NonSerialized] public PropertyFloat Defense = new PropertyFloat();
        /// <summary>
        /// 行动速度（攻击速度）
        /// </summary>
        [NonSerialized] public PropertyFloat ActionSpeed = new PropertyFloat();
        
        protected override void OnAwake(Entity pEntity)
        {
            if (pEntity is Actor)
            {
                Actor actor = pEntity as Actor;
                if (LCConfig.Config.ActorBasePropertyCnf.TryGetValue(actor.Id,out ActorBasePropertyCnf propertyCnf))
                {
                    Hp = new PropertyInt(propertyCnf.hp, propertyCnf.hp, 0);
                    MoveSpeed = new PropertyFloat(propertyCnf.moveSpeed, propertyCnf.moveSpeed, 0);
                    JumpSpeed = new PropertyFloat(propertyCnf.jumpSpeed, propertyCnf.jumpSpeed, 0);
                    Attack = new PropertyFloat(propertyCnf.attack, propertyCnf.attack, 0);
                    Defense = new PropertyFloat(propertyCnf.defense, propertyCnf.defense, 0);
                    ActionSpeed = new PropertyFloat(propertyCnf.actionSpeed, propertyCnf.actionSpeed, 0);
                }
            }
        }
    }
}
