using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views.NodeElements
{
    public partial class Header
    {
        public TextBox TextBoxHeader;

        public Button ButtonHeader;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            TextBoxHeader = this.FindControl<TextBox>("TextBoxHeader");

            ButtonHeader = this.FindControl<Button>("ButtonHeader");
        }
    }
}
