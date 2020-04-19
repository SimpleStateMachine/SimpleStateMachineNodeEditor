using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using DynamicData;
using DynamicData.Binding;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using SimpleStateMachineNodeEditor.Helpers.Transformations;
using System.IO;
using System.Windows.Data;
using System.Xml.Linq;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelNodesCanvas : ReactiveObject
    {
        public IObservableCollection<ViewModelConnect> Connects = new ObservableCollectionExtended<ViewModelConnect>();
        public IObservableCollection<ViewModelNode> Nodes = new ObservableCollectionExtended<ViewModelNode>();
        [Reactive] public ViewModelSelector Selector { get; set; } = new ViewModelSelector();
        [Reactive] public ViewModelCutter Cutter { get; set; }
        [Reactive] public ViewModelConnect DraggedConnect { get; set; }
        [Reactive] public ViewModelConnector ConnectorPreviewForDrop { get; set; }
        [Reactive] public ViewModelNode StartState { get; set; }

        /// <summary>
        /// Масштаб 
        /// </summary>
        [Reactive] public Scale Scale { get; set; } = new Scale();

        public ViewModelNodesCanvas()
        {
            SetupCommands();
            SetupNodes();
            Cutter = new ViewModelCutter(this);
        }

        #region Setup Nodes
        private void SetupNodes()
        {
            StartState = new ViewModelNode(this)
            {
                Name = "Start",
                //NameEnable = false,
                CanBeDelete = false


            };
            StartState.Input.Visible = false;
            Nodes.Add(StartState);
            //ViewModelNode end = new ViewModelNode(this)
            //{
            //    Name = "End",
            //    NameEnable = false,
            //    CanBeDelete = false,
            //    Point1 = new MyPoint(100, 100)
            //};
            //end.TransitionsVisible = null;
            //end.RollUpVisible = null;
            //Nodes.Add(end);
        }

        #endregion Setup Nodes

        #region Setup Commands
        public SimpleCommand CommandRedo { get; set; }
        public SimpleCommand CommandUndo { get; set; }
        public SimpleCommand CommandSelectAll { get; set; }
        public SimpleCommand CommandUnSelectAll { get; set; }
        public SimpleCommand CommandSelectorIntersect { get; set; }
        public SimpleCommand CommandCutterIntersect { get; set; }
        public SimpleCommand CommandDeleteFreeConnect { get; set; }

        public SimpleCommandWithParameter<ValidateObjectProperty<ViewModelNode, string>> CommandValidateNodeName { get; set; }
        public SimpleCommandWithParameter<ValidateObjectProperty<ViewModelConnector, string>> CommandValidateConnectName { get; set; }

        public SimpleCommandWithParameter<object> CommandZoom { get; set; }
        public SimpleCommandWithParameter<MyPoint> CommandSelect { get; set; }
        public SimpleCommandWithParameter<MyPoint> CommandCut { get; set; }
        public SimpleCommandWithParameter<MyPoint> CommandPartMoveAllNode { get; set; }
        public SimpleCommandWithParameter<MyPoint> CommandPartMoveAllSelectedNode { get; set; }

        public SimpleCommandWithParameter<ViewModelConnector> CommandAddFreeConnect { get; set; }
        public Command<ViewModelConnect, ViewModelConnect> CommandAddConnect { get; set; }

        public Command<MyPoint, List<ViewModelNode>> CommandFullMoveAllNode { get; set; }
        public Command<MyPoint, List<ViewModelNode>> CommandFullMoveAllSelectedNode { get; set; }
        public Command<MyPoint, ViewModelNode> CommandAddNode { get; set; }
        //public Command<MyPoint, ViewModelNode> CommandDeleteNode { get; set; }
        public Command<List<ViewModelNode>, List<ViewModelNode>> CommandDeleteSelectedNodes { get; set; }

        //public Command<List<ViewModel>, List<ViewModelNode>> CommandDeleteSelectedConnects { get; set; }


        public SimpleCommandWithParameter<string> CommandSave { get; set; }
        public SimpleCommandWithParameter<string> CommandOpen{ get; set; }

        public double ScaleMax = 5;
        public double ScaleMin = 0.1;
        public double Scales { get; set; } = 0.05;

        //public Command CommandDropOver { get; set; }

        private void SetupCommands()
        {
            CommandRedo = new SimpleCommand(this, CommandUndoRedo.Redo);
            CommandUndo = new SimpleCommand(this, CommandUndoRedo.Undo);
            CommandSelectAll = new SimpleCommand(this, SelectedAll);
            CommandUnSelectAll = new SimpleCommand(this, UnSelectedAll);
            CommandSelectorIntersect = new SimpleCommand(this, SelectNodes);
            CommandCutterIntersect = new SimpleCommand(this, SelectConnects);
            CommandValidateNodeName = new SimpleCommandWithParameter<ValidateObjectProperty<ViewModelNode, string>>(this, ValidateNodeName);
            CommandValidateConnectName = new SimpleCommandWithParameter<ValidateObjectProperty<ViewModelConnector, string>>(this, ValidateConnectName);

            CommandPartMoveAllNode = new SimpleCommandWithParameter<MyPoint>(this, PartMoveAllNode);
            CommandPartMoveAllSelectedNode = new SimpleCommandWithParameter<MyPoint>(this, PartMoveAllSelectedNode);
            CommandZoom = new SimpleCommandWithParameter<object>(this, Zoom);

            //CommandAddConnect = new Command<ViewModelConnect, ViewModelConnect>(this, AddConnect, DeleteConnect);
            //CommandDeleteNode = new Command<MyPoint, ViewModelNode>(this, DeleteNode,);
            CommandSelect = new SimpleCommandWithParameter<MyPoint>(this, StartSelect);
            CommandCut = new SimpleCommandWithParameter<MyPoint>(this, StartCut);

            CommandAddFreeConnect = new SimpleCommandWithParameter<ViewModelConnector>(this, AddFreeConnect);

            CommandDeleteFreeConnect = new SimpleCommand(this, DeleteFreeConnect);
            CommandFullMoveAllNode = new Command<MyPoint, List<ViewModelNode>>(this, FullMoveAllNode, UnFullMoveAllNode);
            CommandFullMoveAllSelectedNode = new Command<MyPoint, List<ViewModelNode>>(this, FullMoveAllSelectedNode, UnFullMoveAllSelectedNode);
            CommandAddNode = new Command<MyPoint, ViewModelNode>(this, AddNode, DeleteNode);
            CommandAddConnect = new Command<ViewModelConnect, ViewModelConnect>(this, AddConnect, DeleteConnect);
            CommandDeleteSelectedNodes = new Command<List<ViewModelNode>, List<ViewModelNode>>(this, DeleteSelectedNode, UnDeleteSelectedNode);


            CommandSave = new SimpleCommandWithParameter<string>(this, Save);
            CommandOpen = new SimpleCommandWithParameter<string>(this, Open);
        }

        #endregion Setup Commands

        private void StartSelect(MyPoint point)
        {
            Selector.CommandStartSelect.Execute(point);
        }
        private void StartCut(MyPoint point)
        {
            Cutter.CommandStartCut.Execute(point);
        }
        private void SelectedAll()
        {
            foreach (var node in Nodes)
            { node.Selected = true; }
        }
        private void UnSelectedAll()
        {
            foreach (var node in Nodes)
            { node.Selected = false; }
        }
        private List<ViewModelNode> FullMoveAllNode(MyPoint delta, List<ViewModelNode> nodes = null)
        {
            MyPoint myPoint = delta.Copy();
            if (nodes == null)
            {
                nodes = Nodes.ToList();
                myPoint.Clear();
            }
            nodes.ForEach(node => node.CommandMove.Execute(myPoint));
            return nodes;
        }
        private List<ViewModelNode> UnFullMoveAllNode(MyPoint delta, List<ViewModelNode> nodes = null)
        {
            MyPoint myPoint = delta.Copy();
            myPoint.Mirror();
            nodes.ForEach(node => node.CommandMove.Execute(myPoint));
            return nodes;
        }
        private List<ViewModelNode> FullMoveAllSelectedNode(MyPoint delta, List<ViewModelNode> nodes = null)
        {
            MyPoint myPoint = delta.Copy();
            if (nodes == null)
            {
                nodes = Nodes.Where(x => x.Selected).ToList();
                myPoint.Clear();
            }
            nodes.ForEach(node => node.CommandMove.Execute(myPoint));
            return nodes;
        }
        private List<ViewModelNode> UnFullMoveAllSelectedNode(MyPoint delta, List<ViewModelNode> nodes = null)
        {
            MyPoint myPoint = delta.Copy();
            myPoint.Mirror();
            nodes.ForEach(node => node.CommandMove.Execute(myPoint));
            return nodes;
        }
        private void PartMoveAllNode(MyPoint delta)
        {
            foreach (var node in Nodes)
            { node.CommandMove.Execute(delta); }
        }
        private void PartMoveAllSelectedNode(MyPoint delta)
        {
            foreach (var node in Nodes.Where(x => x.Selected))
            { node.CommandMove.Execute(delta); }
        }
        private ViewModelNode AddNode(MyPoint parameter, ViewModelNode result)
        {
            ViewModelNode newNode = result;
            if (result == null)
            {
                MyPoint myPoint = parameter.Copy();
                myPoint /= Scale.Value;
                newNode = new ViewModelNode(this)
                {
                    Name = "State " + Nodes.Count.ToString(),
                    Point1 = new MyPoint(myPoint)
                };
            }
            Nodes.Add(newNode);
            return newNode;
        }
        private ViewModelNode DeleteNode(MyPoint parameter, ViewModelNode result)
        {
            Nodes.Remove(result);
            return result;
        }
        private void Zoom(object delta)
        {
            int Delta = (int)delta;
            bool DeltaIsZero = (Delta == 0);
            bool DeltaMax = ((Delta > 0) && (Scale.Value > ScaleMax));
            bool DeltaMin = ((Delta < 0) && (Scale.Value < ScaleMin));
            if (DeltaIsZero || DeltaMax || DeltaMin)
                return;

            Scale.Value += (Delta > 0) ? Scales : -Scales;
        }
        private void SelectConnects()
        {
            MyPoint cutterStartPoint = Cutter.StartPoint / Scale.Value;
            MyPoint cutterEndPoint = Cutter.EndPoint / Scale.Value;

            //MyPoint cutterStartPoint = Cutter.StartPoint;
            //MyPoint cutterEndPoint = Cutter.EndPoint;
            //some optimizations
            var connects = Connects.Where(x => MyUtils.CheckIntersectTwoRectangles(MyUtils.GetStartPointDiagonal(x.StartPoint, x.EndPoint), MyUtils.GetEndPointDiagonal(x.StartPoint, x.EndPoint),
                                               MyUtils.GetStartPointDiagonal(cutterStartPoint, cutterEndPoint), MyUtils.GetEndPointDiagonal(cutterStartPoint, cutterEndPoint)));
            //var connects = Connects;
            foreach (var connect in Connects)
            {
                connect.Selected = false;
            }

            foreach (var connect in connects)
            {
                connect.Selected = MyUtils.CheckIntersectCubicBezierCurveAndLine(connect.StartPoint, connect.Point1, connect.Point2, connect.EndPoint, cutterStartPoint, cutterEndPoint);
            }

        }
        private void SelectNodes()
        {
            MyPoint selectorPoint1 = Selector.Point1WithScale / Scale.Value;
            MyPoint selectorPoint2 = Selector.Point2WithScale / Scale.Value;

            foreach (ViewModelNode node in Nodes)
            {
                node.Selected = MyUtils.CheckIntersectTwoRectangles(node.Point1, node.Point2, selectorPoint1, selectorPoint2);
            }
        }

        private void AddFreeConnect(ViewModelConnector fromConnector)
        {
            DraggedConnect = new ViewModelConnect(fromConnector);
           
            Connects.Add(DraggedConnect);

            //Elements.Add(DraggedConnect);
        }
        private ViewModelConnect AddConnect(ViewModelConnect parameter, ViewModelConnect result)
        {
            if (result == null)
                return parameter;
            result.FromConnector.CommandAdd.Execute();
            Connects.Add(result);
            return result;
        }
        private ViewModelConnect DeleteConnect(ViewModelConnect parameter, ViewModelConnect result)
        {
            Connects.Remove(parameter);
            parameter.FromConnector.CommandDelete.Execute();
            return parameter;
        }
        private void DeleteFreeConnect()
        {
            Connects.Remove(DraggedConnect);
        }
        private List<ViewModelNode> DeleteSelectedNode(List<ViewModelNode> parameter, List<ViewModelNode> result)
        {
            if (result == null)
            {
                result = Nodes.Where(x => x.Selected && x.CanBeDelete).ToList();
            }
            Nodes.RemoveMany(result);
            return result;
        }
        private List<ViewModelNode> UnDeleteSelectedNode(List<ViewModelNode> parameter, List<ViewModelNode> result)
        {
            Nodes.Add(result);
            return result;
        }
        private void ValidateNodeName(ValidateObjectProperty<ViewModelNode, string> obj)
        {
            if (!String.IsNullOrWhiteSpace(obj.Property))
            {
                if (!Nodes.Any(x => x.Name == obj.Property))
                {
                    obj.Obj.Name = obj.Property;
                }
            }
        }
        private void ValidateConnectName(ValidateObjectProperty<ViewModelConnector, string> obj)
        {
            if (!String.IsNullOrWhiteSpace(obj.Property))
            {
                if (!this.Nodes.Where(x=>x.Transitions.Any(x=>x.Name == obj.Property)).Any())
                {
                    obj.Obj.Name = obj.Property;
                }
            }
        }

        private void Open(string fileName)
        {
            this.Nodes.Clear();
            this.Connects.Clear();
            XDocument xDocument = XDocument.Load(fileName);
            XElement stateMachineXElement = xDocument.Element("StateMachine");
            var States = stateMachineXElement.Element("States")?.Elements()?.ToList();
            States?.ForEach(x => this.Nodes.Add(ViewModelNode.FromXElement(this, x)));
            //var startState = stateMachineXElement.Element("StartState");
            //string nameStartState = startState?.Attribute("Name").Value;
            //if (!string.IsNullOrEmpty(nameStartState))
            //    stateMachine.SetStartState(nameStartState);

            var Transitions = stateMachineXElement.Element("Transitions")?.Elements()?.ToList();
            Transitions?.ForEach(x => this.Connects.Add(ViewModelConnect.FromXElement(this, x)));

            //return stateMachine;
        }
        private void Save(string fileName)
        {
            XDocument xDocument = new XDocument();
            XElement stateMachineXElement = new XElement("StateMachine");
            xDocument.Add(stateMachineXElement);
            XElement states = new XElement("States");
            stateMachineXElement.Add(states);
            foreach (var state in Nodes)
            {
                states.Add(state.ToXElement());
            }


            XElement startState = new XElement("StartState");
            stateMachineXElement.Add(startState);
            startState.Add(new XAttribute("Name", StartState.Name));


            XElement transitions = new XElement("Transitions");
            stateMachineXElement.Add(transitions);

            foreach (var transition in Connects)
            {
                transitions.Add(transition.ToXElement());
            }

            xDocument.Save(fileName);
        }
    }
}
