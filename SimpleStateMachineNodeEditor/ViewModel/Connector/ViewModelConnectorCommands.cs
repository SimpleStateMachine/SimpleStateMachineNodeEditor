using ReactiveUI;
using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using System;
using System.Reactive;
using System.Windows;
using System.Windows.Media;

namespace SimpleStateMachineNodeEditor.ViewModel.Connector
{
    public partial class ViewModelConnector
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
                    for (int i = indexTo; i >= indexFrom + 1; i--)
                    {
                        position = this.Node.Transitions[i - 1].PositionConnectPoint;
                        this.Node.Transitions[i].PositionConnectPoint.Set(position);
                    }
                }
                //shift up
                else if (indexFrom > indexTo)
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


        private void Select(bool value)
        {

            this.Foreground = value ? Application.Current.Resources["ColorSelectedElement"] as SolidColorBrush : Application.Current.Resources["ColorConnectorForeground"] as SolidColorBrush;
            this.FormFill = value ? Application.Current.Resources["ColorSelectedElement"] as SolidColorBrush : Application.Current.Resources["ColorConnector"] as SolidColorBrush;
        }
        private void Select(SelectMode selectMode)
        {
            switch (selectMode)
            {
                case SelectMode.Click:
                    {
                        if (!this.Selected)
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
    }
}
