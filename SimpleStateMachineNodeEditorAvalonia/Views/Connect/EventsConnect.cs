using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Reactive.Disposables;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connect
    {
        protected override void SetupEvents()
        {
         
            this.WhenViewModelAnyValue(disposable =>
            {
                //this.Events().PointerReleased.Subscribe(e => OnEllipsePointerReleased(e)).DisposeWith(disposable);
            });
        }
        private void OnEllipsePointerReleased(PointerReleasedEventArgs e)
        {

        }
        public void OnDragEnter(DragEventArgs e)
        {

        }

        public void OnDragLeave(RoutedEventArgs e)
        {

        }

        public void OnDragOver(object sender, DragEventArgs e)
        {

        }

        public void OnDrop(DragEventArgs e)
        {

        }
    }
}
