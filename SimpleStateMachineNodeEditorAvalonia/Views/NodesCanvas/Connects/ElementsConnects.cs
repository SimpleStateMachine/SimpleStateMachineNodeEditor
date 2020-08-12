using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodesCanvasElements
{
    public partial class Connects
    {
        public ItemsControl ItemsControlConnects;

        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            ItemsControlConnects = this.FindControlWithExeption<ItemsControl>("ItemsControlConnects");
        }
    }
}
