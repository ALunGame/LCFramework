using LCToolkit;

namespace LCGAS
{
    /// <summary>
    /// 通用的条件标签
    /// </summary>
    public class CommonConditionTag
    {
        /// <summary>
        /// 需要的标签
        /// </summary>
        public GameplayTagContainer requireTags = new GameplayTagContainer();
        
        /// <summary>
        /// 不能有的标签
        /// </summary>
        public GameplayTagContainer ignoreTags = new GameplayTagContainer();
        
        public bool CheckAbilitySystemCom(AbilitySystemCom pCheckCom)
        {
            if (requireTags == null && requireTags == null)
            {
                return true;
            }

            bool res = false;
            if (requireTags != null)
            {
                res = pCheckCom.Tag.HasAll(requireTags);
            }
            
            if (ignoreTags != null)
            {
                res = !pCheckCom.Tag.HasAll(ignoreTags);
            }

            return res;
        }
    }
    
    /// <summary>
    /// 通用的添加删除标签
    /// </summary>
    public class CommonAddAndRemoveTag
    {
        /// <summary>
        /// 激活需要的标签
        /// </summary>
        public GameplayTagContainer addTags = new GameplayTagContainer();
        
        /// <summary>
        /// 激活不能有的标签
        /// </summary>
        public GameplayTagContainer removeTags = new GameplayTagContainer();



    }
}