using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Timeline
{
    /// <summary>
    /// 轨道
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class TimlineTrackElementAttribute : Attribute
    {
        /// <summary>
        /// 指定数据类型
        /// </summary>
        public Type targetModelType;

        public TimlineTrackElementAttribute(Type targetModelType)
        {
            this.targetModelType = targetModelType;
        }
    }

    public abstract class InternalTrack_Element : PartialView
    {
        public const int TrackWidth = 200;
        public const int TrackHeight = 30;
        
        public VisualElement RootElement { get; protected set; }
        
        public abstract ClipList_Element ClipListElement { get; }
        
        public abstract BaseTrack OwerModel { get;}
        
        public abstract int StartFrame { get;}
        
        public abstract int EndFrame { get;}
        
        public abstract void SetUp(BaseTrack pModel,InternalTrackGroup_Element pTrackGroupElement);
        
        public abstract void AddClip(BaseClip pClip);

        public abstract void RemoveClip(InternalClip_Element clipElement);
        
        public abstract void RemoveClip(BaseClip pClip);
    }
    
    public abstract class Track_Element<T> : InternalTrack_Element where T : BaseTrack
    {
        public const int TrackWidth = 200;
        public const int TrackHeight = 30;
        
        public override BaseTrack OwerModel { get => Model; }
        public T Model { get; protected set; }

        protected TextField trackNameField;
        protected InternalTrackGroup_Element trackGroupElement;
        protected ClipList_Element clipListElement;
        
        public override ClipList_Element ClipListElement
        {
            get => clipListElement;
        }
        
        public override int StartFrame
        {
            get
            {
                return clipListElement.StartFrame;
            }
        }
        
        public override int EndFrame
        {
            get
            {
                return clipListElement.EndFrame;
            }
        }

        #region Init
        
        public override void SetUp(BaseTrack pModel,InternalTrackGroup_Element pTrackGroupElement)
        {
            //数据
            Model = (T)pModel;
            trackGroupElement = pTrackGroupElement;
            
            //创建UI元素
            CreateElement();
            
            //数据绑定
            BindProperties();

            //UI事件
            InitUIEvent();
            
            //创建片段
            for (int i = 0; i < Model.clips.Count; i++)
            {
                clipListElement.CreateClipElement(Model.clips[i]);
            }
            
            //子类初始化
            OnInit();
        }
        
        #endregion

        #region Clear

        public override void OnClear()
        {
            UnBindProperties();
            clipListElement.Clear();
        }

        #endregion

        #region 数据监听

        private void BindProperties()
        {
            
        }
        
        private void UnBindProperties()
        {
            
        }

        #endregion

        #region 数据改变
        
        #endregion

        #region UI元素

        private void CreateElement()
        {
            //根节点
            RootElement = new VisualElement();
            RootElement.name = "trackBox";
            RootElement.style.width = TrackWidth;
            RootElement.style.height = TrackHeight;
            RootElement.style.borderBottomColor = new StyleColor(Color.black);
            RootElement.style.borderBottomWidth = 1;
            RootElement.style.borderRightColor = new StyleColor(Color.black);
            RootElement.style.borderRightWidth = 1;
            RootElement.style.alignItems = Align.Center;
            RootElement.style.flexShrink = 0;
            RootElement.style.flexDirection = FlexDirection.Row;
            
            //空白
            VisualElement empty = new VisualElement();
            empty.style.position = Position.Absolute;
            empty.style.top = 0;
            empty.style.bottom = 0;
            empty.style.left = 0;
            empty.style.width = 20;
            empty.style.height = TrackHeight;
            RootElement.Add(empty);
            
            //名字
            trackNameField = new TextField();
            trackNameField.style.width = TrackWidth - 20;
            trackNameField.style.position = Position.Absolute;
            trackNameField.style.top = 2;
            trackNameField.style.bottom = 2;
            trackNameField.style.right = 0;
            trackNameField.name = "trackName";
            trackNameField.value = Model.trackName;
            trackNameField.RegisterCallback<ChangeEvent<string>>((evt) =>
            {
                Model.trackName = evt.newValue;
            });
            RootElement.Add(trackNameField);
            
            //添加到父节点
            trackGroupElement.TrackScroll.Add(RootElement);
            
            //片段
            clipListElement = PartialView.Create<ClipList_Element>(window);
            clipListElement.SetUp(this);
        }

        #endregion

        #region UI事件

        private void InitUIEvent()
        {
            clipListElement.RootElement.AddManipulator(new ContextualMenuManipulator(BuildContextualMenu));
            RootElement.RegisterCallback<PointerDownEvent>(OnClickClipHandler);
        }
        
        private void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction($"创建{CreateClipMenuName}",(action) =>{
                BaseClip newClip = CreateNewClip();
                newClip.startFrame = window.CalcFrameByPosX(action.eventInfo.mousePosition.x - InternalTrack_Element.TrackWidth);
                newClip.endFrame = newClip.startFrame + 1;
                
                window.CommandDispatcher.Do(new AddClipCommand(this,newClip));
                //AddClip(newClip);
            });
            
            evt.menu.AppendAction("删除整个轨道",(action) =>
            {
                window.CommandDispatcher.Do(new RemoveTrackCommand(trackGroupElement,Model));
                //trackGroupElement.RemoveTrack(this);
            });
        }
        
        private void OnClickClipHandler(PointerDownEvent evt)
        {
            LCToolkit.InspectorExtension.DrawObjectInInspector(Model);
            evt.StopImmediatePropagation();
        }
        
        #endregion
        
        protected virtual void OnInit()
        {
            
        }

        public override void OnFocuseWindow()
        {
            trackNameField.value = Model.trackName;
        }

        protected abstract string CreateClipMenuName { get; }
        protected abstract BaseClip CreateNewClip();

        public override void AddClip(BaseClip pClip)
        {
            Model.clips.Add(pClip);
            clipListElement.CreateClipElement(pClip);
            clipListElement.UpdateFrame();
        }

        public override void RemoveClip(BaseClip pClip)
        {
            RemoveClip(clipListElement.GetClipElemnet(pClip));
        }

        public override void RemoveClip(InternalClip_Element clipElement)
        {
            if (clipElement == null)
            {
                return;
            }
            Model.clips.Remove(clipElement.OwerModel);
            clipListElement.RemoveClipElement(clipElement);
            clipListElement.UpdateFrame();
        }
        
    }
}