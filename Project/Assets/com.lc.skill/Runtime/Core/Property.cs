using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCSkill
{
    /// <summary>
    /// 属性数据结构
    /// 这种数据不应该和人物绑定，在设计层面没有所谓的人物
    /// </summary>
    public struct Property
    {
        ///<summary>
        ///最大生命，基本都得有，哪怕角色只有1，装备可以是0
        ///</summary>
        public int hp;

        ///<summary>
        ///弹仓，其实相当于mp了，只是我是射击游戏所以题材需要换皮。
        ///玩家层面理解，跟普通mp上限的区别是角色这个值上限一般都是0，它来自于装备。
        ///</summary>
        public int mp;

        ///<summary>
        ///攻击力
        ///</summary>
        public int attack;

        ///<summary>
        ///移动速度，他不是米/秒作为单位的，而是一个可以培养的数值。
        ///具体转化为米/秒，是需要一个规则的，所以是策划脚本 int SpeedToMoveSpeed(int speed)来返回
        ///</summary>
        public int moveSpeed;

        ///<summary>
        ///行动速度，和移动速度不同，他是增加角色行动速度，也就是变化timeline和动画播放的scale的，比如wow里面开嗜血就是加行动速度
        ///具体多少也不是一个0.2f（我这个游戏中规则设定的最快为正常速度的20%，你的游戏你自己定）到5.0f（我这个游戏设定了最慢是正常速度20%），和移动速度一样需要脚本接口返回策划公式
        ///</summary>
        public int actionSpeed;

        ///<summary>
        ///体型圆形半径，用于移动碰撞的，单位：米
        ///这个属性因人而异，但是其实在玩法中几乎不可能经营它，只有buff可能会改变一下，所以直接用游戏中用的数据就行了，不需要转化了
        ///</summary>
        public float bodyRadius;

        ///<summary>
        ///挨打圆形半径，同体型圆形，只是用途不同，用在判断子弹是否命中的时候
        ///</summary>
        public float hitRadius;

    }
}
