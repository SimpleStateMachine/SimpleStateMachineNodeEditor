using System;

namespace SimpleStateMachineNodeEditor.Helpers.Extensions
{
    public static class FileDialogResultExtension
    {
        public static Enums.DialogResult ToDialogResult(this System.Windows.Forms.DialogResult fileDialogResult)
        {
            return fileDialogResult switch
            {
                System.Windows.Forms.DialogResult.Yes       => Helpers.Enums.DialogResult.Yes,
                System.Windows.Forms.DialogResult.No        => Helpers.Enums.DialogResult.No,
                System.Windows.Forms.DialogResult.OK        => Helpers.Enums.DialogResult.Ok,
                System.Windows.Forms.DialogResult.Cancel    => Helpers.Enums.DialogResult.Cancel,
                System.Windows.Forms.DialogResult.None      => Helpers.Enums.DialogResult.None,
                System.Windows.Forms.DialogResult.Abort     => Helpers.Enums.DialogResult.Abort,
                System.Windows.Forms.DialogResult.Retry     => Helpers.Enums.DialogResult.Retry,
                System.Windows.Forms.DialogResult.Ignore    => Helpers.Enums.DialogResult.Ignore,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
