using System;
using UnityEditor;
using UnityEngine;
using LCToolkit;

namespace LCTimeline.View
{
    //添加轨道
    public class AddTrackView : BaseView
    {
        private GUIContent AddContent;

        public override void OnInit()
        {
            AddContent = EditorGUIUtility.TrTextContent("Add", "Add new tracks.");
        }

        public override void OnDraw()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal(GUILayout.Width(window.AddTrackSize.width));
            AddButtonGUI();
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }

        private void AddButtonGUI()
        {
            if (EditorGUILayout.DropdownButton(AddContent, FocusType.Passive, "Dropdown"))
            {
                GenCustomMenu();
            }
        }

        public void GenCustomMenu()
        {
            GenericMenu pm = new GenericMenu();

            foreach (var item in Model.GetTracks())
            {
                if (AttributeHelper.TryGetTypeAttribute(item, out TrackMenuAttribute trackAttribute))
                {
                    string menuName = trackAttribute.MenuName;
                    var paste = EditorGUIUtility.TrTextContent(menuName);
                    pm.AddItem(paste, false, () =>
                    {
                        OnAddTrackItem(item);
                    });
                }
            }

            Rect rect = new Rect(Event.current.mousePosition, new Vector2(200, 0));
            pm.DropDown(rect);
        }

        private void OnAddTrackItem(Type track)
        {
            TrackModel trackModel = ReflectionHelper.CreateInstance(track) as TrackModel;
            BaseSequenceView sequenceView = window.GetPartialView<BaseSequenceView>();
            sequenceView.AddTrack(trackModel);
        }
    }
}