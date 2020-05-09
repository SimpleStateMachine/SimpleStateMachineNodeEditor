using System.Windows.Media;
using System.Windows;

using ReactiveUI.Fody.Helpers;
using ReactiveUI;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using System;
using System.Xml.Linq;
using System.Linq;
using SimpleStateMachineNodeEditor.Helpers.Enums;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelConnector : ReactiveObject
    {
        /// <summary>
        /// Координата перехода ( нужна для создания соединения )
        /// </summary>
        [Reactive] public MyPoint PositionConnectPoint { get; set; } = new MyPoint();

        /// <summary>
        /// Имя перехода ( вводится в узле)
        /// </summary>
        [Reactive] public string Name { get; set; }

        /// <summary>
        /// Доступно ли имя перехода для редактирования
        /// </summary>
        [Reactive] public bool TextEnable { get; set; } = false;

        /// <summary>
        /// Отображается ли переход
        /// </summary>
        [Reactive] public bool? Visible { get; set; } = true;

        /// <summary>
        /// Ellipse enable
        /// </summary>
        [Reactive] public bool FormEnable { get; set; } = true;

        /// <summary>
        /// Ellipse stroke color
        /// </summary>
        [Reactive] public Brush FormStroke { get; set; } = Application.Current.Resources["ColorConnectorEllipseEnableBorder"] as SolidColorBrush;

        /// <summary>
        /// Ellipse fill color
        /// </summary>
        [Reactive] public Brush FormFill { get; set; } = Application.Current.Resources["ColorConnectorEllipseEnableBackground"] as SolidColorBrush;

        [Reactive] public Brush Foreground { get; set; } = Application.Current.Resources["ColorForeground"] as SolidColorBrush;

        [Reactive] public double FormStrokeThickness { get; set; } = 1;

        /// <summary>
        /// Узел, которому принадлежит переход
        /// </summary>
        [Reactive] public ViewModelNode Node { get; set; }

        /// <summary>
        /// Соединение, которое связанно с этим переходом
        /// </summary>
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
            this.WhenAnyValue(x => x.Selected).Subscribe(value => { this.Foreground = value ? Application.Current.Resources["ColorSelectedElement"] as SolidColorBrush : Application.Current.Resources["ColorForeground"] as SolidColorBrush; });
        }
        #endregion SetupBinding

        #region Commands

        public SimpleCommand CommandConnectPointDrag { get; set; }
        public SimpleCommand CommandConnectPointDrop { get; set; }
        public SimpleCommand CommandCheckConnectPointDrop { get; set; }
        public SimpleCommand CommandConnectorDrag { get; set; }
        public SimpleCommand CommandConnectorDragEnter { get; set; }
        public SimpleCommand CommandConnectorDrop { get; set; }
        public SimpleCommand CommandSetAsLoop { get; set; }
        public SimpleCommand CommandAdd { get; set; }
        public SimpleCommand CommandDelete { get; set; }
        public SimpleCommandWithParameter<SelectMode> CommandSelect { get; set; }
        public SimpleCommandWithParameter<string> CommandValidateName { get; set; }

        private void SetupCommands()
        {
            
            CommandConnectPointDrag = new SimpleCommand(ConnectPointDrag);
            CommandConnectPointDrop = new SimpleCommand(ConnectPointDrop);
            CommandSetAsLoop = new SimpleCommand(SetAsLoop, NotSaved);
            CommandCheckConnectPointDrop = new SimpleCommand(CheckConnectPointDrop);

            CommandConnectorDrag = new SimpleCommand(ConnectorDrag);
            CommandConnectorDragEnter = new SimpleCommand(ConnectorDragEnter);
            CommandConnectorDrop = new SimpleCommand(ConnectorDrop);

            CommandAdd = new SimpleCommand(Add);
            CommandDelete = new SimpleCommand(Delete);
            CommandValidateName = new SimpleCommandWithParameter<string>(ValidateName, NotSaved);

            CommandSelect = new SimpleCommandWithParameter<SelectMode>(Select);


            //SimpleCommandWithResult<bool, Func<bool>> t = new SimpleCommandWithResult<bool, Func<bool>>()
        }

        private void Select(SelectMode selectMode)
        {
            switch(selectMode)
            {
                case SelectMode.Click:
                    {
                        if(!this.Selected)
                        {
                            //this.Node.CommandUnSelectedAllConnectors.Execute();
                            this.Node.CommandSetConnectorAsStartSelect.Execute(this);
                            //this.Selected = true;                                     
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
                        this.Node.CommandSelectWithShiftForConnectors.Execute(this);
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
            this.FormFill = Application.Current.Resources["ColorRightConnectorEllipseLoop"] as DrawingBrush;

            Node.CommandAddEmptyConnector.Execute();
        }
        private void Add()
        {

            Node.CommandAddConnector.Execute(this);
        }
        private void Delete()
        {
            Node.CommandDeleteConnector.Execute(this);
        }
        private void ConnectPointDrag()
        {
            Node.NodesCanvas.CommandAddFreeConnect.Execute(Node.CurrentConnector);
        }
        private void DragConnector(ViewModelConnector draggedConnector)
        {

        }

        private void ConnectPointDrop()
        {
           
            if (Node.NodesCanvas.DraggedConnect.FromConnector.Node != this.Node)
            {
                var connect = Node.NodesCanvas.DraggedConnect;
                connect.ToConnector = this;
            }

        }

        private void CheckConnectPointDrop()
        {
            if (Node.NodesCanvas.DraggedConnect.ToConnector == null)
            {
                Node.NodesCanvas.CommandDeleteFreeConnect.Execute();
            }
            else
            {
                Node.CommandAddEmptyConnector.Execute();
                Node.NodesCanvas.CommandAddConnect.Execute(Node.NodesCanvas.DraggedConnect);
                Node.NodesCanvas.DraggedConnect = null;
            }
        }

        private void ConnectorDrag()
        {            
            this.NodesCanvas.ConnectorPreviewForDrop = this;
        }
        private void ConnectorDragEnter()
        {
            if (this.Node != this.NodesCanvas.ConnectorPreviewForDrop.Node)
                return;

            int indexTo = this.Node.Transitions.IndexOf(this);
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
            NodesCanvas.CommandValidateConnectName.Execute(new ValidateObjectProperty<ViewModelConnector, string>(this, newName));
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

        public static ViewModelConnect FromXElement(ViewModelNodesCanvas nodesCanvas, XElement node, out string errorMessage, Func<string, bool> actionForCheck)
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
                nodeFrom.CurrentConnector.CommandSetAsLoop.Execute();
            }
            else
            {
                viewModelConnect = new ViewModelConnect(nodeFrom.CurrentConnector);
                viewModelConnect.ToConnector = nodeTo.Input;
                nodeFrom.CommandAddEmptyConnector.Execute();
            }     

            return viewModelConnect;
        }
    }

}
