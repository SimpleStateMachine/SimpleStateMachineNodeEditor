using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodesCanvasElements
{
    public partial class Nodes
    {
        public ItemsControl ItemsControlNodes;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            ItemsControlNodes = this.FindControl<ItemsControl>("ItemsControlNodes");
        }
    }
}
