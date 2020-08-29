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
        public Grid GridNodesCanvas;
        public Nodes NodesNodesCanvas;
        public Connects ConnectsNodesCanvas;
        public Selector SelectorNodesCanvas;
        public Cutter CutterNodesCanvas;

        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            GridNodesCanvas = this.FindControlWithExeption<Grid>("GridNodesCanvas");
            NodesNodesCanvas = this.FindControlWithExeption<Nodes>("NodesNodesCanvas");
            ConnectsNodesCanvas = this.FindControlWithExeption<Connects>("ConnectsNodesCanvas");
            SelectorNodesCanvas = this.FindControlWithExeption<Selector>("SelectorNodesCanvas");
            CutterNodesCanvas = this.FindControlWithExeption<Cutter>("CutterNodesCanvas");
        }

    }
}
