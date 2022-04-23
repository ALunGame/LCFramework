using System;
using System.Collections.Generic;
using LCConfig;


namespace LCConfig
{
    public static class ConfigMapping
    {
        private static Func<string, List<IConfig>> loadFunc = null;

        
        private static TBTestConfig tBTestConfig = null;
        /// <summary>
        /// 
        /// </summary>
        public static TBTestConfig TBTestConfig
        {
            get
            {
                if (tBTestConfig != null)
                    return tBTestConfig;
                else
                {
                    tBTestConfig = new TBTestConfig();
                    tBTestConfig.AddConfig(loadFunc("TestConfig"));
                }
                return null;
            }
        }


        public static void Init(Func<string, List<IConfig>> loadFunc)
        {
            ConfigMapping.loadFunc = loadFunc;
        }

        public static void Reload()
        {
			if(tBTestConfig!= null)
				tBTestConfig.Clear();
        }
    }
}
