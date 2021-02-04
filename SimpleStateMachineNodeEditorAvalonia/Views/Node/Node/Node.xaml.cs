using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
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
            set => this.SetValue(IsSelectProperty, value);
        }
        
        public Node()
        {
            this.WhenViewModelAnyValue(disposable =>
            {
                Console.WriteLine($"Node {ViewModel.Header.Name} are created");
            });
   
        }

    }
}
