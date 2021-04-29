namespace Demo
{
    //决策分组
    public enum DecGroup
    {
        None,                   
        Enemy,                  //普通敌人
    }

    //行为类型
    public enum BevType
    {
        None,
        PlayerMove,             //玩家移动
        PlayerNormalAttack,     //玩家普通攻击
        Attack,                 //攻击
        SeekPath,               //寻路
    }
}
