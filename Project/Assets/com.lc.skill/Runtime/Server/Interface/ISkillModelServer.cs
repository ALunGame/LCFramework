namespace LCSkill
{
    public interface ISkillModelServer
    {
        public bool GetSkillModel(string skillId, out SkillModel model);

        public bool GetTimelineModel(string timelineName, out TimelineModel model);

        public bool GetBuffModel(string buffId, out BuffModel model);

        public bool GetAoeModel(string aoeId, out AoeModel model);

        public bool GetBulletModel(string bulletId, out BulletModel model);
    }
}
