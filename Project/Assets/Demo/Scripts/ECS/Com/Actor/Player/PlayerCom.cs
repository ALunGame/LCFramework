using System;
using LCECS.Core;
using System.Collections;
using UnityEngine;

namespace Demo.Com
{
    public class PlayerCom : BaseCom
    {
        [NonSerialized] public PropertyInt JumpStep;

        protected override void OnInit(Entity pEntity)
        {
            JumpStep = new PropertyInt(0, 2, 0);
        }
    }
}