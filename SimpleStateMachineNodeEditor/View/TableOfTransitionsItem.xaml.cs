using ReactiveUI;
using SimpleStateMachineNodeEditor.Helpers.Extensions;
using SimpleStateMachineNodeEditor.ViewModel;
using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;

namespace SimpleStateMachineNodeEditor.View
{
    /// <summary>
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class TableOfTransitionsItem : UserControl, IViewFor<ConnectorViewModel>
    {
        #region ViewModel
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(ConnectorViewModel), typeof(TableOfTransitionsItem), new PropertyMetadata(null));

        public ConnectorViewModel ViewModel
        {
            get { return (ConnectorViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ConnectorViewModel)value; }
        }
        #endregion ViewModel
        public TableOfTransitionsItem()
        {
            InitializeComponent();
            SetupBinding();
            SetupEvents();
        }

        #region SetupBinding
        private void SetupBinding()
        {
            this.WhenActivated(disposable =>
            {
                this.OneWayBind(ViewModel, x => x.Node.Name, x => x.TextBoxElementStateFrom.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, x => x.Name, x => x.TextBoxElementTransitionName.Text).DisposeWith(disposable);
                if (ViewModel.ItsLoop)
                    this.OneWayBind(ViewModel, x => x.Node.Name, x => x.TextBoxElementStateTo.Text).DisposeWith(disposable);
                else
                    this.OneWayBind(ViewModel, x => x.Connect.ToConnector.Node.Name, x => x.TextBoxElementStateTo.Text).DisposeWith(disposable);
            });

        }
        #endregion SetupBinding

        #region SetupEvents

        private void SetupEvents()
        {
            this.WhenActivated(disposable =>
            {
                TextBoxElementTransitionName.Events().LostFocus.Subscribe(e => ValidateTransitionName(e)).DisposeWith(disposable);
                TextBoxElementStateFrom.Events().LostFocus.Subscribe(e => ValidateStateFrom(e)).DisposeWith(disposable);
                TextBoxElementStateTo.Events().LostFocus.Subscribe(e => ValidateStateTo(e)).DisposeWith(disposable);
            });
        }
        private void ValidateTransitionName(RoutedEventArgs e)
        {
            if (TextBoxElementTransitionName.Text != ViewModel.Name)
                ViewModel.CommandValidateName.ExecuteWithSubscribe(TextBoxElementTransitionName.Text);
            if (TextBoxElementTransitionName.Text != ViewModel.Name)
                TextBoxElementTransitionName.Text = ViewModel.Name;
        }
        private void ValidateStateFrom(RoutedEventArgs e)
        {
            if (TextBoxElementStateFrom.Text != ViewModel.Node.Name)
                ViewModel.Node.CommandValidateName.ExecuteWithSubscribe(TextBoxElementStateFrom.Text);
            if (TextBoxElementStateFrom.Text != ViewModel.Node.Name)
                TextBoxElementStateFrom.Text = ViewModel.Node.Name;
        }
        private void ValidateStateTo(RoutedEventArgs e)
        {
            if (ViewModel.ItsLoop)
                ValidateStateToLoop();
            else
                ValidateStateTo();
        }
        private void ValidateStateToLoop()
        {
            if (TextBoxElementStateTo.Text != ViewModel.Node.Name)
                ViewModel.Node.CommandValidateName.ExecuteWithSubscribe(TextBoxElementStateTo.Text);
            if (TextBoxElementStateTo.Text != ViewModel.Node.Name)
                TextBoxElementStateTo.Text = ViewModel.Node.Name;
        }
        private void ValidateStateTo()
        {
            if (TextBoxElementStateTo.Text != ViewModel.Connect.ToConnector.Node.Name)
                ViewModel.Connect.ToConnector.Node.CommandValidateName.ExecuteWithSubscribe(TextBoxElementStateTo.Text);
            if (TextBoxElementStateTo.Text != ViewModel.Connect.ToConnector.Node.Name)
                TextBoxElementStateTo.Text = ViewModel.Connect.ToConnector.Node.Name;
        }
        #endregion SetupEvents
    }
}
