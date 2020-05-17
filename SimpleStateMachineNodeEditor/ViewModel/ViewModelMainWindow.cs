using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelMainWindow: ReactiveObject
    {
        public IObservableCollection<ViewModelMessage> Messages { get { return NodesCanvas.Messages; } }

        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }


        public double MaxHeightMessagePanel = 150;

        public int CountShowingMessage = 5;

        public ViewModelMainWindow()
        {
            SetupCommands();
        }

        #region Setup Commands

        public SimpleCommandWithParameter<string> CommandCopyError { get; set; }

        private void SetupCommands()
        {
            CommandCopyError = new SimpleCommandWithParameter<string>(CopyError);
        }

        private void CopyError(string errrorText)
        {
            Clipboard.SetText(errrorText);
        }

        #endregion Setup Commands
    }
}
