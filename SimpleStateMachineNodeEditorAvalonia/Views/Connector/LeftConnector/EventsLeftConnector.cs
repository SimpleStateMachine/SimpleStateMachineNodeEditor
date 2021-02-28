using Avalonia.Input;
using SimpleStateMachineNodeEditorAvalonia.Helpers;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class LeftConnector
    {
        protected override void SetupEvents()
        {
            this.WhenViewModelAnyValue(disposable =>
            {

            });
        }

        public void OnConnectDrop(object sender, DragEventArgs e)
        {

        }
    }
}
