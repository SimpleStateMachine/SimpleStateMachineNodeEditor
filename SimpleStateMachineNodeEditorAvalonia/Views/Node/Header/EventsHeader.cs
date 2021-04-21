using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.OpenGL.Egl;
using System;
using SimpleStateMachineNodeEditorAvalonia.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Header
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                this.Events().DoubleTapped.Subscribe(OnTextBoxHeaderDoubleTappedEvent).DisposeWith(disposable);
                this.TextBoxHeader.Events().LostFocus.Subscribe(OnLostFocusEvent).DisposeWith(disposable);
            });
        }

        protected virtual void OnTextBoxHeaderDoubleTappedEvent(RoutedEventArgs e)
        {
            TextBoxHeader.IsHitTestVisible = true;
            TextBoxHeader.Focus();
            TextBoxHeader.CaretIndex = TextBoxHeader.Text.Length;
            TextBoxHeader.ClearSelection();
        }
        
        protected virtual void OnLostFocusEvent(RoutedEventArgs e)
        {
            TextBoxHeader.IsHitTestVisible = false;
        }
    }
}
