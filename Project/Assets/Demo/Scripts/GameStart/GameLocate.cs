namespace Demo
{
    public static class GameLocate
    {
        public static GameCenter Center { get; private set; }
        public static ShapeRenderCom ShapeRender { get; private set; }

        public static void Init(GameCenter center)
        {
            Center = center;
        }

        public static void Clear()
        {
            Center = null;
            ShapeRender = null;
        }

        public static void SetShapeRenderCom(ShapeRenderCom shapeRenderCom)
        {
            ShapeRender = shapeRenderCom;
        }
    }
}
