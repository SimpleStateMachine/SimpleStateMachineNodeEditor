using ReactiveUI;
using System.Reactive;
using System.Windows;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public partial class MainWindowViewModel
    {
        public ReactiveCommand<string, Unit> CommandCopyError { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCopySchemeName { get; set; }

        private void SetupCommands()
        {
            CommandCopyError = ReactiveCommand.Create<string>(CopyError);
            CommandCopySchemeName = ReactiveCommand.Create(CopySchemeName);

        }

        private void CopyError(string errrorText)
        {
            Clipboard.SetText(errrorText);
        }
        private void CopySchemeName()
        {
            Clipboard.SetText(NodesCanvas.SchemePath);
        }
    }
}
