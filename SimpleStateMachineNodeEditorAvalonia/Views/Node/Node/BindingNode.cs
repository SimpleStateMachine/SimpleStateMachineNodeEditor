using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia;
using ReactiveUI;
using System;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.VisualTree;


namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Node
    {
        protected override void SetupBinding()
        {
     
            this.WhenViewModelAnyValue(disposable =>
            {
                _canvas = this.FindAncestorOfType<NodesCanvas>();
                this.OneWayBind(ViewModel, 
                    x => x.Header, 
                    x => x.Header.ViewModel)
                    .DisposeWith(disposable);
                
                this.OneWayBind(ViewModel, 
                    x=>x.IsSelect, 
                    x=>x.IsSelect)
                    .DisposeWith(disposable);

                this.OneWayBind(ViewModel, 
                    x => x.Connectors,
                    x => x.Connectors.ViewModel)
                    .DisposeWith(disposable);
                
                this.OneWayBind(ViewModel, 
                    x => x.Input, 
                    x => x.InputConnector.ViewModel)
                    .DisposeWith(disposable);
                
                this.OneWayBind(ViewModel, 
                    x => x.Output, 
                    x => x.OutputConnector.ViewModel)
                    .DisposeWith(disposable);
                
                this.OneWayBind(ViewModel, 
                    x => x.Header.IsCollapse, 
                    x => x.Connectors.IsVisible, 
                    x=>!x).DisposeWith(disposable);
                
                this.OneWayBind(ViewModel, 
                    x => x.Header.IsCollapse, 
                    x => x.OutputConnector.IsVisible)
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

        // void InputConnectUpdateClasses(bool magnetDragOver)
        // {
        //     if(magnetDragOver)
        //         this.InputConnector.Classes.Add("MagnetDragOver");
        //     else
        //         this.InputConnector.Classes.Remove("MagnetDragOver");
        // }
    }
}
