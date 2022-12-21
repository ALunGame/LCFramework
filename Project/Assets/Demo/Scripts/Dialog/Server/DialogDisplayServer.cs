using LCDialog;
using LCMap;
using System;
using System.Collections.Generic;

namespace Demo.Dialog
{
    public class DialogDisplayServer : IDialogDisplayServer
    {
        public Dictionary<DialogType, IDialogDisplayServer> displayDict = new Dictionary<DialogType, IDialogDisplayServer>() 
        {
            {DialogType.Bubble,new BubbleDialogDisplay()},
        };

        public void OnCreateDialog(DialogObj dialog, List<string> actorUids)
        {
            displayDict[dialog.DialogType].OnCreateDialog(dialog, actorUids);
        }

        public void OnPlayDialog(DialogObj dialog, DialogStepModel stepModel)
        {
            displayDict[dialog.DialogType].OnPlayDialog(dialog, stepModel);
        }

        public void OnClickDispose(DialogObj dialog, int disposeId)
        {
            displayDict[dialog.DialogType].OnClickDispose(dialog, disposeId);
        }

        public void OnCloseDialog(DialogObj dialog)
        {
            displayDict[dialog.DialogType].OnCloseDialog(dialog);
        }
    }
}
