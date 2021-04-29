using OfficeOpenXml;
using System.IO;
using UnityEngine;

namespace LCConfig
{
    /// <summary>
    /// 配置转Excel
    /// 1，数据是唯一
    /// 2，转成表格会直接覆盖表格原有的数据
    /// </summary>
    public class LCConfigToExcel
    {
        public static string ExcelRootPath = "";

        #region 数据转表格

        private static string CreateConfigGroupExcel(ConfigGroup config)
        {
            if (!Directory.Exists(ExcelRootPath))
            {
                Directory.CreateDirectory(ExcelRootPath);
            }
            string confPath = ExcelRootPath + "/" + config.Name + ".xlsx";
            if (!File.Exists(confPath))
            {
                File.Create(confPath).Dispose();
            }

            return confPath;
        }

        private static void ConfigToExcel(ExcelWorksheet worksheet, Config config)
        {
            int columnCnt = config.Items.Count;
            if (columnCnt <= 0)
            {
                return;
            }
            int rowCnt = LCConfigHelp.ConfigItemValueIndex;
            if (config.Items.Count > 0)
            {
                rowCnt = config.Items[0].DataList.Count + LCConfigHelp.ConfigItemValueIndex;
            }

            //多少列
            int sheetColumnCnt = 0;
            if (worksheet.Dimension != null)
            {
                sheetColumnCnt = worksheet.Dimension.Columns;
            }

            int createColumnIndex = sheetColumnCnt;


            for (int i = 0; i < config.Items.Count; i++)
            {
                ConfigItem configItem = config.Items[i];

                //查找到那一列
                int searchColumn = -1;
                for (int j = 1; j <= sheetColumnCnt; j++)
                {
                    string name = worksheet.GetValue<string>(1, j);
                    if (name == configItem.Name)
                    {
                        searchColumn = j;
                    }
                }
                //没有此列创建模板
                if (searchColumn == -1)
                {
                    createColumnIndex++;
                    searchColumn = createColumnIndex;
                }

                //最后一步赋值
                for (int z = 0; z < rowCnt; z++)
                {
                    //名字
                    if (z == 0)
                    {
                        worksheet.SetValue(1, searchColumn, configItem.Name);
                    }
                    //类型
                    else if (z == 1)
                    {
                        worksheet.SetValue(2, searchColumn, configItem.Type.ToString());
                    }
                    //规则数据
                    else if (z == 2)
                    {
                        worksheet.SetValue(3, searchColumn, configItem.RuleType.ToString());
                    }
                    //规则数据
                    else if (z == 3)
                    {
                        worksheet.SetValue(4, searchColumn, configItem.RuleData.ToString());
                    }
                    //注释
                    else if (z == 4)
                    {
                        worksheet.SetValue(5, searchColumn, configItem.Desc.ToString());
                    }
                    else
                    {
                        int dataIndex = z - LCConfigHelp.ConfigItemValueIndex;
                        if (dataIndex < configItem.DataList.Count)
                        {
                            worksheet.SetValue(z + 1, searchColumn, configItem.DataList[dataIndex]);
                        }
                    }
                }
            }
        }

