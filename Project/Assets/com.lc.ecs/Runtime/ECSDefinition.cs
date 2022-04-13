namespace LCECS
{
    /// <summary>
    /// ECS默认路径定义
    /// </summary>
    public static class ECSDefPath
    {
        //实体配置路径
        public const string EntityJsonPath = "Resources/Config/ECS/EntityJson.txt";

        //逻辑层请求权重路径
        public const string LogicReqWeightPath = "Resources/Config/ECS/ReqWeight.txt";

        //系统排序
        public const string SystemSortPath = "Resources/Config/ECS/SystemSort.txt";

        //决策树路径
        public const string DecTreePath = "Resources/Config/ECS/DecTree.txt";
        //决行为树路径
        public const string BevTreePath = "Resources/Config/ECS/BevTree.txt";

        /// <summary>
        /// 配置根目录
        /// </summary>
        public const string CnfRootPath = "Assets/Resources/Config/";

        /// <summary>
        /// 决策树根目录
        /// </summary>
        public const string DecTreeRootPath = CnfRootPath + "DecTree/";

        /// <summary>
        /// 行为树根目录
        /// </summary>
        public const string BevTreeRootPath = CnfRootPath + "BevTree/";

        /// <summary>
        /// 行为树根目录
        /// </summary>
        public const string EntityRootPath = CnfRootPath + "Entity/";


        /// <summary>
        /// 获得决策路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetDecTreePath(string fileName)
        {
            return DecTreeRootPath + fileName + ".txt";
        }

        /// <summary>
        /// 获得行为路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetBevTreePath(string fileName)
        {
            return BevTreeRootPath + fileName + ".txt";
        }

        /// <summary>
        /// 获得实体配置路径
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetEntityPath(string fileName)
        {
            return EntityRootPath + fileName + ".txt";
        }
    }

    /// <summary>
    /// ECS各种定义
    /// </summary>
    public static class ECSDefinition
    {
        //只调用请求实例置换规则
        public const int RESwithRuleSelf = -9;
        //强制置换请求权重
        public const int REForceSwithWeight = -99;
    }

    /// <summary>
    /// 工厂类别
    /// </summary>
    public enum FactoryType
    {
        Entity,                 //实体
        Asset,                  //资源
    }

    /// <summary>
    /// 信息类型
    /// </summary>
    public enum SensorType
    {
        Player,                  //玩家信息
        Map,                     //地图信息
        Entity,                  //实体信息
        Skill,                   //技能信息
    }
}
