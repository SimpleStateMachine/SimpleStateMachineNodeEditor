using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Views.NodeElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Node
    {
        public TranslateTransform TranslateTransformNode;

        protected override void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            TranslateTransformNode = this.RenderTransform as TranslateTransform;
        }
    }
}
