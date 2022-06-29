using LCNode;
using LCNode.View;
using LCToolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Demo.Tree
{
    [CustomNodeView(typeof(Tree_Bev_Act_ExecuteInteractive))]
    public class Tree_Bev_Act_ExecuteInteractiveView : BaseNodeView
    {
        public static Dictionary<string,Type> interactiveDict = new Dictionary<string, Type>()
        {
            {"采集",typeof(CollectInteractive)},
            {"存储",typeof(StorageInteractive)},
        };

        private Button btnSelInteractive;

        public Tree_Bev_Act_ExecuteInteractiveView()
        {
            btnSelInteractive = new Button();
            controlsContainer.Add(btnSelInteractive);
        }

        protected override void OnInitialized()
        {
            Tree_Bev_Act_ExecuteInteractive node = Model as Tree_Bev_Act_ExecuteInteractive;
            btnSelInteractive.text = node.interactiveName;
            btnSelInteractive.clicked += OnClickSelInteractive;
        }

        private void OnClickSelInteractive()
        {
            Tree_Bev_Act_ExecuteInteractive node = Model as Tree_Bev_Act_ExecuteInteractive;

            List<string> namelist = interactiveDict.Keys.ToList();
            MiscHelper.Menu<string>(namelist,(name) =>{
                node.interactiveName = name;
                btnSelInteractive.text = node.interactiveName;
            });
        }
    }
}
