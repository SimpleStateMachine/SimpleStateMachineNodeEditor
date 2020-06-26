using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class MainWindow
    {
        public TextBlock TextBlockElement => this.FindControl<TextBlock>("TextBlockElement");
    }
}
