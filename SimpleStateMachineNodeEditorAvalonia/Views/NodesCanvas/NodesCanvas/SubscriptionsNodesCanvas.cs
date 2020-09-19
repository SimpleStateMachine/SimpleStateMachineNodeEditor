using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class NodesCanvas
    {
        protected override void SetupSubscriptions()
        {
            this.WhenViewModelAnyValue(disposable =>
            {

            });
        }
    }
}

