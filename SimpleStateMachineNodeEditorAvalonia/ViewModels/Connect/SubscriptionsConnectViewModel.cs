using ReactiveUI;
using System;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class ConnectViewModel
    {
        IDisposable fromConnectorPositionSubscrube;
        protected override void SetupSubscriptions()
        {
            // this.WhenAnyValue(x => x.FromConnector.Node.Header.IsCollapse).Subscribe(value => UpdateSubscriptionForPosition(value));
        }

        // private void UpdateSubscriptionForPosition(bool nodeIsCollapse)
        // {
        //     if (!nodeIsCollapse)
        //     {
        //         fromConnectorPositionSubscrube?.Dispose();
        //         fromConnectorPositionSubscrube = this.WhenAnyValue(x => x.FromConnector.Position).BindTo(this, vm => vm.StartPoint);
        //
        //     }
        //     else
        //     {
        //         fromConnectorPositionSubscrube?.Dispose();
        //         fromConnectorPositionSubscrube = this.WhenAnyValue(x => x.FromConnector.Node.Output.Position).BindTo(this, vm => vm.StartPoint);
        //     }
        // }
    }
}
