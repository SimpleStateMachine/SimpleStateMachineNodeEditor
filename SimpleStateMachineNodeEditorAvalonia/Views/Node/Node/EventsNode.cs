using Avalonia;
using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Reactive.Disposables;
using Avalonia.VisualTree;
using SimpleStateMachineNodeEditor.Helpers.Extensions;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Node
    {
        private Point oldPosition;
        protected override void SetupEvents()
        {  
            this.WhenViewModelAnyValue(disposable =>
            {           
                BorderNode.Events().PointerPressed.Subscribe(OnEventBorderPointerPressed).DisposeWith(disposable);
                BorderNode.Events().PointerReleased.Subscribe(OnEventBorderPointerReleased).DisposeWith(disposable);
                // AddHandler();
                // InputConnectorMagnetBorder.AddHandler(DragDrop.);
            });
        }

        private void OnEventBorderPointerPressed(PointerPressedEventArgs e)
        {
            ViewModel.SelectCommand.ExecuteWithSubscribe((e.KeyModifiers & KeyModifiers.Control)!= 0 ? SelectMode.ClickWithCtrl : SelectMode.Click);
            oldPosition = e.GetPosition(_canvas);
            PointerMoved += OnEventPointerMoved;
        }

        private void OnEventBorderPointerReleased(PointerReleasedEventArgs e)
        {
            PointerMoved -= OnEventPointerMoved;
        }

        private void OnEventPointerMoved(object subject, PointerEventArgs e)
        {
            var currentPosition = e.GetPosition(_canvas);
            ViewModel.NodesCanvas.Nodes.MoveCommand.ExecuteWithSubscribe(currentPosition - oldPosition);
            oldPosition = currentPosition;
        }
    }
}
