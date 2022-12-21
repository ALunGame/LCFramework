using System.Collections.Generic;
using com.lc.config.Editor.Excel.Core;

namespace LCConfig.Excel.Export
{
    public class ExcelExportSystem
    {
        private ExcelExportModule _exportModule = new ExcelExportModule();
        
        public void ExportAll(List<GenConfigInfo> pConfigs)
        {
            _exportModule.ExportAll(pConfigs);
        }

        public List<T> Export<T>(GenConfigInfo pInfo)
        {
            return _exportModule.Export<T>(pInfo);
        }
    }
}