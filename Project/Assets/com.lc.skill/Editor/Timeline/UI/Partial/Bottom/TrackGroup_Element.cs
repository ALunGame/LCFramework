using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Timeline
{
    /// <summary>
    /// 轨道组
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class TimlineGroupElementAttribute : Attribute
    {
        /// <summary>
        /// 指定数据类型
        /// </summary>
        public Type targetModelType;

        /// <summary>
        /// 创建菜单名
        /// </summary>
        public string menuName;
    
        public TimlineGroupElementAttribute(Type targetModelType, string menuName)
        {
            this.targetModelType = targetModelType;
            this.menuName = menuName;
        }
    }
    
    public abstract class InternalTrackGroup_Element : PartialView
    {
        public const int TrackGroupWidth = 200;
        public const int TrackGroupHeight = 35;
        
        public BottomPartialView BottomView { get; protected set; }
        
        public VisualElement RootElement { get; protected set; }
        
        public VisualElement TrackAreaElement  { get; protected set; }
        
        public VisualElement ClipAreaElement  { get; protected set; }
        
        public ScrollView TrackScroll { get; protected set; }
        
        public ScrollView ClipAreaScroll { get; protected set; }
        
        public abstract BaseTrackGroup OwerModel { get;}
        
        public abstract void SetUp(BaseTrackGroup pModel, BottomPartialView pBottomView);

        public abstract void AddTrack(BaseTrack pTrack);

        public abstract void RemoveTrack(InternalTrack_Element pTrackElement);
        
        public abstract void RemoveTrack(BaseTrack pTrack);

        public abstract int GetEndFrame();
    }
    
    /// <summary>
    /// 轨道组
    /// </summary>
    public abstract class TrackGroup_Element<T> : InternalTrackGroup_Element where T : BaseTrackGroup
    {
        public override BaseTrackGroup OwerModel { get => Model; }

        protected BottomPartialView bottomPartialView;
        public T Model { get; protected set; }

        protected List<InternalTrack_Element> tracklist = new List<InternalTrack_Element>();

        /// <summary>
        /// 组名
        /// </summary>
        public abstract string GroupName { get; }
        
        /// <summary>
        /// 组名提示名
        /// </summary>
        public abstract string GroupToolTipName { get; }

        #region Init

        public override void SetUp(BaseTrackGroup pModel, BottomPartialView pBottomView)
        {
            //数据
            Model          = (T)pModel;

            //UI元素
            BottomView     = pBottomView; 
            RootElement    = pBottomView.RootElement;
            TrackScroll    = RootElement.Q<ScrollView>("Tracklist");
            ClipAreaScroll = RootElement.Q<ScrollView>("ClipScrollArea");
            CreateElement();
            
            //数据绑定
            BindProperties();

            //UI事件
            InitUIEvent();
            
            //轨道
            for (int i = 0; i < Model.tracks.Count; i++)
            {
                CreateTrackElement(Model.tracks[i]);
            }
            
            //子类初始化
            OnInit();
        }

        #endregion


        #region Clear

        public override void OnClear()
        {
            UnBindProperties();
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
            //轨道区域
            TrackAreaElement = new VisualElement();
            TrackAreaElement.name = "trackGroupArea";
            TrackAreaElement.tooltip = GroupToolTipName;
            TrackAreaElement.style.width = TrackGroupWidth;
            TrackAreaElement.style.height = TrackGroupHeight;
            TrackAreaElement.style.borderBottomColor = new StyleColor(UnityEngine.Color.black);
            TrackAreaElement.style.borderBottomWidth = 1;
            TrackAreaElement.style.borderRightColor = new StyleColor(UnityEngine.Color.black);
            TrackAreaElement.style.borderRightWidth = 1;
            TrackAreaElement.style.alignItems = Align.Center;
            TrackAreaElement.style.flexShrink = 0;
            TrackAreaElement.style.backgroundColor = new StyleColor(UnityEngine.Color.gray);
            TrackAreaElement.style.flexDirection = FlexDirection.Row;
            
            Label label = new Label(GroupName);
            label.name = "trackName";
            label.style.flexGrow = 1;
            label.style.marginTop = 2;
            label.style.marginBottom = 2;
            label.style.color = new StyleColor(UnityEngine.Color.black);
            label.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            label.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
            label.style.fontSize = 20;
            TrackAreaElement.Add(label);

            Button addTrackBtn = new Button(() =>
            {
                window.CommandDispatcher.Do(new AddTrackCommand(this,CreateNewTrack()));
                //AddTrack(CreateNewTrack());
            });
            addTrackBtn.text = "+";
            addTrackBtn.style.position = Position.Absolute;
            addTrackBtn.style.top = 0;
            addTrackBtn.style.bottom = 0;
            addTrackBtn.style.right = 0;
            TrackAreaElement.Add(addTrackBtn);
            
            //片段区域
            ClipAreaElement = new VisualElement();
            ClipAreaElement.name = "clipGroupArea";
            ClipAreaElement.style.height = TrackGroupHeight;
            ClipAreaElement.style.marginRight = 0;
            ClipAreaElement.style.borderBottomColor = new StyleColor(UnityEngine.Color.black);
            ClipAreaElement.style.borderBottomWidth = 1;
            ClipAreaElement.style.flexDirection = FlexDirection.Row;
            ClipAreaElement.style.backgroundColor = new StyleColor(UnityEngine.Color.gray);
            
            TrackScroll.Add(TrackAreaElement);
            ClipAreaScroll.Add(ClipAreaElement);
        }

        #endregion

        #region UI事件

        private void InitUIEvent()
        {
            TrackAreaElement.RegisterCallback<PointerDownEvent>(OnClickClipHandler);
            ClipAreaElement.RegisterCallback<PointerDownEvent>(OnClickClipHandler);
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
        
        protected abstract BaseTrack CreateNewTrack();

        public override void AddTrack(BaseTrack pTrack)
        {
            if (pTrack == null)
            {
                return;
            }
            Model.tracks.Add(pTrack);
            CreateTrackElement(pTrack);
            
            BottomView.RefreshTracks();
        }
        
        public override void RemoveTrack(BaseTrack pTrack)
        {
            InternalTrack_Element removeElement = null;
            for (int i = 0; i < tracklist.Count; i++)
            {
                if (tracklist[i].OwerModel.Equals(pTrack))
                {
                    removeElement = tracklist[i];
                    break;
                }
            }

            RemoveTrack(removeElement);
        }

        public override void RemoveTrack(InternalTrack_Element pTrackElement)
        {
            if (pTrackElement == null)
            {
                return;
            }
            if (!tracklist.Contains(pTrackElement))
            {
                return;
            }

            tracklist.Remove(pTrackElement);
            TrackScroll.Remove(pTrackElement.RootElement);
            
            ClipAreaScroll.Remove(pTrackElement.ClipListElement.RootElement);
            
            Model.tracks.Remove(pTrackElement.OwerModel);
            pTrackElement.Clear();
            
            BottomView.RefreshTracks();
        }

        private void CreateTrackElement(BaseTrack pTrack)
        {
            InternalTrack_Element element = (InternalTrack_Element)PartialView.Create(window,TimelineUtil.GetTrackElementType(pTrack));
            element.SetUp(pTrack,this);
            
            int startIndex = TrackScroll.IndexOf(TrackAreaElement);
            if (startIndex > TrackScroll.childCount)
            {
                TrackScroll.Add(element.RootElement);
            }
            else
            {
                TrackScroll.Insert(startIndex + 1,element.RootElement);
            }
            
            if (startIndex > TrackScroll.childCount)
            {
                ClipAreaScroll.Add(element.ClipListElement.RootElement);
            }
            else
            {
                ClipAreaScroll.Insert(startIndex + 1,element.ClipListElement.RootElement);
            }
            
            tracklist.Add(element);
        }
        
        public override int GetEndFrame()
        {
            int endFrame = 0;
            foreach (InternalTrack_Element trackElement in tracklist)
            {
                if (trackElement.EndFrame > endFrame)
                {
                    endFrame = trackElement.EndFrame;
                }
            }

            return endFrame;
        }
    }
}