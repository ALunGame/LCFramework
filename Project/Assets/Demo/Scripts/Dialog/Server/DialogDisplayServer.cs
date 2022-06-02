using LCDialog;
using System;
using System.Collections.Generic;

namespace Demo.Dialog
{
    public class DialogDisplayServer : IDialogDisplayServer
    {
        public void OnClickDispose(DialogObj dialog, int disposeId)
        {
            throw new NotImplementedException();
        }

        public void OnCloseDialog(DialogObj dialog)
        {
            throw new NotImplementedException();
        }

        public void OnCreateDialog(DialogObj dialog, List<int> actorUids)
        {
            throw new NotImplementedException();
        }

        public void OnPlayDialog(DialogObj dialog, DialogStepModel stepModel)
        {
            throw new NotImplementedException();
        }
    }
}
