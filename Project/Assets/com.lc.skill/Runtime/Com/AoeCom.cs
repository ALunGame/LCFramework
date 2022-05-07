using LCECS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCSkill
{
    /// <summary>
    /// Aoe组件
    /// 存储游戏中所有的Aoe
    /// </summary>
    public class AoeCom : BaseCom
    {
        [NonSerialized]
        private List<AoeObj> aoes = new List<AoeObj>();

        public IReadOnlyList<AoeObj> Aoes { get => aoes; }

        public void AddAoe(AoeObj aoeObj)
        {
            aoes.Add(aoeObj);
        }

        public void RemoveAoe(AoeObj aoeObj)
        {
            for (int i = 0; i < aoes.Count; i++)
            {
                if (aoes[i].Equals(aoeObj))
                {
                    aoes.RemoveAt(i);
                }
            }
        }
    }
}
