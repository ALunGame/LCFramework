using LCJson;
using LCToolkit;
using UnityEditor;
using UnityEngine;

namespace LCTimeline.View
{
    public enum ClipDragMode
    { None, Drag, Left, Right }

    /// <summary>
    /// 片段视图（一个轨道包含多个片段）
    /// </summary>
    public class BaseClipView : BaseView
    {
        #region 字段

        public ClipModel Clip;

        #endregion

        #region Display

        public Color UnSelectColor = Color.white;

        public Color SelectColor = Color.green;

        public Color DisplayNameColor = Color.black;

        #endregion Display

        private ClipDragMode dragMode;

        public ClipDragMode DragMode
        {
            get { return dragMode; }
            set { dragMode = value; }
        }

        public BaseTrackView Track;

        public Rect ShowRect;
        public Rect LeftRect;
        public Rect RightRect;

        public bool IsSelected = false;

        public override void OnInit()
        {
            if (AttributeHelper.TryGetTypeAttribute(Clip.GetType(),out ClipColorAttribute attr))
            {
                UnSelectColor = attr.UnSelectColor;
                SelectColor = attr.SelectColor;
                DisplayNameColor = attr.DisplayNameColor;
            }
        }

        public override void OnDraw()
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();
            Rect timeAreaRect = timeAreaView.TimeContent;

            //显示区域
            ShowRect = Track.TrackViewRect;
            ShowRect.x = timeAreaView.TimeToPixel(Clip.StartTime);
            float y = timeAreaView.TimeToPixel(Clip.EndTime);
            ShowRect.x = Mathf.Max(ShowRect.x, timeAreaRect.x);
            y = Mathf.Min(y, timeAreaRect.xMax);
            ShowRect.width = y - ShowRect.x;
            ShowRect.height = ShowRect.height - 2;
            if (ShowRect.width < 0) ShowRect.width = 0;

            //显示
            EditorGUI.DrawRect(ShowRect, IsSelected ? SelectColor : UnSelectColor);

            //左右侧拖拽
            Rect left = ShowRect;
            left.x = ShowRect.x - Mathf.Min(10, ShowRect.width / 4);
            left.x = Mathf.Max(left.x, timeAreaRect.x);
            left.width = Mathf.Min(20, ShowRect.width / 2);
            EditorGUIUtility.AddCursorRect(left, MouseCursor.SplitResizeLeftRight);
            LeftRect = left;
            //EditorGUI.DrawRect(LeftRect, Color.green);

            Rect right = left;
            right.x = ShowRect.x + ShowRect.width - Mathf.Min(10, ShowRect.width / 4);
            right.x = Mathf.Max(right.x, timeAreaRect.x);
            EditorGUIUtility.AddCursorRect(right, MouseCursor.SplitResizeLeftRight);
            RightRect = right;
            //EditorGUI.DrawRect(RightRect, Color.blue);

            GUILayout.BeginArea(ShowRect);
            OnDrawClip();
            GUILayout.EndArea();

            MiscHelper.DrawOutline(ShowRect, 1, Color.black);
        }

        public override void OnHandleEvent(Event evt)
        {
            Vector2 mousePos = evt.mousePosition;
            switch (evt.type)
            {
                case EventType.MouseDown:
                    IsSelected = false;
                    if (LeftRect.Contains(mousePos))
                    {
                        dragMode = ClipDragMode.Left;
                    }
                    else if (RightRect.Contains(mousePos))
                    {
                        dragMode = ClipDragMode.Right;
                    }
                    else if (ShowRect.Contains(mousePos))
                    {
                        dragMode = ClipDragMode.Drag;
                    }
                    else
                    {
                        dragMode = ClipDragMode.None;
                    }
                    if (dragMode != ClipDragMode.None)
                    {
                        IsSelected = true;
                        OnSelect();
                        if (Event.current.button == 1)
                        {
                            GenRightClickMenu();
                        }
                    }
                    break;

                case EventType.MouseUp:
                    dragMode = ClipDragMode.None;
                    break;

                case EventType.MouseDrag:
                case EventType.ScrollWheel:
                    HandleDrag(evt);
                    break;

                default:
                    break;
            }
        }

        public override void OnRunningTimeChange(double runningTime)
        {
            clipRunningTime = runningTime - Clip.StartTime;
            clipLeftTime = Clip.EndTime - runningTime;

            //结束判断
            if (runningTime < Clip.StartTime || runningTime > Clip.EndTime)
            {
                EndPlay();
                return;
            }

            //开始判断
            if (runningTime >= Clip.StartTime)
            {
                StartPlay();
            }

            //正在判断
            if (runningTime >= Clip.StartTime && runningTime <= Clip.EndTime)
            {
                Playing(clipRunningTime);
            }
        }

