using Avalonia;
using ReactiveUI;
using System.Reactive;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class RightConnectorViewModel
    {
        public ReactiveCommand<Point, Unit> AddConnectCommand { get; set; }

        protected override void SetupCommands()
        {
            base.SetupCommands();
            AddConnectCommand = ReactiveCommand.Create<Point>(AddConnect);

        }

        public void AddConnect(Point point)
        {
            // Connect = Node.NodesCanvas.Connects.GetNewConnect(this, point);
        }


    }
}
