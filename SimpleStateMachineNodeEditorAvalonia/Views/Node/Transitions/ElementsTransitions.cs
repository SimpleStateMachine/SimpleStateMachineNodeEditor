using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Transitions
    {
        public ItemsControl ItemsControlTransitions;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            ItemsControlTransitions = this.FindControl<ItemsControl>("ItemsControlTransitions");
        }
    }
}
