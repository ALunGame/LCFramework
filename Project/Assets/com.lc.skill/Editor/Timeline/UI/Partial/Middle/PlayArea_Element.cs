using LCToolkit;
using LCToolkit.Element;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LCSkill.Timeline
{
    /// <summary>
    /// 播放区域
    /// </summary>
    public class PlayArea_Element : PartialView
    {
        #region 播放操作UI元素

        private GUIContent PlayContent;

        private GUIContent GotoBeginingContent;

        private GUIContent GotoEndContent;

        private GUIContent NextFrameContent;

        private GUIContent PreviousFrameContent;
        
        private GUIContent AddContent;

        #endregion 播放操作UI元素

        private bool isPlaying;
        
        public VisualElement RootElement { get; private set; }
        
        public void Init(MiddlePartialView pMiddlePartialView)
        {
            RootElement = pMiddlePartialView.RootElement.Q<VisualElement>("PlayArea");
            InitTop();
            InitBottom();
        }

        private void InitTop()
        {
            PlayContent = EditorGUIUtility.TrIconContent("Animation.Play", "Play the timeline");
            GotoBeginingContent = EditorGUIUtility.TrIconContent("Animation.FirstKey", "Go to the beginning of the timeline");
            GotoEndContent = EditorGUIUtility.TrIconContent("Animation.LastKey", "Go to the end of the timeline");
            NextFrameContent = EditorGUIUtility.TrIconContent("Animation.NextKey", "Go to the next frame");
            PreviousFrameContent = EditorGUIUtility.TrIconContent("Animation.PrevKey", "Go to the previous frame");
            
            IMGUIContainer startFrameBtn = RootElement.Q<IMGUIContainer>("StartFrameBtn");
            startFrameBtn.onGUIHandler = () =>
            {
                if (GUILayout.Button(GotoBeginingContent, EditorStyles.toolbarButton))
                {
                }
            };
            
            IMGUIContainer preFrameBtn = RootElement.Q<IMGUIContainer>("PreFrameBtn");
            preFrameBtn.onGUIHandler = () =>
            {
                if (GUILayout.Button(PreviousFrameContent, EditorStyles.toolbarButton))
                {
                }
            };
            
            IMGUIContainer playBtn = RootElement.Q<IMGUIContainer>("PlayBtn");
            playBtn.onGUIHandler = () =>
            {
                EditorGUI.BeginChangeCheck();
                bool playbackEnabled = GUILayout.Toggle(window.IsPlaying, PlayContent, EditorStyles.toolbarButton);
                if (EditorGUI.EndChangeCheck())
                {
                    window.SetPlaying(playbackEnabled);
                }
            };
            
            IMGUIContainer laterFrameBtn = RootElement.Q<IMGUIContainer>("LaterFrameBtn");
            laterFrameBtn.onGUIHandler = () =>
            {
                if (GUILayout.Button(NextFrameContent, EditorStyles.toolbarButton))
                {
                }
            };
            
            IMGUIContainer endFrameBtn = RootElement.Q<IMGUIContainer>("EndFrameBtn");
            endFrameBtn.onGUIHandler = () =>
            {
                if (GUILayout.Button(GotoEndContent, EditorStyles.toolbarButton))
                {
                }
            };
        }

        private void InitBottom()
        {
            AddContent = EditorGUIUtility.TrTextContent("Add", "Add new group.");
            
            IMGUIContainer addGroupBtn = RootElement.Q<IMGUIContainer>("AddGroupBtn");
            addGroupBtn.onGUIHandler = () =>
            {
                if (EditorGUILayout.DropdownButton(AddContent, FocusType.Passive, "Dropdown"))
                {
                    GenAddGroupMenu();
                }
            };
            
            IntegerField currFame = RootElement.Q<IntegerField>("CurrFrame");
            currFame.value = window.CurrFrameCnt;
            currFame.RegisterCallback<ChangeEvent<int>>((evt) =>
            {
                if (!window.IsPlaying)
                {
                    window.SetCurrFrame(evt.newValue);
                }
            });
            window.OnCurrFrameChange += () =>
            {
                currFame.value = window.CurrFrameCnt;
            };

            Label totalFrame = RootElement.Q<Label>("TotalFrame");
            totalFrame.text = window.TotalFrameCnt.ToString();
            window.OnTotalFrameChange += () =>
            {
                totalFrame.text = window.TotalFrameCnt.ToString();
            };
        }
        
        public void GenAddGroupMenu()
        {
            GenericMenu pm = new GenericMenu();

            foreach (var type in ReflectionHelper.GetChildTypes<InternalTrackGroup_Element>())
            {
                if (type.IsAbstract)
                    continue;
                if (type == typeof(InternalTrackGroup_Element))
                    continue;
                if (AttributeHelper.TryGetTypeAttribute(type, out TimlineGroupElementAttribute attr))
                {
                    string menuName = attr.menuName;
                    var paste = EditorGUIUtility.TrTextContent(menuName);
                    pm.AddItem(paste, false, () =>
                    {
                        OnAddTrackGroupItem(type);
                    });
                }
            }

            Rect rect = new Rect(Event.current.mousePosition, new Vector2(200, 0));
            pm.DropDown(rect);
        }
        
        private void OnAddTrackGroupItem(System.Type track)
        {
            BaseTrackGroup groupModel = ReflectionHelper.CreateInstance(TimelineUtil.GetTrackGroupModelByElementType(track)) as BaseTrackGroup;
            window.Bottom.AddTrackGroup(groupModel);
        }
    }
}