
using System;
using System.Collections.Generic;
using LCLoad;
using MemoryPack;
using TT;
using Cnf;
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
                    Byte[] byteArray = LoadHelper.LoadBytes("TbTest");
                    List<Test> configs = MemoryPackSerializer.Deserialize<List<Test>>(byteArray);
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
                    Byte[] byteArray = LoadHelper.LoadBytes("TbItemCnf");
                    List<ItemCnf> configs = MemoryPackSerializer.Deserialize<List<ItemCnf>>(byteArray);
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
                    Byte[] byteArray = LoadHelper.LoadBytes("TbItemRecipeCnf");
                    List<ItemRecipeCnf> configs = MemoryPackSerializer.Deserialize<List<ItemRecipeCnf>>(byteArray);
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
                    Byte[] byteArray = LoadHelper.LoadBytes("TbItemRepairCnf");
                    List<ItemRepairCnf> configs = MemoryPackSerializer.Deserialize<List<ItemRepairCnf>>(byteArray);
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
                    Byte[] byteArray = LoadHelper.LoadBytes("TbSkillCnf");
                    List<SkillCnf> configs = MemoryPackSerializer.Deserialize<List<SkillCnf>>(byteArray);
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
                    Byte[] byteArray = LoadHelper.LoadBytes("TbActorCnf");
                    List<ActorCnf> configs = MemoryPackSerializer.Deserialize<List<ActorCnf>>(byteArray);
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
                    Byte[] byteArray = LoadHelper.LoadBytes("TbActorBasePropertyCnf");
                    List<ActorBasePropertyCnf> configs = MemoryPackSerializer.Deserialize<List<ActorBasePropertyCnf>>(byteArray);
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
                    Byte[] byteArray = LoadHelper.LoadBytes("TbWeaponCnf");
                    List<WeaponCnf> configs = MemoryPackSerializer.Deserialize<List<WeaponCnf>>(byteArray);
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
                    Byte[] byteArray = LoadHelper.LoadBytes("TbEventCnf");
                    List<EventCnf> configs = MemoryPackSerializer.Deserialize<List<EventCnf>>(byteArray);
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
                    Byte[] byteArray = LoadHelper.LoadBytes("TbUIPanelCnf");
                    List<UIPanelCnf> configs = MemoryPackSerializer.Deserialize<List<UIPanelCnf>>(byteArray);
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
