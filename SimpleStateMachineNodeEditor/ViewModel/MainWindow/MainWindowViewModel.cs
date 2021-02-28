using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using System.Reactive.Linq;
using System.Linq;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public partial class MainWindowViewModel : ReactiveObject
    {
        public ObservableCollectionExtended<MessageViewModel> Messages { get; set; } = new ObservableCollectionExtended<MessageViewModel>();

        [Reactive] public NodesCanvasViewModel NodesCanvas { get; set; }
        [Reactive] public TypeMessage DisplayMessageType { get; set; }
        [Reactive] public bool? DebugEnable { get; set; } = true;

        [Reactive] public int CountError { get; set; }
        [Reactive] public int CountWarning { get; set; }
        [Reactive] public int CountInformation { get; set; }
        [Reactive] public int CountDebug { get; set; }

        private IDisposable ConnectToMessages;
        public double DefaultHeightMessagePanel = 150;
        public double DefaultWidthTransitionsTable = 350;
        public ObservableCollectionExtended<ConnectorViewModel> Transitions { get; set; }  = new ObservableCollectionExtended<ConnectorViewModel>();
      

        public MainWindowViewModel(NodesCanvasViewModel viewModelNodesCanvas)
        {
            NodesCanvas = viewModelNodesCanvas;
            NodesCanvas.Nodes.Connect().TransformMany(x=>x.Transitions).AutoRefresh(x => x.Name).Filter(x=> Filter(x.Name)).Bind(Transitions).Subscribe();
            bool Filter(string name)
            {
                return !string.IsNullOrEmpty(name);
            }
            SetupCommands();
            SetupSubscriptions();
        }

        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.NodesCanvas.DisplayMessageType).Subscribe(_ => UpdateMessages());
            this.WhenAnyValue(x => x.NodesCanvas.Messages.Count).Subscribe(_ => UpdateCountMessages());

        }
        private void UpdateMessages()
        {
            ConnectToMessages?.Dispose();
            Messages.Clear();

            bool debugEnable = DebugEnable.HasValue && DebugEnable.Value;
            bool displayAll = NodesCanvas.DisplayMessageType == TypeMessage.All;

            ConnectToMessages = NodesCanvas.Messages.ToObservableChangeSet().Filter(x => CheckForDisplay(x.TypeMessage)).ObserveOnDispatcher().Bind(Messages).DisposeMany().Subscribe();

            bool CheckForDisplay(TypeMessage typeMessage)
            {
                if (typeMessage == NodesCanvas.DisplayMessageType)
                {
                    return true;
                }
                else if (typeMessage == TypeMessage.Debug)
                {
                    return debugEnable && displayAll;
                }

                return displayAll;
            }
        }
        private void UpdateCountMessages()
        {
            var counts = NodesCanvas.Messages.GroupBy(x => x.TypeMessage).ToDictionary(x => x.Key, x => x.Count());
            CountError = counts.Keys.Contains(TypeMessage.Error) ? counts[TypeMessage.Error] : 0;
            CountWarning = counts.Keys.Contains(TypeMessage.Warning) ? counts[TypeMessage.Warning] : 0;
            CountInformation = counts.Keys.Contains(TypeMessage.Information) ? counts[TypeMessage.Information] : 0;
            CountDebug = counts.Keys.Contains(TypeMessage.Debug) ? counts[TypeMessage.Debug] : 0;
        }

        #endregion Setup Subscriptions

    }
}
