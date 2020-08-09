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
        public NodesCanvas NodesCanvasMainWindow;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            NodesCanvasMainWindow = this.FindControlWithExeption<NodesCanvas>("NodesCanvasMainWindow");
        }

    }
}
