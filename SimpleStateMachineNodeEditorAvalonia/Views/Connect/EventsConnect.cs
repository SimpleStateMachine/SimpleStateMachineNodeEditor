using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Diagnostics;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connect
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                // NodesCanvas.Current.PointerMoved += OnEventPointerMoved;
                // NodesCanvas.Current.PointerReleased += OnDragOver;
            });
        }
        void OnEventPointerMoved(object subject, PointerEventArgs e)
        {
            var point = e.GetPosition(NodesCanvas.Current);
            this.ViewModel.EndPoint = point;
        }
        
        public void OnDragEnter(DragEventArgs e)
        {

        }

        public void OnDragLeave(RoutedEventArgs e)
        {

        }

        public void OnDragOver(object sender, PointerEventArgs e)
        {
            NodesCanvas.Current.PointerMoved -= OnEventPointerMoved;
            NodesCanvas.Current.PointerPressed -= OnDragOver;
        }

        public void OnDrop(DragEventArgs e)
        {

        }
    }
}
