using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodesCanvasElements
{
    public partial class Nodes
    {
        protected override void SetupBinding()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.OneWayBind(ViewModel, 
                    x => x.NodesForView, 
                    x => x.ItemsControlNodes.Items).DisposeWith(disposable);
            });
        }
    }
}
