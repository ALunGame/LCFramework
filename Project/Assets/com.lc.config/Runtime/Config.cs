
using System;
using System.Collections.Generic;
using LCLoad;
using TT;
using Demo.Config;
using LCMap;
using Demo;


namespace LCConfig
{
    public static class Config
    {
        
        private static TbTest _Test = null;
        /// <summary>
        /// 测试
        /// </summary>
        public static TbTest Test
        {
            get
            {
                if (_Test == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbTest");
                    List<Test> configs = LCJson.JsonMapper.ToObject<List<Test>>(jsonStr);
                    _Test = new TbTest();
                    _Test.AddConfig(configs);
                }
                return _Test;
            }
        }

        private static TbItemCnf _ItemCnf = null;
        /// <summary>
        /// 物品信息
        /// </summary>
        public static TbItemCnf ItemCnf
        {
            get
            {
                if (_ItemCnf == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbItemCnf");
                    List<ItemCnf> configs = LCJson.JsonMapper.ToObject<List<ItemCnf>>(jsonStr);
                    _ItemCnf = new TbItemCnf();
                    _ItemCnf.AddConfig(configs);
                }
                return _ItemCnf;
            }
        }

        private static TbItemRecipeCnf _ItemRecipeCnf = null;
        /// <summary>
        /// 物品配方信息
        /// </summary>
        public static TbItemRecipeCnf ItemRecipeCnf
        {
            get
            {
                if (_ItemRecipeCnf == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbItemRecipeCnf");
                    List<ItemRecipeCnf> configs = LCJson.JsonMapper.ToObject<List<ItemRecipeCnf>>(jsonStr);
                    _ItemRecipeCnf = new TbItemRecipeCnf();
                    _ItemRecipeCnf.AddConfig(configs);
                }
                return _ItemRecipeCnf;
            }
        }

        private static TbItemRepairCnf _ItemRepairCnf = null;
        /// <summary>
        /// 物品修复信息
        /// </summary>
        public static TbItemRepairCnf ItemRepairCnf
        {
            get
            {
                if (_ItemRepairCnf == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbItemRepairCnf");
                    List<ItemRepairCnf> configs = LCJson.JsonMapper.ToObject<List<ItemRepairCnf>>(jsonStr);
                    _ItemRepairCnf = new TbItemRepairCnf();
                    _ItemRepairCnf.AddConfig(configs);
                }
                return _ItemRepairCnf;
            }
        }

        private static TbSkillCnf _SkillCnf = null;
        /// <summary>
        /// 技能信息
        /// </summary>
        public static TbSkillCnf SkillCnf
        {
            get
            {
                if (_SkillCnf == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbSkillCnf");
                    List<SkillCnf> configs = LCJson.JsonMapper.ToObject<List<SkillCnf>>(jsonStr);
                    _SkillCnf = new TbSkillCnf();
                    _SkillCnf.AddConfig(configs);
                }
                return _SkillCnf;
            }
        }

        private static TbActorCnf _ActorCnf = null;
        /// <summary>
        /// 演员信息
        /// </summary>
        public static TbActorCnf ActorCnf
        {
            get
            {
                if (_ActorCnf == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbActorCnf");
                    List<ActorCnf> configs = LCJson.JsonMapper.ToObject<List<ActorCnf>>(jsonStr);
                    _ActorCnf = new TbActorCnf();
                    _ActorCnf.AddConfig(configs);
                }
                return _ActorCnf;
            }
        }

        private static TbActorBasePropertyCnf _ActorBasePropertyCnf = null;
        /// <summary>
        /// 演员基础属性
        /// </summary>
        public static TbActorBasePropertyCnf ActorBasePropertyCnf
        {
            get
            {
                if (_ActorBasePropertyCnf == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbActorBasePropertyCnf");
                    List<ActorBasePropertyCnf> configs = LCJson.JsonMapper.ToObject<List<ActorBasePropertyCnf>>(jsonStr);
                    _ActorBasePropertyCnf = new TbActorBasePropertyCnf();
                    _ActorBasePropertyCnf.AddConfig(configs);
                }
                return _ActorBasePropertyCnf;
            }
        }

        private static TbWeaponCnf _WeaponCnf = null;
        /// <summary>
        /// 武器基础属性
        /// </summary>
        public static TbWeaponCnf WeaponCnf
        {
            get
            {
                if (_WeaponCnf == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbWeaponCnf");
                    List<WeaponCnf> configs = LCJson.JsonMapper.ToObject<List<WeaponCnf>>(jsonStr);
                    _WeaponCnf = new TbWeaponCnf();
                    _WeaponCnf.AddConfig(configs);
                }
                return _WeaponCnf;
            }
        }

        private static TbEventCnf _EventCnf = null;
        /// <summary>
        /// 事件信息
        /// </summary>
        public static TbEventCnf EventCnf
        {
            get
            {
                if (_EventCnf == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbEventCnf");
                    List<EventCnf> configs = LCJson.JsonMapper.ToObject<List<EventCnf>>(jsonStr);
                    _EventCnf = new TbEventCnf();
                    _EventCnf.AddConfig(configs);
                }
                return _EventCnf;
            }
        }

        private static TbUIPanelCnf _UIPanelCnf = null;
        /// <summary>
        /// 界面配置
        /// </summary>
        public static TbUIPanelCnf UIPanelCnf
        {
            get
            {
                if (_UIPanelCnf == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbUIPanelCnf");
                    List<UIPanelCnf> configs = LCJson.JsonMapper.ToObject<List<UIPanelCnf>>(jsonStr);
                    _UIPanelCnf = new TbUIPanelCnf();
                    _UIPanelCnf.AddConfig(configs);
                }
                return _UIPanelCnf;
            }
        }


        public static void Reload()
        {

            if(_Test!= null)
				_Test.Clear();

            if(_ItemCnf!= null)
				_ItemCnf.Clear();

            if(_ItemRecipeCnf!= null)
				_ItemRecipeCnf.Clear();

            if(_ItemRepairCnf!= null)
				_ItemRepairCnf.Clear();

            if(_SkillCnf!= null)
				_SkillCnf.Clear();

            if(_ActorCnf!= null)
				_ActorCnf.Clear();

            if(_ActorBasePropertyCnf!= null)
				_ActorBasePropertyCnf.Clear();

            if(_WeaponCnf!= null)
				_WeaponCnf.Clear();

            if(_EventCnf!= null)
				_EventCnf.Clear();

            if(_UIPanelCnf!= null)
				_UIPanelCnf.Clear();

        }
    }
}
