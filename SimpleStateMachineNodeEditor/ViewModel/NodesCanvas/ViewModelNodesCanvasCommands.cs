using DynamicData;
using ReactiveUI;
using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace SimpleStateMachineNodeEditor.ViewModel
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
        public ReactiveCommand<Unit, Unit> CommandExportToPNG { get; set; }
        public ReactiveCommand<Unit, Unit> CommandExportToJPEG { get; set; }
        public ReactiveCommand<Unit, Unit> CommandOpen { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSave { get; set; }
        public ReactiveCommand<Unit, Unit> CommandSaveAs { get; set; }
        public ReactiveCommand<Unit, Unit> CommandExit { get; set; }

        public ReactiveCommand<Unit, Unit> CommandChangeTheme { get; set; }

        #endregion commands without parameter

        #region commands with parameter

        public ReactiveCommand<ViewModelConnect, Unit> CommandAddConnect { get; set; }
        public ReactiveCommand<ViewModelConnect, Unit> CommandDeleteConnect { get; set; }
        public ReactiveCommand<ViewModelConnector, Unit> CommandAddDraggedConnect { get; set; }
        public ReactiveCommand<(ViewModelNode objectForValidate, string newValue), Unit> CommandValidateNodeName { get; set; }
        public ReactiveCommand<(ViewModelConnector objectForValidate, string newValue), Unit> CommandValidateConnectName { get; set; }
        public ReactiveCommand<int, Unit> CommandZoom { get; set; }
        public ReactiveCommand<Point, Unit> CommandSelect { get; set; }
        public ReactiveCommand<Point, Unit> CommandCut { get; set; }
        public ReactiveCommand<Point, Unit> CommandPartMoveAllNode { get; set; }
        public ReactiveCommand<Point, Unit> CommandPartMoveAllSelectedNode { get; set; }
        public ReactiveCommand<string, Unit> CommandLogDebug { get; set; }
        public ReactiveCommand<string, Unit> CommandLogError { get; set; }
        public ReactiveCommand<string, Unit> CommandLogInformation { get; set; }
        public ReactiveCommand<string, Unit> CommandLogWarning { get; set; }

        #endregion commands with parameter

        #region commands with undo-redo

        public Command<ViewModelConnector, ViewModelConnector> CommandAddConnectorWithConnect { get; set; }
        public Command<Point, List<ViewModelNode>> CommandFullMoveAllNode { get; set; }
        public Command<Point, List<ViewModelNode>> CommandFullMoveAllSelectedNode { get; set; }
        public Command<Point, ViewModelNode> CommandAddNodeWithUndoRedo { get; set; }
        public Command<ElementsForDelete, ElementsForDelete> CommandDeleteSelectedNodes { get; set; }
        public Command<List<(int index, ViewModelConnector element)>, List<(int index, ViewModelConnector element)>> CommandDeleteSelectedConnectors { get; set; }
        public Command<DeleteMode, DeleteMode> CommandDeleteSelectedElements { get; set; }


        public Command<(ViewModelNode node, string newName), (ViewModelNode node, string oldName)> CommandChangeNodeName { get; set; }
        public Command<(ViewModelConnector connector, string newName), (ViewModelConnector connector, string oldName)> CommandChangeConnectName { get; set; }

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
            CommandExportToPNG = ReactiveCommand.Create(ExportToPNG);
            CommandExportToJPEG = ReactiveCommand.Create(ExportToJPEG);

            CommandNew = ReactiveCommand.Create(New);
            CommandOpen = ReactiveCommand.Create(Open);
            CommandSave = ReactiveCommand.Create(Save);
            CommandSaveAs = ReactiveCommand.Create(SaveAs);
            CommandExit = ReactiveCommand.Create(Exit);
            CommandChangeTheme = ReactiveCommand.Create(ChangeTheme);


            CommandValidateNodeName = ReactiveCommand.Create<(ViewModelNode objectForValidate, string newValue)>(ValidateNodeName);
            CommandValidateConnectName = ReactiveCommand.Create<(ViewModelConnector objectForValidate, string newValue)>(ValidateConnectName);
            CommandAddConnect = ReactiveCommand.Create<ViewModelConnect>(AddConnect);
            CommandDeleteConnect = ReactiveCommand.Create<ViewModelConnect>(DeleteConnect);
            CommandZoom = ReactiveCommand.Create<int>(Zoom);
            CommandLogDebug = ReactiveCommand.Create<string>((message)=>LogDebug(message));
            CommandLogError = ReactiveCommand.Create<string>((message) => LogError(message));
            CommandLogInformation = ReactiveCommand.Create<string>((message) => LogInformation(message));
            CommandLogWarning = ReactiveCommand.Create<string>((message) => LogWarning(message));
            CommandSelect = ReactiveCommand.Create<Point>(StartSelect);
            CommandCut = ReactiveCommand.Create<Point>(StartCut);
            CommandAddDraggedConnect = ReactiveCommand.Create<ViewModelConnector>(AddDraggedConnect);
            CommandDeleteDraggedConnect = ReactiveCommand.Create(DeleteDraggedConnect);
                       
            
            CommandPartMoveAllNode = ReactiveCommand.Create<Point>(PartMoveAllNode);
            CommandPartMoveAllSelectedNode = ReactiveCommand.Create<Point>(PartMoveAllSelectedNode);


            CommandFullMoveAllNode = new Command<Point, List<ViewModelNode>>(FullMoveAllNode, UnFullMoveAllNode, NotSaved);
            CommandFullMoveAllSelectedNode = new Command<Point, List<ViewModelNode>>(FullMoveAllSelectedNode, UnFullMoveAllSelectedNode, NotSaved);
            CommandAddConnectorWithConnect = new Command<ViewModelConnector, ViewModelConnector>(AddConnectorWithConnect, DeleteConnectorWithConnect, NotSaved);
            CommandAddNodeWithUndoRedo = new Command<Point, ViewModelNode>(AddNodeWithUndoRedo, DeleteNodeWithUndoRedo, NotSaved);
            CommandDeleteSelectedNodes = new Command<ElementsForDelete, ElementsForDelete>(DeleteSelectedNodes, UnDeleteSelectedNodes, NotSaved);
            CommandDeleteSelectedConnectors = new Command<List<(int index, ViewModelConnector element)>, List<(int index, ViewModelConnector connector)>>(DeleteSelectedConnectors, UnDeleteSelectedConnectors, NotSaved);
            CommandDeleteSelectedElements = new Command<DeleteMode, DeleteMode>(DeleteSelectedElements, UnDeleteSelectedElements);
            CommandChangeNodeName = new Command<(ViewModelNode node, string newName), (ViewModelNode node, string oldName)>(ChangeNodeName, UnChangeNodeName);
            CommandChangeConnectName = new Command<(ViewModelConnector connector, string newName), (ViewModelConnector connector, string oldName)>(ChangeConnectName, UnChangeConnectName);


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
            foreach (var node in Nodes.Items)
            { node.Selected = true; }
        }
        private void CollapseUpAll()
        {
            foreach (var node in Nodes.Items)
            {
                node.IsCollapse = true;
            }
        }
        private void ExpandDownAll()
        {
            foreach (var node in Nodes.Items)
            {
                node.IsCollapse = false;
            }
        }
        private void ErrosUpdaate()
        {
            Messages.RemoveMany(Messages.Where(x => x.TypeMessage == DisplayMessageType || DisplayMessageType == TypeMessage.All));
        }
        private void ChangeTheme()
        {
            if (Theme == Themes.Dark)
            {
                SetTheme(Themes.Light);
            }
            else if (Theme == Themes.Light)
            {
                SetTheme(Themes.Dark);
            }

        }
        private void SetTheme(Themes theme)
        {
            Application.Current.Resources.Clear();
            var uri = new Uri(themesPaths[theme], UriKind.RelativeOrAbsolute);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);       
            LoadIcons();
            Theme = theme;
        }
        private void LoadIcons()
        {
            string path = @"Icons\Icons.xaml";
            var uri = new Uri(path, UriKind.RelativeOrAbsolute);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
        private void CollapseUpSelected()
        {
            foreach (var node in Nodes.Items.Where(x => x.Selected))
            {
                node.IsCollapse = true;
            }
        }
        private void ExpandDownSelected()
        {
            foreach (var node in Nodes.Items.Where(x => x.Selected))
            {
                node.IsCollapse = false;
            }
        }
        private void UnSelectedAll()
        {
            foreach (var node in Nodes.Items)
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
            Point cutterStartPoint = Cutter.StartPoint.Division(Scale.Value);
            Point cutterEndPoint = Cutter.EndPoint.Division(Scale.Value);

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
            Point selectorPoint1 = Selector.Point1WithScale.Division(Scale.Value);
            Point selectorPoint2 = Selector.Point2WithScale.Division(Scale.Value);

            foreach (ViewModelNode node in Nodes.Items)
            {
                node.Selected = MyUtils.CheckIntersectTwoRectangles(node.Point1, node.Point2, selectorPoint1, selectorPoint2);
            }
        }
        private void ExportToPNG()
        {
            Dialog.ShowSaveFileDialog("PNG Image (.png)|*.png", SchemeName(), "Export scheme to PNG");
            if (Dialog.Result != DialogResult.Ok)
                return;
            ImageFormat = ImageFormats.PNG;
            ImagePath = Dialog.FileName;
        }
        private void ExportToJPEG()
        {
            Dialog.ShowSaveFileDialog("JPEG Image (.jpeg)|*.jpeg", SchemeName(), "Export scheme to JPEG");
            if (Dialog.Result != DialogResult.Ok)
                return;
            ImageFormat = ImageFormats.JPEG;
            ImagePath = Dialog.FileName;
        }
        private void New()
        {
            if (!WithoutSaving())
                return;
            ClearScheme();
            this.SetupStartState();
        }
        private void ClearScheme()
        {
            this.Nodes.Clear();
            this.Connects.Clear();
            this.NodesCount = 0;
            this.TransitionsCount = 0;
            this.SchemePath = "";
            this.ImagePath = "";
            WithoutMessages = false;
            this.Messages.Clear();
            ItSaved = true;
        }
        private void Open()
        {
            if (!WithoutSaving())
                return;

            Dialog.ShowOpenFileDialog("XML-File | *.xml", SchemeName(), "Import scheme from xml file");
            if (Dialog.Result != DialogResult.Ok)
                return;
            Mouse.OverrideCursor = Cursors.Wait;
            string fileName = Dialog.FileName;
            ClearScheme();
            WithoutMessages = true;
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
                this.SetAsStart(this.Nodes.Items.Single(x => x.Name == startState));

            #endregion  setup start state

            #endregion  setup states/nodes

            #region setup Transitions/connects

            var Transitions = stateMachineXElement.Element("Transitions")?.Elements()?.ToList() ?? new List<XElement>();
            ViewModelConnect viewModelConnect;
            Transitions?.Reverse();
            foreach (var transition in Transitions)
            {
                viewModelConnect = ViewModelConnector.FromXElement(this, transition, out string errorMesage, ConnectsExist);
                if (WithError(errorMesage, x => Connects.Add(x), viewModelConnect))
                    return;
            }
            SchemePath = fileName;

            #endregion  setup Transitions/connects
            Mouse.OverrideCursor = null;
            WithoutMessages = false;
            LogDebug("Scheme was loaded from file \"{0}\"", SchemePath);

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
                ClearScheme();
                LogError("File is not valid. " + errorMessage);
                this.SetupStartState();
                Mouse.OverrideCursor = null;
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
                WithValidateScheme(() =>
                {
                    Save(SchemePath);
                });
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
            WithValidateScheme(()=>
            {
                Dialog.ShowSaveFileDialog("XML-File | *.xml", SchemeName(), "Save scheme as...");
                if (Dialog.Result != DialogResult.Ok)
                    return;

                Save(Dialog.FileName);
            });
        }
        private void Save(string fileName)
        {            
            Mouse.OverrideCursor = Cursors.Wait;
            XDocument xDocument = new XDocument();
            XElement stateMachineXElement = new XElement("StateMachine");
            xDocument.Add(stateMachineXElement);
            XElement states = new XElement("States");
            stateMachineXElement.Add(states);
            foreach (var state in Nodes.Items)
            {
                states.Add(state.ToXElement());
            }

            XElement startState = new XElement("StartState");
            stateMachineXElement.Add(startState);
            startState.Add(new XAttribute("Name", StartState.Name));


            XElement transitions = new XElement("Transitions");
            stateMachineXElement.Add(transitions);
            foreach (var transition in Nodes.Items.SelectMany(x => x.TransitionsForView.Where(y => !string.IsNullOrEmpty(y.Name))))
            {
                transitions.Add(transition.ToXElement());
            }

            xDocument.Save(fileName);
            ItSaved = true;
            SchemePath = fileName;
            Mouse.OverrideCursor = null;
            LogDebug("Scheme was saved as \"{0}\"", SchemePath);
        }
        private void WithValidateScheme(Action action)
        {
            var unReachable = ValidateScheme();
            if (unReachable.Count < 1)
            {
                action.Invoke();
            }
            else
            {
                LogError("Nodes without connects: {0}", string.Join(',', unReachable));
            }
        }
        private List<string> ValidateScheme()
        {
          Dictionary<string, bool> forValidate =  Nodes.Items.Where(x=>x!=StartState).ToDictionary(x => x.Name, x=>false);

            foreach(var connect in Connects )
            {
                forValidate[connect.ToConnector.Node.Name] = true;
            }

          return forValidate.Where(x => !x.Value).Select(x=>x.Key).ToList();
        }

        private void StartSelect(Point point)
        {
            Selector.CommandStartSelect.ExecuteWithSubscribe(point);
        }
        private void StartCut(Point point)
        {
            Cutter.CommandStartCut.ExecuteWithSubscribe(point);
        }
        private void PartMoveAllNode(Point delta)
        {
            foreach (var node in Nodes.Items)
            { node.CommandMove.ExecuteWithSubscribe(delta); }
        }
        private void PartMoveAllSelectedNode(Point delta)
        {
            foreach (var node in Nodes.Items.Where(x => x.Selected))
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
                    LogDebug("Node \"{0}\"  has been renamed . New name is \"{1}\"", obj.objectForValidate.Name, obj.newValue);

                    CommandChangeNodeName.Execute((obj.objectForValidate, obj.newValue));
                }
                else
                {
                    LogError("Name for node doesn't set, because node with name \"{0}\" already exist", obj.newValue);
                }
            }
            else
            {
                LogError("Name for node doesn't set, name off node should not be empty", obj.newValue);
            }
        }
        private void ValidateConnectName((ViewModelConnector objectForValidate, string newValue) obj)
        {
            if (!String.IsNullOrWhiteSpace(obj.newValue))
            {
                if (!ConnectsExist(obj.newValue))
                {
                    LogDebug("Transition \"{0}\"  has been renamed . New name is \"{1}\"", obj.objectForValidate.Name, obj.newValue);

                    CommandChangeConnectName.Execute((obj.objectForValidate, obj.newValue));
                }
                else
                {
                    LogError("Name for transition doesn't set, because transition with name \"{0}\" already exist", obj.newValue);
                }
            }
            else
            {
                LogError("Name for transition doesn't set, name off transition should not be empty", obj.newValue);
            }
        }


        private List<ViewModelNode> FullMoveAllNode(Point delta, List<ViewModelNode> nodes = null)
        {
            if (nodes == null)
            {
                nodes = Nodes.Items.ToList();
                delta = new Point();
            }
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(delta));
            return nodes;
        }
        private List<ViewModelNode> UnFullMoveAllNode(Point delta, List<ViewModelNode> nodes = null)
        {
            Point myPoint = delta.Copy();
            myPoint = myPoint.Mirror();
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(myPoint));
            return nodes;
        }
        private List<ViewModelNode> FullMoveAllSelectedNode(Point delta, List<ViewModelNode> nodes = null)
        {
            Point myPoint = delta.Copy();
            if (nodes == null)
            {
                nodes = Nodes.Items.Where(x => x.Selected).ToList();
                myPoint = new Point();
            }
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(myPoint));
            return nodes;
        }
        private List<ViewModelNode> UnFullMoveAllSelectedNode(Point delta, List<ViewModelNode> nodes = null)
        {
            Point myPoint = delta.Copy();
            myPoint = myPoint.Mirror();
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(myPoint));
            return nodes;
        }
        private ViewModelNode AddNodeWithUndoRedo(Point parameter, ViewModelNode result)
        {
            ViewModelNode newNode = result;
            if (result == null)
            {
                //MyPoint myPoint = parameter.Copy();
                //myPoint /= Scale.Value;
                newNode = new ViewModelNode(this, GetNameForNewNode(), parameter.Division(Scale.Value));
               
            }
            else
            {
                NodesCount--;
            }
            Nodes.Add(newNode);
            LogDebug("Node with name \"{0}\" was added", newNode.Name);
            return newNode;
        }
        private ViewModelNode DeleteNodeWithUndoRedo(Point parameter, ViewModelNode result)
        {
            Nodes.Remove(result);
            LogDebug("Node with name \"{0}\" was removed", result.Name);
            return result;
        }
        private ViewModelConnector AddConnectorWithConnect(ViewModelConnector parameter, ViewModelConnector result)
        {
            if (result == null)
            {
                return parameter;
                
            }
            else
                TransitionsCount--;

            result.Node.CommandAddConnectorWithConnect.ExecuteWithSubscribe((1, result));
            LogDebug("Transition with name \"{0}\" was added", result.Name);
            return result;
        }
        private ViewModelConnector DeleteConnectorWithConnect(ViewModelConnector parameter, ViewModelConnector result)
        {
            result.Node.CommandDeleteConnectorWithConnect.ExecuteWithSubscribe(result);
            LogDebug("Transition with name \"{0}\" was removed", result.Name);
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
                LogDebug("Transition with name \"{0}\" was removed", element.connector.Name);
            }
            return result;
        }
        private List<(int index, ViewModelConnector connector)> UnDeleteSelectedConnectors(List<(int index, ViewModelConnector connector)> parameter, List<(int index, ViewModelConnector connector)> result)
        {
            foreach (var element in result)
            {
                TransitionsCount--;
                element.connector.Node.CommandAddConnectorWithConnect.ExecuteWithSubscribe((element.index, element.connector));
                LogDebug("Transition with name \"{0}\" was added", element.connector.Name);
            }

            return result;
        }
        private (ViewModelConnector connector, string oldName) ChangeConnectName((ViewModelConnector connector, string newName) parameter, (ViewModelConnector connector, string oldName) result)
        {
            string oldName = parameter.connector.Name;
            parameter.connector.Name = parameter.newName;
            return (parameter.connector, oldName);
        }
        private (ViewModelConnector connector, string oldName) UnChangeConnectName((ViewModelConnector connector, string newName) parameter, (ViewModelConnector connector, string oldName) result)
        {
            result.connector.Name = result.oldName;
            return result;
        }


        private (ViewModelNode node, string oldName) ChangeNodeName((ViewModelNode node, string newName) parameter, (ViewModelNode node, string oldName) result)
        {
            string oldName = parameter.node.Name;
            parameter.node.Name = parameter.newName;
            return (parameter.node, oldName);
        }
        private (ViewModelNode node, string oldName) UnChangeNodeName((ViewModelNode node, string newName) parameter, (ViewModelNode node, string oldName) result)
        {
            result.node.Name = result.oldName;
            return result;
        }
        private ElementsForDelete DeleteSelectedNodes(ElementsForDelete parameter, ElementsForDelete result)
        {
            if (result == null)
            {
                result = new ElementsForDelete();
                result.NodesToDelete = Nodes.Items.Where(x => x.Selected && x.CanBeDelete).ToList();
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
                LogDebug("Transition with name \"{0}\" was removed", element.connect.FromConnector.Name);
            }

            Connects.RemoveMany(result.ConnectsToDelete);
            Nodes.RemoveMany(result.NodesToDelete);
            foreach(var node in result.NodesToDelete)
            {
                LogDebug("Node with name \"{0}\" was removed", node.Name);
            }

            return result;
        }
        private ElementsForDelete UnDeleteSelectedNodes(ElementsForDelete parameter, ElementsForDelete result)
        {
            NodesCount -= result.NodesToDelete.Count;
            Nodes.AddRange(result.NodesToDelete);
            foreach (var node in result.NodesToDelete)
            {
                LogDebug("Node with name \"{0}\" was added", node.Name);
            }
            Connects.AddRange(result.ConnectsToDelete);
            result.ConnectsToDeleteWithConnectors.Sort(ElementsForDelete.Sort);
            foreach (var element in result.ConnectsToDeleteWithConnectors)
            {
                TransitionsCount--;
                element.connect.FromConnector.Node.CommandAddConnectorWithConnect.ExecuteWithSubscribe((element.connectorIndex, element.connect.FromConnector));
                LogDebug("Transition with name \"{0}\" was added", element.connect.FromConnector.Name);
            }

            return result;
        }
        private IEnumerable<ViewModelConnector> GetAllConnectors()
        {
            return this.Nodes.Items.SelectMany(x => x.Transitions.Items);
        }

        private bool ConnectsExist(string nameConnect)
        {
            return GetAllConnectors().Any(x => x.Name == nameConnect);
        }
        private bool NodesExist(string nameNode)
        {
            return Nodes.Items.Any(x => x.Name == nameNode);
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
