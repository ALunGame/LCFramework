using Demo.Com;
using Demo.Server;
using LCECS;
using LCECS.Data;

namespace Demo
{
    public static class GameLocate
    {
        public static GameLogServer Log = new GameLogServer();

        public static FuncModuleCtrl FuncModule = new FuncModuleCtrl();


        public static GameCenter Center { get; private set; }

        public static ShapeRenderCom ShapeRender { get; private set; }

        private static InputCom _InputCom = null;

        public static TimerServer TimerServer { get; private set; }

        public static void Init(GameCenter center)
        {
            Center = center;
            TimerServer = new TimerServer();
            FuncModule.Init();
        }

        public static void Clear()
        {
            Center      = null;
            ShapeRender = null;
            _InputCom   = null;
            FuncModule.Clear(); 
        }

        public static void SetShapeRenderCom(ShapeRenderCom shapeRenderCom)
        {
            ShapeRender = shapeRenderCom;
        }

        public static void PushInputAction(InputAction action,RequestData param)
        {
            if (_InputCom == null)
                _InputCom = ECSLocate.ECS.GetWorld().GetCom<InputCom>();
            _InputCom.PushAction(action, param);
        }
    }
}
