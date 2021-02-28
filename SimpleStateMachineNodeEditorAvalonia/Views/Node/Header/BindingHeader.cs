using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Header
    {
        protected override void SetupBinding()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.OneWayBind(ViewModel, x => x.Name.Value, x => x.TextBoxHeader.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.Name.IsEnabled, x => x.TextBoxHeader.IsEnabled).DisposeWith(disposable);
                this.Bind(ViewModel, x => x.IsCollapse, x => x.ToggleButtonHeader.IsChecked).DisposeWith(disposable);
            });
        }
    }
}