        public static void ConfigGroupToExcel(ConfigGroup configGroup)
        {
            if (configGroup.Configs.Count <= 0)
            {
                return;
            }
            string excelPath = CreateConfigGroupExcel(configGroup);

            FileStream fileStream = new FileStream(excelPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            using (fileStream)
            {
                using (ExcelPackage package = new ExcelPackage(fileStream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheets sheets = package.Workbook.Worksheets;

                    for (int i = 0; i < configGroup.Configs.Count; i++)
                    {
                        Config config = configGroup.Configs[i];

                        ExcelWorksheet sheet = null;
                        foreach (ExcelWorksheet workSheet in sheets)
                        {
                            if (workSheet.Name == config.Name)
                            {
                                sheet = workSheet;
                                break;
                            }
                        }
                        //工作表不在模板中
                        if (sheet == null)
                            sheet = sheets.Add(config.Name);

                        ConfigToExcel(sheet, config);
                    }

                    package.Save();
                    package.Dispose();
                };
            }
        }

        public static void ConfigToExcel(ConfigGroup configGroup, Config config)
        {
            string excelPath = CreateConfigGroupExcel(configGroup);

            FileStream fileStream = new FileStream(excelPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            using (fileStream)
            {
                using (ExcelPackage package = new ExcelPackage(fileStream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheets sheets = package.Workbook.Worksheets;

                    for (int i = 0; i < configGroup.Configs.Count; i++)
                    {
                        if (configGroup.Configs[i].Name == config.Name)
                        {
                            ExcelWorksheet sheet = null;
                            foreach (ExcelWorksheet workSheet in sheets)
                            {
                                if (workSheet.Name == config.Name)
                                {
                                    sheet = workSheet;
                                    break;
                                }
                            }
                            //工作表不在模板中
                            if (sheet == null)
                                sheet = sheets.Add(config.Name);

                            ConfigToExcel(sheet, config);
                            break;
                        }
                    }

                    package.Save();
                    package.Dispose();
                };
            }
        }

        #endregion

        #region 表格转数据

        //cover  是否覆盖数据
        private static void ExcelToConfig(ExcelWorksheet worksheet, Config config, bool cover)
        {
            //表格列
            int sheetColumnCnt = worksheet.Dimension.Columns;
            if (sheetColumnCnt <= 0)
                return;
            //数据列
            int columnCnt = config.Items.Count;
            if (columnCnt <= 0)
                return;

            //if (sheetColumnCnt< columnCnt)
            //{
            //    Debug.LogError("表格结构与数据结构不一致》》》》》》 " + worksheet.Name);
            //    return;
            //}

            //多少行
            int rowCnt = worksheet.Dimension.Rows;

            //结构检测
            for (int i = 0; i < config.Items.Count; i++)
            {
                ConfigItem configItem = config.Items[i];

                bool has = false;
                for (int j = 1; j <= columnCnt; j++)
                {
                    string name = worksheet.GetValue<string>(1, j);
                    if (name == configItem.Name)
                    {
                        has = true;
                    }
                }

                if (has == false)
                {
                    Debug.LogError("表格结构与数据结构不一致》》》》》转换后会有数据缺失请注意！！！！！ " + worksheet.Name);
                    break;
                    //return;
                }
            }

            //赋值
            for (int i = 0; i < config.Items.Count; i++)
            {
                ConfigItem configItem = config.Items[i];
                for (int j = 1; j <= columnCnt; j++)
                {
                    string name = worksheet.GetValue<string>(1, j);
                    if (name == configItem.Name)
                    {
                        //覆盖
                        if (cover)
                            configItem.DataList.Clear();

                        for (int z = 1; z <= rowCnt; z++)
                        {
                            //第五行是数据
                            if (z > LCConfigHelp.ConfigItemValueIndex)
                            {
                                string value = worksheet.GetValue<string>(z, j);
                                configItem.DataList.Add(value);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 表格转配置组
        /// </summary>
        /// <param name="excelPath">表格路径</param>
        /// <param name="configGroup"></param>
        /// <param name="cover">是否覆盖原有数据</param>
        public static void ExcelToConfigGroup(string excelPath, ConfigGroup configGroup, bool cover = false)
        {
            if (!File.Exists(excelPath))
            {
                return;
            }

            FileStream fileStream = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (fileStream)
            {
                using (ExcelPackage package = new ExcelPackage(fileStream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheets sheets = package.Workbook.Worksheets;

                    for (int i = 0; i < configGroup.Configs.Count; i++)
                    {
                        Config config = configGroup.Configs[i];

                        ExcelWorksheet sheet = null;
                        foreach (ExcelWorksheet workSheet in sheets)
                        {
                            if (workSheet.Name == config.Name)
                            {
                                sheet = workSheet;
                                break;
                            }
                        }
                        //工作表不在模板中
                        if (sheet == null)
                            sheet = sheets.Add(config.Name);

                        ExcelToConfig(sheet, config, cover);
                    }
                };
            }
        }

        /// <summary>
        /// 表格转配置
        /// </summary>
        /// <param name="excelPath">表格路径</param>
        /// <param name="configGroup"></param>
        /// <param name="config"></param>
        /// <param name="cover">是否覆盖原有数据</param>
        public static void ExcelToConfig(string excelPath, ConfigGroup configGroup, Config config, bool cover = false)
        {
            if (!File.Exists(excelPath))
            {
                return;
            }

            FileStream fileStream = new FileStream(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (fileStream)
            {
                using (ExcelPackage package = new ExcelPackage(fileStream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheets sheets = package.Workbook.Worksheets;

                    ExcelWorksheet sheet = null;
                    foreach (ExcelWorksheet workSheet in sheets)
                    {
                        if (workSheet.Name == config.Name)
                        {
                            sheet = workSheet;
                            break;
                        }
                    }
                    //工作表不在模板中
                    if (sheet == null)
                    {
                        Debug.LogError("选择的表格并没有指定的配置   " + config.Name);
                        return;
                    }
                    ExcelToConfig(sheet, config, cover);
                };
            }
        }

        #endregion
    }
}
