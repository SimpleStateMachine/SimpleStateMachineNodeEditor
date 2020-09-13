using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public class EmptyTextBox : TextBox
    {
        public EmptyTextBox()
        {
            this.InitializeComponent();
            this.ContextMenu = null;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        //protected override void OnDragEnter(DragEventArgs e)
        //{
        //    if (e.DragEffects != DragDropEffects.Move)
        //    {
        //        base.OnDragEnter(e);
        //    }
        //}


        //protected override void OnDragOver(DragEventArgs e)
        //{
        //    if (e.DragEffects != DragDropEffects.Move)
        //    {
        //        base.OnDragOver(e);
        //    }
        //}
    }
}
