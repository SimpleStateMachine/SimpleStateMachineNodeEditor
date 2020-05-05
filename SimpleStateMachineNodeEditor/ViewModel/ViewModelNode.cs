using System;
using System.Windows;
using System.Windows.Media;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Helpers;

using DynamicData.Binding;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Commands;
using System.Linq;
using System.Xml.Linq;

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

        [Reactive] public ViewModelConnector Input { get; set; }

        [Reactive] public ViewModelConnector Output { get; set; }

        [Reactive] public ViewModelConnector CurrentConnector { get; set; }

        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }

        public int Zindex { get; private set; }

        public IObservableCollection<ViewModelConnector> Transitions { get; set; } = new ObservableCollectionExtended<ViewModelConnector>();



        public ViewModelNode(ViewModelNodesCanvas nodesCanvas)
        {
            NodesCanvas = nodesCanvas;
            Zindex = nodesCanvas.Nodes.Count;
            //this.WhenAnyValue(x=>x.Name).Subscribe()
            this.WhenAnyValue(x => x.Selected).Subscribe(value => { this.BorderBrush = value ? Brushes.Red : Brushes.LightGray; });
            this.WhenAnyValue(x => x.Point1.Value, x => x.Size).Subscribe(_ => UpdatePoint2());
            SetupConnectors();
            SetupCommands();
        }

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

        #region Commands
        public SimpleCommandWithParameter<object> CommandSelect { get; set; }
        public SimpleCommandWithParameter<MyPoint> CommandMove { get; set; }
        public SimpleCommandWithParameter<object> CommandCollapse { get; set; }
        public SimpleCommandWithParameter<ViewModelConnector> CommandAddConnector { get; set; }
        public SimpleCommandWithParameter<ViewModelConnector> CommandDeleteConnector { get; set; }

        public SimpleCommandWithParameter<string> CommandValidateName { get; set; }

        public SimpleCommand CommandAddEmptyConnector { get; set; }
       

        //public SimpleCommand CommandTransitionsDragLeave { get; set; }

        //public SimpleCommand CommandTransitionsDragEnter { get; set; }

        //public SimpleCommand CommandTransitionsDrop { get; set; }

        //public SimpleCommand CommandTransitionsDragOver { get; set; }

        //public SimpleCommandWithParameter<ViewModelConnector> CommandConnectorDrag { get; set; }

        private void SetupCommands()
        {
            CommandSelect = new SimpleCommandWithParameter<object>(Select);
            CommandMove = new SimpleCommandWithParameter<MyPoint>(Move, NotSaved);
            CommandAddEmptyConnector = new SimpleCommand(AddEmptyConnector);
            CommandCollapse = new SimpleCommandWithParameter<object>(Collapse, NotSaved);
            //CommandTransitionsDragLeave = new SimpleCommand(TransitionsDragLeave);
            //CommandTransitionsDragEnter = new SimpleCommand(TransitionsDragEnter);
            //CommandTransitionsDrop = new SimpleCommand(TransitionsDrop);
            //CommandTransitionsDragOver = new SimpleCommand(TransitionsDragOver);
            //CommandConnectorDrag = new SimpleCommandWithParameter<ViewModelConnector>(this, ConnectorDrag);

            CommandAddConnector = new SimpleCommandWithParameter<ViewModelConnector>(AddConnector, NotSaved);
            CommandDeleteConnector = new SimpleCommandWithParameter<ViewModelConnector>(DeleteConnector, NotSaved);
            CommandValidateName = new SimpleCommandWithParameter<string>(ValidateName, NotSaved);


        }
        private void NotSaved()
        {
            NodesCanvas.ItSaved = false;
        }
        #endregion Commands
        private void Collapse(object obj)
        {
            bool value = (bool)obj;

            if (value)
            {
                TransitionsVisible = value;
                Output.Visible = null;
            }
            else
            {
                TransitionsVisible = null;
                Output.Visible = true;
            }
        }
        private void AddConnector(ViewModelConnector connector)
        {
            Transitions.Add(connector);
        }
        private void DeleteConnector(ViewModelConnector connector)
        {
            Transitions.Remove(connector);
        }
        private void Select(object obj = null)
        {
            if (obj == null)
            {
                this.Selected = !this.Selected;
                return;
            }
            if (Selected != true)
            {
                NodesCanvas.CommandUnSelectAll.Execute();
                this.Selected = true;
            }
        }
        private void Move(MyPoint delta)
        {
            Point1 += delta / NodesCanvas.Scale.Value;
        }
        private void ValidateName(string newName)
        {
            NodesCanvas.CommandValidateNodeName.Execute(new ValidateObjectProperty<ViewModelNode, string>(this, newName));
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
                if(string.IsNullOrEmpty(CurrentConnector.Name))
                    CurrentConnector.Name = "Transition " + NodesCanvas.Nodes.Sum(x => x.Transitions.Count - 1).ToString();
            }
            CurrentConnector = new ViewModelConnector(NodesCanvas, this)
            {
                TextEnable = false
            };
            Transitions.Insert(0, CurrentConnector);
        }

        public XElement ToXElement()
        {
            XElement element = new XElement("State");
            element.Add(new XAttribute("Name", Name));
            element.Add(new XAttribute("Position", Point1.ToString()));
            element.Add(new XAttribute("IsCollapse", (TransitionsVisible!=true).ToString()));
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
            if(position!=null)
                viewModelNode.Point1 = MyPoint.Parse(position);

            var isCollapse = node.Attribute("IsCollapse")?.Value;
            if (isCollapse != null)
                viewModelNode.Collapse(!bool.Parse(isCollapse));
      
            return viewModelNode;
        }
    }
}
