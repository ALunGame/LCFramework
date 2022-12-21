//using LCNode;
//using LCNode.View;
//using UnityEditor;
//using UnityEngine;
//using LCConfig;
//using LCMap;
//using System.Collections.Generic;

//namespace LCDialog.DialogGraph
//{
//    [CustomNodeView(typeof(Dialog_SpeakerNode))]
//    public class Dialog_SpeakerNodeView : BaseNodeView
//    {
//        private List<ActorCnf> actorCnfs = new List<ActorCnf>();

//        public Dialog_SpeakerNodeView()
//        {
//            actorCnfs = ExcelReadCtrl.GetConfig<ActorCnf>();
//        }

//        protected override void OnInitialized()
//        {
//            DrawSpeaker();
//        }

//        protected override void OnDrawerValuesChange()
//        {
//            DrawSpeaker();
//        }

//        private void DrawSpeaker()
//        {
//            Dialog_SpeakerNode node = Model as Dialog_SpeakerNode;

//            for (int i = 0; i < actorCnfs.Count; i++)
//            {
//                ActorCnf actorCnf = actorCnfs[i];
//                if (actorCnf.id == node.speakerId)
//                {
//                    node.Title = $"{node.speakerId}:{actorCnf.name}";
//                    return;
//                }
//            }
//            node.Title = $"{node.speakerId}:没有此对象";
//        }
//    }
//}