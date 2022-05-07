using LCECS.Core;
using System;
using UnityEngine;

namespace Demo.Com
{
    [Serializable]
    public class PropertyInfo
    {
        private int max;
        private int min;
        private int curr;

        public int Max { get => max; }
        public int Min { get => min;}
        public int Curr { get => curr; set => curr = Mathf.Clamp(value, min, max); }

        public PropertyInfo()
        {

        }

        public PropertyInfo(int max,int min,int curr)
        {
            this.max = max;
            this.min = min;
            this.curr = curr;
        }

        public void SetMax(int max)
        {
            if (max <= min)
                return;
            this.max = max;
        }

        public void SetMin(int min)
        {
            if (min >= max)
                return;
            this.min = min;
        }

        public static PropertyInfo Zero
        {
            get
            {
                return new PropertyInfo(0, 0, 0);
            }
        }
    }

    /// <summary>
    /// 属性组件
    /// </summary>
    [Serializable]
    public class PropertyCom : BaseCom
    {
        /// <summary>
        /// 生命值
        /// </summary>
        public PropertyInfo Hp;
        /// <summary>
        /// 魔法之类的
        /// </summary>
        public PropertyInfo Mp;
        /// <summary>
        /// 攻击
        /// </summary>
        public PropertyInfo Attack;
        /// <summary>
        /// 移动速度
        /// </summary>
        public PropertyInfo MoveSpeed;
        /// <summary>
        /// 跳跃速度
        /// </summary>
        public PropertyInfo JumpSpeed = new PropertyInfo(10,5,10);
        /// <summary>
        /// 爬墙速度
        /// </summary>
        public PropertyInfo ClimbSpeed = new PropertyInfo(5, 2, 3);
        /// <summary>
        /// 行动速度（攻击速度）
        /// </summary>
        public PropertyInfo ActionSpeed;
        /// <summary>
        /// 质量
        /// </summary>
        public float Mass = 1;
    }
}