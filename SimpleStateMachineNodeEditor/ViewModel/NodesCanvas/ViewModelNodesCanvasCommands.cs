using DynamicData;
using ReactiveUI;
using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;

namespace SimpleStateMachineNodeEditor.ViewModel.NodesCanvas
{
    public partial class ViewModelNodesCanvas
    {
        #region commands without parameter
        public ReactiveCommand<Unit, Unit> CommandNew { get; set; }
        public ReactiveCommand<Unit, Unit> CommandRedo { get; set; }
        public ReactiveCommand<Unit, Unit> CommandUndo { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSelectAll { get; set; }
        public ReactiveCommand<Unit, Unit> CommandUnSelectAll { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSelectorIntersect { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCutterIntersect { get; set; }
        public ReactiveCommand<Unit, Unit> CommandDeleteDraggedConnect { get; set; }
        public ReactiveCommand<Unit, Unit> CommandZoomIn { get; set; }
        public ReactiveCommand<Unit, Unit> CommandZoomOut { get; set; }
        public ReactiveCommand<Unit, Unit> CommandZoomOriginalSize { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCollapseUpAll { get; set; }
        public ReactiveCommand<Unit, Unit> CommandExpandDownAll { get; set; }
        public ReactiveCommand<Unit, Unit> CommandCollapseUpSelected { get; set; }
        public ReactiveCommand<Unit, Unit> CommandExpandDownSelected { get; set; }
        public ReactiveCommand<Unit, Unit> CommandErrorListUpdate { get; set; }
        public ReactiveCommand<Unit, Unit> CommandExportToJPEG { get; set; }
        public ReactiveCommand<Unit, Unit> CommandOpen { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSave { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSaveAs { get; set; }
        public ReactiveCommand<Unit, Unit> CommandExit { get; set; }

        #endregion commands without parameter

        #region commands with parameter

        public ReactiveCommand<ViewModelConnect, Unit> CommandAddConnect { get; set; }
        public ReactiveCommand<ViewModelConnect, Unit> CommandDeleteConnect { get; set; }
        public ReactiveCommand<ViewModelConnector, Unit> CommandAddDraggedConnect { get; set; }
        public ReactiveCommand<(ViewModelNode objectForValidate, string newValue), Unit> CommandValidateNodeName { get; set; }
        public ReactiveCommand<(ViewModelConnector objectForValidate, string newValue), Unit> CommandValidateConnectName { get; set; }
        public ReactiveCommand<int, Unit> CommandZoom { get; set; }
        public ReactiveCommand<MyPoint, Unit> CommandSelect { get; set; }
        public ReactiveCommand<MyPoint, Unit> CommandCut { get; set; }
        public ReactiveCommand<MyPoint, Unit> CommandPartMoveAllNode { get; set; }
        public ReactiveCommand<MyPoint, Unit> CommandPartMoveAllSelectedNode { get; set; }
        public ReactiveCommand<string, Unit> CommandLogDebug { get; set; }
        public ReactiveCommand<string, Unit> CommandLogError { get; set; }
        public ReactiveCommand<string, Unit> CommandLogInformation { get; set; }
        public ReactiveCommand<string, Unit> CommandLogWarning { get; set; }

        #endregion commands with parameter

        #region commands with undo-redo

        public Command<ViewModelConnector, ViewModelConnector> CommandAddConnectorWithConnect { get; set; }
        public Command<MyPoint, List<ViewModelNode>> CommandFullMoveAllNode { get; set; }
        public Command<MyPoint, List<ViewModelNode>> CommandFullMoveAllSelectedNode { get; set; }
        public Command<MyPoint, ViewModelNode> CommandAddNodeWithUndoRedo { get; set; }
        public Command<ElementsForDelete, ElementsForDelete> CommandDeleteSelectedNodes { get; set; }
        public Command<List<(int index, ViewModelConnector element)>, List<(int index, ViewModelConnector element)>> CommandDeleteSelectedConnectors { get; set; }
        public Command<DeleteMode, DeleteMode> CommandDeleteSelectedElements { get; set; }

        #endregion commands with undo-redo

        private void SetupCommands()
        {

            CommandRedo = ReactiveCommand.Create(ICommandWithUndoRedo.Redo);
            CommandUndo = ReactiveCommand.Create(ICommandWithUndoRedo.Undo);
            CommandSelectAll = ReactiveCommand.Create(SelectedAll);
            CommandUnSelectAll = ReactiveCommand.Create(UnSelectedAll);
            CommandSelectorIntersect = ReactiveCommand.Create(SelectNodes);
            CommandCutterIntersect = ReactiveCommand.Create(SelectConnects);
            CommandZoomIn = ReactiveCommand.Create(() => { Scale.Value += Scales; });
            CommandZoomOut = ReactiveCommand.Create(() => { Scale.Value -= Scales; });
            CommandZoomOriginalSize = ReactiveCommand.Create(() => { Scale.Value = 1; });
            CommandCollapseUpAll = ReactiveCommand.Create(CollapseUpAll);
            CommandExpandDownAll = ReactiveCommand.Create(ExpandDownAll);
            CommandCollapseUpSelected = ReactiveCommand.Create(CollapseUpSelected);
            CommandExpandDownSelected = ReactiveCommand.Create(ExpandDownSelected);
            CommandErrorListUpdate = ReactiveCommand.Create(ErrosUpdaate);
            CommandExportToJPEG = ReactiveCommand.Create(ExportToJPEG);
            CommandOpen = ReactiveCommand.Create(Open);
            CommandSave = ReactiveCommand.Create(Save);
            CommandSaveAs = ReactiveCommand.Create(SaveAs);
            CommandExit = ReactiveCommand.Create(Exit);
            

            CommandValidateNodeName = ReactiveCommand.Create<(ViewModelNode objectForValidate, string newValue)>(ValidateNodeName);
            CommandValidateConnectName = ReactiveCommand.Create<(ViewModelConnector objectForValidate, string newValue)>(ValidateConnectName);
            CommandAddConnect = ReactiveCommand.Create<ViewModelConnect>(AddConnect);
            CommandDeleteConnect = ReactiveCommand.Create<ViewModelConnect>(DeleteConnect);
            CommandZoom = ReactiveCommand.Create<int>(Zoom);
            CommandLogDebug = ReactiveCommand.Create<string>(LogDebug);
            CommandLogError = ReactiveCommand.Create<string>(LogError);
            CommandLogInformation = ReactiveCommand.Create<string>(LogInformation);
            CommandLogWarning = ReactiveCommand.Create<string>(LogWarning);
            CommandSelect = ReactiveCommand.Create<MyPoint>(StartSelect);
            CommandCut = ReactiveCommand.Create<MyPoint>(StartCut);
            CommandAddDraggedConnect = ReactiveCommand.Create<ViewModelConnector>(AddDraggedConnect);
            CommandDeleteDraggedConnect = ReactiveCommand.Create(DeleteDraggedConnect);
                       
            CommandNew = ReactiveCommand.Create(NewScheme);
            CommandPartMoveAllNode = ReactiveCommand.Create<MyPoint>(PartMoveAllNode);
            CommandPartMoveAllSelectedNode = ReactiveCommand.Create<MyPoint>(PartMoveAllSelectedNode);


            CommandFullMoveAllNode = new Command<MyPoint, List<ViewModelNode>>(FullMoveAllNode, UnFullMoveAllNode, NotSaved);
            CommandFullMoveAllSelectedNode = new Command<MyPoint, List<ViewModelNode>>(FullMoveAllSelectedNode, UnFullMoveAllSelectedNode, NotSaved);
            CommandAddConnectorWithConnect = new Command<ViewModelConnector, ViewModelConnector>(AddConnectorWithConnect, DeleteConnectorWithConnect, NotSaved);
            CommandAddNodeWithUndoRedo = new Command<MyPoint, ViewModelNode>(AddNodeWithUndoRedo, DeleteNodeWithUndoRedo, NotSaved);
            CommandDeleteSelectedNodes = new Command<ElementsForDelete, ElementsForDelete>(DeleteSelectedNodes, UnDeleteSelectedNodes, NotSaved);
            CommandDeleteSelectedConnectors = new Command<List<(int index, ViewModelConnector element)>, List<(int index, ViewModelConnector connector)>>(DeleteSelectedConnectors, UnDeleteSelectedConnectors, NotSaved);
            CommandDeleteSelectedElements = new Command<DeleteMode, DeleteMode>(DeleteSelectedElements, UnDeleteSelectedElements);

            NotSavedSubscrube();
        }

        private void NotSaved()
        {
            ItSaved = false;
        }
        private void NotSavedSubscrube()
        {
            CommandRedo.Subscribe(_ => NotSaved());
            CommandUndo.Subscribe(_ => NotSaved());
            CommandAddConnect.Subscribe(_ => NotSaved());
            CommandDeleteConnect.Subscribe(_ => NotSaved());
        }
        private void SelectedAll()
        {
            foreach (var node in Nodes)
            { node.Selected = true; }
        }
        private void CollapseUpAll()
        {
            foreach (var node in Nodes)
            {
                node.IsCollapse = true;
            }
        }
        private void ExpandDownAll()
        {
            foreach (var node in Nodes)
            {
                node.IsCollapse = false;
            }
        }
        private void ErrosUpdaate()
        {
            Messages.RemoveMany(Messages.Where(x => x.TypeMessage == DisplayMessageType || DisplayMessageType == TypeMessage.All));
        }
        private void CollapseUpSelected()
        {
            foreach (var node in Nodes.Where(x => x.Selected))
            {
                node.IsCollapse = true;
            }
        }
        private void ExpandDownSelected()
        {
            foreach (var node in Nodes.Where(x => x.Selected))
            {
                node.IsCollapse = false;
            }
        }
        private void UnSelectedAll()
        {
            foreach (var node in Nodes)
            {
                node.Selected = false;
                node.CommandUnSelectedAllConnectors.ExecuteWithSubscribe();
            }
        }
        private string GetNameForNewNode()
        {
            return "State " + NodesCount.ToString();
        }
        private void SelectConnects()
        {
            MyPoint cutterStartPoint = Cutter.StartPoint / Scale.Value;
            MyPoint cutterEndPoint = Cutter.EndPoint / Scale.Value;

            var connects = Connects.Where(x => MyUtils.CheckIntersectTwoRectangles(MyUtils.GetStartPointDiagonal(x.StartPoint, x.EndPoint), MyUtils.GetEndPointDiagonal(x.StartPoint, x.EndPoint),
                                               MyUtils.GetStartPointDiagonal(cutterStartPoint, cutterEndPoint), MyUtils.GetEndPointDiagonal(cutterStartPoint, cutterEndPoint)));
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
        private void ExportToJPEG()
        {
            Dialog.ShowSaveFileDialog("JPEG Image (.jpeg)|*.jpeg", SchemeName(), "Export scheme");
            if (Dialog.Result != DialogResult.Ok)
                return;
            //"Png Image (.png)|*.png";
            JPEGPath = Dialog.FileName;
        }
        private void NewScheme()
        {
            this.Nodes.Clear();
            this.Connects.Clear();

            this.SetupStartState();
        }
        private void Open()
        {
            if (!WithoutSaving())
                return;

            Dialog.ShowOpenFileDialog("XML-File | *.xml", SchemeName(), "Import scheme");
            if (Dialog.Result != DialogResult.Ok)
                return;

            string fileName = Dialog.FileName;
            //string fileName = @"C:\Users\roman\Downloads\100 States.xml";
            this.Nodes.Clear();
            this.Connects.Clear();

            XDocument xDocument = XDocument.Load(fileName);
            XElement stateMachineXElement = xDocument.Element("StateMachine");
            if (stateMachineXElement == null)
            {
                Error("not contanins StateMachine");
                return;
            }
            #region setup states/nodes

            var States = stateMachineXElement.Element("States")?.Elements()?.ToList() ?? new List<XElement>();
            ViewModelNode viewModelNode = null;
            foreach (var state in States)
            {
                viewModelNode = ViewModelNode.FromXElement(this, state, out string errorMesage, NodesExist);
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

            var Transitions = stateMachineXElement.Element("Transitions")?.Elements()?.ToList() ?? new List<XElement>();
            ViewModelConnect viewModelConnect;
            foreach (var transition in Transitions)
            {
                viewModelConnect = ViewModelConnector.FromXElement(this, transition, out string errorMesage, ConnectsExist);
                if (WithError(errorMesage, x => Connects.Add(x), viewModelConnect))
                    return;
            }
            SchemePath = fileName;
            #endregion  setup Transitions/connects



            bool WithError<T>(string errorMessage, Action<T> action, T obj)
            {
                if (string.IsNullOrEmpty(errorMessage))
                {
                    if (!object.Equals(obj, default(T)))
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
        private void Save()
        {
            if (string.IsNullOrEmpty(SchemePath))
            {
                SaveAs();
            }
            else
            {
                Save(SchemePath);
            }
        }
        private void Exit()
        {
            if (!WithoutSaving())
                return;
            this.NeedExit = true;
        }
        private void SaveAs()
        {
            Dialog.ShowSaveFileDialog("XML-File | *.xml", SchemeName(), "Save scheme");
            if (Dialog.Result != DialogResult.Ok)
                return;

            Save(Dialog.FileName);
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
            SchemePath = fileName;
        }


        private void StartSelect(MyPoint point)
        {
            Selector.CommandStartSelect.ExecuteWithSubscribe(point);
        }
        private void StartCut(MyPoint point)
        {
            Cutter.CommandStartCut.ExecuteWithSubscribe(point);
        }
        private void PartMoveAllNode(MyPoint delta)
        {
            foreach (var node in Nodes)
            { node.CommandMove.ExecuteWithSubscribe(delta); }
        }
        private void PartMoveAllSelectedNode(MyPoint delta)
        {
            foreach (var node in Nodes.Where(x => x.Selected))
            { node.CommandMove.ExecuteWithSubscribe(delta); }
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
        private void AddConnect(ViewModelConnect ViewModelConnect)
        {
            Connects.Add(ViewModelConnect);
        }
        private void DeleteConnect(ViewModelConnect ViewModelConnect)
        {
            Connects.Remove(ViewModelConnect);
        }
        private void ValidateNodeName((ViewModelNode objectForValidate, string newValue) obj)
        {
            if (!String.IsNullOrWhiteSpace(obj.newValue))
            {
                if (!NodesExist(obj.newValue))
                {
                    obj.objectForValidate.Name = obj.newValue;
                }
            }
        }
        private void ValidateConnectName((ViewModelConnector objectForValidate, string newValue) obj)
        {
            if (!String.IsNullOrWhiteSpace(obj.newValue))
            {
                if (!ConnectsExist(obj.newValue))
                {
                    obj.objectForValidate.Name = obj.newValue;
                }
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
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(myPoint));

            LogInformation(Messages.Count.ToString());
            return nodes;
        }
        private List<ViewModelNode> UnFullMoveAllNode(MyPoint delta, List<ViewModelNode> nodes = null)
        {
            MyPoint myPoint = delta.Copy();
            myPoint.Mirror();
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(myPoint));
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
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(myPoint));
            return nodes;
        }
        private List<ViewModelNode> UnFullMoveAllSelectedNode(MyPoint delta, List<ViewModelNode> nodes = null)
        {
            MyPoint myPoint = delta.Copy();
            myPoint.Mirror();
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(myPoint));
            return nodes;
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
        private ViewModelConnector AddConnectorWithConnect(ViewModelConnector parameter, ViewModelConnector result)
        {
            if (result == null)
                return parameter;
            result.Node.CommandAddConnectorWithConnect.ExecuteWithSubscribe((1, result));
            return result;
        }
        private ViewModelConnector DeleteConnectorWithConnect(ViewModelConnector parameter, ViewModelConnector result)
        {
            result.Node.CommandDeleteConnectorWithConnect.ExecuteWithSubscribe(result);
            return parameter;
        }
        private DeleteMode DeleteSelectedElements(DeleteMode parameter, DeleteMode result)
        {
            if (result == DeleteMode.noCorrect)
            {
                bool keyN = Keyboard.IsKeyDown(Key.N);
                bool keyC = Keyboard.IsKeyDown(Key.C);

                if (keyN == keyC)
                {
                    result = DeleteMode.DeleteAllSelected;
                }
                else if (keyN)
                {
                    result = DeleteMode.DeleteNodes;
                }
                else if (keyC)
                {
                    result = DeleteMode.DeleteConnects;
                }

            }

            if ((result == DeleteMode.DeleteConnects) || (result == DeleteMode.DeleteAllSelected))
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

            if ((result == DeleteMode.DeleteNodes) || (result == DeleteMode.DeleteConnects))
            {
                count = 1;
            }
            else if (result == DeleteMode.DeleteAllSelected)
            {
                count = 2;
            }

            for (int i = 0; i < count; i++)
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
                foreach (var connector in GetAllConnectors().Where(x => x.Selected))
                {
                    result.Add((connector.Node.GetConnectorIndex(connector), connector));
                }
            }
            foreach (var element in result)
            {
                element.connector.Node.CommandDeleteConnectorWithConnect.ExecuteWithSubscribe(element.connector);
            }
            return result;
        }
        private List<(int index, ViewModelConnector connector)> UnDeleteSelectedConnectors(List<(int index, ViewModelConnector connector)> parameter, List<(int index, ViewModelConnector connector)> result)
        {
            foreach (var element in result)
            {
                element.connector.Node.CommandAddConnectorWithConnect.ExecuteWithSubscribe((element.index, element.connector));
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

                foreach (var connect in Connects)
                {
                    if (result.NodesToDelete.Contains(connect.FromConnector.Node))
                    {
                        result.ConnectsToDelete.Add(connect);
                    }
                    else if (result.NodesToDelete.Contains(connect.ToConnector.Node))
                    {
                        result.ConnectsToDeleteWithConnectors.Add((connect.FromConnector.Node.GetConnectorIndex(connect.FromConnector), connect));
                    }
                }
            }
            foreach (var element in result.ConnectsToDeleteWithConnectors)
            {
                element.connect.FromConnector.Node.CommandDeleteConnectorWithConnect.ExecuteWithSubscribe(element.connect.FromConnector);
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
                element.connect.FromConnector.Node.CommandAddConnectorWithConnect.ExecuteWithSubscribe((element.connectorIndex, element.connect.FromConnector));
            }

            return result;
        }
        private IEnumerable<ViewModelConnector> GetAllConnectors()
        {
            return this.Nodes.SelectMany(x => x.Transitions);
        }

        private bool ConnectsExist(string nameConnect)
        {
            return GetAllConnectors().Any(x => x.Name == nameConnect);
        }
        private bool NodesExist(string nameNode)
        {
            return Nodes.Any(x => x.Name == nameNode);
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

        bool WithoutSaving()
        {
            if (!ItSaved)
            {
                Dialog.ShowMessageBox("Exit without saving ?", "Exit without saving", MessageBoxButton.YesNo);

                return Dialog.Result == DialogResult.Yes;
            }

            return true;
        }
    }
}
