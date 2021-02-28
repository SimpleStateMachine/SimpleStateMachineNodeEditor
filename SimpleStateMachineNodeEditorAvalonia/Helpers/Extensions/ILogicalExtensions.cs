using Avalonia.LogicalTree;
using System.Collections.Generic;

namespace SimpleStateMachineNodeEditorAvalonia.Helpers
{
    public static class ILogicalExtensions
    {
        public static T FindWithExeption<T>(this ILogical anchor, string name) where T : class
        {
            T element = anchor.FindWithExeption<T>(name);

            if (element != null)
            {
                return element;
            }

            throw new KeyNotFoundException(message: string.Format("Element with name '\'{0}\' not found", name));
        }
    }
}
