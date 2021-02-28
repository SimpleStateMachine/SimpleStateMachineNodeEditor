using System;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using DynamicData.Binding;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using System.IO;
using Splat;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media;
using DynamicData;
using Microsoft.Extensions.Configuration;
using System.Windows.Input;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public partial class NodesCanvasViewModel : ReactiveObject
    {
        public ObservableCollectionExtended<ConnectViewModel> Connects = new ObservableCollectionExtended<ConnectViewModel>();

        //public ObservableCollectionExtended<ViewModelNode> Nodes = new ObservableCollectionExtended<ViewModelNode>();
        public SourceList<NodeViewModel> Nodes = new SourceList<NodeViewModel>();

        public ObservableCollectionExtended<NodeViewModel> NodesForView = new ObservableCollectionExtended<NodeViewModel>();
        public ObservableCollectionExtended<MessageViewModel> Messages { get; set; } = new ObservableCollectionExtended<MessageViewModel>();

        [Reactive] public Point PositionRight { get; set; }
        [Reactive] public Point PositionLeft { get; set; }
        [Reactive] public SelectorViewModel Selector { get; set; } = new SelectorViewModel();
        [Reactive] public DialogViewModel Dialog { get; set; } = new DialogViewModel();
        [Reactive] public CutterViewModel Cutter { get; set; }
        [Reactive] public ConnectViewModel DraggedConnect { get; set; }
        [Reactive] public ConnectorViewModel ConnectorPreviewForDrop { get; set; }
        [Reactive] public NodeViewModel StartState { get; set; }
        [Reactive] public Matrix RenderTransformMatrix { get; set; }
        [Reactive] public bool ItSaved { get; set; } = true;
        [Reactive] public TypeMessage DisplayMessageType { get; set; }
        [Reactive] public string SchemePath { get; set; }

        /// <summary>
        /// Flag for close application
        /// </summary>
        [Reactive] public bool NeedExit { get; set; }
        [Reactive] public string ImagePath { get; set; }
        [Reactive] public ImageFormats ImageFormat { get; set; }
        [Reactive] public bool WithoutMessages { get; set; }
        [Reactive] public Themes Theme { get; set; }
        [Reactive] public NodeCanvasClickMode ClickMode { get; set; } = NodeCanvasClickMode.Default;


        static Dictionary<Themes, string> themesPaths = new Dictionary<Themes, string>()
        {
            {Themes.Dark, @"Styles\Themes\Dark.xaml" },
            {Themes.Light, @"Styles\Themes\Light.xaml"},
        };

        public int NodesCount = 0;
        public int TransitionsCount = 0;
        public double ScaleMax = 5;
        public double ScaleMin = 0.2;
        public double ScaleStep { get; set; } = 1.2;
        public Point ScaleCenter { get; set; }


        public NodesCanvasViewModel()
        {
            var configuration = Locator.Current.GetService<IConfiguration>();
            Theme = configuration.GetSection("Appearance:Theme").Get<Themes>();
            if (Theme == Themes.noCorrect) Theme = Themes.Dark;
            SetTheme(Theme);
            Cutter = new CutterViewModel(this);
            Nodes.Connect().ObserveOnDispatcher().Bind(NodesForView).Subscribe();
            SetupCommands();
            SetupSubscriptions();
            SetupStartState();
        }

        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.NodesForView.Count).Buffer(2, 1).Select(x => (Previous: x[0], Current: x[1])).Subscribe(x => UpdateCount(x.Previous, x.Current));
            this.WhenAnyValue(x => x.ClickMode).Subscribe(value => ChangeMouseCursor(value));
        }

        #endregion Setup Subscriptions

        #region Setup Nodes

        private void SetupStartState()
        {
            string name = Nodes.Items.Any(x => x.Name == "Start") ? GetNameForNewNode() : "Start";
            StartState = new NodeViewModel(this, name, new Point());
            SetAsStart(StartState);
            Nodes.Add(StartState);
            ItSaved = true;
        }
        private void SetAsStart(NodeViewModel node)
        {
            node.Input.Visible = false;
            node.CanBeDelete = false;
            StartState = node;
        }

        #endregion Setup Nodes


        #region Logging

        public void LogDebug(string message, params object[] args)
        {
            if (!WithoutMessages)
                Messages.Add(new MessageViewModel(TypeMessage.Debug, string.Format(message, args)));
        }
        public void LogError(string message, params object[] args)
        {
            DisplayMessageType = TypeMessage.Error;
            if (!WithoutMessages)
                Messages.Add(new MessageViewModel(TypeMessage.Error, string.Format(message, args)));
        }
        public void LogInformation(string message, params object[] args)
        {
            if (!WithoutMessages)
                Messages.Add(new MessageViewModel(TypeMessage.Information, string.Format(message, args)));
        }
        public void LogWarning(string message, params object[] args)
        {
            if (!WithoutMessages)
                Messages.Add(new MessageViewModel(TypeMessage.Warning, string.Format(message, args)));
        }

        #endregion Logging
        private void ChangeMouseCursor(NodeCanvasClickMode clickMode)
        {
            Mouse.OverrideCursor = clickMode switch
            {
                //NodeCanvasClickMode.Default => null,
                //NodeCanvasClickMode.AddNode => null,
                NodeCanvasClickMode.Delete => Cursors.No,
                NodeCanvasClickMode.Select => Cursors.Cross,
                NodeCanvasClickMode.Cut => Cursors.Hand,
                NodeCanvasClickMode.noCorrect => throw new NotImplementedException(),
                _ => null
            };
        }
        private void UpdateCount(int oldValue, int newValue)
        {
            if (newValue > oldValue)
            {
                NodesCount++;
            }
        }
        private string SchemeName()
        {
            if (!string.IsNullOrEmpty(SchemePath))
            {
                return Path.GetFileNameWithoutExtension(SchemePath);
            }
            else
            {
                return "SimpleStateMachine";
            }
        }
    }
}
