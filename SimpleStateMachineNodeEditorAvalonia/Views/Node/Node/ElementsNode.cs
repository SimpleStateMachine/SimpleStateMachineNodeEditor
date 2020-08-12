using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Views.NodeElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Node
    {
        public Border BorderNode;
        public Header HeaderNode;
        public LeftConnector InputNode;
        public RightConnector OutputNode;
        public Connectors ConnectorsNode;

        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            BorderNode      = this.FindControlWithExeption<Border>("BorderNode");
            HeaderNode      = this.FindControlWithExeption<Header>("HeaderNode");
            InputNode       = this.FindControlWithExeption<LeftConnector>("InputNode");
            OutputNode      = this.FindControlWithExeption<RightConnector>("OutputNode");
            ConnectorsNode = this.FindControlWithExeption<Connectors>("ConnectorsNode");
        }
    }
}
