using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodesCanvasElements
{
    public partial class Connects
    {
        public ItemsControl ItemsControlConnects;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            ItemsControlConnects = this.FindControl<ItemsControl>("ItemsControlConnects");
        }
    }
}
