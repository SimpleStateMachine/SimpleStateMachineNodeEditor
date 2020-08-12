using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Cutter
    {
        public Line LineCutter;

        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            LineCutter = this.FindControlWithExeption<Line>("LineCutter");
        }
    }
}
