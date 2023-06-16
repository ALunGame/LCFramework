using UnityEngine;

namespace Demo.Com.MainActor.NewMove
{
    /// <summary>
    /// 键盘按键
    /// </summary>
    internal class MainActorKeyCodeBtn : MainActorVirtualBtn
    {
        private KeyCode key;
        
        public MainActorKeyCodeBtn(KeyCode pKey,int pWaitFrames) : base(pWaitFrames)
        {
            key = pKey;
        }

        public override bool CheckPressed()
        {
            return UnityEngine.Input.GetKeyDown(key);
        }

        public override bool Hold()
        {
            return UnityEngine.Input.GetKey(key);
        }
    }
}