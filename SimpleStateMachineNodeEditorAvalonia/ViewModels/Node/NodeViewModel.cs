using Avalonia;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public partial class NodeViewModel:BaseViewModel
    {
        [Reactive] public HeaderNodeViewModel Header { get; set; }
        [Reactive] public ConnectorsNodeViewModel Connectors { get; set; } = new ConnectorsNodeViewModel();
        [Reactive] public ConnectorViewModel Input { get; set; } = new ConnectorViewModel("Input");
        [Reactive] public ConnectorViewModel Output { get; set; } = new ConnectorViewModel("Output");
        [Reactive] public ConnectorViewModel FreeConnector { get; set; }
        [Reactive] public Point Point1 { get; set; }
        [Reactive] public Size Size { get; set; }
        [Reactive] public Point Point2 { get; set; }
        [Reactive] public bool DeleteEnable { get; set; } = true;

        public NodeViewModel(string name, Point position = default)
        {
            Header = new HeaderNodeViewModel(name);
            Point1 = position;
        }
        public NodeViewModel(string name, int x = 0, int y = 0):this(name, new Point(x, y))
        {

        }
    }
}
