using System;
using System.Reflection;
using LCSkill.Timeline;
using LCToolkit;
using LCToolkit.Core;
using LCToolkit.Element;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.UIElements.Cursor;

namespace LCSkill.Timeline
{
    /// <summary>
    /// 片段
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class TimlineClipElementAttribute : Attribute
    {
        /// <summary>
        /// 指定数据类型
        /// </summary>
        public Type targetModelType;

        public TimlineClipElementAttribute(Type targetModelType)
        {
            this.targetModelType = targetModelType;
        }
    }

    public abstract class InternalClip_Element : PartialView
    {
        public abstract BaseClip OwerModel { get;}

        public VisualElement RootElement { get; protected set; }
        
        public abstract void SetUp(BaseClip pModel, ClipList_Element pCliplistElement);
    }
    
    public class Clip_Element<T> : InternalClip_Element where T : BaseClip
    {
        public const float MinWidth = Frame_Element.FrameWidth;
        
        public override BaseClip OwerModel { get => Model; }
        public T Model { get; protected set; }
        
        protected VisualElement leftDragElment;
        protected DragManipulator leftDragManipulator;
        protected VisualElement rightDragElment;
        protected DragManipulator rightDragManipulator;
        protected VisualElement middleDragElment;
        protected DragManipulator middleDragManipulator;

        protected ClipList_Element cliplistElement;

        private float frameWidth;

        #region Init

        public override void SetUp(BaseClip pModel, ClipList_Element pCliplistElement)
        {
            //数据
            Model = (T)pModel;
            cliplistElement = pCliplistElement;
            
            //创建UI元素
            CreateElement();

            //更新元素
            UpdateClipArea();
            
            //数据绑定
            BindProperties();
            
            //UI事件
            InitEvent();
            InitDragEvent();
            
            //子类初始化
            OnInit();

            //绘制默认元素
            DrawDefaultElement();
        }

        #endregion

        #region Clear

        public override void OnClear()
        {
            UnBindProperties();
            OnClear();
        }

        #endregion

        #region UI元素

        private void CreateElement()
        {
            RootElement = new VisualElement();
            RootElement.name = "clip";
            RootElement.style.width = 100;
            RootElement.style.height = InternalTrack_Element.TrackHeight;
            RootElement.BorderWidthColor(1,Color.black);
            RootElement.style.flexShrink = 0;
            RootElement.style.flexDirection = FlexDirection.Row;
            RootElement.style.backgroundColor = Color.grey;
            RootElement.style.position = Position.Absolute;

            leftDragElment = new VisualElement();
            leftDragElment.name = "leftDrag";
            leftDragElment.style.width = MinWidth/2-5;
            leftDragElment.style.height = InternalTrack_Element.TrackHeight;
            leftDragElment.style.position = Position.Absolute;
            leftDragElment.style.left = 0;
            leftDragElment.SetCursor(MouseCursor.SplitResizeLeftRight);
            leftDragManipulator = new DragManipulator(leftDragElment);
            RootElement.Add(leftDragElment);
            
            middleDragElment = new VisualElement();
            middleDragElment.name = "middleDrag";
            middleDragElment.style.height = InternalTrack_Element.TrackHeight;
            middleDragElment.style.width = new StyleLength(StyleKeyword.Auto);
            middleDragElment.style.position = Position.Absolute;
            middleDragElment.style.left = MinWidth/2-5;
            middleDragElment.style.right = MinWidth/2-5;
            middleDragManipulator = new DragManipulator(middleDragElment);
            RootElement.Add(middleDragElment);
            
            rightDragElment = new VisualElement();
            rightDragElment.name = "rightDrag";
            rightDragElment.style.width = MinWidth/2-5;
            rightDragElment.style.height = InternalTrack_Element.TrackHeight;
            rightDragElment.style.position = Position.Absolute;
            rightDragElment.style.right = 0;
            rightDragElment.SetCursor(MouseCursor.SplitResizeLeftRight);
            rightDragManipulator = new DragManipulator(rightDragElment);
            RootElement.Add(rightDragElment);

            InitDragEvent();
        }
        
        #endregion
        
        #region 数据监听

        private void BindProperties()
        {
            window.OnCurrFrameChange += OnCurrFrameChange;
        }
        
        private void UnBindProperties()
        {
            window.OnCurrFrameChange -= OnCurrFrameChange;
        }

        #endregion

        #region 数据改变

        private void OnClipStartFrameChange(int newValue)
        {
            if (newValue == Model.startFrame)
                return;
            FixClipPosSize();
        }
        
        private void OnClipEndFrameChange(int newValue)
        {
            if (newValue == Model.endFrame)
                return;
            FixClipPosSize();
        }

        private void OnTitleNameChange(string titleName)
        {
            if (defaultTitleLable != null)
            {
                defaultTitleLable.text = titleName;
            }
        }

        private void OnCurrFrameChange()
        {
            int currFrame = window.CurrFrameCnt;
            if (currFrame < Model.startFrame || currFrame > Model.endFrame)
            {
                OnExitPlaying();
                return;
            }

            float playTime = (currFrame - Model.startFrame) * SkillTimelineWindow.FrameDeltaTime;
            OnPlaying(playTime);
        }
        
        #endregion

        #region UI元素事件

