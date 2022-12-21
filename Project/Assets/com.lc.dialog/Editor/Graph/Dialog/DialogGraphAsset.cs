using LCNode.Model;
using LCToolkit;

namespace LCDialog.DialogGraph
{
    public class DialogGraphAsset : BaseGraphAsset<DialogGraph>
    {
        [ReadOnly]
        public DialogType dialogType;
    }
}
