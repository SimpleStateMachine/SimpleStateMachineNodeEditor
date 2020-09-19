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
        [Reactive] public NodesCanvasViewModel NodesCanvas { get; set; }
        [Reactive] public HeaderNodeViewModel Header { get; set; }
        [Reactive] public ConnectorsNodeViewModel Connectors { get; set; }
        [Reactive] public LeftConnectorViewModel Input { get; set; }
        [Reactive] public RightConnectorViewModel Output { get; set; }
        [Reactive] public Point Point1 { get; set; }
        [Reactive] public Size Size { get; set; }
        [Reactive] public Point Point2 { get; set; }
        [Reactive] public bool DeleteEnable { get; set; } = true;

        private NodeViewModel(NodesCanvasViewModel nodeCanvas)
        {
            NodesCanvas = nodeCanvas;
            Connectors = new ConnectorsNodeViewModel(this);
            Input = new LeftConnectorViewModel(this, "Input");
            Output = new RightConnectorViewModel(this, "Output");

        }
        public NodeViewModel(NodesCanvasViewModel nodeCanvas, string name, Point position = default):this(nodeCanvas)
        {
            Header = new HeaderNodeViewModel(name);
            Point1 = position;
        }
        public NodeViewModel(NodesCanvasViewModel nodeCanvas, string name, int x = 0, int y = 0):this(nodeCanvas, name, new Point(x, y))
        {

        }
    }
}
