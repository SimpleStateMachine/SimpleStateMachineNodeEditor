using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Reactive.Linq;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using SimpleStateMachineNodeEditor.Helpers;
using SimpleStateMachineNodeEditor.Helpers.Enums;
using System.Xml.Linq;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelConnect : ReactiveObject
    {
        /// <summary>
        /// Точка, из которой выходит линия ( совпадает с центром элемента, из которого выходит линия)
        /// </summary>
        [Reactive] public MyPoint StartPoint { get; set; } = new MyPoint();

        /// <summary>
        /// Точка, в которую приходит линия ( совпадает с центром элемента, в который приходит линия)
        /// </summary>
        [Reactive] public MyPoint EndPoint { get; set; } = new MyPoint();

        /// <summary>
        /// Первая промежуточная точка линии 
        /// </summary>
        [Reactive] public MyPoint Point1 { get; set; } = new MyPoint();

        /// <summary>
        /// Вторая промежуточная точка линии
        /// </summary>
        [Reactive] public MyPoint Point2 { get; set; } = new MyPoint();

        /// <summary>
        /// Цвет линии
        /// </summary>
        [Reactive] public Brush Stroke { get; set; } = Application.Current.Resources["ColorConnector"] as SolidColorBrush;

        /// <summary>
        /// Флаг того, что соединение выбрано
        /// </summary>
        [Reactive] public bool Selected { get; set; } = true;

        /// <summary>
        /// Элемент, из которого выходит линия
        /// </summary>
        [Reactive] public ViewModelConnector FromConnector { get; set; }

        /// <summary>
        /// Элемент, в который приходит линия
        /// </summary>
        [Reactive] public ViewModelConnector ToConnector { get; set; }

        [Reactive] public DoubleCollection StrokeDashArray { get; set; }

        [Reactive] public double StrokeThickness { get; set; } = 1;

        public ViewModelConnect(ViewModelConnector fromConnector)
        {

            SetupSubscriptions();
            SetupCommands();

            FromConnector = fromConnector;
            FromConnector.Connect = this;
            //SetupCommands();
        }
        #region Setup Subscriptions

        private void SetupSubscriptions()
        {
            this.WhenAnyValue(x => x.FromConnector.PositionConnectPoint.Value).Subscribe(value => StartPointUpdate(value));
            this.WhenAnyValue(x => x.ToConnector.PositionConnectPoint.Value).Subscribe(value => EndPointUpdate(value));
            this.WhenAnyValue(x => x.StartPoint.Value, x => x.EndPoint.Value).Subscribe(_ => UpdateMedium());
            this.WhenAnyValue(x => x.FromConnector).Where(x => x != null).Subscribe(_ => FromConnectChanged());
            this.WhenAnyValue(x => x.ToConnector).Where(x => x != null).Subscribe(_ => ToConnectChanged());

            this.WhenAnyValue(x => x.FromConnector.Node.NodesCanvas.Scale.Value).Subscribe(value => StrokeThickness = value);
            this.WhenAnyValue(x => x.Selected).Subscribe(value => { this.StrokeDashArray = value ? new DoubleCollection() { 10, 3 } : null; });
        }

        private void FromConnectChanged()
        {
            StartPointUpdate(FromConnector.PositionConnectPoint.ToPoint());

        }
        private void ToConnectChanged()
        {
            EndPointUpdate(ToConnector.PositionConnectPoint.ToPoint());
            Selected = false;
        }
        private void StartPointUpdate(Point point)
        {
            StartPoint.Set(point);
        }
        private void EndPointUpdate(Point point)
        {
            EndPoint.Set(point);
        }
        private void UpdateMedium()
        {
            MyPoint different = EndPoint - StartPoint;
            Point1.Set(StartPoint.X + 3 * different.X / 8, StartPoint.Y + 1 * different.Y / 8);
            Point2.Set(StartPoint.X + 5 * different.X / 8, StartPoint.Y + 7 * different.Y / 8);
        }

        #endregion Setup Subscriptions

        #region Setup Commands

        private void SetupCommands()
        {

        }

        #endregion Setup Commands

        public XElement ToXElement()
        {
            XElement element = new XElement("Transition");
            element.Add(new XAttribute("Name", FromConnector.Name));
            element.Add(new XAttribute("From", FromConnector.Node.Name));
            element.Add(new XAttribute("To", ToConnector.Node.Name));

            return element;
        }

        public static ViewModelConnect FromXElement(ViewModelNodesCanvas nodesCanvas, XElement node)
        {
            string name = node.Attribute("Name")?.Value;
            string from = node.Attribute("From")?.Value;
            string to = node.Attribute("To")?.Value;
            ViewModelNode nodeFrom = nodesCanvas.Nodes.Single(x => x.Name == from);
            ViewModelNode nodeTo = nodesCanvas.Nodes.Single(x => x.Name == to);

            nodeFrom.CurrentConnector.Name = name;
            ViewModelConnect viewModelConnect = new ViewModelConnect(nodeFrom.CurrentConnector);
            viewModelConnect.ToConnector = nodeTo.Input;
            nodeFrom.CommandAddEmptyConnector.Execute();

            return viewModelConnect;
        }
    }
}
