using Avalonia;
using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Reactive.Disposables;

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
            });
        }

        void OnEventBorderPointerPressed(PointerPressedEventArgs e)
        {
            ViewModel.SelectCommand.ExecuteWithSubscribe(Keyboard.IsKeyDown(Key.LeftCtrl) ? SelectMode.ClickWithCtrl : SelectMode.Click);
            oldPosition = e.GetPosition(NodesCanvas.Current);
            PointerMoved += OnEventPointerMoved;
        }

        void OnEventBorderPointerReleased(PointerReleasedEventArgs e)
        {
            PointerMoved -= OnEventPointerMoved;
        }

        void OnEventPointerMoved(object subject, PointerEventArgs e)
        {
            var currentPosition = e.GetPosition(NodesCanvas.Current);
            ViewModel.NodesCanvas.Nodes.MoveCommand.ExecuteWithSubscribe(currentPosition - oldPosition);
            oldPosition = currentPosition;
        }
    }
}
