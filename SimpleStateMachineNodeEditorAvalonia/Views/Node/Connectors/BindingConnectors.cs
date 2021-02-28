using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Connectors
    {
        protected override void SetupBinding()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                //this.OneWayBind(this.ViewModel, x => x.ConnectorsForView, x => x.ItemsControlConnectors.Items).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.ConnectorsForView, x => x.ListBoxConnectors.Items).DisposeWith(disposable);
            });
        }
    }
}
