using System.Windows.Media;
using System.Windows;

using ReactiveUI.Fody.Helpers;
using ReactiveUI;

using StateMachineNodeEditorNerCore.Helpers;
using StateMachineNodeEditorNerCore.Helpers.Commands;

namespace StateMachineNodeEditorNerCore.ViewModel
{
    public class ViewModelConnector : ReactiveObject
    {
        /// <summary>
        /// Координата самого коннектора
        /// </summary>
        [Reactive]
        public MyPoint Position { get; set; } = new MyPoint();

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
        [Reactive] public Brush FormStroke { get; set; } = Brushes.Black;

        /// <summary>
        /// Цвет перехода
        /// </summary>
        [Reactive] public Brush FormFill { get; set; } = Brushes.DarkGray;

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
        public SimpleCommand CommandConnectorDragOver { get; set; }
        public SimpleCommand CommandConnectorDragLeave { get; set; }
        public SimpleCommand CommandConnectorDrop { get; set; }

        public SimpleCommand CommandCheckConnectorDrop { get; set; }


        public SimpleCommand CommandAdd { get; set; }
        public SimpleCommand CommandDelete { get; set; }



        public SimpleCommandWithParameter<string> CommandValidateName { get; set; }
        public SimpleCommandWithParameter<MyPoint> CommandMove { get; set; }

        private void SetupCommands()
        {

            CommandConnectPointDrag = new SimpleCommand(this, ConnectPointDrag);
            CommandConnectPointDrop = new SimpleCommand(this, ConnectPointDrop);

            CommandCheckConnectPointDrop = new SimpleCommand(this, CheckConnectPointDrop);

            CommandConnectorDrag = new SimpleCommand(this, ConnectorDrag);
            CommandConnectorDragEnter = new SimpleCommand(this, ConnectorDragEnter);
            CommandConnectorDragOver = new SimpleCommand(this, ConnectorDragOver);
            CommandConnectorDragLeave = new SimpleCommand(this, ConnectorDragLeave);

            CommandConnectorDrop = new SimpleCommand(this, ConnectorDrop);
            CommandCheckConnectorDrop = new SimpleCommand(this, CheckConnectorDrop);






            CommandAdd = new SimpleCommand(this, Add);
            CommandDelete = new SimpleCommand(this, Delete);
            CommandMove = new SimpleCommandWithParameter<MyPoint>(this, Move);
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
        private void Move(MyPoint delta)
        {
            Position += delta / NodesCanvas.Scale.Value;
        }
        private void ConnectPointDrop()
        {
            if (Node.NodesCanvas.DraggedConnect.FromConnector.Node != this.Node)
            {
                Node.NodesCanvas.DraggedConnect.ToConnector = this;
            }
            else
            {

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
            int index = this.Node.Transitions.IndexOf(this);
            if (index == -1)
                index = 0;

            this.NodesCanvas.ConnectorPreviewForDrop = this.NodesCanvas.DraggedConnector;
            this.NodesCanvas.DraggedConnector = null;

            this.Node.Transitions.Insert(index + 1, this.NodesCanvas.ConnectorPreviewForDrop);
            this.NodesCanvas.ConnectorPreviewForDrop.Position.Clear();
            this.NodesCanvas.ConnectorPreviewForDrop.Node = this.Node;
        }
        private void ConnectorDragLeave()
        {
            this.Node.Transitions.Remove(this.NodesCanvas.ConnectorPreviewForDrop);
            this.NodesCanvas.DraggedConnector = this.NodesCanvas.ConnectorPreviewForDrop;
            this.NodesCanvas.ConnectorPreviewForDrop = null;

        }
        private void ConnectorDragOver()
        {
            //if (this.NodesCanvas.ConnectorPreviewForDrop != this)
            //{
            //    this.Node.Point1 += 0.0001;
            //    return;
            //}
            this.Node.Transitions.Remove(this.NodesCanvas.ConnectorPreviewForDrop);
            this.NodesCanvas.DraggedConnector = this.NodesCanvas.ConnectorPreviewForDrop;
            this.NodesCanvas.ConnectorPreviewForDrop = null;
            return;
        }
        private void ConnectorDrop()
        {
            this.NodesCanvas.ConnectorPreviewForDrop = null;
        }
        private void CheckConnectorDrop()
        {
            //if (Node.NodesCanvas.CurrentConnect.ToConnector == null)
            //{
            //    Node.NodesCanvas.CommandDeleteFreeConnect.Execute();
            //}
            //else
            //{
            //    Node.CommandAddEmptyConnector.Execute();
            //    Node.NodesCanvas.CommandAddConnect.Execute(Node.NodesCanvas.CurrentConnect);
            //    Node.NodesCanvas.CurrentConnect = null;
            //}
        }
        private void ValidateName(string newName)
        {
            NodesCanvas.CommandValidateConnectName.Execute(new ValidateObjectProperty<ViewModelConnector, string>(this, newName));
        }
    }

}
