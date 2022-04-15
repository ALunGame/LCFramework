using System.Text.RegularExpressions;

namespace Demo.Config
{
    /// <summary>
    /// 临时的类
    /// </summary>
    public class TempConfig
    {
        public static int MapSizeX = 30;
        public static int MapSizeY = 10;

        public static void Init()
        {
        }

        //清除字符串括号
        public static string RemoveStrBracket(string str)
        {
            str.Replace("（", "(").Replace("）", ")");
            str = Regex.Replace(str.Replace("（", "(").Replace("）", ")"), @"\([^\(]*\)", "");
            return str;
        }

    }
}