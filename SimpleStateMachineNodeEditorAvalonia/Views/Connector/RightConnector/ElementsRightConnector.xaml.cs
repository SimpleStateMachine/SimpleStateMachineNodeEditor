using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
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

            GridRightConnector = this.FindControl<Grid>("GridRightConnector");
            TextBoxRightConnector = this.FindControl<TextBox>("TextBoxRightConnector");
            EllipseRightConnector = this.FindControl<Ellipse>("EllipseRightConnector");
        }
    }
}
