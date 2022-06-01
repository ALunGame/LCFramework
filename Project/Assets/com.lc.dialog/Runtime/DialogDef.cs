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

        public static string GetDialogCnfName()
        {
            return ConfigDef.GetCnfNoExName("Dialog");
        }

        public static string GetDialogCnfPath()
        {
            return DialogRootPath + ConfigDef.GetCnfName("Dialog");
        }
    }
}
