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
        //public ReadOnlyObservableCollection<ViewModelMessage> Messages;
        //[Reactive] public ObservableCollection<ViewModelMessage> Messages { get { return new ObservableCollection<ViewModelMessage>(NodesCanvas.Messages.Where(x => x.TypeMessage == this.NodesCanvas.DisplayMessageType || this.NodesCanvas.DisplayMessageType == TypeMessage.All)); } set { } }
        public ObservableCollectionExtended<ViewModelMessage> Messages { get; set; } = new ObservableCollectionExtended<ViewModelMessage>();
        //public ReadOnlyObservableCollection<ViewModelMessage> Messages;
        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }
        [Reactive] public TypeMessage DisplayMessageType { get; set; }

        private IDisposable ConnectToMessages;

        public double MaxHeightMessagePanel = 150;

        public int CountShowingMessage = 5;

        public ViewModelMainWindow()
        {
            SetupCommands();
            SetupSubscriptions();
           
        }
        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
           this.WhenAnyValue(x => x.NodesCanvas).Where(x=>x!=null).Subscribe(_ => UpdateMessages());
           this.WhenAnyValue(x => x.NodesCanvas.DisplayMessageType).Subscribe(_ => Test());
           //this.WhenAnyValue(x => x.NodesCanvas.DisplayMessageType).Subscribe(_ => Test());
            //this.WhenAnyValue(x => x.NodesCanvas.Messages).Subscribe(_ => Test());

        }
        private void Test()
        {
            ConnectToMessages?.Dispose();
            Messages.Clear();
            //Messages.AddRange(this.NodesCanvas.Messages.Where(x => x.TypeMessage == this.NodesCanvas.DisplayMessageType || this.NodesCanvas.DisplayMessageType == TypeMessage.All));
            ConnectToMessages = this.NodesCanvas.Messages.ToObservableChangeSet().Filter(x => x.TypeMessage == this.NodesCanvas.DisplayMessageType || this.NodesCanvas.DisplayMessageType == TypeMessage.All).
                ObserveOnDispatcher().Bind(Messages).DisposeMany().Subscribe();
        }
        private void UpdateMessages()
        {

            //var t = NodesCanvas.Messages.Where(x => x.TypeMessage == this.NodesCanvas.DisplayMessageType).ToList();
            //Messages = t ;

            //var observable = this.NodesCanvas.ObservableForProperty(x => x.DisplayMessageType).Select(x => x.Value);
            //this.NodesCanvas.DisplayMessageType.T
            //    Observable
            //var observable = this.NodesCanvas.ObservableForProperty(x => x.DisplayMessageType);
            //NodesCanvas.Messages.Connect().Filter(x=>x.TypeMessage == this.NodesCanvas.DisplayMessageType || this.NodesCanvas.DisplayMessageType==TypeMessage.All).ObserveOnDispatcher().Bind(out Messages).DisposeMany().Subscribe();
            //NodesCanvas.Messages.Connect().Filter(x => x.TypeMessage == this.NodesCanvas.DisplayMessageType || this.NodesCanvas.DisplayMessageType == TypeMessage.All).ObserveOnDispatcher().Bind(out Messages).DisposeMany().Subscribe();
            //NodesCanvas.Messages.Connect().Filter(x => x!=null).ObserveOnDispatcher().Bind(out Messages).DisposeMany().Subscribe();
            //Messages = ;
            //NodesCanvas.Messages.
            //NodesCanvas.Messages.Connect().ObserveOnDispatcher().Bind(out Messages).DisposeMany().Subscribe();
            //NodesCanvas.Messages.Where(x => x.TypeMessage == this.NodesCanvas.DisplayMessageType || this.NodesCanvas.DisplayMessageType == TypeMessage.All).ToObservable().\

            //Correct for new
            //NodesCanvas.Messages.ToObservableChangeSet().Filter(x => x.TypeMessage == this.NodesCanvas.DisplayMessageType || this.NodesCanvas.DisplayMessageType == TypeMessage.All).ObserveOnDispatcher().Bind(out Messages).DisposeMany().Subscribe();
            //NodesCanvas.Messages.Connect(x => x.TypeMessage == this.NodesCanvas.DisplayMessageType || this.NodesCanvas.DisplayMessageType == TypeMessage.All).ObserveOnDispatcher().Bind(out Messages).DisposeMany().Subscribe();
            //var mySource = NodesCanvas.Messages.Connect().Filter(x => x.TypeMessage == this.NodesCanvas.DisplayMessageType || this.NodesCanvas.DisplayMessageType == TypeMessage.All);
            //var myBindingOperation = mySource.ObserveOn(RxApp.MainThreadScheduler).Bind(out Messages).Subscribe();
        
            //NodesCanvas.Messages.All(x => x.TypeMessage == this.NodesCanvas.DisplayMessageType || this.NodesCanvas.DisplayMessageType == TypeMessage.All).ObservableForProperty(x=>x.M)
            //NodesCanvas.Messages.ToObservableChangeSet().Filter(x => x.TypeMessage == this.NodesCanvas.DisplayMessageType).ObserveOnDispatcher().Bind(out Messages).DisposeMany().Subscribe();
            //NodesCanvas.Messages.Connect().Filter(x => x.TypeMessage == this.NodesCanvas.DisplayMessageType || this.NodesCanvas.DisplayMessageType == TypeMessage.All).Bind(out Messages).Subscribe();

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
