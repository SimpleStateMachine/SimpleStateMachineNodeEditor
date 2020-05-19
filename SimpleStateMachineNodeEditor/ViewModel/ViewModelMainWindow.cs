using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using System.Reactive.Linq;
using System.Collections.ObjectModel;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelMainWindow: ReactiveObject
    {
        public ObservableCollectionExtended<ViewModelMessage> Messages { get; set; } = new ObservableCollectionExtended<ViewModelMessage>();

        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }
        [Reactive] public TypeMessage DisplayMessageType { get; set; }
        [Reactive] public bool? DebugEnable { get; set; } = true;

        private IDisposable ConnectToMessages;
        public double MaxHeightMessagePanel = 150;

        public ViewModelMainWindow()
        {
            SetupCommands();
            SetupSubscriptions();        
        }

        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
           this.WhenAnyValue(x => x.NodesCanvas.DisplayMessageType).Subscribe(_ => UpdateMessages());

        }
        private void UpdateMessages()
        {
            ConnectToMessages?.Dispose();
            Messages.Clear();

            bool debugEnable = DebugEnable.HasValue && DebugEnable.Value;
            bool displayAll = this.NodesCanvas.DisplayMessageType == TypeMessage.All;

            ConnectToMessages = this.NodesCanvas.Messages.ToObservableChangeSet().Filter(x=> CheckForDisplay(x.TypeMessage)).ObserveOnDispatcher().Bind(Messages).DisposeMany().Subscribe();

            bool CheckForDisplay(TypeMessage typeMessage)
            {
                if (typeMessage == this.NodesCanvas.DisplayMessageType)
                {
                    return true;
                }
                else if(typeMessage==TypeMessage.Debug)
                {
                    return debugEnable && displayAll;
                }

                return displayAll;
            }
        }
        #endregion Setup Subscriptions
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
