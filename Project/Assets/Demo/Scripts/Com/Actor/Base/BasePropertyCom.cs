using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCECS.Core;
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
        public int Min { get => min; }
        public int Curr { get => curr; set => curr = Mathf.Clamp(value, min, max); }

        public PropertyInfo()
        {

        }

        public PropertyInfo(int max, int min, int curr)
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
    /// 基础属性
    /// </summary>
    public class BasePropertyCom : BaseCom
    {
        /// <summary>
        /// 食物
        /// </summary>
        public PropertyInfo Food;
        /// <summary>
        /// 生命值
        /// </summary>
        public PropertyInfo Hp;
        /// <summary>
        /// 攻击
        /// </summary>
        public PropertyInfo Attack;
        /// <summary>
        /// 移动速度
        /// </summary>
        public PropertyInfo MoveSpeed;
        /// <summary>
        /// 行动速度（攻击速度）
        /// </summary>
        public PropertyInfo ActionSpeed;
    }
}
