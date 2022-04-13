using LCNode.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LCECS.Tree
{
    public class Behavior : BaseGraph
    {
        public override List<string> NodeNamespace => new List<string>() { "LCECS.Tree" };
    }
}
