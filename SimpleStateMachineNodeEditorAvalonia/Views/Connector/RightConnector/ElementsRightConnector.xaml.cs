using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class RightConnector
    {
        public Grid GridRightConnector;
        public TextBox TextBoxRightConnector;
        public Ellipse EllipseRightConnector;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            GridRightConnector = this.FindControlWithExeption<Grid>("GridRightConnector");
            TextBoxRightConnector = this.FindControlWithExeption<TextBox>("TextBoxRightConnector");
            EllipseRightConnector = this.FindControlWithExeption<Ellipse>("EllipseRightConnector");
        }
    }
}
