using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCDialog
{
    public interface IDialogConfigServer
    {
        void Init();

        void Clear();

        TBDialogCnf GetCnf(DialogType dialogType);

        bool GetDialogModel(DialogType dialogType, int dialogId, out DialogModel model);

        bool GetDialogStepModel(DialogModel model, int step, out DialogStepModel stepModel);

        bool GetDialogDisposeModel(DialogStepModel model, int disposeId, out DialogDisposeModel disposeModel);
    }
}
