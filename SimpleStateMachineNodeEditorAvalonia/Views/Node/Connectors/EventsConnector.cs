using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Connectors
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
            });
        }
    }
}
