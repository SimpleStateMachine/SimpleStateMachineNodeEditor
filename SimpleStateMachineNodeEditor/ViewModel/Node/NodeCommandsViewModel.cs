using DynamicData;
using ReactiveUI;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Windows;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public partial class NodeViewModel
    {
        public ReactiveCommand<Unit, Unit> CommandUnSelectedAllConnectors { get; set; }
        public ReactiveCommand<Unit, Unit> CommandAddEmptyConnector { get; set; }
        public ReactiveCommand<SelectMode, Unit> CommandSelect { get; set; }
        public ReactiveCommand<Point, Unit> CommandMove { get; set; }
        public ReactiveCommand<(int index, ConnectorViewModel connector), Unit> CommandAddConnectorWithConnect { get; set; }
        public ReactiveCommand<ConnectorViewModel, Unit> CommandDeleteConnectorWithConnect { get; set; }
        public ReactiveCommand<string, Unit> CommandValidateName { get; set; }

        public ReactiveCommand<ConnectorViewModel, Unit> CommandSelectWithShiftForConnectors { get; set; }
        public ReactiveCommand<ConnectorViewModel, Unit> CommandSetConnectorAsStartSelect { get; set; }

        private void SetupCommands()
        {
            CommandSelect = ReactiveCommand.Create<SelectMode>(Select);
            CommandMove = ReactiveCommand.Create<Point>(Move);
            CommandAddEmptyConnector = ReactiveCommand.Create(AddEmptyConnector);
            CommandSelectWithShiftForConnectors = ReactiveCommand.Create<ConnectorViewModel>(SelectWithShiftForConnectors);
            CommandSetConnectorAsStartSelect = ReactiveCommand.Create<ConnectorViewModel>(SetConnectorAsStartSelect);
            CommandUnSelectedAllConnectors = ReactiveCommand.Create(UnSelectedAllConnectors);
            CommandAddConnectorWithConnect = ReactiveCommand.Create<(int index, ConnectorViewModel connector)>(AddConnectorWithConnect);
            CommandDeleteConnectorWithConnect = ReactiveCommand.Create<ConnectorViewModel>(DeleteConnectorWithConnec);
            CommandValidateName = ReactiveCommand.Create<string>(ValidateName);

            NotSavedSubscrube();
        }
        private void NotSavedSubscrube()
        {
            CommandMove.Subscribe(_ => NotSaved());
            CommandAddConnectorWithConnect.Subscribe(_ => NotSaved());
            CommandDeleteConnectorWithConnect.Subscribe(_ => NotSaved());
            CommandValidateName.Subscribe(_ => NotSaved());
        }
        private void NotSaved()
        {
            NodesCanvas.ItSaved = false;
        }

        public int GetConnectorIndex(ConnectorViewModel connector)
        {
            return Transitions.Items.IndexOf(connector);
        }

        private void AddConnectorWithConnect((int index, ConnectorViewModel connector) element)
        {
            Transitions.Insert(element.index, element.connector);
            if (element.connector.Connect != null)
            {
                NodesCanvas.CommandAddConnect.ExecuteWithSubscribe(element.connector.Connect);
            }
        }
        private void DeleteConnectorWithConnec(ConnectorViewModel connector)
        {
            if (connector.Connect != null)
            {
                NodesCanvas.CommandDeleteConnect.ExecuteWithSubscribe(connector.Connect);
            }
            Transitions.Remove(connector);
        }
        private void Select(SelectMode selectMode)
        {
            if (selectMode == SelectMode.ClickWithCtrl)
            {
                this.Selected = !this.Selected;
                return;
            }
            else if ((selectMode == SelectMode.Click) && (!Selected))
            {
                NodesCanvas.CommandUnSelectAll.ExecuteWithSubscribe();
                this.Selected = true;
            }
        }
        private void Move(Point delta)
        {
            //Point moveValue = delta.Division(NodesCanvas.Scale.Value);
            Point1 = Point1.Addition(delta);
        }
        private void ValidateName(string newName)
        {

            NodesCanvas.CommandValidateNodeName.ExecuteWithSubscribe((this, newName));
        }


        private void AddEmptyConnector()
        {
            if (CurrentConnector != null)
            {
                CurrentConnector.TextEnable = true;
                CurrentConnector.FormEnable = false;
                if (string.IsNullOrEmpty(CurrentConnector.Name))
                    CurrentConnector.Name = "Transition " + NodesCanvas.TransitionsCount.ToString();
                NodesCanvas.LogDebug("Transition with name \"{0}\" was added", CurrentConnector.Name);
            }
            double width = Size.Width == 0 ? 80 : Size.Width;
            CurrentConnector = new ConnectorViewModel(NodesCanvas, this, "", Point1.Addition(width, 54))
            {
                TextEnable = false
            };
            Transitions.Insert(0, CurrentConnector);
        }
        private void UnSelectedAllConnectors()
        {
            foreach (var transition in Transitions.Items)
            {
                transition.Selected = false;
            }

            IndexStartSelectConnectors = 0;
        }
        private void SetConnectorAsStartSelect(ConnectorViewModel viewModelConnector)
        {
            IndexStartSelectConnectors = Transitions.Items.IndexOf(viewModelConnector) - 1;
        }
        private void SelectWithShiftForConnectors(ConnectorViewModel viewModelConnector)
        {
            if (viewModelConnector == null)
                return;

            var transitions = this.Transitions.Items.Skip(1);
            int indexCurrent = transitions.IndexOf(viewModelConnector);
            int indexStart = IndexStartSelectConnectors;
            UnSelectedAllConnectors();
            IndexStartSelectConnectors = indexStart;
            transitions = transitions.Skip(Math.Min(indexCurrent, indexStart)).SkipLast(Transitions.Count - Math.Max(indexCurrent, indexStart) - 2);
            foreach (var transition in transitions)
            {
                transition.Selected = true;
            }
        }
    }
}