        private void InitEvent()
        {
            RootElement.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));
            RootElement.RegisterCallback<PointerDownEvent>(OnClickClipHandler);
        }
        
        private void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("删除",(action) =>{
                window.CommandDispatcher.Do(new RemoveClipCommand(cliplistElement.trackElement,Model));

                //cliplistElement.RemoveClip(this);
            });
        }
        
        private void InitDragEvent()
        {
            leftDragManipulator.OnDragStart = OnLeftDragStart;
            leftDragManipulator.OnDraging = OnLeftDraging;
            leftDragManipulator.OnDragEnd = OnLeftDragEnd;
            
            middleDragManipulator.OnDragStart = OnMiddleDragStart;
            middleDragManipulator.OnDraging = OnMiddleDraging;
            middleDragManipulator.OnDragEnd = OnMiddleDragEnd;
            
            rightDragManipulator.OnDragStart = OnRightDragStart;
            rightDragManipulator.OnDraging = OnRightDraging;
            rightDragManipulator.OnDragEnd = OnRightDragEnd;
        }

        #region 点击

        private void OnClickClipHandler(PointerDownEvent evt)
        {
            LCToolkit.InspectorExtension.DrawObjectInInspector(Model);
            evt.StopImmediatePropagation();
        }

        #endregion
        
        #region 左右拖拽

        private void OnLeftDragStart(IPointerEvent obj)
        {
            frameWidth = window.FrameArrow.GetFrameWidth();
        }
        
        private void OnLeftDraging(IPointerEvent obj)
        {
            float xDelta = obj.deltaPosition.x;
            Drag(xDelta, -xDelta);
        }

        private void OnLeftDragEnd(IPointerEvent obj)
        {
            FixClipPosSize();
        }
        
        private void OnRightDragStart(IPointerEvent obj)
        {
            frameWidth = window.FrameArrow.GetFrameWidth();
        }
        
        private void OnRightDraging(IPointerEvent obj)
        {
            float xDelta = obj.deltaPosition.x;
            Drag(0, xDelta);
        }

        private void OnRightDragEnd(IPointerEvent obj)
        {
            FixClipPosSize();
        }
        
        #endregion

        #region 中间拖动

        private void OnMiddleDragStart(IPointerEvent obj)
        {
            frameWidth = window.FrameArrow.GetFrameWidth();
        }
        
        private void OnMiddleDraging(IPointerEvent obj)
        {
            float xDelta = obj.deltaPosition.x;
            Drag(xDelta, 0);
        }

        private void OnMiddleDragEnd(IPointerEvent obj)
        {
            FixClipPosSize();
        }

        #endregion
        
        #endregion

        public override void OnFocuseWindow()
        {
            FixClipPosSize();
        }

        private void Drag(float xDelta, float xDeltaWidth)
        {
            Vector2 pos = RootElement.transform.position;
            float newPosX = pos.x += xDelta;
            newPosX = Mathf.Clamp(newPosX,0, frameWidth);
            pos.x = newPosX;
            RootElement.transform.position = pos;
            
            StyleLength length = RootElement.style.width;
            float newWidth = length.value.value + xDeltaWidth;
            if (newWidth <= Frame_Element.FrameWidth)
                newWidth = Frame_Element.FrameWidth;
            RootElement.style.width = newWidth;
        }

        private void FixClipPosSize()
        {
            Vector2 pos = RootElement.transform.position;
            float width = RootElement.style.width.value.value;

            //起始位置对齐
            int startFrame = window.CalcFrameByPosX(pos.x);

            //宽度对齐
            int addFrame = window.CalcFrameByPosX(width);

            //更新数据
            Model.startFrame = startFrame;
            Model.endFrame   = startFrame + (addFrame - 1);
            
            UpdateClipArea();
            
            cliplistElement.UpdateFrame();
            
            ObjectInspector.Repaint();
        }

        private void UpdateClipArea()
        {
            Vector2 pos = RootElement.transform.position;
            float width = RootElement.style.width.value.value;
            
            //起始位置
            pos.x = Model.startFrame * Frame_Element.FrameWidth;
            RootElement.transform.position = pos;
            
            //结束位置
            float newWidth = Model.DurationFrame * Frame_Element.FrameWidth;
            RootElement.style.width = newWidth;
        }

        protected virtual void OnInit()
        {
            
        }

        protected virtual void OnPlaying(float pPlayTime)
        {
            
        }
        
        protected virtual void OnExitPlaying()
        {
            
        }

        /// <summary>
        /// 绘制默认元素
        /// </summary>
        private Label defaultTitleLable;
        protected virtual void DrawDefaultElement()
        {
            Label titleLable = new Label();
            titleLable.pickingMode = PickingMode.Ignore;
            titleLable.text = Model.name;
            titleLable.style.flexGrow = 1;
            titleLable.style.color = UnityEngine.Color.black;
            titleLable.style.right = 1;
            titleLable.style.left = 1;
            titleLable.style.overflow = Overflow.Hidden;
            titleLable.style.unityTextAlign = TextAnchor.MiddleLeft;
            titleLable.style.unityFontStyleAndWeight = FontStyle.Bold;

            defaultTitleLable = titleLable;
            RootElement.Add(titleLable);
        }
        
    }
}