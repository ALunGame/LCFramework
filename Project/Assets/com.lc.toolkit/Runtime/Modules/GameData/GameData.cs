namespace LCToolkit
{
    public abstract class GameData
    {
        /// <summary>
        /// 文件保存名
        /// </summary>
        public abstract string FileName { get; }

        public GameData()
        {
            ToolkitLocate.GameData.AddGameData(GetType(), FileName);
        }
    }
}
