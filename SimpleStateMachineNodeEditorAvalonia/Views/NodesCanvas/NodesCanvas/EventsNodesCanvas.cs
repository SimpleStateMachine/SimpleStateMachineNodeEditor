using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Reactive.Disposables;


namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class NodesCanvas
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                //this.Events().PointerMoved.Subscribe(x => OnEventMouseMove(x)).DisposeWith(disposable);
            });
        }
        
        void OnEventMouseMove(PointerEventArgs e)
        {
 
        }
    }
}

