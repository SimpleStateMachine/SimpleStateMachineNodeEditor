using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Node
    {
        protected override void SetupBinding()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.OneWayBind(this.ViewModel, x => x.Header, x => x.HeaderNode.ViewModel).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Connectors, x => x.ConnectorsNode.ViewModel).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Input, x => x.InputNode.ViewModel).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Output, x => x.OutputNode.ViewModel).DisposeWith(disposable);

                this.OneWayBind(this.ViewModel, x=>x.Position.X, x=>x.TranslateTransformNode.X).DisposeWith(disposable);
                this.OneWayBind(this.ViewModel, x => x.Position.Y, x => x.TranslateTransformNode.Y).DisposeWith(disposable);
            });
        }
    }
}
