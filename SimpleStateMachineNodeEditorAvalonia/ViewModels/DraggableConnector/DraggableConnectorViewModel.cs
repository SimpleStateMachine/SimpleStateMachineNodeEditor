
using Avalonia;
using ReactiveUI.Fody.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public class DraggableConnectorViewModel:BaseViewModel
    {
        [Reactive] public Point StartPoint { get; set; }
        [Reactive] public Point EndPoint { get; set; }

        protected override void SetupCommands()
        {
            
        }

        protected override void SetupSubscriptions()
        {
            
        }
    }
}