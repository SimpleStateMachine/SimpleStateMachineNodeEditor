using System;

namespace SimpleStateMachineNodeEditor.Helpers.Extensions
{
    public static class FileDialogResultExtension
    {
        public static Enums.DialogResult ToDialogResult(this System.Windows.Forms.DialogResult fileDialogResult)
        {
            return fileDialogResult switch
            {
                System.Windows.Forms.DialogResult.Yes       => Enums.DialogResult.Yes,
                System.Windows.Forms.DialogResult.No        => Enums.DialogResult.No,
                System.Windows.Forms.DialogResult.OK        => Enums.DialogResult.Ok,
                System.Windows.Forms.DialogResult.Cancel    => Enums.DialogResult.Cancel,
                System.Windows.Forms.DialogResult.None      => Enums.DialogResult.None,
                System.Windows.Forms.DialogResult.Abort     => Enums.DialogResult.Abort,
                System.Windows.Forms.DialogResult.Retry     => Enums.DialogResult.Retry,
                System.Windows.Forms.DialogResult.Ignore    => Enums.DialogResult.Ignore,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
