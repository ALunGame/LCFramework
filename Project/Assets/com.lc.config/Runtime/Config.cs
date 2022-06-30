using System;
using System.Collections.Generic;
using LCLoad;
using Demo.Config;
using LCMap;


namespace LCConfig
{
    public static class Config
    {
        
        private static TBItemCnf _ItemCnf = null;
        /// <summary>
        /// ItemCnf
        /// </summary>
        public static TBItemCnf ItemCnf
        {
            get
            {
                if (_ItemCnf == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbItemCnf");
                    List<ItemCnf> configs = LCJson.JsonMapper.ToObject<List<ItemCnf>>(jsonStr);
                    _ItemCnf = new TBItemCnf();
                    _ItemCnf.AddConfig(configs);
                }
                return _ItemCnf;
            }
        }

        private static TBProduceCnf _ProduceCnf = null;
        /// <summary>
        /// ProduceCnf
        /// </summary>
        public static TBProduceCnf ProduceCnf
        {
            get
            {
                if (_ProduceCnf == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbProduceCnf");
                    List<ProduceCnf> configs = LCJson.JsonMapper.ToObject<List<ProduceCnf>>(jsonStr);
                    _ProduceCnf = new TBProduceCnf();
                    _ProduceCnf.AddConfig(configs);
                }
                return _ProduceCnf;
            }
        }

        private static TBActorCnf _ActorCnf = null;
        /// <summary>
        /// ActorCnf
        /// </summary>
        public static TBActorCnf ActorCnf
        {
            get
            {
                if (_ActorCnf == null)
                {
                    string jsonStr = LoadHelper.LoadString("TbActorCnf");
                    List<ActorCnf> configs = LCJson.JsonMapper.ToObject<List<ActorCnf>>(jsonStr);
                    _ActorCnf = new TBActorCnf();
                    _ActorCnf.AddConfig(configs);
                }
                return _ActorCnf;
            }
        }


        public static void Reload()
        {

            if(_ItemCnf!= null)
				_ItemCnf.Clear();

            if(_ProduceCnf!= null)
				_ProduceCnf.Clear();

            if(_ActorCnf!= null)
				_ActorCnf.Clear();

        }
    }
}
