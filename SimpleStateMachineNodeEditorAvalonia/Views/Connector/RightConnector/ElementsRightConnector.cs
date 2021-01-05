using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class RightConnector
    {
        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            base.InitializeComponent();
          
        }
    }
}
