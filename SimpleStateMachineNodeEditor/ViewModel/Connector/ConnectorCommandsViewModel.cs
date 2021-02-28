using DynamicData;
using ReactiveUI;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using System;
using System.Linq;
using System.Reactive;
using System.Windows;
using System.Windows.Media;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public partial class ConnectorViewModel
    {
        public ReactiveCommand<Unit, Unit> CommandConnectPointDrag { get; set; }
        public ReactiveCommand<Unit, Unit> CommandConnectPointDrop { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCheckConnectPointDrop { get; set; }
        public ReactiveCommand<Unit, Unit> CommandConnectorDrag { get; set; }
        public ReactiveCommand<Unit, Unit> CommandConnectorDragEnter { get; set; }
        public ReactiveCommand<Unit, Unit> CommandConnectorDrop { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSetAsLoop { get; set; }
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
            CommandSetAsLoop.Subscribe(_ => NotSaved());
            CommandValidateName.Subscribe(_ => NotSaved());
        }

        private void NotSaved()
        {
            NodesCanvas.ItSaved = false;
        }

        private void SetAsLoop()
        {
            if (this == Node.Output)
                return;
            ToLoop();
            ItsLoop = true;
            Node.CommandAddEmptyConnector.ExecuteWithSubscribe();
        }
        private void ToLoop()
        {            
            FormStrokeThickness = 0;
            FormFill = Application.Current.Resources["IconLoop"] as DrawingBrush;
        }
        private void ConnectPointDrag()
        {
            NodesCanvas.CommandAddDraggedConnect.ExecuteWithSubscribe(Node.CurrentConnector);
        }

        private void ConnectPointDrop()
        {
            var connect = NodesCanvas.DraggedConnect;
            if (connect.FromConnector.Node != Node)
            {              
                connect.ToConnector = this;
            }
            else
            {
                connect.FromConnector.SetAsLoop();
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

            int indexTo = Node.Transitions.Items.IndexOf(this);
            if (indexTo == 0)
                return;
            int count = Node.Transitions.Count;
            int indexFrom = Node.Transitions.Items.IndexOf(NodesCanvas.ConnectorPreviewForDrop);

            if ((indexFrom > -1) && (indexTo > -1) && (indexFrom < count) && (indexTo < count))
            {
                Point positionTo = Node.Transitions.Items.ElementAt(indexTo).PositionConnectPoint;
                Point position;
                //shift down
                if (indexTo > indexFrom)
                {
                    for (int i = indexTo; i >= indexFrom + 1; i--)
                    {
                        position = Node.Transitions.Items.ElementAt(i - 1).PositionConnectPoint;
                        Node.Transitions.Items.ElementAt(i).PositionConnectPoint = position;
                    }
                }
                //shift up
                else if (indexFrom > indexTo)
                {
                    for (int i = indexTo; i <= indexFrom - 1; i++)
                    {
                        position = Node.Transitions.Items.ElementAt(i + 1).PositionConnectPoint;
                        Node.Transitions.Items.ElementAt(i).PositionConnectPoint = position;
                    }
                }
                Node.Transitions.Items.ElementAt(indexFrom).PositionConnectPoint = positionTo;
                Node.Transitions.Move(indexFrom, indexTo);
            }
        }
        private void ConnectorDrop()
        {
            NodesCanvas.ConnectorPreviewForDrop = null;
        }
        private void ValidateName(string newName)
        {
            NodesCanvas.CommandValidateConnectName.ExecuteWithSubscribe((this, newName));
        }


        private void Select(bool value)
        {
            FormStroke = Application.Current.Resources["ColorNodesCanvasBackground"] as SolidColorBrush;
            Foreground = Application.Current.Resources[Selected ? "ColorSelectedElement" : "ColorConnectorForeground"] as SolidColorBrush;
            if (!ItsLoop)
                FormFill = Application.Current.Resources[Selected ? "ColorSelectedElement" : "ColorConnector"] as SolidColorBrush;
            else
                FormFill = Application.Current.Resources[Selected ? "IconSelectedLoop" : "IconLoop"] as DrawingBrush;

        }
        private void Select(SelectMode selectMode)
        {
            switch (selectMode)
            {
                case SelectMode.Click:
                    {
                        if (!Selected)
                        {
                            Node.CommandSetConnectorAsStartSelect.ExecuteWithSubscribe(this);
                        }

                        break;
                    }
                case SelectMode.ClickWithCtrl:
                    {
                        Selected = !Selected;
                        break;
                    }
                case SelectMode.ClickWithShift:
                    {
                        Node.CommandSelectWithShiftForConnectors.ExecuteWithSubscribe(this);
                        break;
                    }
            }


        }
    }
}
