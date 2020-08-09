using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
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

            CanvasNodesCanvas   = this.FindControlWithExeption<Canvas>("CanvasNodesCanvas");
            SelectorNodesCanvas = this.FindControlWithExeption<Selector>("SelectorNodesCanvas");
            NodesNodesCanvas    = this.FindControlWithExeption<Nodes>("NodesNodesCanvas");
            ConnectsNodesCanvas = this.FindControlWithExeption<Connects>("ConnectsNodesCanvas");
        }

    }
}
