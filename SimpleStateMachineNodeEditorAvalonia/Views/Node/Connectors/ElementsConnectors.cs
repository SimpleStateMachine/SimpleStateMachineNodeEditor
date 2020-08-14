using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Connectors
    {
        public ItemsControl ItemsControlConnectors;
        public ListBox ListBoxConnectors;
        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            //ItemsControlConnectors = this.FindControlWithExeption<ItemsControl>("ItemsControlConnectors");
            ListBoxConnectors = this.FindControlWithExeption<ListBox>("ListBoxConnectors");
        }
      
    }
}
