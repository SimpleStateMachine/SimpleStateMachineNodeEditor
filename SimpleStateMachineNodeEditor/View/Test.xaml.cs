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
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class Test : UserControl, IViewFor<ViewModelConnector>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ViewModelConnector), typeof(Test), new PropertyMetadata(null));

        public ViewModelConnector ViewModel
        {
            get { return (ViewModelConnector)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ViewModelConnector)value; }
        }
        #endregion ViewModel
        public Test()
        {
            InitializeComponent();
            SetupBinding();
        }
        #region SetupBinding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {

            });
        }
        #endregion SetupBinding
    }
}
