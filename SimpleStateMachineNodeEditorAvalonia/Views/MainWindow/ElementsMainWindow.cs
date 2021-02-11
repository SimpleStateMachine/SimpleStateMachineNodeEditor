using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class MainWindow
    {
        protected virtual void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
