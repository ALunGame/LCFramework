using LCConfig;
using LCECS.Core.ECS;
using LCHelp;
using LCSkill;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.Com
{
    public class SkillInfo
    {
        public int SkillId;
        public SkillImpact CurImpact;
        public TimeLine SkillLine;
    }

    [Com(ViewName = "技能组件", GroupName = "Entity")]
    public class SkillCom : BaseCom
    {
        //请求释放的技能
        [ComValue]
        public int ReqSkillId;
        //当前显示的技能
        [ComValue]
        public int CurShowSkillId;
        //上一个显示的技能
        [ComValue]
        public int LastShowSkillId;
        
        //当前起作用的技能
        public Dictionary<int, SkillInfo> CurrSkillDict = new Dictionary<int, SkillInfo>();

        //技能检测的点
        public Transform SkillCheckPoint = null;
        //技能检测的大小
        public Vector3 SkillCheckSize=Vector3.zero;

        protected override void OnInit(GameObject go)
        {
            string sizeData = LCConfigLocate.GetConfigItemValue("BaseInfo", EntityCnfId.ToString(),"SkillUseSize");
            if (sizeData!=null)
            {
                SkillCheckSize = (Vector3)LCConvert.StrChangeToObject(sizeData, typeof(Vector3).FullName);
            }

            if (go.transform.Find("SkillPoint")!=null)
            {
                SkillCheckPoint = go.transform.Find("SkillPoint");
            }
            else
            {
                SkillCheckPoint = go.transform;
            }    
        }
    }
}
