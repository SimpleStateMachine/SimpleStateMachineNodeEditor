using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.ViewModels
{
    public class BaseViewModel : ReactiveObject, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; }

        public BaseViewModel()
        {
            Activator = new ViewModelActivator();
            //this.WhenActivated(disposables =>
            //{
            //    this.Test();
            //});
        }
        public void Test()
        {

        }
    }
}
