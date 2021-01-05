using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Views
{
    public abstract class BaseView<TViewModel>: ReactiveUserControl<TViewModel>
    where TViewModel:class
    {
        protected abstract void SetupBinding();
        protected abstract void SetupEvents();
        protected abstract void SetupSubscriptions();
        protected abstract void InitializeComponent();

        public BaseView()
        {
            InitializeComponent();
            SetupBinding();
            SetupEvents();
            SetupSubscriptions();
          
        }
    }
}
