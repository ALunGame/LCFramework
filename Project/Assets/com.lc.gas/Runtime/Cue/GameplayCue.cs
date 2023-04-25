using LCToolkit;

namespace LCGAS
{
    /// <summary>
    /// 处理GE一些跟逻辑无关的效果，音效，屏幕震动啥的
    /// </summary>
    public abstract class GameplayCue
    {
        //标签
        public GameplayTagContainer tags = new GameplayTagContainer();
        public abstract void HandleCue(GameplayEffectSpec pEffectSpec);
    }
}