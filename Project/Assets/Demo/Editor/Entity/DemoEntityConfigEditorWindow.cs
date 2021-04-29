using LCECS.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Demo.Entity
{
    public class DemoEntityConfigEditorWindow : EntityConfigEditorWindow
    {
        [MenuItem("实体/编辑")]
        public static void OpenWindow()
        {
            DemoEntityConfigEditorWindow window = GetWindow<DemoEntityConfigEditorWindow>("实体编辑");
            window.minSize = new Vector2(800, 600);
            window.Show();
        }

        public override string GetDecNameById(int decId)
        {
            return Enum.GetName(typeof(DecGroup), decId);
        }

        public override List<string> GetDecNames()
        {
            return Enum.GetNames(typeof(DecGroup)).ToList();
        }
    }
}
