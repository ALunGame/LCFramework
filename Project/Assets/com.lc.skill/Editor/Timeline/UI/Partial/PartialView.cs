namespace LCSkill.Timeline
{
    public class PartialView
    {
        protected SkillTimelineWindow window;
        
        public void SetWindow(SkillTimelineWindow pWindow)
        {
            window = pWindow;
            window.OnFocuseWindow += OnFocuseWindow;
        }

        public virtual void OnFocuseWindow()
        {
            
        }
        
        public void Clear()
        {
            window.OnFocuseWindow -= OnFocuseWindow;
            OnClear();
        }

        public virtual void OnClear()
        {
            
        }
        
        public static T Create<T>(SkillTimelineWindow pWindow) where T : PartialView,new()
        {
            T view = new T();
            view.SetWindow(pWindow);
            return view;
        }
        
        public static object Create(SkillTimelineWindow pWindow, System.Type pViewType)
        {
            object view = LCToolkit.ReflectionHelper.CreateInstance(pViewType);
            ((PartialView)view).SetWindow(pWindow);
            return view;
        }
    }
}