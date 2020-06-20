using System;
using System.Windows;

namespace SimpleStateMachineNodeEditor.Helpers.Extensions
{
    public static class MessageBoxResultExtension
    {
        public static Helpers.Enums.DialogResult ToDialogResult(this MessageBoxResult messageBoxResult)
        {
            return messageBoxResult switch
            {
                MessageBoxResult.Yes    => Helpers.Enums.DialogResult.Yes,
                MessageBoxResult.No     => Helpers.Enums.DialogResult.No,
                MessageBoxResult.OK     => Helpers.Enums.DialogResult.Ok,
                MessageBoxResult.Cancel => Helpers.Enums.DialogResult.Cancel,
                MessageBoxResult.None   => Helpers.Enums.DialogResult.None,
                _ => throw new NotImplementedException()
            };
        }
    }
}
