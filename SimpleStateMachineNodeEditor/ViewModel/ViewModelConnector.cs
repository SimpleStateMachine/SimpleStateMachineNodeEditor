using System.Windows.Media;
using System.Windows;

using ReactiveUI.Fody.Helpers;
using ReactiveUI;

using SimpleStateMachineNodeEditor.Helpers;
using System;
using System.Xml.Linq;
using System.Linq;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using System.Reactive;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using SimpleStateMachineNodeEditor.ViewModel.NodesCanvas;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelConnector : ReactiveObject
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

        #region Commands

        public ReactiveCommand<Unit,Unit> CommandConnectPointDrag { get; set; }
        public ReactiveCommand<Unit,Unit> CommandConnectPointDrop { get; set; }
        public ReactiveCommand<Unit,Unit> CommandCheckConnectPointDrop { get; set; }
        public ReactiveCommand<Unit,Unit> CommandConnectorDrag { get; set; }
        public ReactiveCommand<Unit,Unit> CommandConnectorDragEnter { get; set; }
        public ReactiveCommand<Unit,Unit> CommandConnectorDrop { get; set; }
        public ReactiveCommand<Unit,Unit> CommandSetAsLoop { get; set; }
        public ReactiveCommand<SelectMode, Unit> CommandSelect { get; set; }
        public ReactiveCommand<string, Unit> CommandValidateName { get; set; }

        private void SetupCommands()
        {
            
            CommandConnectPointDrag = ReactiveCommand.Create(ConnectPointDrag);
            CommandConnectPointDrop = ReactiveCommand.Create(ConnectPointDrop);
            CommandSetAsLoop = ReactiveCommand.Create(SetAsLoop);
            CommandCheckConnectPointDrop = ReactiveCommand.Create(CheckConnectPointDrop);

            CommandConnectorDrag = ReactiveCommand.Create(ConnectorDrag);
            CommandConnectorDragEnter = ReactiveCommand.Create(ConnectorDragEnter);
            CommandConnectorDrop = ReactiveCommand.Create(ConnectorDrop);

            CommandValidateName = ReactiveCommand.Create<string>(ValidateName);

            CommandSelect = ReactiveCommand.Create<SelectMode>(Select);


            NotSavedSubscribe();
        }
        private void NotSavedSubscribe()
        {
            CommandSetAsLoop.Subscribe(_=>NotSaved());
            CommandValidateName.Subscribe(_ => NotSaved());
        }

        private void Select(bool value)
        {

            this.Foreground = value ? Application.Current.Resources["ColorSelectedElement"] as SolidColorBrush : Application.Current.Resources["ColorConnectorForeground"] as SolidColorBrush;
            this.FormFill = value ? Application.Current.Resources["ColorSelectedElement"] as SolidColorBrush : Application.Current.Resources["ColorConnector"] as SolidColorBrush;
        }
        private void Select(SelectMode selectMode)
        {
            switch(selectMode)
            {
                case SelectMode.Click:
                    {
                        if(!this.Selected)
                        {
                            this.Node.CommandSetConnectorAsStartSelect.ExecuteWithSubscribe(this);                                  
                        }
                        
                        break;
                    }
                case SelectMode.ClickWithCtrl:
                    {
                        this.Selected = !this.Selected;
                        break;
                    }
                case SelectMode.ClickWithShift:
                    {
                        this.Node.CommandSelectWithShiftForConnectors.ExecuteWithSubscribe(this);
                        break;
                    }
            }

            
        }
        private void NotSaved()
        {
            NodesCanvas.ItSaved = false;
        }
        #endregion Commands
        private void SetAsLoop()
        {
            if (this == Node.Output)
                return;
            this.FormStrokeThickness = 0;
            this.FormFill = Application.Current.Resources["IconLoop"] as DrawingBrush;

            Node.CommandAddEmptyConnector.ExecuteWithSubscribe();
        }
        private void ConnectPointDrag()
        {
            NodesCanvas.CommandAddDraggedConnect.ExecuteWithSubscribe(Node.CurrentConnector);
        }

        private void ConnectPointDrop()
        {           
            if (NodesCanvas.DraggedConnect.FromConnector.Node != this.Node)
            {
                var connect = NodesCanvas.DraggedConnect;
                connect.ToConnector = this;
            }

        }

        private void CheckConnectPointDrop()
        {
            if (NodesCanvas.DraggedConnect.ToConnector == null)
            {
                NodesCanvas.CommandDeleteDraggedConnect.ExecuteWithSubscribe();
            }
            else
            {
                NodesCanvas.CommandAddConnectorWithConnect.Execute(Node.CurrentConnector);
                Node.CommandAddEmptyConnector.ExecuteWithSubscribe();
                NodesCanvas.DraggedConnect = null;
            }
        }

        private void ConnectorDrag()
        {            
            NodesCanvas.ConnectorPreviewForDrop = this;
        }
        private void ConnectorDragEnter()
        {
            if (Node != NodesCanvas.ConnectorPreviewForDrop.Node)
                return;

            int indexTo = Node.Transitions.IndexOf(this);
            if (indexTo == 0)
                return;

            int count = this.Node.Transitions.Count;
            int indexFrom = this.Node.Transitions.IndexOf(this.NodesCanvas.ConnectorPreviewForDrop);
            
            if ((indexFrom > -1) && (indexTo > -1) && (indexFrom < count) && (indexTo < count))
            {
                MyPoint positionTo = this.Node.Transitions[indexTo].PositionConnectPoint;
                MyPoint position;
                //shift down
                if (indexTo > indexFrom)
                {
                    for (int i = indexTo ; i >= indexFrom + 1; i--)
                    {
                        position = this.Node.Transitions[i -1].PositionConnectPoint;
                        this.Node.Transitions[i].PositionConnectPoint.Set(position);
                    }
                }
                //shift up
                else if ( indexFrom > indexTo)
                {
                    for (int i = indexTo; i <= indexFrom - 1; i++)
                    {
                        position = this.Node.Transitions[i + 1].PositionConnectPoint;
                        this.Node.Transitions[i].PositionConnectPoint.Set(position);
                    }
                }
                this.Node.Transitions[indexFrom].PositionConnectPoint.Set(positionTo);
                this.Node.Transitions.Move(indexFrom, indexTo);
            }
        }
        private void ConnectorDrop()
        {
            this.NodesCanvas.ConnectorPreviewForDrop = null;
        }
        private void ValidateName(string newName)
        {
            NodesCanvas.CommandValidateConnectName.ExecuteWithSubscribe((this, newName));
        }

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
