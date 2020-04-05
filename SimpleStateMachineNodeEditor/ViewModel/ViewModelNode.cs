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


namespace SimpleStateMachineNodeEditor.ViewModel
{
    /// <summary>
    /// Отображение для узла
    /// </summary>
    public class ViewModelNode : ReactiveValidationObject<ViewModelNode>
    {
        /// <summary>
        /// Точка левого верхнего угла
        /// </summary>
        [Reactive] public MyPoint Point1 { get; set; } = new MyPoint();

        /// <summary>
        /// Точка нижнего правого угла
        /// </summary>
        [Reactive] public MyPoint Point2 { get; set; } = new MyPoint();

        /// <summary>
        /// Размер узла
        /// </summary>
        [Reactive] public Size Size { get; set; }

        /// <summary>
        /// Название узла (Указано в шапке)
        /// </summary>
        [Reactive] public string Name { get; set; }

        /// <summary>
        /// Допен ли заголовок для редактирования
        /// </summary>
        [Reactive] public bool NameEnable { get; set; } = true;


        /// <summary>
        /// Флаг того, что узел выбран
        /// </summary>
        [Reactive] public bool Selected { get; set; }

        /// <summary>
        /// Цвет рамки вокруг узла
        /// </summary>
        [Reactive] public Brush BorderBrush { get; set; } = Application.Current.Resources["ColorNodeBorder"] as SolidColorBrush;

        /// <summary>
        /// Отображаются ли переходы
        /// </summary>
        [Reactive] public bool? TransitionsVisible { get; set; } = true;

        /// <summary>
        /// Отображаются ли переходы
        /// </summary>
        [Reactive] public bool? RollUpVisible { get; set; } = true;

        /// <summary>
        /// Может ли быть удален
        /// </summary>
        [Reactive] public bool CanBeDelete { get; set; } = true;

        /// <summary>
        /// Вход для соединения с этим узлом
        /// </summary>
        [Reactive] public ViewModelConnector Input { get; set; }

        /// <summary>
        /// Выход ( используется, когда список переходов свернут )
        /// </summary>
        [Reactive] public ViewModelConnector Output { get; set; }

        /// <summary>
        /// Текущий переход, из которого можно создать соединение
        /// </summary>
        [Reactive] public ViewModelConnector CurrentConnector { get; set; }

        /// <summary>
        /// Канвас, которому принадлежит узел
        /// </summary>
        [Reactive] public ViewModelNodesCanvas NodesCanvas { get; set; }

        /// <summary>
        /// Список переходов
        /// </summary>
        public IObservableCollection<ViewModelConnector> Transitions { get; set; } = new ObservableCollectionExtended<ViewModelConnector>();



        public ViewModelNode(ViewModelNodesCanvas nodesCanvas)
        {
            NodesCanvas = nodesCanvas;
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
            CommandSelect = new SimpleCommandWithParameter<object>(this, Select);
            CommandMove = new SimpleCommandWithParameter<MyPoint>(this, Move);
            CommandCollapse = new SimpleCommandWithParameter<object>(this, Collapse);
            CommandAddEmptyConnector = new SimpleCommand(this, AddEmptyConnector);

            //CommandTransitionsDragLeave = new SimpleCommand(this, TransitionsDragLeave);
            //CommandTransitionsDragEnter = new SimpleCommand(this, TransitionsDragEnter);
            //CommandTransitionsDrop = new SimpleCommand(this, TransitionsDrop);
            //CommandTransitionsDragOver = new SimpleCommand(this, TransitionsDragOver);
            //CommandConnectorDrag = new SimpleCommandWithParameter<ViewModelConnector>(this, ConnectorDrag);

            CommandAddConnector = new SimpleCommandWithParameter<ViewModelConnector>(this, AddConnector);
            CommandDeleteConnector = new SimpleCommandWithParameter<ViewModelConnector>(this, DeleteConnector);
            CommandValidateName = new SimpleCommandWithParameter<string>(this, ValidateName);


        }

        #endregion Commands
        private void Collapse(object obj)
        {
            bool value = (bool)obj;
            Output.Visible = !value;
            if (value)
                TransitionsVisible = value;
            else
                TransitionsVisible = null;

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

                //CurrentConnector.Name = "Transition_" + NodesCanvas.Connects.Count.ToString();
                CurrentConnector.Name = "T" + NodesCanvas.Connects.Count.ToString();
            }
            CurrentConnector = new ViewModelConnector(NodesCanvas, this)
            {
                TextEnable = false
            };
            Transitions.Insert(0, CurrentConnector);
        }
    }
}
