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
using System.Windows.Input;
using System.Reactive;
using SimpleStateMachineNodeEditor.Helpers.Extensions;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelNodesCanvas : ReactiveObject
    {
        public ObservableCollectionExtended<ViewModelConnect> Connects = new ObservableCollectionExtended<ViewModelConnect>();
        public ObservableCollectionExtended<ViewModelNode> Nodes = new ObservableCollectionExtended<ViewModelNode>();
        public ObservableCollectionExtended<ViewModelMessage> Messages { get; set; } = new ObservableCollectionExtended<ViewModelMessage>();

        [Reactive] public ViewModelSelector Selector { get; set; } = new ViewModelSelector();
        [Reactive] public ViewModelCutter Cutter { get; set; }
        [Reactive] public ViewModelConnect DraggedConnect { get; set; }
        [Reactive] public ViewModelConnector ConnectorPreviewForDrop { get; set; }
        [Reactive] public ViewModelNode StartState { get; set; }
        [Reactive] public bool ItSaved { get; set; } = true;
        [Reactive] public string Path { get; set; }
        [Reactive] public TypeMessage DisplayMessageType { get; set; }
        

        [Reactive] public Scale Scale { get; set; } = new Scale();

        public ViewModelNodesCanvas()
        {
            SetupCommands();
            SetupStartState();
            Cutter = new ViewModelCutter(this);
            for (int i = 1; i <= 5; i++)
            {
                LogError("Error " + i.ToString());
            }
            for (int i = 1; i <= 5; i++)
            {
                LogInformation("Information " + i.ToString());
            }
            for (int i = 1; i <= 5; i++)
            {
                LogWarning("Warning " + i.ToString());
            }
            for (int i = 1; i <= 5; i++)
            {
                LogDebug("Debug " + i.ToString());
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
        public ReactiveCommand<Unit,Unit> CommandNewScheme { get; set; }
        public ReactiveCommand<Unit,Unit> CommandRedo { get; set; }
        public ReactiveCommand<Unit,Unit> CommandUndo { get; set; }
        //public ReactiveCommand<Unit,Unit> CommandSelectAll { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSelectAll { get; set; }
        public ReactiveCommand<Unit,Unit> CommandUnSelectAll { get; set; }
        public ReactiveCommand<Unit,Unit> CommandSelectorIntersect { get; set; }
        public ReactiveCommand<Unit,Unit> CommandCutterIntersect { get; set; }


        //public SimpleCommandWithParameter<ViewModelConnect> CommandAddConnect { get; set; }
        public ReactiveCommand<ViewModelConnect, Unit> CommandAddConnect { get; set; }
        public SimpleCommandWithParameter<ViewModelConnect> CommandDeleteConnect { get; set; }
        //public SimpleCommandWithParameter<(int connectorIndex, ViewModelConnect connect)> CommandAddConnectWithConnector { get; set; }
        //public SimpleCommandWithParameter<ViewModelConnect> CommandDeleteConnectWithConnector { get; set; }

        public SimpleCommandWithParameter<ViewModelConnector> CommandAddDraggedConnect { get; set; }
        public ReactiveCommand<Unit,Unit> CommandDeleteDraggedConnect { get; set; }
   
        public SimpleCommandWithParameter<(ViewModelNode objectForValidate, string newValue)> CommandValidateNodeName { get; set; }
        public SimpleCommandWithParameter<(ViewModelNode objectForValidate, string newValue)> CommandValidateConnectName { get; set; }

        public SimpleCommandWithParameter<int> CommandZoom { get; set; }
        public SimpleCommandWithParameter<MyPoint> CommandSelect { get; set; }
        public SimpleCommandWithParameter<MyPoint> CommandCut { get; set; }
        //public SimpleCommandWithParameter<MyPoint> CommandPartMoveAllNode { get; set; }
        //public SimpleCommandWithParameter<MyPoint> CommandPartMoveAllSelectedNode { get; set; }
        public ReactiveCommand<MyPoint, Unit> CommandPartMoveAllNode { get; set; }
        public ReactiveCommand<MyPoint, Unit> CommandPartMoveAllSelectedNode { get; set; }
        public SimpleCommandWithParameter<string> CommandLogDebug { get; set; }
        public SimpleCommandWithParameter<string> CommandLogError { get; set; }
        public SimpleCommandWithParameter<string> CommandLogInformation { get; set; }
        public SimpleCommandWithParameter<string> CommandLogWarning { get; set; }
        //public Command<ViewModelConnect, ViewModelConnect> CommandAddConnectWithUndoRedo { get; set; }
        public Command<ViewModelConnector, ViewModelConnector> CommandAddConnectorWithConnect {get; set; }
        public Command<MyPoint, List<ViewModelNode>> CommandFullMoveAllNode { get; set; }
        public Command<MyPoint, List<ViewModelNode>> CommandFullMoveAllSelectedNode { get; set; }
        public Command<MyPoint, ViewModelNode> CommandAddNodeWithUndoRedo { get; set; }
        public Command<ElementsForDelete, ElementsForDelete> CommandDeleteSelectedNodes { get; set; }
        public Command<List<(int index, ViewModelConnector element)>, List<(int index, ViewModelConnector element)>> CommandDeleteSelectedConnectors { get; set; }
        public Command<DeleteMode, DeleteMode> CommandDeleteSelectedElements { get; set; }


        public SimpleCommandWithParameter<string> CommandSave { get; set; }
        public SimpleCommandWithParameter<string> CommandOpen{ get; set; }

        public double ScaleMax = 5;
        public double ScaleMin = 0.1;
        public double Scales { get; set; } = 0.05;

        private void SetupCommands()
        {
            CommandRedo = ReactiveCommand.Create(CommandWithUndoRedo.Redo);
            CommandUndo = ReactiveCommand.Create(CommandWithUndoRedo.Undo);
            //CommandSelectAll = new SimpleCommand(SelectedAll);
            CommandSelectAll = ReactiveCommand.Create(SelectedAll);
            CommandUnSelectAll = ReactiveCommand.Create(UnSelectedAll);
            CommandSelectorIntersect = ReactiveCommand.Create(SelectNodes);
            CommandCutterIntersect = ReactiveCommand.Create(SelectConnects);
            CommandValidateNodeName = new SimpleCommandWithParameter<(ViewModelNode objectForValidate, string newValue)>(ValidateNodeName);
            CommandValidateConnectName = new SimpleCommandWithParameter<(ViewModelNode objectForValidate, string newValue)>(ValidateConnectName);
            //CommandAddConnect = new SimpleCommandWithParameter<ViewModelConnect>(AddConnect, NotSaved);

            CommandAddConnect = ReactiveCommand.Create< ViewModelConnect>(AddConnect);
            CommandDeleteConnect = new SimpleCommandWithParameter<ViewModelConnect>(DeleteConnect, NotSaved);

            //CommandPartMoveAllNode = new SimpleCommandWithParameter<MyPoint>(PartMoveAllNode);
            //CommandPartMoveAllSelectedNode = new SimpleCommandWithParameter<MyPoint>(PartMoveAllSelectedNode);

            CommandPartMoveAllNode = ReactiveCommand.Create<MyPoint>(PartMoveAllNode);
            CommandPartMoveAllSelectedNode = ReactiveCommand.Create<MyPoint>(PartMoveAllSelectedNode);

            CommandFullMoveAllNode = new Command<MyPoint, List<ViewModelNode>>(FullMoveAllNode, UnFullMoveAllNode, NotSaved);
            CommandFullMoveAllSelectedNode = new Command<MyPoint, List<ViewModelNode>>(FullMoveAllSelectedNode, UnFullMoveAllSelectedNode, NotSaved);
            CommandAddConnectorWithConnect = new Command<ViewModelConnector, ViewModelConnector>(AddConnectorWithConnect, DeleteConnectorWithConnect, NotSaved);
            CommandZoom = new SimpleCommandWithParameter<int>(Zoom);
            CommandLogDebug = new SimpleCommandWithParameter<string>(LogDebug);
            CommandLogError = new SimpleCommandWithParameter<string>(LogError);
            CommandLogInformation = new SimpleCommandWithParameter<string>(LogInformation);
            CommandLogWarning = new SimpleCommandWithParameter<string>(LogWarning);
            CommandSelect = new SimpleCommandWithParameter<MyPoint>(StartSelect);
            CommandCut = new SimpleCommandWithParameter<MyPoint>(StartCut);
            CommandAddDraggedConnect = new SimpleCommandWithParameter<ViewModelConnector>(AddDraggedConnect);
            CommandDeleteDraggedConnect = ReactiveCommand.Create(DeleteDraggedConnect);
            CommandAddNodeWithUndoRedo = new Command<MyPoint, ViewModelNode>(AddNodeWithUndoRedo, DeleteNodeWithUndoRedo, NotSaved);
            //CommandAddConnectWithUndoRedo = new Command<ViewModelConnect, ViewModelConnect>(AddConnectWithUndoRedo, DeleteConnectWithUndoRedo, NotSaved);
            CommandDeleteSelectedNodes = new Command<ElementsForDelete, ElementsForDelete>(DeleteSelectedNodes, UnDeleteSelectedNodes, NotSaved);
            CommandDeleteSelectedConnectors = new Command<List<(int index, ViewModelConnector element)>, List<(int index, ViewModelConnector connector)>>(DeleteSelectedConnectors, UnDeleteSelectedConnectors, NotSaved);
            CommandDeleteSelectedElements = new Command<DeleteMode, DeleteMode>(DeleteSelectedElements, UnDeleteSelectedElements);
            CommandSave = new SimpleCommandWithParameter<string>(Save);
            CommandOpen = new SimpleCommandWithParameter<string>(Open);
            CommandNewScheme = ReactiveCommand.Create(NewScheme);
        }
        private void NotSaved()
        {
            ItSaved = false;
        }

        private void NotSavedSubscrube()
        {
            CommandRedo.Subscribe(_=> NotSaved());
            CommandUndo.Subscribe(_ => NotSaved());

            CommandAddConnect.Subscribe(_ => NotSaved());
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
                node.CommandUnSelectedAllConnectors.ExecuteWithSubscribe();
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

            LogInformation(Messages.Count.ToString());
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
        private ViewModelNode AddNodeWithUndoRedo(MyPoint parameter, ViewModelNode result)
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
        private ViewModelNode DeleteNodeWithUndoRedo(MyPoint parameter, ViewModelNode result)
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

        private void AddDraggedConnect(ViewModelConnector fromConnector)
        {
            DraggedConnect = new ViewModelConnect(this, fromConnector);

            AddConnect(DraggedConnect);
        }
        private void DeleteDraggedConnect()
        {
            Connects.Remove(DraggedConnect);
            DraggedConnect.FromConnector.Connect = null;

        }
        private ViewModelConnector AddConnectorWithConnect(ViewModelConnector parameter, ViewModelConnector result)
        {
            if (result == null)
                return parameter;
            result.Node.CommandAddConnectorWithConnect.Execute((1, result));
            return result;
        }
        private ViewModelConnector DeleteConnectorWithConnect(ViewModelConnector parameter, ViewModelConnector result)
        {
            result.Node.CommandDeleteConnectorWithConnect.Execute(result);
            return parameter;
        }

        //private ViewModelConnect AddConnectWithUndoRedo(ViewModelConnect parameter, ViewModelConnect result)
        //{
        //    if (result == null)
        //        return parameter;
        //    result.FromConnector.Node.CommandAddConnectorWithConnect.Execute((1, result.FromConnector));
        //    return result;
        //}
        //private ViewModelConnect DeleteConnectWithUndoRedo(ViewModelConnect parameter, ViewModelConnect result)
        //{
        //    result.FromConnector.Node.CommandDeleteConnectorWithConnect.Execute(result.FromConnector);
        //    return parameter;
        //}


        private void AddConnect(ViewModelConnect ViewModelConnect)
        {
            Connects.Add(ViewModelConnect);
        }
        private void DeleteConnect(ViewModelConnect ViewModelConnect)
        {
            Connects.Remove(ViewModelConnect);
        }

        private DeleteMode DeleteSelectedElements(DeleteMode parameter, DeleteMode result)
        {   
            if(result==DeleteMode.noCorrect)
            {
                bool keyN = Keyboard.IsKeyDown(Key.N);
                bool keyC = Keyboard.IsKeyDown(Key.C);

                if(keyN  == keyC)
                {
                    result = DeleteMode.DeleteAllSelected;
                }
                else if(keyN)
                {
                    result = DeleteMode.DeleteNodes;
                }
                else if (keyC)
                {
                    result = DeleteMode.DeleteConnects;
                }

            }

            if ((result == DeleteMode.DeleteConnects)|| (result == DeleteMode.DeleteAllSelected))
            {
               CommandDeleteSelectedConnectors.Execute(null);
            }
            if ((result == DeleteMode.DeleteNodes) || (result == DeleteMode.DeleteAllSelected))
            {
                CommandDeleteSelectedNodes.Execute(null);
            }

            return result;
        }
        private DeleteMode UnDeleteSelectedElements(DeleteMode parameter, DeleteMode result)
        {
            int count = 0;

            if((result == DeleteMode.DeleteNodes)|| (result == DeleteMode.DeleteConnects))
            {
                count = 1;
            }else if (result == DeleteMode.DeleteAllSelected)
            {
                count = 2;
            }

                for (int i = 0;i< count; i++)
            {
                CommandUndo.ExecuteWithSubscribe();
            }
            return result;
        }
        private List<(int index, ViewModelConnector connector)> DeleteSelectedConnectors(List<(int index, ViewModelConnector connector)> parameter, List<(int index, ViewModelConnector connector)> result)
        {
            if (result == null)
            {
                result = new List<(int index, ViewModelConnector element)>();
                foreach(var connector in GetAllConnectors().Where(x=>x.Selected))
                {
                     result.Add((connector.Node.GetConnectorIndex(connector), connector));
                }
            }
            foreach (var element in result)
            {
                element.connector.Node.CommandDeleteConnectorWithConnect.Execute(element.connector);
            }
            return result;
        }
        private List<(int index, ViewModelConnector connector)> UnDeleteSelectedConnectors(List<(int index, ViewModelConnector connector)> parameter, List<(int index, ViewModelConnector connector)> result)
        {
            foreach(var element in result)
            {
                element.connector.Node.CommandAddConnectorWithConnect.Execute((element.index, element.connector));
            }

            return result;
        }
        private ElementsForDelete DeleteSelectedNodes(ElementsForDelete parameter, ElementsForDelete result)
        {
            if (result == null)
            {
                result = new ElementsForDelete();
                result.NodesToDelete = Nodes.Where(x => x.Selected && x.CanBeDelete).ToList();
                result.ConnectsToDelete = new List<ViewModelConnect>();
                result.ConnectsToDeleteWithConnectors = new List<(int connectorIndex, ViewModelConnect connect)>();

                foreach(var connect in Connects)
                {
                    if(result.NodesToDelete.Contains(connect.FromConnector.Node))
                    {
                        result.ConnectsToDelete.Add(connect);
                    }
                    else if(result.NodesToDelete.Contains(connect.ToConnector.Node))
                    {
                        result.ConnectsToDeleteWithConnectors.Add((connect.FromConnector.Node.GetConnectorIndex(connect.FromConnector), connect));
                    }
                }
            }
            foreach(var element in result.ConnectsToDeleteWithConnectors)
            {
                element.connect.FromConnector.Node.CommandDeleteConnectorWithConnect.Execute(element.connect.FromConnector);
            }

            Connects.RemoveMany(result.ConnectsToDelete);
            Nodes.RemoveMany(result.NodesToDelete);
            
            return result;
        }
        private ElementsForDelete UnDeleteSelectedNodes(ElementsForDelete parameter, ElementsForDelete result)
        {
            Nodes.AddRange(result.NodesToDelete);
            Connects.AddRange(result.ConnectsToDelete);
            result.ConnectsToDeleteWithConnectors.Sort(ElementsForDelete.Sort);
            foreach (var element in result.ConnectsToDeleteWithConnectors)
            {
                element.connect.FromConnector.Node.CommandAddConnectorWithConnect.Execute((element.connectorIndex, element.connect.FromConnector));
            }

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
        private IEnumerable<ViewModelConnector> GetAllConnectors()
        {
            return this.Nodes.SelectMany(x => x.Transitions);
        }
        private bool ConnectExist(string nameConnect)
        {
            return GetAllConnectors().Any(x=>x.Name == nameConnect);
        }

        private bool NodeExist(string nameNode)
        {
            return Nodes.Any(x => x.Name == nameNode);
        }

        private void NewScheme()
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
            Path = fileName;
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
                NewScheme();
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
            Path = fileName;
        }

        public class ElementsForDelete
        {
            public List<ViewModelNode> NodesToDelete;
            public List<ViewModelConnect> ConnectsToDelete;
            public List<(int connectorIndex, ViewModelConnect connect)> ConnectsToDeleteWithConnectors;

            public static int Sort((int connectorIndex, ViewModelConnect connect) A, (int connectorIndex, ViewModelConnect connect) B)
            {
                return A.connectorIndex.CompareTo(B.connectorIndex);
            }
        }
    }
}
