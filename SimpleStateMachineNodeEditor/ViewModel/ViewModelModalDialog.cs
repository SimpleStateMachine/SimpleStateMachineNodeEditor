using MvvmDialogs;
using ReactiveUI.Validation.Helpers;
using SimpleStateMachineNodeEditor.View;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditor.ViewModel
{
    public class ViewModelModalDialog : ReactiveValidationObject<ViewModalDialog>
    {
    
    private readonly IDialogService dialogService;

    public ViewModelModalDialog(IDialogService dialogService)
    {
        this.dialogService = dialogService;
    }

    private void ShowDialog()
    {
       //dialogService.ShowMessageBox()
        //var dialogViewModel = new AddTextDialogViewModel();
        

        //bool? success = dialogService.ShowDialog(this, dialogViewModel);
        //if (success == true)
        //{
        //    Texts.Add(dialogViewModel.Text);
        //}
    }
}
}
