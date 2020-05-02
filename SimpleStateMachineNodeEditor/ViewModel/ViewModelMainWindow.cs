using ReactiveUI;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelMainWindow: ReactiveObject
    {
        public ViewModelMainWindow()
        {
            SetupCommands();
        }

        public string Path { get;  set; }

        #region Setup Commands

        public SimpleCommandWithParameter<string> CommandCopyError { get; set; }

        private void SetupCommands()
        {
            CommandCopyError = new SimpleCommandWithParameter<string>(this, CopyError);
        }

        private void CopyError(string errrorText)
        {
            Clipboard.SetText(errrorText);
        }

        #endregion Setup Commands
    }
}
