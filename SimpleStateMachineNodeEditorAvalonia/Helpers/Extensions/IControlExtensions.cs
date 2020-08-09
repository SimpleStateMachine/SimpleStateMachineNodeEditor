using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public static class IControlExtensions
    {
        public static T FindControlWithExeption<T>(this IControl control, string name) where T : class, IControl
        {
            T element = control.FindControl<T>(name);
            if(element!=null)
            {
                return element;
            }
            throw new KeyNotFoundException(message: string.Format("Element with name '\'{0}\' not found", name));
             
        }
    }
}
