using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCConfig
{
    [Config("测试")]
    public class TestConfig:IConfig
    {
        [ConfigKey(1,"ccc")]
        public int key = 0;

        [ConfigKey(2, "aa")]
        public string name = "";

        [ConfigKey(3, "bbbb")]
        public string aaaaa = "";
    }
}
