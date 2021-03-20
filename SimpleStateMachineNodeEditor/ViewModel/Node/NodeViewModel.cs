using System;
using System.Windows;
using System.Windows.Media;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;

using DynamicData.Binding;
using System.Xml.Linq;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using DynamicData;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public partial class NodeViewModel : ReactiveValidationObject
    {
        [Reactive] public Point Point1 { get; set; }
        [Reactive] public Point Point2 { get; set; }
        [Reactive] public Size Size { get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public bool NameEnable { get; set; } = true;
        [Reactive] public bool Selected { get; set; }
        [Reactive] public Brush BorderBrush { get; set; } = Application.Current.Resources["ColorNodeBorder"] as SolidColorBrush;
        [Reactive] public bool? TransitionsVisible { get; set; } = true;
        [Reactive] public bool? RollUpVisible { get; set; } = true;
        [Reactive] public bool CanBeDelete { get; set; } = true;
        [Reactive] public bool IsCollapse { get; set; }
        [Reactive] public ConnectorViewModel Input { get; set; }
        [Reactive] public ConnectorViewModel Output { get; set; }
        [Reactive] public ConnectorViewModel CurrentConnector { get; set; }
        [Reactive] public NodesCanvasViewModel NodesCanvas { get; set; }
        [Reactive] public int IndexStartSelectConnectors { get; set; } = 0;

        [Reactive] public double HeaderWidth { get; set; } = 80;

        public SourceList<ConnectorViewModel> Transitions { get; set; } = new SourceList<ConnectorViewModel>();
        public ObservableCollectionExtended<ConnectorViewModel> TransitionsForView = new ObservableCollectionExtended<ConnectorViewModel>();

        private NodeViewModel()
        {
            SetupCommands();
        }


        public NodeViewModel(NodesCanvasViewModel nodesCanvas, string name, Point point = default(Point))
        {
            NodesCanvas = nodesCanvas;
            Name = name;
            Point1 = point;
            Transitions.Connect().ObserveOnDispatcher().Bind(TransitionsForView).Subscribe();
            SetupConnectors();

            SetupCommands();
            SetupSubscriptions();
        }

        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.Selected).Subscribe(value => { BorderBrush = value ? Application.Current.Resources["ColorSelectedElement"] as SolidColorBrush : Brushes.LightGray; });
            this.WhenAnyValue(x => x.TransitionsForView.Count).Buffer(2, 1).Select(x => (Previous: x[0], Current: x[1])).Subscribe(x => UpdateCount(x.Previous, x.Current));
            this.WhenAnyValue(x => x.Point1, x => x.Size).Subscribe(_ => UpdatePoint2());
            this.WhenAnyValue(x => x.IsCollapse).Subscribe(Collapse);

            //this.WhenAnyValue(x => x.Transitions.Count).Subscribe(value => UpdateCount(value));
        }
        private void UpdateCount(int value)
        {
            NodesCanvas.TransitionsCount++;
        }
        #endregion Setup Subscriptions
        #region Connectors
        private void SetupConnectors()
        {
            Input = new ConnectorViewModel(NodesCanvas, this, "Input", Point1.Addition(0, 30));
            Output = new ConnectorViewModel(NodesCanvas, this, "Output", Point1.Addition(80, 54))
            {
                Visible = null
            };
            AddEmptyConnector();
        }
        #endregion Connectors
        private void UpdatePoint2()
        {
            Point2 = Point1.Addition(Size);
        }
        private void UpdateCount(int oldValue, int newValue)
        {
            if ((oldValue > 0) && (newValue > oldValue))
            {
                NodesCanvas.TransitionsCount++;
            }
        }
        private void Collapse(bool value)
        {
            if (!value)
            {

                TransitionsVisible = true;
                Output.Visible = null;
            }
            else
            {
                TransitionsVisible = null;
                Output.Visible = true;
                UnSelectedAllConnectors();
            }
            NotSaved();
        }


        public XElement ToXElement()
        {
            XElement element = new XElement("State");
            element.Add(new XAttribute("Name", Name));
            return element;
        }
        public XElement ToVisualizationXElement()
        {
            XElement element = ToXElement();
            element.Add(new XAttribute("Position", PointExtensition.PointToString(Point1)));
            element.Add(new XAttribute("IsCollapse", IsCollapse.ToString()));
            return element;
        }
        public static NodeViewModel FromXElement(NodesCanvasViewModel nodesCanvas, XElement node, out string errorMessage, Func<string, bool> actionForCheck)
        {
            errorMessage = null;
            NodeViewModel viewModelNode = null;
            string name = node.Attribute("Name")?.Value;

            if (string.IsNullOrEmpty(name))
            {
                errorMessage = "Node without name";
                return viewModelNode;
            }

            if (actionForCheck(name))
            {
                errorMessage = String.Format("Contains more than one node with name \"{0}\"", name);
                return viewModelNode;
            }

            viewModelNode = new NodeViewModel(nodesCanvas, name);

            return viewModelNode;
        }
    }
}
