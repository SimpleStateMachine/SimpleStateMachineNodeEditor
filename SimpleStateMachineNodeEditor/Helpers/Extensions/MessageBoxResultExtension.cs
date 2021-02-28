using System;
using System.Windows;

namespace SimpleStateMachineNodeEditor.Helpers.Extensions
{
    public static class MessageBoxResultExtension
    {
        public static Enums.DialogResult ToDialogResult(this MessageBoxResult messageBoxResult)
        {
            return messageBoxResult switch
            {
                MessageBoxResult.Yes    => Enums.DialogResult.Yes,
                MessageBoxResult.No     => Enums.DialogResult.No,
                MessageBoxResult.OK     => Enums.DialogResult.Ok,
                MessageBoxResult.Cancel => Enums.DialogResult.Cancel,
                MessageBoxResult.None   => Enums.DialogResult.None,
                _ => throw new NotImplementedException()
            };
        }
    }
}
