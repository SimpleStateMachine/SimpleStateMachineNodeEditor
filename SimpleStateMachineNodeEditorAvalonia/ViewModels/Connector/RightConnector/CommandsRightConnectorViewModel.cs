using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class RightConnectorViewModel
    {
        public ReactiveCommand<Unit, Unit> AddConnectCommand { get; set; }

        protected override void SetupCommands()
        {
            base.SetupCommands();
            AddConnectCommand = ReactiveCommand.Create(AddConnect);
        }

        public void AddConnect()
        {
            Connect = Node.NodesCanvas.Connects.GetNewConnect(this);
        }


    }
}
