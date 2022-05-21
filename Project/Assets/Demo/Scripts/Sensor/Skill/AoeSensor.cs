using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LCSkill;
using LCMap;
using LCECS;

namespace Demo
{
    [WorldSensor(SensorType.Skill_Aoe)]
    public class AoeSensor : BaseAoeSensor
    {
        public override bool CheckActorInRange(AoeObj aoeObj, ActorObj actor)
        {
            return false;
        }

        public override bool CheckBulletInRange(AoeObj aoeObj, BulletObj bullet)
        {
            return false;
        }

        public override List<ActorObj> GetActorsInRange(AoeObj aoeObj)
        {
            return new List<ActorObj>();
        }

        public override List<BulletObj> GetBulletsInRange(AoeObj aoeObj)
        {
            return new List<BulletObj>();
        }
    }
}
