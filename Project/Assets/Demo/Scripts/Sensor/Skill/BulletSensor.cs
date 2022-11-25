using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LCSkill;
using LCMap;
using LCECS;

namespace Demo
{
    [WorldSensor(SensorType.Skill_Bullet)]
    public class BulletSensor : BaseBulletSensor
    {
        public override List<Actor> GetHitActors(BulletObj bulletObj)
        {
            throw new global::System.NotImplementedException();
        }
    }
}
