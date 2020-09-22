using Avalonia;
using Avalonia.Controls;
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
                this.BorderNode.Events().PointerPressed.Subscribe(e => OnEventBorderPointerPressed(e)).DisposeWith(disposable);
                this.BorderNode.Events().PointerReleased.Subscribe(x => OnEventBorderPointerReleased(x)).DisposeWith(disposable);
            });
        }

        void OnEventBorderPointerPressed(PointerPressedEventArgs e)
        {
            this.ViewModel.SelectCommand.ExecuteWithSubscribe(Keyboard.IsKeyDown(Key.LeftCtrl) ? SelectMode.ClickWithCtrl : SelectMode.Click);
            oldPosition = e.GetPosition(NodesCanvas.Current);
            this.PointerMoved += OnEventPointerMoved;
        }

        void OnEventBorderPointerReleased(PointerReleasedEventArgs e)
        {
            this.PointerMoved -= OnEventPointerMoved;
        }

        void OnEventPointerMoved(object subject, PointerEventArgs e)
        {
            var currentPosition = e.GetPosition(NodesCanvas.Current);
            this.ViewModel.NodesCanvas.Nodes.MoveCommand.ExecuteWithSubscribe(currentPosition - oldPosition);
            oldPosition = currentPosition;
        }
    }
}
