using System.Windows.Media;
using System.Windows;

using ReactiveUI.Fody.Helpers;
using ReactiveUI;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using System;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelConnector : ReactiveObject
    {
        ///// <summary>
        ///// Координата самого коннектора
        ///// </summary>
        //[Reactive]
        //public MyPoint Position { get; set; } = new MyPoint();

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
        /// Размер узла
        /// </summary>
        [Reactive] public Size Size { get; set; }

        /// <summary>
        /// Доступен ли переход для создания соединия
        /// </summary>
        [Reactive] public bool FormEnable { get; set; } = true;

        /// <summary>
        /// Цвет рамки, вокруг перехода
        /// </summary>
        [Reactive] public Brush FormStroke { get; set; }

        /// <summary>
        /// Цвет перехода
        /// </summary>
        [Reactive] public Brush FormFill { get; set; }

        /// <summary>
        /// Узел, которому принадлежит переход
        /// </summary>
        [Reactive] public ViewModelNode Node { get; set; }

        /// <summary>
        /// Соединение, которое связанно с этим переходом
        /// </summary>
        [Reactive] public ViewModelConnect Connect { get; set; }

        /// <summary>
        /// Канвас, которому принадлежит соединение
        /// </summary>
        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }

        public ViewModelConnector(ViewModelNodesCanvas nodesCanvas, ViewModelNode viewModelNode)
        {
            Node = viewModelNode;
            NodesCanvas = nodesCanvas;
            SetupCommands();
        }
        #region Commands

        public SimpleCommand CommandConnectPointDrag { get; set; }
        public SimpleCommand CommandConnectPointDrop { get; set; }
        public SimpleCommand CommandCheckConnectPointDrop { get; set; }


        public SimpleCommand CommandConnectorDrag { get; set; }
        public SimpleCommand CommandConnectorDragEnter { get; set; }
        public SimpleCommand CommandConnectorDrop { get; set; }



        public SimpleCommand CommandAdd { get; set; }
        public SimpleCommand CommandDelete { get; set; }
        public SimpleCommandWithParameter<string> CommandValidateName { get; set; }

        private void SetupCommands()
        {

            CommandConnectPointDrag = new SimpleCommand(this, ConnectPointDrag);
            CommandConnectPointDrop = new SimpleCommand(this, ConnectPointDrop);

            CommandCheckConnectPointDrop = new SimpleCommand(this, CheckConnectPointDrop);

            CommandConnectorDrag = new SimpleCommand(this, ConnectorDrag);
            CommandConnectorDragEnter = new SimpleCommand(this, ConnectorDragEnter);
            CommandConnectorDrop = new SimpleCommand(this, ConnectorDrop);

            CommandAdd = new SimpleCommand(this, Add);
            CommandDelete = new SimpleCommand(this, Delete);
            CommandValidateName = new SimpleCommandWithParameter<string>(this, ValidateName);


            //SimpleCommandWithResult<bool, Func<bool>> t = new SimpleCommandWithResult<bool, Func<bool>>()
        }
        #endregion Commands

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
            //if (Node.NodesCanvas.DraggedConnect.FromConnector.Node != this.Node)
            //{
                Node.NodesCanvas.DraggedConnect.ToConnector = this;
            //}
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
    }

}
