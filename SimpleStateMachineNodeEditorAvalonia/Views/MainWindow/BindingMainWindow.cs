using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class MainWindow
    {
        protected virtual void SetupBinding()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.OneWayBind(ViewModel, x => x.NodesCanvas, x => x.NodesCanvasMainWindow.ViewModel).DisposeWith(disposable);
            });
        }
    }
}
