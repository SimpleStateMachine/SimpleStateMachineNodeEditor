
using Avalonia;
using ReactiveUI.Fody.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public class DraggableConnectorViewModel:BaseViewModel
    {
        private NodeViewModel Node { get;}
        [Reactive] public Point StartPoint { get; set; }
        [Reactive] public Point EndPoint { get; set; }
        public DraggableConnectorViewModel(NodeViewModel node, Point startPoint)
        {
            Node = node;
            StartPoint = startPoint;
            EndPoint = startPoint;
        }
        
        protected override void SetupCommands()
        {
            
        }

        protected override void SetupSubscriptions()
        {
            
        }
    }
}