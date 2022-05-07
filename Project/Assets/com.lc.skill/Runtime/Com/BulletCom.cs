using LCECS.Core;
using System;
using System.Collections.Generic;

namespace LCSkill
{
    /// <summary>
    /// 子弹组件
    /// 存储游戏中所有的子弹
    /// </summary>
    public class BulletCom : BaseCom
    {
        [NonSerialized]
        private List<BulletObj> bullets = new List<BulletObj>();

        public IReadOnlyList<BulletObj> Bullets { get => bullets; }

        public void AddBullet(BulletObj bulletObj)
        {
            bullets.Add(bulletObj);
        }

        public void RemoveBullet(BulletObj bulletObj)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].Equals(bulletObj))
                {
                    bullets.RemoveAt(i);
                }
            }
        }
    }
}
