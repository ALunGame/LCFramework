using LCToolkit.ViewModel;

namespace LCNode.Model
{
    public interface IBindableView
    {
        void UnBindingProperties();
    }

    public interface IBindableView<VM> : IBindableView where VM : ViewModel
    {
        VM Model { get; }
    }
}
