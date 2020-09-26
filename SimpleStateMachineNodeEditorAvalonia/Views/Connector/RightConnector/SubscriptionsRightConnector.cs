using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ReactiveUI;
using SimpleStateMachineNodeEditorAvalonia.Helpers;
using SimpleStateMachineNodeEditorAvalonia.Views.NodeElements;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public partial class RightConnector
    {
        protected override void SetupSubscriptions()
        {
            base.SetupSubscriptions();
            this.WhenViewModelAnyValue(disposable =>
            {
                this.ViewModel.WhenAnyValue(x => x.IsSelected).Subscribe(x => UpdateForeground(x)).DisposeWith(disposable);
            });
        }

        private void UpdateForeground(bool isSelected)
        {
            //this.TextBoxConnector.Foreground = Application.Current.Resources["ColorRightConnectorTextBoxForeground"+ (isSelected ?"Selected":"")] as SolidColorBrush;
        }
    }

}