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
using SimpleStateMachineNodeEditor.Helpers.Enums;
using System.Windows.Media;
using System.Windows;

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

        [Reactive] public bool ItSaved { get; set; } = true;

        public IObservableCollection<ViewModelMessage> Messages { get; set; } = new ObservableCollectionExtended<ViewModelMessage>();

        /// <summary>
        /// Масштаб 
        /// </summary>
        [Reactive] public Scale Scale { get; set; } = new Scale();

        public ViewModelNodesCanvas()
        {
            SetupCommands();
            SetupStartState();
            Cutter = new ViewModelCutter(this);
            for (int i = 1; i <= 30; i++)
            {
                LogError("Message " + i.ToString());
            }

        }

        #region Setup Nodes
        private void SetupStartState()
        {
            string name = Nodes.Any(x => x.Name == "Start")? GetNameForNewNode() : "Start";
            StartState = new ViewModelNode(this)
            {
                Name = name
            };
            SetAsStart(StartState);
            Nodes.Add(StartState);
            this.ItSaved = true;
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
        private void SetAsStart(ViewModelNode node)
        {
            node.Input.Visible = false;
            node.CanBeDelete = false;
        }

        #endregion Setup Nodes

        #region Setup Commands
        public SimpleCommand CommandNew { get; set; }
        public SimpleCommand CommandRedo { get; set; }
        public SimpleCommand CommandUndo { get; set; }
        public SimpleCommand CommandSelectAll { get; set; }
        public SimpleCommand CommandUnSelectAll { get; set; }
        public SimpleCommand CommandSelectorIntersect { get; set; }
        public SimpleCommand CommandCutterIntersect { get; set; }
        public SimpleCommand CommandDeleteFreeConnect { get; set; }
   
        public SimpleCommandWithParameter<(ViewModelNode objectForValidate, string newValue)> CommandValidateNodeName { get; set; }
        public SimpleCommandWithParameter<(ViewModelNode objectForValidate, string newValue)> CommandValidateConnectName { get; set; }

        public SimpleCommandWithParameter<int> CommandZoom { get; set; }
        public SimpleCommandWithParameter<MyPoint> CommandSelect { get; set; }
        public SimpleCommandWithParameter<MyPoint> CommandCut { get; set; }
        public SimpleCommandWithParameter<MyPoint> CommandPartMoveAllNode { get; set; }
        public SimpleCommandWithParameter<string> CommandLogDebug { get; set; }
        public SimpleCommandWithParameter<string> CommandLogError { get; set; }
        public SimpleCommandWithParameter<string> CommandLogInformation { get; set; }
        public SimpleCommandWithParameter<string> CommandLogWarning { get; set; }
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
           
            CommandRedo = new SimpleCommand(CommandUndoRedo.Redo, NotSaved);
            CommandUndo = new SimpleCommand(CommandUndoRedo.Undo, NotSaved);
            CommandSelectAll = new SimpleCommand(SelectedAll);
            CommandUnSelectAll = new SimpleCommand(UnSelectedAll);
            CommandSelectorIntersect = new SimpleCommand(SelectNodes);
            CommandCutterIntersect = new SimpleCommand(SelectConnects);
            CommandValidateNodeName = new SimpleCommandWithParameter<(ViewModelNode objectForValidate, string newValue)>(ValidateNodeName);
            CommandValidateConnectName = new SimpleCommandWithParameter<(ViewModelNode objectForValidate, string newValue)>(ValidateConnectName);

            CommandPartMoveAllNode = new SimpleCommandWithParameter<MyPoint>(PartMoveAllNode);
            CommandPartMoveAllSelectedNode = new SimpleCommandWithParameter<MyPoint>(PartMoveAllSelectedNode);
            CommandZoom = new SimpleCommandWithParameter<int>(Zoom);

            CommandLogDebug = new SimpleCommandWithParameter<string>(LogDebug);
            CommandLogError = new SimpleCommandWithParameter<string>(LogError);
            CommandLogInformation = new SimpleCommandWithParameter<string>(LogInformation);
            CommandLogWarning = new SimpleCommandWithParameter<string>(LogWarning);

            //CommandAddConnect = new Command<ViewModelConnect, ViewModelConnect>(this, AddConnect, DeleteConnect);
            //CommandDeleteNode = new Command<MyPoint, ViewModelNode>(this, DeleteNode,);
            CommandSelect = new SimpleCommandWithParameter<MyPoint>(StartSelect);
            CommandCut = new SimpleCommandWithParameter<MyPoint>(StartCut);

            CommandAddFreeConnect = new SimpleCommandWithParameter<ViewModelConnector>(AddFreeConnect);

            CommandDeleteFreeConnect = new SimpleCommand(DeleteFreeConnect);
            CommandFullMoveAllNode = new Command<MyPoint, List<ViewModelNode>>(FullMoveAllNode, UnFullMoveAllNode, NotSaved);
            CommandFullMoveAllSelectedNode = new Command<MyPoint, List<ViewModelNode>>(FullMoveAllSelectedNode, UnFullMoveAllSelectedNode, NotSaved);
            CommandAddNode = new Command<MyPoint, ViewModelNode>(AddNode, DeleteNode, NotSaved);
            CommandAddConnect = new Command<ViewModelConnect, ViewModelConnect>(AddConnect, DeleteConnect, NotSaved);
            CommandDeleteSelectedNodes = new Command<List<ViewModelNode>, List<ViewModelNode>>(DeleteSelectedNode, UnDeleteSelectedNode, NotSaved);


            CommandSave = new SimpleCommandWithParameter<string>(Save);
            CommandOpen = new SimpleCommandWithParameter<string>(Open);
            CommandNew = new SimpleCommand(New);
        }
        private void NotSaved()
        {
            ItSaved = false;
        }
        #endregion Setup Commands

        #region Logging

        public void LogDebug(string message)
        {
            Messages.Add(new ViewModelMessage(TypeMessage.Debug, message));
        }
        public void LogError(string message)
        {
            Messages.Add(new ViewModelMessage(TypeMessage.Error, message));
        }
        public void LogInformation(string message)
        {
            Messages.Add(new ViewModelMessage(TypeMessage.Information, message));
        }
        public void LogWarning(string message)
        {
            Messages.Add(new ViewModelMessage(TypeMessage.Warning, message));
        }

        #endregion Logging
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
            {
                node.Selected = false;
                node.CommandUnSelectedAllConnectors.Execute();
            }
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
        private string GetNameForNewNode()
        {
            return "State " + Nodes.Count.ToString();
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
                    Name = GetNameForNewNode(),
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
        private void Zoom(int delta)
        {
            bool DeltaIsZero = (delta == 0);
            bool DeltaMax = ((delta > 0) && (Scale.Value > ScaleMax));
            bool DeltaMin = ((delta < 0) && (Scale.Value < ScaleMin));
            if (DeltaIsZero || DeltaMax || DeltaMin)
                return;

            Scale.Value += (delta > 0) ? Scales : -Scales;
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
                connect.FromConnector.Selected = false;
            }

            foreach (var connect in connects)
            {
                connect.FromConnector.Selected = MyUtils.CheckIntersectCubicBezierCurveAndLine(connect.StartPoint, connect.Point1, connect.Point2, connect.EndPoint, cutterStartPoint, cutterEndPoint);
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
        private void ValidateNodeName((ViewModelNode objectForValidate, string newValue) obj)
        {
            if (!String.IsNullOrWhiteSpace(obj.newValue))
            {
                if (!NodeExist(obj.newValue))
                {
                    obj.objectForValidate.Name = obj.newValue;
                }
            }
        }
        private void ValidateConnectName((ViewModelNode objectForValidate, string newValue) obj)
        {
            if (!String.IsNullOrWhiteSpace(obj.newValue))
            {
                if (!ConnectExist(obj.newValue))
                {
                    obj.objectForValidate.Name = obj.newValue;
                }
            }
        }
        private bool ConnectExist(string nameConnect)
        {
            return this.Nodes.SelectMany(x=>x.Transitions).Any(x=>x.Name == nameConnect);
        }

        private bool NodeExist(string nameNode)
        {
            return Nodes.Any(x => x.Name == nameNode);
        }

        private void New()
        {
            this.Nodes.Clear();
            this.Connects.Clear();
            
            this.SetupStartState();
        }
        private void Open(string fileName)
        {        
            this.Nodes.Clear();
            this.Connects.Clear();

            XDocument xDocument = XDocument.Load(fileName);
            XElement stateMachineXElement = xDocument.Element("StateMachine");
            if(stateMachineXElement==null)
            {
                Error("not contanins StateMachine");
                return;
            }
            #region setup states/nodes

            var States = stateMachineXElement.Element("States")?.Elements()?.ToList() ?? new List<XElement>();
            ViewModelNode viewModelNode = null;
            foreach (var state in States)
            {
                viewModelNode = ViewModelNode.FromXElement(this, state, out string errorMesage, NodeExist);
                if (WithError(errorMesage, x => Nodes.Add(x), viewModelNode))
                    return;
            }

                #region setup start state

                var startState = stateMachineXElement.Element("StartState")?.Attribute("Name")?.Value;

                if (string.IsNullOrEmpty(startState))
                    this.SetupStartState();
                else
                    this.SetAsStart(this.Nodes.Single(x => x.Name == startState));

                #endregion  setup start state

            #endregion  setup states/nodes

            #region setup Transitions/connects

            var Transitions = stateMachineXElement.Element("Transitions")?.Elements()?.ToList()??new List<XElement>();
            ViewModelConnect viewModelConnect;
            foreach (var transition in Transitions)
            {
                viewModelConnect = ViewModelConnector.FromXElement(this, transition, out string errorMesage, ConnectExist);
                if (WithError(errorMesage, x => Connects.Add(x), viewModelConnect))
                    return;
            }

            #endregion  setup Transitions/connects

            bool WithError<T>(string errorMessage, Action<T> action, T obj)
            {
                if(string.IsNullOrEmpty(errorMessage))
                {
                    if(!object.Equals(obj,default(T)))
                        action.Invoke(obj);
                }
                else
                {
                    Error(errorMessage);
                    return true;
                }
                return false;
            }
            void Error(string errorMessage)
            {               
                LogError("File is not valid: " + errorMessage);
                New();
            }
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
            foreach (var transition in Nodes.SelectMany(x => x.Transitions.Where(y => !string.IsNullOrEmpty(y.Name))).Reverse())
            {
                transitions.Add(transition.ToXElement());
            }

            xDocument.Save(fileName);
            ItSaved = true;
        }
    }
}
