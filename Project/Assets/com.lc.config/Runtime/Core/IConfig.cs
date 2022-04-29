namespace LCConfig
{
    /// <summary>
    /// 表明是配置项
    /// </summary>
    public interface IConfig
    {
        IConfig Clone();
    }
}