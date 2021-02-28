using System;
using Avalonia;
using Avalonia.Media;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.ViewModels;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class Node : BaseView<NodeViewModel>
    {
        public static readonly AvaloniaProperty<bool> IsSelectProperty =
            AvaloniaProperty.Register<Node, bool>("IsSelect", inherits: true);

        public bool IsSelect
        {
            get => this.GetValue<bool>(IsSelectProperty);
            set => SetValue(IsSelectProperty, value);
        }
        public TranslateTransform TranslateTransformNode;
        public Node()
        {
            InitializeComponent();
            TranslateTransformNode = RenderTransform as TranslateTransform;
            this.WhenViewModelAnyValue(disposable =>
            {
                Console.WriteLine($"Node {ViewModel.Header.Name} are created");
            });
   
        }

    }
}
