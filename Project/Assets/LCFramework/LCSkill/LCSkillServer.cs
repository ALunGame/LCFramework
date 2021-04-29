using LCHelp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPToolchains.Json;

namespace LCSkill
{
    public class LCSkillServer
    {
        private SkillList skillList;

        private void Load()
        {
            if (LCSkillLocate.ReLoadData)
            {
                skillList = null;
                LCSkillLocate.ReLoadData = false;
            }

            if (skillList == null)
            {
                string dataJson = LCIO.ReadText(LCSkillLocate.SkillJsonPath);
                skillList = JsonMapper.ToObject<SkillList>(dataJson);
            }
        }

        public SkillJson GetSkillInfo(int skillId)
        {
            Load();
            for (int i = 0; i < skillList.List.Count; i++)
            {
                if (skillList.List[i].Id == skillId)
                {
                    return skillList.List[i];
                }
            }
            return null;
        }
    }
}
