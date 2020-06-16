using System;
using System.Windows;
using System.Windows.Media;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;

using DynamicData.Binding;
using System.Linq;
using System.Xml.Linq;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using DynamicData;
using System.Collections.ObjectModel;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public partial class ViewModelNode : ReactiveValidationObject<ViewModelNode>
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
        [Reactive] public ViewModelConnector Input { get; set; }
        [Reactive] public ViewModelConnector Output { get; set; }
        [Reactive] public ViewModelConnector CurrentConnector { get; set; }
        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }
        [Reactive] public int IndexStartSelectConnectors { get; set; } = 0;

        [Reactive] public double HeaderWidth { get; set; } = 80;

        public SourceList<ViewModelConnector> Transitions { get; set; } = new SourceList<ViewModelConnector>();
        public ObservableCollectionExtended<ViewModelConnector> Transitions2  = new ObservableCollectionExtended<ViewModelConnector>();
        public int Zindex { get; private set; }
        
        private ViewModelNode()
        {
            SetupCommands();
            SetupBinding();
        }


        public ViewModelNode(ViewModelNodesCanvas nodesCanvas, string name, Point point)
        {
            NodesCanvas = nodesCanvas;
            Name = name;
            Zindex = nodesCanvas.Nodes.Count;
            Point1 = point;
            Transitions.Connect().ObserveOnDispatcher().Bind(Transitions2).Subscribe();
            SetupConnectors();
        
            SetupCommands();
            SetupBinding();
            SetupSubscriptions();
        }

        #region SetupBinding
        private void SetupBinding()
        {
        }
        #endregion SetupBinding

        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.Selected).Subscribe(value => { this.BorderBrush = value ? Application.Current.Resources["ColorSelectedElement"] as SolidColorBrush : Brushes.LightGray; });
            this.WhenAnyValue(x => x.Transitions2.Count).Buffer(2, 1).Select(x => (Previous: x[0], Current: x[1])).Subscribe(x => UpdateCount(x.Previous, x.Current));
            this.WhenAnyValue(x => x.Point1, x => x.Size).Subscribe(_ => UpdatePoint2());
            this.WhenAnyValue(x => x.IsCollapse).Subscribe(value => Collapse(value));

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
            Input = new ViewModelConnector(NodesCanvas, this, "Input", Point1.Addition(0, 30));
            Output = new ViewModelConnector(NodesCanvas, this, "Output", Point1.Addition(80, 54))
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
            if ((oldValue>0)&&(newValue > oldValue))
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
            element.Add(new XAttribute("Position", PointExtensition.PointToString(Point1)));
            element.Add(new XAttribute("IsCollapse", IsCollapse.ToString()));
            return element;
        }

        public static ViewModelNode FromXElement(ViewModelNodesCanvas nodesCanvas, XElement node, out string errorMessage, Func<string, bool> actionForCheck)
        {
            errorMessage = null;
            ViewModelNode viewModelNode = null;
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

            var position =  node.Attribute("Position")?.Value;
            Point point = string.IsNullOrEmpty(position) ? new Point() : PointExtensition.StringToPoint(position);
            viewModelNode = new ViewModelNode(nodesCanvas, name, point);
            var isCollapse = node.Attribute("IsCollapse")?.Value;
            if (isCollapse != null)
                viewModelNode.IsCollapse = bool.Parse(isCollapse);




            return viewModelNode;
        }
    }
}
