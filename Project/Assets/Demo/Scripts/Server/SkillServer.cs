using LCSkill;
using LCToolkit;

namespace Demo
{
    public class SkillServer : ISkillServer
    {
        public void CreateAoe(SkillCom ower, AddAoeModel addAoe)
        {
            if (SkillLocate.Model.GetAoeModel(addAoe.id,out AoeModel model))
            {
                AoeCom aoeCom = LCECS.ECSLocate.ECS.GetWorld().GetCom<AoeCom>();
                AoeObj aoeObj = new AoeObj();

                //配置
                aoeObj.model = model;

                //预制体
                if (model.asset != null && !string.IsNullOrEmpty(model.asset.ObjName))
                {
                    aoeObj.go = ToolkitLocate.GoPool.Take(model.asset.ObjName);
                }

                //拥有者
                aoeObj.ower = ower;

                //配置数据
                aoeObj.size = addAoe.size;
                aoeObj.duration = addAoe.duration;
                aoeObj.follow = addAoe.follow;

                //保存
                aoeCom.AddAoe(aoeObj);
            }
        }

        public void CreateBuff(SkillCom ower, SkillCom target, AddBuffModel addBuff)
        {
            if (SkillLocate.Model.GetBuffModel(addBuff.id,out BuffModel model))
            {
                AddBuffInfo addBuffInfo = new AddBuffInfo();
                addBuffInfo.ower = ower;
                addBuffInfo.target = target;
                addBuffInfo.buffModel = model;
                addBuffInfo.addStack = addBuff.addStack;
                addBuffInfo.durationSetType = addBuff.durationSetType;
                addBuffInfo.duration = addBuff.duration;
                addBuffInfo.isPermanent = addBuff.isPermanent;
                target.AddBuff(addBuffInfo);
            }
        }

        public void CreateBullet(SkillCom ower, AddBulletModel addBullet)
        {
            if (SkillLocate.Model.GetBulletModel(addBullet.id, out BulletModel model))
            {
                BulletCom bulletCom = LCECS.ECSLocate.ECS.GetWorld().GetCom<BulletCom>();

                BulletObj bulletObj = new BulletObj();

                //配置
                bulletObj.model = model;

                //预制体
                if (model.asset != null && !string.IsNullOrEmpty(model.asset.ObjName))
                {
                    bulletObj.go = ToolkitLocate.GoPool.Take(model.asset.ObjName);
                }

                //拥有者
                bulletObj.ower = ower;

                //配置数据
                bulletObj.firePos = addBullet.firePos;
                bulletObj.fireDir = addBullet.fireDir;
                bulletObj.speed = addBullet.speed;
                bulletObj.duration = addBullet.duration;
                bulletObj.useFireDirForever = addBullet.useFireDirForever;
                bulletObj.canHitAfterCreated = addBullet.canHitAfterCreated;

                //保存
                bulletCom.AddBullet(bulletObj);
            }
        }

        public void LearnSkill(SkillCom target, string skillId, int level = 1)
        {
            if (SkillLocate.Model.GetSkillModel(skillId,out SkillModel model))
            {
                target.LearnSkill(model, level);
            }
        }

        public bool ReleaseSkill(SkillCom target, string skillId)
        {
            return target.ReleaseSkill(skillId);
        }
    }
}
