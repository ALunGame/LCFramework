using System.Reflection;
using LCToolkit.Element;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using System.Reflection;
using System.Reflection.Emit;

namespace LCSkill.Timeline
{
    /// <summary>
    /// 帧箭头
    /// </summary>
    public class Frame_Arrow : PartialView
    {
        public const float ARROW_WIDTH = 6f;
        public const float ARROW_Height = ARROW_WIDTH * 2f * 1.82f;
        public const float FrameStartPosX = 201;
        
        private VisualElement framelistElement;
        private VisualElement framelistContainerElement;

        private Rect arrowRect;
        private DragManipulator dragManipulator;

        private bool isDraging;
        private Vector3 dragPos;
        private Texture2D arrowTex;

        public void Init(VisualElement pFramelistElement)
        {
            this.framelistElement = pFramelistElement;
            this.framelistContainerElement = pFramelistElement.Q("unity-content-container");

            InitEvent();
            
            arrowTex = AssetDatabase.LoadAssetAtPath<Texture2D>(@"Assets/com.lc.skill/Editor/Texture/Timecursor.png");
        }

        private void InitEvent()
        {
            dragManipulator = new DragManipulator(framelistElement);
            dragManipulator.OnDragStart = OnDragStart;
            dragManipulator.OnDraging = OnDraging;
            dragManipulator.OnDragEnd = OnDragEnd;

            //禁止滚轮
            framelistContainerElement.RegisterCallback<WheelEvent>(new EventCallback<WheelEvent>(this.OnScrollWheel));
        }

        private void OnScrollWheel(WheelEvent evt)
        {
            evt.StopImmediatePropagation();
        }

        private void OnDragStart(IPointerEvent obj)
        {
            int newFrame = CalcFrameByPosX(obj.position.x);
            window.SetCurrFrame(newFrame);
        }
        
        private void OnDraging(IPointerEvent obj)
        {
            int newFrame = CalcFrameByPosX(obj.position.x);
            window.SetCurrFrame(newFrame);
        }

        private void OnDragEnd(IPointerEvent obj)
        {
            int newFrame = CalcFrameByPosX(obj.position.x);
            window.SetCurrFrame(newFrame);
        }

        public void Draw()
        {
            float framePosX = CalcFramePosX();

            Color cl01 = GUI.color;
            GUI.color = Color.red;
            float textY = framelistElement.worldBound.position.y;
            arrowRect = new Rect(framePosX - ARROW_WIDTH / 2, textY, ARROW_WIDTH * 2f, ARROW_Height);
            GUI.DrawTexture(arrowRect, arrowTex);
            
            GUI.color = cl01;
            Rect lineRect = new Rect(framePosX, textY, 1, window.rootVisualElement.worldBound.size.y);
            EditorGUI.DrawRect(lineRect, Color.red);
        }
        
        private float CalcFramePosX()
        {
            float scrollPosX = GetScrollPosX();
            int startFrameCnt = CalcScrollStartFrame();
            float offsetPix = scrollPosX % Frame_Element.FrameWidth;

            if (window.CurrFrameCnt < startFrameCnt)
            {
                return FrameStartPosX;
            }
            else
            {
                float addPosX = (window.CurrFrameCnt - startFrameCnt) * Frame_Element.FrameWidth + Frame_Element.FrameWidth/2;
                float resPosX = addPosX + FrameStartPosX - offsetPix;
                if (resPosX <= FrameStartPosX)
                {
                    return FrameStartPosX;
                }
                return resPosX;
            }
        }

        private int CalcScrollStartFrame()
        {
            float scrollPosX = framelistContainerElement.worldBound.position.x - FrameStartPosX;
            scrollPosX = -scrollPosX;
            int startFrameCnt = (int)(scrollPosX / Frame_Element.FrameWidth);
            return startFrameCnt;
        }

        private float GetScrollPosX()
        {
            float scrollPosX = framelistContainerElement.worldBound.position.x - FrameStartPosX;
            scrollPosX = -scrollPosX;
            return scrollPosX;
        }

        public float GetFrameWidth()
        {
            return framelistContainerElement.worldBound.width;
        }

        public float GetFrameStartPosX()
        {
            return framelistContainerElement.worldBound.position.x;
        }
        
        public int CalcFrameByPosX(float pPosX)
        {
            float scrollPosX = GetScrollPosX();
            int startFrame = CalcScrollStartFrame();
            float offsetPix = scrollPosX % Frame_Element.FrameWidth;

            float desX = pPosX - FrameStartPosX + offsetPix;
            int frame = (int)(desX / Frame_Element.FrameWidth);
            frame = frame <= 0 ? 0 : frame;
            return frame;
        }
    }
}