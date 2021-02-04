using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class RightConnectorViewModel : ConnectorViewModel
    {
        [Reactive] public ConnectViewModel Connect { get; set; }
        [Reactive] public bool IsSelect { get; set; }
        public RightConnectorViewModel(NodeViewModel node, string name = "", bool isEnable = true):base(node, name, isEnable)
        {
        }
    }
}
