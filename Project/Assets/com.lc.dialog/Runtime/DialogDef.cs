using LCConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCDialog
{
    public static class DialogDef
    {
        public const string CnfRootPath = "Assets/Demo/Asset/Config/";

        /// <summary>
        /// 对话根目录
        /// </summary>
        public const string DialogRootPath = CnfRootPath + "Dialog/";

        public static string GetDialogCnfName(DialogType dialogType)
        {
            return ConfigDef.GetCnfNoExName($"{dialogType}Dialog");
        }

        public static string GetDialogCnfPath(DialogType dialogType)
        {
            return DialogRootPath + ConfigDef.GetCnfName($"{dialogType}Dialog");
        }
    }

    public enum DialogType
    {
        /// <summary>
        /// 人物气泡对话
        /// </summary>
        Bubble,
    }
}
