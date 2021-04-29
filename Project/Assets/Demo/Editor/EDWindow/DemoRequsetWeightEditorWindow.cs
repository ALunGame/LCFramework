using LCECS.EDWindow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Demo.EDWindow
{
    public class DemoRequsetWeightEditorWindow : RequsetWeightEditorWindow
    {
        [MenuItem("逻辑/行为权重")]
        public static void OpenWindow()
        {
            DemoRequsetWeightEditorWindow window = GetWindow<DemoRequsetWeightEditorWindow>("行为权重配置");
            window.Show();
        }

        public override List<int> GetAllRequests()
        {
            List<int> reqestList = new List<int>();
            Array array = Enum.GetValues(typeof(BevType));
            foreach (var item in array)
            {
                reqestList.Add((int)item);
            }
            return reqestList;
        }

        public override string GetRequestDisplayName(int reqId)
        {
            return Enum.GetName(typeof(BevType), reqId);
        }
    }
}
