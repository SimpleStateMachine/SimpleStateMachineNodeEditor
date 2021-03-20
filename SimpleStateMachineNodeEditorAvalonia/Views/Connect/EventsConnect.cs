using System.Reactive.Disposables;
using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using Avalonia;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connect
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                
                var t = Application.Current;
                // Avalonia.Application.Current.InputManager
                // NodesCanvas.Current.Events().PointerMoved.Subscribe(OnEventPointerMoved).DisposeWith(disposable);
            });
        }
        
        void OnEventPointerMoved(PointerEventArgs e)
        {
            // var point = e.GetPosition(NodesCanvas.Current);
            // ViewModel.EndPoint = point;
        }
        

        // public void OnDragOver(object sender, PointerEventArgs e)
        // {
        //     NodesCanvas.Current.PointerMoved -= OnEventPointerMoved;
        //     NodesCanvas.Current.PointerPressed -= OnDragOver;
        // }

    }
}