        #region 播放

        private bool isStart = false;
        protected double clipRunningTime;
        protected double clipLeftTime;

        private void StartPlay()
        {
            if (!isStart)
            {
                isStart = true;
                Debug.Log("StartPlay");
                OnStartPlay();
            }
        }

        private void Playing(double runningTime)
        {
            if (isStart)
            {
                Debug.Log("Playing");
                OnPlaying(runningTime);
            }
        }

        private void EndPlay()
        {
            Debug.Log("EndPlay");
            isStart = false;
            OnEndPlay();
        }

        /// <summary>
        /// 开始播放
        /// </summary>
        public virtual void OnStartPlay()
        {
        }

        /// <summary>
        /// 正在播放
        /// </summary>
        public virtual void OnPlaying(double runningTime)
        {
        }

        /// <summary>
        /// 结束
        /// </summary>
        public virtual void OnEndPlay()
        {
        }
        #endregion

        #region 拖拽处理

        private void HandleDrag(Event e)
        {
            if (dragMode == ClipDragMode.Left)
            {
                DragStart(e);
            }
            else if (dragMode == ClipDragMode.Right)
            {
                DragEnd(e);
            }
            else if (dragMode == ClipDragMode.Drag)
            {
                Draging(e);
            }
            SyncClipData();
        }

        private void DragStart(Event e)
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();

            ShowRect.x = timeAreaView.TimeToPixel(Clip.StartTime);
            ShowRect.x += e.delta.x;

            var start2 = timeAreaView.PiexlToTime(ShowRect.x);
            if (start2 >= 0 && start2 <= Clip.EndTime)
            {
                Clip.DurationTime -= (start2 - Clip.StartTime);
                Clip.StartTime = Mathf.Max(0, start2);
                e.Use();
            }

            OnDragStart();
        }

        private void DragEnd(Event e)
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();

            ShowRect.x = timeAreaView.TimeToPixel(Clip.EndTime);
            ShowRect.x += e.delta.x;

            var end = timeAreaView.PiexlToTime(ShowRect.x);
            if (end > Clip.StartTime)
            {
                Clip.DurationTime += (end - Clip.EndTime);
                e.Use();
            }

            OnDragEnd();
        }

        private void Draging(Event e)
        {
            TimeAreaView timeAreaView = window.GetPartialView<TimeAreaView>();
            ShowRect.x += e.delta.x;
            Clip.StartTime = timeAreaView.PiexlToTime(ShowRect.x);
            Clip.StartTime = Mathf.Max(0, Clip.StartTime);
            e.Use();

            OnDraging();
        }

        #endregion 拖拽处理

        #region 右键菜单

        /// <summary>
        /// 右键菜单
        /// </summary>
        private void GenRightClickMenu()
        {
            GenericMenu pm = new GenericMenu();

            //删除
            var delPaste = EditorGUIUtility.TrTextContent("删除");
            pm.AddItem(delPaste, false, () =>
            {
                Track.RemoveClipView(this);
            });

            //复制
            var copyPaste = EditorGUIUtility.TrTextContent("复制");
            pm.AddItem(copyPaste, false, () =>
            {
                CopyView();
            });

            Rect rect = new Rect(Event.current.mousePosition, new Vector2(200, 0));
            pm.DropDown(rect);
        }

        #endregion

        private void SyncClipData()
        {
            Clip.EndTime = Clip.StartTime + Clip.DurationTime;
        }

        private void CopyView()
        {
            string jsonStr = JsonMapper.ToJson(Clip);
            ClipModel newClip = JsonMapper.ToObject<ClipModel>(jsonStr);
            newClip.StartTime = Clip.StartTime + 2;
            newClip.EndTime = Clip.EndTime + 2;
            newClip.DurationTime = newClip.EndTime - newClip.StartTime;
            Track.AddClip(newClip);
        }

        #region Virtual

        /// <summary>
        /// 绘制片段
        /// </summary>
        public virtual void OnDrawClip()
        {
            GUI.color = DisplayNameColor;
            EditorGUILayout.LabelField(Clip.TitleName, EditorStylesExtension.MiddleLabelStyle, GUILayout.Height(ShowRect.height));
            GUI.color = Color.white;
        }

        public virtual void OnSelect()
        {
            InspectorExtension.DrawObjectInInspector(Clip);
        }

        public virtual void OnDragStart()
        {
        }

        public virtual void OnDraging()
        {
        }

        public virtual void OnDragEnd()
        {
        }

        #endregion
    }
}