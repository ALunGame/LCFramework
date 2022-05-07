using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCSkill
{
    public struct State
    {
        ///<summary>
        ///是否可以移动坐标
        ///</summary>
        public bool canMove;

        ///<summary>
        ///是否可以转身
        ///</summary>
        public bool canRotate;

        ///<summary>
        ///是否可以使用技能，这里的是“使用技能”特指整个技能流程是否可以开启
        ///如果是类似中了沉默，则应该走buff的onCast，尤其是类似wow里面沉默了不能施法但是还能放致死打击（部分技能被分类为法术，会被沉默，而不是法术的不会）
        ///</summary>
        public bool canUseSkill;
    }
}
