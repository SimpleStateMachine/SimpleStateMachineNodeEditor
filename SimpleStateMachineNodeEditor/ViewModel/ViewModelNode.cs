using System;
using System.Windows;
using System.Windows.Media;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;

using DynamicData.Binding;

using SimpleStateMachineNodeEditor.Helpers;
using System.Linq;
using System.Xml.Linq;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using DynamicData;
using System.Reactive;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using SimpleStateMachineNodeEditor.ViewModel.NodesCanvas;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelNode : ReactiveValidationObject<ViewModelNode>
    {
        [Reactive] public MyPoint Point1 { get; set; } = new MyPoint();
        [Reactive] public MyPoint Point2 { get; set; } = new MyPoint();
        [Reactive] public Size Size { get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public bool NameEnable { get; set; } = true;
        [Reactive] public bool Selected { get; set; }
        [Reactive] public Brush BorderBrush { get; set; } = Application.Current.Resources["ColorNodeBorder"] as SolidColorBrush;
        [Reactive] public bool? TransitionsVisible { get; set; } = true;
        [Reactive] public bool? RollUpVisible { get; set; } = true;
        [Reactive] public bool CanBeDelete { get; set; } = true;
        [Reactive] public bool IsCollapse { get; set; }
        [Reactive] public ViewModelConnector Input { get; set; }
        [Reactive] public ViewModelConnector Output { get; set; }
        [Reactive] public ViewModelConnector CurrentConnector { get; set; }
        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }
        [Reactive] public int IndexStartSelectConnectors { get; set; } = 0;

        public IObservableCollection<ViewModelConnector> Transitions { get; set; } = new ObservableCollectionExtended<ViewModelConnector>();
        public int Zindex { get; private set; }
        
        private ViewModelNode()
        {
            SetupCommands();
            SetupBinding();
        }


        public ViewModelNode(ViewModelNodesCanvas nodesCanvas)
        {
            NodesCanvas = nodesCanvas;
            Zindex = nodesCanvas.Nodes.Count;
            
            SetupConnectors();
            SetupCommands();
            SetupBinding();
            SetupSubscriptions();
        }

        #region SetupBinding
        private void SetupBinding()
        {
        }
        #endregion SetupBinding

        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.Selected).Subscribe(value => { this.BorderBrush = value ? Application.Current.Resources["ColorSelectedElement"] as SolidColorBrush : Brushes.LightGray; });
            this.WhenAnyValue(x => x.Transitions.Count).Buffer(2, 1).Select(x => (Previous: x[0], Current: x[1])).Subscribe(x => UpdateCount(x.Previous, x.Current));

            this.WhenAnyValue(x => x.Point1.Value, x => x.Size).Subscribe(_ => UpdatePoint2());
            this.WhenAnyValue(x => x.IsCollapse).Subscribe(value => Collapse(value));
        }
        #endregion Setup Subscriptions
        #region Connectors
        private void SetupConnectors()
        {
            Input = new ViewModelConnector(NodesCanvas, this)
            {
                Name = "Input"
            };
            Output = new ViewModelConnector(NodesCanvas, this)
            {
                Name = "Output",
                Visible = null
            };
            AddEmptyConnector();
        }
        #endregion Connectors

        #region Setup Commands

        public ReactiveCommand<Unit, Unit> CommandUnSelectedAllConnectors { get; set; }
        public ReactiveCommand<Unit, Unit> CommandAddEmptyConnector { get; set; }
        public ReactiveCommand<SelectMode, Unit> CommandSelect { get; set; }
        public ReactiveCommand<MyPoint, Unit> CommandMove { get; set; }
        public ReactiveCommand<(int index, ViewModelConnector connector), Unit> CommandAddConnectorWithConnect { get; set; }
        public ReactiveCommand<ViewModelConnector, Unit> CommandDeleteConnectorWithConnect { get; set; }
        public ReactiveCommand<string, Unit> CommandValidateName { get; set; }

        public ReactiveCommand<ViewModelConnector, Unit> CommandSelectWithShiftForConnectors { get; set; }
        public ReactiveCommand<ViewModelConnector, Unit> CommandSetConnectorAsStartSelect { get; set; }

        private void SetupCommands()
        {
            CommandSelect = ReactiveCommand.Create<SelectMode>(Select);
            CommandMove = ReactiveCommand.Create<MyPoint>(Move);
            CommandAddEmptyConnector = ReactiveCommand.Create(AddEmptyConnector);
            CommandSelectWithShiftForConnectors = ReactiveCommand.Create<ViewModelConnector>(SelectWithShiftForConnectors);
            CommandSetConnectorAsStartSelect = ReactiveCommand.Create<ViewModelConnector>(SetConnectorAsStartSelect);
            CommandUnSelectedAllConnectors = ReactiveCommand.Create(UnSelectedAllConnectors);
            CommandAddConnectorWithConnect = ReactiveCommand.Create<(int index, ViewModelConnector connector)>(AddConnectorWithConnect);
            CommandDeleteConnectorWithConnect = ReactiveCommand.Create<ViewModelConnector>(DeleteConnectorWithConnec);
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
        private void UpdateCount(int oldValue, int newValue)
        {
            if (newValue > oldValue)
            {
                NodesCanvas.TransitionsCount++;
            }
        }
        #endregion Setup Commands

        private void Collapse(bool value)
        {
            if (!value)
            {
                TransitionsVisible = true;
                Output.Visible = null;
            }
            else
            {
                TransitionsVisible = null;
                Output.Visible = true;
                UnSelectedAllConnectors();
            }
            NotSaved();
        }
        public int GetConnectorIndex(ViewModelConnector connector)
        {
            return Transitions.IndexOf(connector);
        }

        private void AddConnectorWithConnect((int index, ViewModelConnector connector) element)
        {
            Transitions.Insert(element.index, element.connector);
            if(element.connector.Connect!=null)
            {
                NodesCanvas.CommandAddConnect.ExecuteWithSubscribe(element.connector.Connect);
            }
        }
        private void DeleteConnectorWithConnec(ViewModelConnector connector)
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
        private void Move(MyPoint delta)
        {
            Point1 += delta / NodesCanvas.Scale.Value;
        }
        private void ValidateName(string newName)
        {

              NodesCanvas.CommandValidateNodeName.ExecuteWithSubscribe((this, newName));
        }
        private void UpdatePoint2()
        {
            Point2.Set(Point1.X + Size.Width, Point1.Y + Size.Height);
        }

        private void AddEmptyConnector()
        {
            if (CurrentConnector != null)
            {
                CurrentConnector.TextEnable = true;
                CurrentConnector.FormEnable = false;
                if (string.IsNullOrEmpty(CurrentConnector.Name))
                    CurrentConnector.Name = "Transition " + NodesCanvas.TransitionsCount.ToString();
            }
            CurrentConnector = new ViewModelConnector(NodesCanvas, this)
            {
                TextEnable = false
            };
            Transitions.Insert(0, CurrentConnector);
        }
        private void UnSelectedAllConnectors()
        {
           foreach(var transition in Transitions)
            {
                transition.Selected = false;
            }

            IndexStartSelectConnectors = 0;
        }
        private void SetConnectorAsStartSelect(ViewModelConnector viewModelConnector)
        {
            IndexStartSelectConnectors = Transitions.IndexOf(viewModelConnector) - 1;
        }
        private void SelectWithShiftForConnectors(ViewModelConnector viewModelConnector)
        {
            if (viewModelConnector == null)
                return;

            var transitions = this.Transitions.Skip(1);
            int indexCurrent = transitions.IndexOf(viewModelConnector);
            int indexStart = IndexStartSelectConnectors;
            UnSelectedAllConnectors();
            IndexStartSelectConnectors = indexStart;
            transitions = transitions.Skip(Math.Min(indexCurrent, indexStart)).SkipLast(Transitions.Count() - Math.Max(indexCurrent, indexStart) - 2);
            foreach (var transition in transitions)
            {
                transition.Selected = true;
            }
        }
        public XElement ToXElement()
        {
            XElement element = new XElement("State");
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("Position", Point1.ToString()));
            element.Add(new XAttribute("IsCollapse", IsCollapse.ToString()));
            return element;
        }

        public static ViewModelNode FromXElement(ViewModelNodesCanvas nodesCanvas, XElement node, out string errorMessage, Func<string, bool> actionForCheck)
        {
            errorMessage = null;
            ViewModelNode viewModelNode = null;
            string name = node.Attribute("Name")?.Value;

            if (string.IsNullOrEmpty(name))
            {
                errorMessage = "Node without name";
                return viewModelNode;
            }

            if (actionForCheck(name))
            {
                errorMessage = String.Format("Contains more than one node with name \"{0}\"", name);
                return viewModelNode;
            }

            viewModelNode = new ViewModelNode(nodesCanvas);
            viewModelNode.Name = name;

            var position = node.Attribute("Position")?.Value;
            if (position != null)
                viewModelNode.Point1 = MyPoint.Parse(position);

            var isCollapse = node.Attribute("IsCollapse")?.Value;
            if (isCollapse != null)
                viewModelNode.IsCollapse = bool.Parse(isCollapse);

            return viewModelNode;
        }
    }
}
