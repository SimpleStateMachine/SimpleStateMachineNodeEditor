using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connector<TViewModel>
    {
        protected override void SetupBinding()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Name.Value, x => x.textBox.Text).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Name.IsEnabled, x => x.textBox.IsEnabled).DisposeWith(disposable);
            });
          
        }
    }
}
