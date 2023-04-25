using LCSkill.Timeline;
using LCToolkit.Core;
using LCToolkit.Element;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Common
{
    [TimlineGroupElement(typeof(BreakTrackGroup),"技能生命周期")]
    public class BreakTrackGroup_Element : TrackGroup_Element<BreakTrackGroup>
    {
        public const int ArrowWidth = 10;
        
        public override string GroupName { get => "技能生命周期"; }
        public override string GroupToolTipName { get => "技能生命周期"; }

        private Texture2D arrowTex;
        private float frameWidth;
        
        private VisualElement leftArrowElement;
        private DragManipulator leftArrowDragManipulator;
        
        private VisualElement rightArrowElement;
        private DragManipulator rightArrowDragManipulator;
        
        private VisualElement lifeRangeElement;
        
        protected override BaseTrack CreateNewTrack()
        {
            return null;
        }

        protected override void OnInit()
        {
            arrowTex = AssetDatabase.LoadAssetAtPath<Texture2D>(@"Assets/com.lc.skill/Editor/Texture/Timecursor.png");
            
            lifeRangeElement = new VisualElement();
            lifeRangeElement.name = "life";
            lifeRangeElement.tooltip = "绿色区域之外,表示技能可以被打断";
            lifeRangeElement.style.flexShrink = 0;
            lifeRangeElement.style.height = InternalTrackGroup_Element.TrackGroupHeight;
            lifeRangeElement.style.width = Frame_Element.FrameWidth;
            lifeRangeElement.style.backgroundColor = new Color(0,1,0,70.0f/255.0f);
            lifeRangeElement.style.position = Position.Absolute;
            ClipAreaElement.Add(lifeRangeElement);
            
            leftArrowElement = new VisualElement();
            leftArrowElement.name = "leftArrow";
            leftArrowElement.style.flexShrink = 0;
            leftArrowElement.style.height = InternalTrackGroup_Element.TrackGroupHeight;
            leftArrowElement.style.width = ArrowWidth;
            leftArrowElement.style.backgroundImage = arrowTex;
            leftArrowElement.style.unityBackgroundImageTintColor = Color.red;
            leftArrowElement.style.position = Position.Absolute;
            leftArrowDragManipulator = new DragManipulator(leftArrowElement);

            ClipAreaElement.Add(leftArrowElement);
            
            rightArrowElement = new VisualElement();
            rightArrowElement.name = "rightArrow";
            rightArrowElement.style.flexShrink = 0;
            rightArrowElement.style.height = InternalTrackGroup_Element.TrackGroupHeight;
            rightArrowElement.style.width = ArrowWidth;
            rightArrowElement.style.backgroundImage = arrowTex;
            rightArrowElement.style.unityBackgroundImageTintColor = Color.red;
            rightArrowElement.style.position = Position.Absolute;
            rightArrowDragManipulator = new DragManipulator(rightArrowElement);
            
            ClipAreaElement.Add(rightArrowElement);

            InitArrowDragEvent();
            
            UpdateLifeRangeArea();
        }

        #region 拖拽事件

        private void InitArrowDragEvent()
        {
            leftArrowDragManipulator.OnDragStart = OnLeftDragStart;
            leftArrowDragManipulator.OnDraging = OnLeftDraging;
            leftArrowDragManipulator.OnDragEnd = OnLeftDragEnd;
            
            rightArrowDragManipulator.OnDragStart = OnRightDragStart;
            rightArrowDragManipulator.OnDraging = OnRightDraging;
            rightArrowDragManipulator.OnDragEnd = OnRightDragEnd;
        }
        
        private void OnLeftDragStart(IPointerEvent obj)
        {
            frameWidth = window.FrameArrow.GetFrameWidth();
        }
        
        private void OnLeftDraging(IPointerEvent obj)
        {
            float xDelta = obj.deltaPosition.x;
            Drag(xDelta, leftArrowElement);
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
            Drag(xDelta, rightArrowElement);
        }

        private void OnRightDragEnd(IPointerEvent obj)
        {
            FixClipPosSize();
        }

        #endregion

        public override void OnFocuseWindow()
        {
            UpdateLifeRangeArea();
        }

        private void Drag(float xDelta, VisualElement pArrowElement)
        {
            Vector2 pos = pArrowElement.transform.position;
            float newPosX = pos.x += xDelta;
            newPosX = Mathf.Clamp(newPosX,0, frameWidth);
            pos.x = newPosX;
            pArrowElement.transform.position = pos;
        }

        private void FixClipPosSize()
        {
            Vector2 pos = leftArrowElement.transform.position;
            //起始
            int startFrame = window.CalcFrameByPosX(pos.x);

            pos = rightArrowElement.transform.position;
            
            //结束
            int endFrame = window.CalcFrameByPosX(pos.x);

            //更新数据
            if (endFrame<startFrame)
            {
                endFrame = startFrame;
            }

            Model.startFrame = Mathf.Clamp(startFrame,0,window.TotalFrameCnt);
            Model.endFrame = Mathf.Clamp(endFrame,0,window.TotalFrameCnt);;

            UpdateLifeRangeArea();
            
            ObjectInspector.Repaint();
        }

        private void UpdateLifeRangeArea()
        {
            Vector2 pos = leftArrowElement.transform.position;
            
            //起始位置
            pos.x = Model.startFrame * Frame_Element.FrameWidth;
            leftArrowElement.transform.position = pos;
            
            //结束位置
            pos = rightArrowElement.transform.position;
            pos.x = Model.endFrame * Frame_Element.FrameWidth + (Frame_Element.FrameWidth - ArrowWidth);
            rightArrowElement.transform.position = pos;

            //生命周期
            pos = lifeRangeElement.transform.position;
            pos.x = Model.startFrame * Frame_Element.FrameWidth;
            lifeRangeElement.transform.position = pos;
            lifeRangeElement.style.width = Model.DurationFrame * Frame_Element.FrameWidth;
        }
        
    }
}