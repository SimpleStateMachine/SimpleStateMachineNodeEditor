using Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class ConnectViewModel : BaseViewModel
    {
        [Reactive] public Point StartPoint { get; set; }
        [Reactive] public Point EndPoint { get; set; } = new Point(500, 500);

        private ConnectViewModel()
        {

        }
        public ConnectViewModel(RightConnectorViewModel connectorFrom):this()
        {
            StartInit(connectorFrom);
        }

        public void StartInit(RightConnectorViewModel ConnectorFrom)
        {
            ConnectorFrom.WhenAnyValue(cf => cf.Position).BindTo(this, vm=>vm.StartPoint);
        }

        public void EndInit(LeftConnectorViewModel ConnectorTo)
        {
            ConnectorTo.WhenAnyValue(ct => ct.Position).BindTo(this, vm => vm.EndPoint);
        }
    }
}
