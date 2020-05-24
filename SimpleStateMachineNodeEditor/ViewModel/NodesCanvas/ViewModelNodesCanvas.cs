using System;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using DynamicData.Binding;
using SimpleStateMachineNodeEditor.Helpers.Transformations;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using System.Windows.Data;
using System.IO;
using Splat;

namespace SimpleStateMachineNodeEditor.ViewModel.NodesCanvas
{
    public partial class ViewModelNodesCanvas : ReactiveObject
    {
        public ObservableCollectionExtended<ViewModelConnect> Connects = new ObservableCollectionExtended<ViewModelConnect>();

        public ObservableCollectionExtended<ViewModelNode> Nodes = new ObservableCollectionExtended<ViewModelNode>();
        public ObservableCollectionExtended<ViewModelMessage> Messages { get; set; } = new ObservableCollectionExtended<ViewModelMessage>();

        [Reactive] public ViewModelSelector Selector { get; set; } = new ViewModelSelector();
        [Reactive] public ViewModelDialog Dialog { get; set; } = new ViewModelDialog();
        [Reactive] public ViewModelCutter Cutter { get; set; }
        [Reactive] public ViewModelConnect DraggedConnect { get; set; }
        [Reactive] public ViewModelConnector ConnectorPreviewForDrop { get; set; }
        [Reactive] public ViewModelNode StartState { get; set; }
        [Reactive] public Scale Scale { get; set; } = new Scale();
        [Reactive] public bool ItSaved { get; set; } = true;
        [Reactive] public TypeMessage DisplayMessageType { get; set; }
        [Reactive] public string SchemePath { get; set; }

        /// <summary>
        /// Flag for close application
        /// </summary>
        [Reactive] public bool NeedExit { get; set; }

        [Reactive] public string JPEGPath{ get; set; }

        public int NodesCount = 0;
        public int TransitionsCount = 0;
        public double ScaleMax = 5;
        public double ScaleMin = 0.1;
        public double Scales { get; set; } = 0.05;
        

        public ViewModelNodesCanvas()
        {
            Cutter = new ViewModelCutter(this);
            SetupCommands();
            SetupSubscriptions();
            SetupStartState();
        }

        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.Nodes.Count).Buffer(2, 1).Select(x => (Previous: x[0], Current: x[1])).Subscribe(x => UpdateCount(x.Previous, x.Current));
        }
        #endregion Setup Subscriptions
        #region Setup Nodes

        private void SetupStartState()
        {
            string name = Nodes.Any(x => x.Name == "Start") ? GetNameForNewNode() : "Start";
            StartState = new ViewModelNode(this)
            {
                Name = name
            };
            SetAsStart(StartState);
            Nodes.Add(StartState);
            this.ItSaved = true;
            //ViewModelNode end = new ViewModelNode(this)
            //{
            //    Name = "End",
            //    NameEnable = false,
            //    CanBeDelete = false,
            //    Point1 = new MyPoint(100, 100)
            //};
            //end.TransitionsVisible = null;
            //end.RollUpVisible = null;
            //Nodes.Add(end);
        }
        private void SetAsStart(ViewModelNode node)
        {
            node.Input.Visible = false;
            node.CanBeDelete = false;
        }

        #endregion Setup Nodes


        #region Logging

        public void LogDebug(string message, params object[] args)
        {
            Messages.Add(new ViewModelMessage(TypeMessage.Debug, string.Format(message, args)));
        }
        public void LogError(string message, params object[] args)
        {
            Messages.Add(new ViewModelMessage(TypeMessage.Error, string.Format(message, args)));
        }
        public void LogInformation(string message, params object[] args)
        {
            Messages.Add(new ViewModelMessage(TypeMessage.Information, string.Format(message, args)));
        }
        public void LogWarning(string message, params object[] args)
        {
            Messages.Add(new ViewModelMessage(TypeMessage.Warning, string.Format(message, args)));
        }

        #endregion Logging
        private void UpdateCount(int oldValue, int newValue)
        {
            if (newValue > oldValue)
            {
                NodesCount++;
            }   
        }
        private string SchemeName()
        {
            if (!string.IsNullOrEmpty(this.SchemePath))
            {
                return Path.GetFileNameWithoutExtension(this.SchemePath);
            }
            else
            {
                return "SimpleStateMachine";
            }
        }
    }
}
