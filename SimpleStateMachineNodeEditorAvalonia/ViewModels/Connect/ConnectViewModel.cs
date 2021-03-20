using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class ConnectViewModel : BaseViewModel
    {
        [Reactive] public RightConnectorViewModel FromConnector { get; set; }
        [Reactive] public LeftConnectorViewModel ToConnector { get; set; }

        public ConnectViewModel(RightConnectorViewModel connectorFrom, LeftConnectorViewModel connectorTo)
        {
            FromConnector = connectorFrom;
            ToConnector = connectorTo;
        }
    }
}
