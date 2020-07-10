using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using SimpleStateMachineNodeEditorAvalonia.Views.NodeElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Node : BaseView
    {
        public Border BorderNode;
        public Header HeaderNode;
        public LeftConnector InputNode;
        public RightConnector OutputNode;
        public Transitions TransitionsNode;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            BorderNode      = this.FindControl<Border>("BorderNode");
            HeaderNode      = this.FindControl<Header>("HeaderNode");
            InputNode       = this.FindControl<LeftConnector>("InputNode");
            OutputNode      = this.FindControl<RightConnector>("OutputNode");
            TransitionsNode = this.FindControl<Transitions>("TransitionsNode");
        }
    }
}
