using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Views.NodesCanvasElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class NodesCanvas
    {
        public Canvas CanvasNodesCanvas;
        public Selector SelectorNodesCanvas;
        public Nodes NodesNodesCanvas;
        public Connects ConnectsNodesCanvas;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            CanvasNodesCanvas   = this.FindControl<Canvas>("CanvasNodesCanvas");
            SelectorNodesCanvas = this.FindControl<Selector>("SelectorNodesCanvas");
            NodesNodesCanvas    = this.FindControl<Nodes>("NodesNodesCanvas");
            ConnectsNodesCanvas = this.FindControl<Connects>("ConnectsNodesCanvas");
        }

    }
}
