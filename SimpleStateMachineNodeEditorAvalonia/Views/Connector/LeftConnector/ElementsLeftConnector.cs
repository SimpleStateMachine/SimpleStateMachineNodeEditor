using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
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

            GridLeftConnector = this.FindControlWithExeption<Grid>("GridLeftConnector");
            TextBoxLeftConnector = this.FindControlWithExeption<TextBox>("TextBoxLeftConnector");
            EllipseLeftConnector = this.FindControlWithExeption<Ellipse>("EllipseLeftConnector");
        }
    }
}
