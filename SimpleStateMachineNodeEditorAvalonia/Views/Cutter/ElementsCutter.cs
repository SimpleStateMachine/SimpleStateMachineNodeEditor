using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Cutter
    {
        public Line LineCutter;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            LineCutter = this.FindControl<Line>("LineCutter");
        }
    }
}
