using System.Windows.Media;
using System.Windows;

using ReactiveUI.Fody.Helpers;
using ReactiveUI;

using SimpleStateMachineNodeEditor.Helpers;
using System;
using System.Xml.Linq;
using System.Linq;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using System.Reactive.Linq;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public partial class ViewModelConnector : ReactiveObject
    {
        [Reactive] public Point PositionConnectPoint { get; set; } 
        [Reactive] public string Name { get; set; }
        [Reactive] public bool TextEnable { get; set; } = false;
        [Reactive] public bool? Visible { get; set; } = true;
        [Reactive] public bool FormEnable { get; set; } = true;
        [Reactive] public Brush FormStroke { get; set; } = Application.Current.Resources["ColorNodesCanvasBackground"] as SolidColorBrush;
        [Reactive] public Brush FormFill { get; set; } = Application.Current.Resources["ColorConnector"] as SolidColorBrush;
        [Reactive] public Brush Foreground { get; set; } = Application.Current.Resources["ColorConnectorForeground"] as SolidColorBrush;
        [Reactive] public double FormStrokeThickness { get; set; } = 1;
        [Reactive] public ViewModelNode Node { get; set; }
        [Reactive] public ViewModelConnect Connect { get; set; }
        [Reactive] public bool ItsLoop { get; set; } = false;
        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }
        [Reactive] public bool Selected { get; set; }

        private IDisposable subscriptionOnNodeWidthChange;
        public ViewModelConnector(ViewModelNodesCanvas nodesCanvas, ViewModelNode viewModelNode, string name, Point myPoint)
        {
            Node = viewModelNode;
            NodesCanvas = nodesCanvas;
            Name = name;
            PositionConnectPoint = myPoint;
            SetupCommands();
            SetupSubscriptions();
        }
        #region Setup Subscriptions
        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.Selected).Subscribe(value => Select(value));
            

            if(this.Name!="Input")
            {
                if (this.Name != "Output")
                {
                    this.WhenAnyValue(x => x.Node.Transitions.Count).Subscribe(x => UpdatePositionOnTransitionCountChange());
                    this.WhenAnyValue(x => x.Node.IsCollapse).Subscribe(value => UpdateSubscriptionForPosition(value));
                }             
            }

            this.WhenAnyValue(x => x.Node.Point1).Buffer(2, 1).Subscribe(value => PositionConnectPoint = PositionConnectPoint.Addition(value[1].Subtraction(value[0])));
        }
        private void UpdateSubscriptionForPosition(bool nodeIsCollapse)
        {
            if (!nodeIsCollapse)
            {
                subscriptionOnNodeWidthChange = this.WhenAnyValue(x => x.Node.Size.Width).Buffer(2, 1).Where(x => x[0] >= 80 && x[1] >= 80 && x[1]!=Node.WidthBeforeCollapse)
                    .Subscribe(x => UpdatePositionOnWidthChange(x[1] - x[0]));
            }
            else
            {
                subscriptionOnNodeWidthChange?.Dispose();
            }
        }
        private void UpdatePositionOnTransitionCountChange()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                int index = Node.Transitions.IndexOf(this);
                this.PositionConnectPoint = Node.Output.PositionConnectPoint.Addition(0, index*19);
            }
        }
        private void UpdatePositionOnWidthChange(double value)
        {
            this.PositionConnectPoint = this.PositionConnectPoint.Addition(value, 0);
        }

        #endregion Setup Subscriptions
        public XElement ToXElement()
        {
            XElement element = new XElement("Transition");
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("From", Node.Name));
            var ToConnectorName = this.Connect?.ToConnector?.Node.Name;
            element.Add(new XAttribute("To", ToConnectorName?? Node.Name));

            return element;
        }

        public static ViewModelConnect FromXElement(ViewModelNodesCanvas nodesCanvas,XElement node, out string errorMessage, Func<string, bool> actionForCheck)
        {
            ViewModelConnect viewModelConnect = null;

            errorMessage = null;
            string name = node.Attribute("Name")?.Value;
            string from = node.Attribute("From")?.Value;
            string to = node.Attribute("To")?.Value;

            if (string.IsNullOrEmpty(name))
            {
                errorMessage = "Connect without name";
                return viewModelConnect;
            }
            if (string.IsNullOrEmpty(from))
            {
                errorMessage = "Connect without from point";
                return viewModelConnect;
            }
            if (string.IsNullOrEmpty(to))
            {
                errorMessage = "Connect without to point";
                return viewModelConnect;
            }
            if (actionForCheck(name))
            {
                errorMessage = String.Format("Contains more than one connect with name \"{0}\"", name);
                return viewModelConnect;
            }

            ViewModelNode nodeFrom = nodesCanvas.Nodes.Single(x => x.Name == from);
            ViewModelNode nodeTo = nodesCanvas.Nodes.Single(x => x.Name == to);
          
            nodeFrom.CurrentConnector.Name = name;


            if (nodeFrom == nodeTo)
            {
                nodeFrom.CurrentConnector.CommandSetAsLoop.ExecuteWithSubscribe();
            }
            else
            {
                viewModelConnect = new ViewModelConnect(nodeFrom.NodesCanvas, nodeFrom.CurrentConnector);
                viewModelConnect.ToConnector = nodeTo.Input;
                nodeFrom.CommandAddEmptyConnector.ExecuteWithSubscribe();
            }     

            return viewModelConnect;
        }
    }

}
