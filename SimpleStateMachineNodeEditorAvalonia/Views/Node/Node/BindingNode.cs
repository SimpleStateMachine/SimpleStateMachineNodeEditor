using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;
using Avalonia.Controls;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Node
    {
        protected override void SetupBinding()
        {
     
            this.WhenViewModelAnyValue(disposable =>
            {
                this.OneWayBind(ViewModel, 
                    x => x.Header, 
                    x => x.HeaderNode.ViewModel)
                    .DisposeWith(disposable);
                
                this.OneWayBind(ViewModel, 
                    x=>x.IsSelect, 
                    x=>x.IsSelect)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, 
                    x => x.Connectors,
                    x => x.ConnectorsNode.ViewModel)
                    .DisposeWith(disposable);
                
                this.OneWayBind(ViewModel, 
                    x => x.Input, 
                    x => x.InputNode.ViewModel)
                    .DisposeWith(disposable);
                
                this.OneWayBind(ViewModel, 
                    x => x.Output, 
                    x => x.OutputNode.ViewModel)
                    .DisposeWith(disposable);
                
                this.OneWayBind(ViewModel, 
                    x => x.Header.IsCollapse, 
                    x => x.ConnectorsNode.IsVisible, 
                    x=>!x).DisposeWith(disposable);
                
                this.OneWayBind(ViewModel, 
                    x => x.Header.IsCollapse, 
                    x => x.OutputNode.IsVisible)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, 
                    x=>x.Point1.X, 
                    x=>x.TranslateTransformNode.X)
                    .DisposeWith(disposable);
                
                this.OneWayBind(ViewModel, 
                    x => x.Point1.Y, 
                    x => x.TranslateTransformNode.Y)
                    .DisposeWith(disposable);
                
                ViewModel.WhenAnyValue(x => x.Point1.X)
                    .BindTo(this, x => x.Parent.ZIndex)
                    .DisposeWith(disposable);
            });
        }
    }
}
