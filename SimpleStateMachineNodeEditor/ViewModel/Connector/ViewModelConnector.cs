using System.Windows.Media;
using System.Windows;

using ReactiveUI.Fody.Helpers;
using ReactiveUI;

using SimpleStateMachineNodeEditor.Helpers;
using System;
using System.Xml.Linq;
using System.Linq;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using SimpleStateMachineNodeEditor.ViewModel.NodesCanvas;
using SimpleStateMachineNodeEditor.ViewModel.Connect;

namespace SimpleStateMachineNodeEditor.ViewModel.Connector
{
    public partial class ViewModelConnector : ReactiveObject
    {
        [Reactive] public MyPoint PositionConnectPoint { get; set; } = new MyPoint();
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

        public ViewModelConnector(ViewModelNodesCanvas nodesCanvas, ViewModelNode viewModelNode)
        {
            Node = viewModelNode;
            NodesCanvas = nodesCanvas;
            SetupCommands();
            SetupBinding();
        }

        #region SetupBinding
        private void SetupBinding()
        {
            this.WhenAnyValue(x => x.Selected).Subscribe(value => Select(value));
        }
        #endregion SetupBinding

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
