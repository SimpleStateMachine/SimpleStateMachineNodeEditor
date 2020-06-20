using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            this.ContextMenu = null;
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
