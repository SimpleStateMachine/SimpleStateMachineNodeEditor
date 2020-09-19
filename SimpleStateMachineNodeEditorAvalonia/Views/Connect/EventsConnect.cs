using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Connect
    {
        protected override void SetupEvents()
        {
         
            this.WhenViewModelAnyValue(disposable =>
            {
                
            });
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
