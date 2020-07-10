using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class LeftConnector
    {
        public Grid GridLeftConnector;
        public TextBox TextBoxLeftConnector;
        public Ellipse EllipseLeftConnector;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            GridLeftConnector = this.FindControl<Grid>("GridLeftConnector");
            TextBoxLeftConnector = this.FindControl<TextBox>("TextBoxLeftConnector");
            EllipseLeftConnector = this.FindControl<Ellipse>("EllipseLeftConnector");
        }
    }
}
