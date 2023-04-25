using LCToolkit;

namespace Demo.Com.MainActor.NewMove
{
    /// <summary>
    /// 界面按键
    /// </summary>
    internal class MainActorUIBtn : MainActorVirtualBtn
    {
        private BaseButton btn;
        
        
        public MainActorUIBtn(BaseButton pBtn, int pWaitFrames) : base(pWaitFrames)
        {
            btn = pBtn;
        }

        public override bool CheckPressed()
        {
            return btn.IsPressed;
        }

        public override bool Hold()
        {
            return btn.IsHold;
        }
    }
}