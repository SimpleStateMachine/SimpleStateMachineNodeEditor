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
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using SimpleStateMachineNodeEditor.Helpers.Configuration;
using Splat;
using Matrix = System.Windows.Media.Matrix;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public partial class NodesCanvasViewModel
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

        public ReactiveCommand<ConnectViewModel, Unit> CommandAddConnect { get; set; }
        public ReactiveCommand<ConnectViewModel, Unit> CommandDeleteConnect { get; set; }
        public ReactiveCommand<ConnectorViewModel, Unit> CommandAddDraggedConnect { get; set; }
        public ReactiveCommand<(NodeViewModel objectForValidate, string newValue), Unit> CommandValidateNodeName { get; set; }
        public ReactiveCommand<(ConnectorViewModel objectForValidate, string newValue), Unit> CommandValidateConnectName { get; set; }
        public ReactiveCommand<(Point point, double delta), Unit> CommandZoom { get; set; }
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

        public Command<ConnectorViewModel, ConnectorViewModel> CommandAddConnectorWithConnect { get; set; }
        public Command<Point, List<NodeViewModel>> CommandFullMoveAllNode { get; set; }
        public Command<Point, List<NodeViewModel>> CommandFullMoveAllSelectedNode { get; set; }
        public Command<Point, NodeViewModel> CommandAddNodeWithUndoRedo { get; set; }
        public Command<List<NodeViewModel>, ElementsForDelete> CommandDeleteSelectedNodes { get; set; }
        public Command<List<ConnectorViewModel>, List<(int index, ConnectorViewModel element)>> CommandDeleteSelectedConnectors { get; set; }
        public Command<DeleteMode, DeleteMode> CommandDeleteSelectedElements { get; set; }


        public Command<(NodeViewModel node, string newName), (NodeViewModel node, string oldName)> CommandChangeNodeName { get; set; }
        public Command<(ConnectorViewModel connector, string newName), (ConnectorViewModel connector, string oldName)> CommandChangeConnectName { get; set; }

        #endregion commands with undo-redo

        private void SetupCommands()
        {

            CommandRedo = ReactiveCommand.Create(ICommandWithUndoRedo.Redo);
            CommandUndo = ReactiveCommand.Create(ICommandWithUndoRedo.Undo);




            CommandSelectAll = ReactiveCommand.Create(SelectedAll);
            CommandUnSelectAll = ReactiveCommand.Create(UnSelectedAll);
            CommandSelectorIntersect = ReactiveCommand.Create(SelectNodes);
            CommandCutterIntersect = ReactiveCommand.Create(SelectConnects);
            CommandZoomIn = ReactiveCommand.Create(() => { ZoomIn(); });
            CommandZoomOut = ReactiveCommand.Create(() => { ZoomOut(); });
            CommandZoomOriginalSize = ReactiveCommand.Create(() => { ZoomOriginalSize(); });
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


            CommandValidateNodeName = ReactiveCommand.Create<(NodeViewModel objectForValidate, string newValue)>(ValidateNodeName);
            CommandValidateConnectName = ReactiveCommand.Create<(ConnectorViewModel objectForValidate, string newValue)>(ValidateConnectName);
            CommandAddConnect = ReactiveCommand.Create<ConnectViewModel>(AddConnect);
            CommandDeleteConnect = ReactiveCommand.Create<ConnectViewModel>(DeleteConnect);
            CommandZoom = ReactiveCommand.Create<(Point point, double delta)>(Zoom);
            CommandLogDebug = ReactiveCommand.Create<string>((message)=>LogDebug(message));
            CommandLogError = ReactiveCommand.Create<string>((message) => LogError(message));
            CommandLogInformation = ReactiveCommand.Create<string>((message) => LogInformation(message));
            CommandLogWarning = ReactiveCommand.Create<string>((message) => LogWarning(message));
            CommandSelect = ReactiveCommand.Create<Point>(StartSelect);
            CommandCut = ReactiveCommand.Create<Point>(StartCut);
            CommandAddDraggedConnect = ReactiveCommand.Create<ConnectorViewModel>(AddDraggedConnect);
            CommandDeleteDraggedConnect = ReactiveCommand.Create(DeleteDraggedConnect);
                       
            
            CommandPartMoveAllNode = ReactiveCommand.Create<Point>(PartMoveAllNode);
            CommandPartMoveAllSelectedNode = ReactiveCommand.Create<Point>(PartMoveAllSelectedNode);


            CommandFullMoveAllNode = new Command<Point, List<NodeViewModel>>(FullMoveAllNode, UnFullMoveAllNode);




            CommandFullMoveAllSelectedNode = new Command<Point, List<NodeViewModel>>(FullMoveAllSelectedNode, UnFullMoveAllSelectedNode, NotSaved);
            CommandAddConnectorWithConnect = new Command<ConnectorViewModel, ConnectorViewModel>(AddConnectorWithConnect, DeleteConnectorWithConnect, NotSaved);
            CommandAddNodeWithUndoRedo = new Command<Point, NodeViewModel>(AddNodeWithUndoRedo, DeleteNodetWithUndoRedo, NotSaved);
             CommandDeleteSelectedNodes = new Command<List<NodeViewModel>, ElementsForDelete>(DeleteSelectedNodes, UnDeleteSelectedNodes, NotSaved);
            CommandDeleteSelectedConnectors = new Command<List<ConnectorViewModel>, List<(int index, ConnectorViewModel connector)>>(DeleteSelectedConnectors, UnDeleteSelectedConnectors, NotSaved);
            CommandDeleteSelectedElements = new Command<DeleteMode, DeleteMode>(DeleteSelectedElements, UnDeleteSelectedElements);
            CommandChangeNodeName = new Command<(NodeViewModel node, string newName), (NodeViewModel node, string oldName)>(ChangeNodeName, UnChangeNodeName);
            CommandChangeConnectName = new Command<(ConnectorViewModel connector, string newName), (ConnectorViewModel connector, string oldName)>(ChangeConnectName, UnChangeConnectName);

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
            var configuration = Locator.Current.GetService<IConfiguration>();
            configuration.GetSection("Appearance:Theme").Set(theme);
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
            //Point cutterStartPoint = Cutter.StartPoint.Division(Scale.Value);
            //Point cutterEndPoint = Cutter.EndPoint.Division(Scale.Value);

            Point cutterStartPoint = Cutter.StartPoint;
            Point cutterEndPoint = Cutter.EndPoint;

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
            //Point selectorPoint1 = Selector.Point1WithScale.Division(Scale.Value);
            //Point selectorPoint2 = Selector.Point2WithScale.Division(Scale.Value);
            Point selectorPoint1 = Selector.Point1WithScale;
            Point selectorPoint2 = Selector.Point2WithScale;
            foreach (NodeViewModel node in Nodes.Items)
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
            SetupStartState();
        }
        private void ClearScheme()
        {
            Nodes.Clear();
            Connects.Clear();
            NodesCount = 0;
            TransitionsCount = 0;
            SchemePath = "";
            ImagePath = "";
            WithoutMessages = false;
            Messages.Clear();
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
            NodeViewModel viewModelNode = null;
            foreach (var state in States)
            {
                viewModelNode = NodeViewModel.FromXElement(this, state, out string errorMesage, NodesExist);
                if (WithError(errorMesage, x => Nodes.Add(x), viewModelNode))
                    return;
            }

            #region setup start state

            var startStateElement = stateMachineXElement.Element("StartState");
            if (startStateElement == null)
            {
                SetupStartState();
            }
            else
            {
                var startStateAttribute = startStateElement.Attribute("Name");
                if (startStateAttribute == null)
                {
                    Error("Start state element don't has name attribute");
                    return;
                }
                else
                {
                    string startStateName = startStateAttribute.Value;
                    if(string.IsNullOrEmpty(startStateName))
                    {
                        Error(string.Format("Name attribute of start state is empty.", startStateName));
                        return;
                    }

                    var startNode = Nodes.Items.SingleOrDefault(x => x.Name == startStateName);
                    if (startNode == null)
                    {
                        Error(string.Format("Unable to set start state. Node with name \"{0}\" don't exists", startStateName));
                        return;
                    }
                    else
                    {
                        SetAsStart(startNode);
                    }
                }

            }

            #endregion  setup start state

            #endregion  setup states/nodes

            #region setup Transitions/connects

            var Transitions = stateMachineXElement.Element("Transitions")?.Elements()?.ToList() ?? new List<XElement>();
            ConnectViewModel viewModelConnect;
            Transitions?.Reverse();
            foreach (var transition in Transitions)
            {
                viewModelConnect = ConnectorViewModel.FromXElement(this, transition, out string errorMesage, ConnectsExist);
                if (WithError(errorMesage, x => Connects.Add(x), viewModelConnect))
                    return;
            }
            SchemePath = fileName;

            #endregion  setup Transitions/connects

            #region setup Visualization
            XElement Visualization = stateMachineXElement.Element("Visualization");

          
            if (Visualization != null)
            {
                var visualizationStates = Visualization.Elements()?.ToList();
                if(visualizationStates!=null)
                {
                    var nodes = Nodes.Items.ToDictionary(x => x.Name, x => x);
                    Point point;
                    bool isCollapse;
                    string name;
                    string pointAttribute;
                    string isCollapseAttribute;
                    foreach (var visualization in visualizationStates)
                    {
                        name = visualization.Attribute("Name")?.Value;
                        if(nodes.TryGetValue(name, out NodeViewModel node))
                        {
                            pointAttribute = visualization.Attribute("Position")?.Value;
                            if (!PointExtensition.TryParseFromString(pointAttribute, out point))
                            {
                                Error(String.Format("Error parse attribute \'position\' for state with name \'{0}\'", name));
                                return;
                            }
                            isCollapseAttribute = visualization.Attribute("IsCollapse")?.Value;
                            if (!bool.TryParse(isCollapseAttribute, out isCollapse))
                            {
                                Error(String.Format("Error parse attribute \'isCollapse\' for state with name \'{0}\'", name));
                                return;
                            }
                            node.Point1 = point;
                            node.IsCollapse = isCollapse;
                        }
                        else
                        {
                            Error(String.Format("Visualization for state with name \'{0}\' that not exist", name));
                            return;
                        }
                    }
                }
                
            
                //NodeViewModel nodeViewModel = Nodes.w 
                //var position = node.Attribute("Position")?.Value;
                //Point point = string.IsNullOrEmpty(position) ? new Point() : PointExtensition.StringToPoint(position);
                //var isCollapse = node.Attribute("IsCollapse")?.Value;
                //if (isCollapse != null)
                //    viewModelNode.IsCollapse = bool.Parse(isCollapse);

            }
            #endregion  setup Visualization

            Mouse.OverrideCursor = null;
            WithoutMessages = false;
            LogDebug("Scheme was loaded from file \"{0}\"", SchemePath);

            bool WithError<T>(string errorMessage, Action<T> action, T obj)
            {
                if (string.IsNullOrEmpty(errorMessage))
                {
                    if (!Equals(obj, default(T)))
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
                SetupStartState();
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
            NeedExit = true;
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


            XElement visualizationXElement = new XElement("Visualization");
            stateMachineXElement.Add(visualizationXElement);
            foreach (var state in Nodes.Items)
            {
                visualizationXElement.Add(state.ToVisualizationXElement());
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
        private void Zoom((Point point, double delta) element)
        {
            ScaleCenter = element.point;
            double scaleValue = RenderTransformMatrix.M11;
            bool DeltaIsZero = (element.delta == 0);
            bool DeltaMax = ((element.delta > 0) && (scaleValue > ScaleMax));
            bool DeltaMin = ((element.delta < 0) && (scaleValue < ScaleMin));
            if (DeltaIsZero || DeltaMax || DeltaMin)
                return;

            double zoom = element.delta > 0 ? ScaleStep : 1 / ScaleStep;
            RenderTransformMatrix = MatrixExtension.ScaleAtPrepend(RenderTransformMatrix, zoom, zoom, element.point.X, element.point.Y);
        }

        //private void Zoom((Point point, double delta) element)
        //{
        //    Zoom(element.delta, element.point);
        //}

        //private void Zoom(double delta, Point? point=null)
        //{
        //    double scaleValue = RenderTransformMatrix.M11;
        //    bool DeltaIsZero = (delta == 0);
        //    bool DeltaMax = ((delta > 0) && (scaleValue > ScaleMax));
        //    bool DeltaMin = ((delta < 0) && (scaleValue < ScaleMin));
        //    if (DeltaIsZero || DeltaMax || DeltaMin)
        //        return;

        //    double zoom = delta > 0 ? ScaleStep : 1 / ScaleStep;
        //    if(point.HasValue)
        //        RenderTransformMatrix = MatrixExtension.ScaleAtPrepend(RenderTransformMatrix, zoom, zoom, point.Value.X, point.Value.Y);
        //    else
        //        RenderTransformMatrix = MatrixExtension.ScaleAtPrepend(RenderTransformMatrix, zoom, zoom);


        //}

        private void ZoomIn()
        {
            //Point point = new Point(RenderTransformMatrix.OffsetX, RenderTransformMatrix.OffsetY);
            Zoom((ScaleCenter,1));
        }
        private void ZoomOut()
        {
            //Point point = new Point(RenderTransformMatrix.OffsetX, RenderTransformMatrix.OffsetY);
            Zoom((ScaleCenter, -1));
        }
        private void ZoomOriginalSize()
        {
            RenderTransformMatrix = Matrix.Identity;
        }
        private void AddDraggedConnect(ConnectorViewModel fromConnector)
        {
            DraggedConnect = new ConnectViewModel(this, fromConnector);

            AddConnect(DraggedConnect);
        }
        private void DeleteDraggedConnect()
        {
            Connects.Remove(DraggedConnect);
            DraggedConnect.FromConnector.Connect = null;

        }
        private void AddConnect(ConnectViewModel ViewModelConnect)
        {
            Connects.Add(ViewModelConnect);
        }
        private void DeleteConnect(ConnectViewModel ViewModelConnect)
        {
            Connects.Remove(ViewModelConnect);
        }
        private void ValidateNodeName((NodeViewModel objectForValidate, string newValue) obj)
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
        private void ValidateConnectName((ConnectorViewModel objectForValidate, string newValue) obj)
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


        private List<NodeViewModel> FullMoveAllNode(Point delta, List<NodeViewModel> nodes = null)
        {
            if (nodes == null)
            {
                nodes = Nodes.Items.ToList();
                delta = new Point();
            }
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(delta));
            return nodes;
        }
        private List<NodeViewModel> UnFullMoveAllNode(Point delta, List<NodeViewModel> nodes = null)
        {
            Point myPoint = delta.Copy();
            myPoint = myPoint.Mirror();
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(myPoint));
            return nodes;
        }
        private List<NodeViewModel> FullMoveAllSelectedNode(Point delta, List<NodeViewModel> nodes = null)
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
        private List<NodeViewModel> UnFullMoveAllSelectedNode(Point delta, List<NodeViewModel> nodes = null)
        {
            Point myPoint = delta.Copy();
            myPoint = myPoint.Mirror();
            nodes.ForEach(node => node.CommandMove.ExecuteWithSubscribe(myPoint));
            return nodes;
        }
        private NodeViewModel AddNodeWithUndoRedo(Point parameter, NodeViewModel result)
        {
            NodeViewModel newNode = result;
            if (result == null)
            {
                //MyPoint myPoint = parameter.Copy();
                //myPoint /= Scale.Value;
                //newNode = new ViewModelNode(this, GetNameForNewNode(), parameter.Division(Scale.Value));
                newNode = new NodeViewModel(this, GetNameForNewNode(), parameter);
            }
            else
            {
                NodesCount--;
            }
            Nodes.Add(newNode);
            LogDebug("Node with name \"{0}\" was added", newNode.Name);
            return newNode;
        }
        private NodeViewModel DeleteNodetWithUndoRedo(Point parameter, NodeViewModel result)
        {
            Nodes.Remove(result);
            LogDebug("Node with name \"{0}\" was removed", result.Name);
            return result;
        }
        private ConnectorViewModel AddConnectorWithConnect(ConnectorViewModel parameter, ConnectorViewModel result)
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
        private ConnectorViewModel DeleteConnectorWithConnect(ConnectorViewModel parameter, ConnectorViewModel result)
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
                CommandDeleteSelectedConnectors.Execute();
            }
            if ((result == DeleteMode.DeleteNodes) || (result == DeleteMode.DeleteAllSelected))
            {
                CommandDeleteSelectedNodes.Execute();
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
        private List<(int index, ConnectorViewModel connector)> DeleteSelectedConnectors(List<ConnectorViewModel> parameter, List<(int index, ConnectorViewModel connector)> result)
        {
            if (result == null)
            {
               
                result = new List<(int index, ConnectorViewModel element)>();
                foreach (var connector in parameter?? GetAllConnectors().Where(x => x.Selected))
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
        private List<(int index, ConnectorViewModel connector)> UnDeleteSelectedConnectors(List<ConnectorViewModel>  parameter, List<(int index, ConnectorViewModel connector)> result)
        {
            foreach (var element in result)
            {
                TransitionsCount--;
                element.connector.Node.CommandAddConnectorWithConnect.ExecuteWithSubscribe((element.index, element.connector));
                LogDebug("Transition with name \"{0}\" was added", element.connector.Name);
            }

            return result;
        }
        private (ConnectorViewModel connector, string oldName) ChangeConnectName((ConnectorViewModel connector, string newName) parameter, (ConnectorViewModel connector, string oldName) result)
        {
            string oldName = parameter.connector.Name;
            parameter.connector.Name = parameter.newName;
            return (parameter.connector, oldName);
        }
        private (ConnectorViewModel connector, string oldName) UnChangeConnectName((ConnectorViewModel connector, string newName) parameter, (ConnectorViewModel connector, string oldName) result)
        {
            result.connector.Name = result.oldName;
            return result;
        }


        private (NodeViewModel node, string oldName) ChangeNodeName((NodeViewModel node, string newName) parameter, (NodeViewModel node, string oldName) result)
        {
            string oldName = parameter.node.Name;
            parameter.node.Name = parameter.newName;
            return (parameter.node, oldName);
        }
        private (NodeViewModel node, string oldName) UnChangeNodeName((NodeViewModel node, string newName) parameter, (NodeViewModel node, string oldName) result)
        {
            result.node.Name = result.oldName;
            return result;
        }
        private ElementsForDelete DeleteSelectedNodes(List<NodeViewModel> parameter, ElementsForDelete result)
        {
            if (result == null)
            {
                result = new ElementsForDelete();
             
                result.NodesToDelete = (parameter?.Where(x=>x.CanBeDelete)?? Nodes.Items.Where(x => x.Selected && x.CanBeDelete)).ToList();
                result.ConnectsToDelete = new List<ConnectViewModel>();
                result.ConnectsToDeleteWithConnectors = new List<(int connectorIndex, ConnectViewModel connect)>();

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
        private ElementsForDelete UnDeleteSelectedNodes(List<NodeViewModel> parameter, ElementsForDelete result)
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
        private IEnumerable<ConnectorViewModel> GetAllConnectors()
        {
            return Nodes.Items.SelectMany(x => x.Transitions.Items);
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
            public List<NodeViewModel> NodesToDelete;
            public List<ConnectViewModel> ConnectsToDelete;
            public List<(int connectorIndex, ConnectViewModel connect)> ConnectsToDeleteWithConnectors;

            public static int Sort((int connectorIndex, ConnectViewModel connect) A, (int connectorIndex, ConnectViewModel connect) B)
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
