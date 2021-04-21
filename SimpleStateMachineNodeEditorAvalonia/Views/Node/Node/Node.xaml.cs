using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.VisualTree;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Node : BaseView<NodeViewModel>
    {
        private NodesCanvas _canvas;
        
        #region IsSelectProperty

        public static readonly AvaloniaProperty<bool> IsSelectProperty =
            AvaloniaProperty.Register<Node, bool>("IsSelect", inherits: true);
        public bool IsSelect
        {
            get => this.GetValue<bool>(IsSelectProperty);
            set => SetValue(IsSelectProperty, value);
        }

        #endregion

        public TranslateTransform TranslateTransformNode;
        public Node()
        {
            InitializeComponent();
            TranslateTransformNode = RenderTransform as TranslateTransform;
        }

    }
}
