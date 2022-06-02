using LCDialog;
using UnityEngine;

namespace Demo.Dialog
{
    public class DialogCenter : MonoBehaviour
    {
        private void Awake()
        {
            DialogLocate.Init();
            DialogLocate.SetDisplayServer(new DialogDisplayServer());
        }

        private void OnDestroy()
        {
            DialogLocate.Clear();
        }
    }
}
