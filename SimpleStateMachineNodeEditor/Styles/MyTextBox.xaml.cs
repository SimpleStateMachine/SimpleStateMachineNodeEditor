using System.Windows;
using System.Windows.Controls;

namespace SimpleStateMachineNodeEditor.Styles
{
    /// <summary>
    /// Логика взаимодействия для TestBox.xaml
    /// </summary>
    public partial class MyTextBox : TextBox
    {
        public MyTextBox()
        {
            InitializeComponent();
            ContextMenu = null;
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            if(e.Effects != DragDropEffects.Move)
            {
                base.OnDragEnter(e);
            }
        }


        protected override void OnDragOver(DragEventArgs e)
        {
            if (e.Effects != DragDropEffects.Move)
            {
                base.OnDragOver(e);
            }
        }
    }
}
