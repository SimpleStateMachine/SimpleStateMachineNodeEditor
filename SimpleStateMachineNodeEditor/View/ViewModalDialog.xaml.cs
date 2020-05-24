using ReactiveUI;
using SimpleStateMachineNodeEditor.ViewModel;
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

namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Interaction logic for ViewModalDialog.xaml
    /// </summary>
    public partial class ViewModalDialog : UserControl, IViewFor<ViewModelModalDialog>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ViewModelModalDialog), typeof(ViewModalDialog), new PropertyMetadata(null));

        public ViewModelModalDialog ViewModel
        {
            get { return (ViewModelModalDialog)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ViewModelModalDialog)value; }
        }
        #endregion ViewModel

        public ViewModalDialog()
        {
            InitializeComponent();
        }
    }
}
