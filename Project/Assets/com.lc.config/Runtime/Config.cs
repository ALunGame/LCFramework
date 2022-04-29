using System;
using System.Collections.Generic;
using LCLoad;
using LCMap;


namespace LCConfig
{
    public static class Config
    {
        
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

            if(_ActorCnf!= null)
				_ActorCnf.Clear();

        }
    }
}
