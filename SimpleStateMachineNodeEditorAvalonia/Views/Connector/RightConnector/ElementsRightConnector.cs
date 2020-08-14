﻿using Avalonia.Controls;
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
        public Grid GridConnector;
        public TextBox TextBoxConnector;
        public Ellipse EllipseConnector;
        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            GridConnector = this.FindControlWithExeption<Grid>("GridConnector");
            TextBoxConnector = this.FindControlWithExeption<TextBox>("TextBoxConnector");
            EllipseConnector = this.FindControlWithExeption<Ellipse>("EllipseConnector");
        }
    }
}
