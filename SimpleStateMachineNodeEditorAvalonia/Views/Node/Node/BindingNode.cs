using Avalonia;
using Avalonia.Input;
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

        protected virtual void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                //this.WhenAnyValue(x => x.IsMouseOver).Subscribe(value => OnEventMouseOver(value)).DisposeWith(disposable);
                //this.Events().MouseLeftButtonDown.Subscribe(e => OnEventMouseLeftDowns(e)).DisposeWith(disposable);
                //this.Events().PointerPressed.Subscribe(_= this.ViewModel.Position = this.ViewModel.Position + new Point(10,10)).DisposeWith(disposable);
                //this.Events().MouseUp.Subscribe(e => OnEventMouseUp(e)).DisposeWith(disposable);
                //this.Events().MouseMove.Subscribe(e => OnMouseMove(e)).DisposeWith(disposable);

                //this.NodeHeaderElement.ButtonCollapse.Events().Click.Subscribe(_ => ViewModel.IsCollapse = !ViewModel.IsCollapse).DisposeWith(disposable);
                //this.NodeHeaderElement.Events().LostFocus.Subscribe(e => Validate(e)).DisposeWith(disposable);
                //this.ViewModel.WhenAnyValue(x => x.IsCollapse).Subscribe(value => OnEventCollapse(value)).DisposeWith(disposable);
            });
        }

    }
}
