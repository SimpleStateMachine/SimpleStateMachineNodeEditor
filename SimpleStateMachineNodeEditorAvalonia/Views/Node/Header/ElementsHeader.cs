using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Header
    {
        public TextBox TextBoxHeader;

        public ToggleButton ToggleButtonHeader;

        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            TextBoxHeader = this.FindControlWithExeption<TextBox>("TextBoxHeader");

            ToggleButtonHeader = this.FindControlWithExeption<ToggleButton>("ToggleButtonHeader");
        }
    }
}
