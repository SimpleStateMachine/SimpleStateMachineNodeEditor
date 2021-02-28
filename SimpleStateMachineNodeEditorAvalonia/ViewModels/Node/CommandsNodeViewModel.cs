using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System.Reactive;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodeViewModel
    {
        public ReactiveCommand<SelectMode, Unit> SelectCommand;
        protected override void SetupCommands()
        {
            SelectCommand = ReactiveCommand.Create<SelectMode>(Select);
        }

        private void Select(SelectMode selectMode)
        {
            if (selectMode == SelectMode.ClickWithCtrl)
            {
                IsSelect = !IsSelect;
            }
            else if((selectMode == SelectMode.Click)&&(!IsSelect))
            {
                NodesCanvas.Nodes.SetIsSelectAllNodesCommand.ExecuteWithSubscribe((false, null));
                IsSelect = true;
            }

        }
    }
}
