using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Selector
    {
        public Rectangle RectangleSelector;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            RectangleSelector = this.FindControlWithExeption<Rectangle>("RectangleSelector");
        }
    }
}
