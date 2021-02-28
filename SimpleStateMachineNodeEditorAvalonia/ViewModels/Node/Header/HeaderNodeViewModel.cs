using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class HeaderNodeViewModel:BaseViewModel
    {
        public HeaderNodeViewModel(string name = "")
        {
            Name = new StringWithEnable(name);
        }

        [Reactive] public StringWithEnable Name { get; set; }
        [Reactive] public bool IsCollapse { get; set; }
    }
}
